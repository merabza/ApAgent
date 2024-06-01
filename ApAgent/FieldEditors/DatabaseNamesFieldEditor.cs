using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using CliParameters.FieldEditors;
using DatabasesManagement;
using DbTools;
using DbTools.Models;
using LibApAgentData;
using LibApAgentData.Models;
using LibApiClientParameters;
using LibDatabaseParameters;
using LibMenuInput;
using LibParameters;
using Microsoft.Extensions.Logging;
using SystemToolsShared;

namespace ApAgent.FieldEditors;

public sealed class DatabaseNamesFieldEditor : FieldEditor<List<string>>
{
    private readonly string? _databaseBackupParametersPropertyName;

    private readonly string _databaseServerConnectionNamePropertyName;

    private readonly string _databaseSetPropertyName;
    private readonly string _databaseWebAgentNamePropertyName;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;
    private readonly IParametersManager _parametersManager;

    // ReSharper disable once ConvertToPrimaryConstructor
    public DatabaseNamesFieldEditor(ILogger logger, IHttpClientFactory httpClientFactory, string propertyName,
        IParametersManager parametersManager, string databaseServerConnectionNamePropertyName,
        string databaseWebAgentNamePropertyName, string databaseSetPropertyName,
        string? databaseBackupParametersPropertyName = null, bool enterFieldDataOnCreate = false) : base(propertyName,
        enterFieldDataOnCreate)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _parametersManager = parametersManager;
        _databaseServerConnectionNamePropertyName = databaseServerConnectionNamePropertyName;
        _databaseWebAgentNamePropertyName = databaseWebAgentNamePropertyName;
        _databaseSetPropertyName = databaseSetPropertyName;
        _databaseBackupParametersPropertyName = databaseBackupParametersPropertyName;
    }

    public override void UpdateField(string? recordName, object recordForUpdate)
    {
        var databaseServerConnectionName =
            GetValue<string>(recordForUpdate, _databaseServerConnectionNamePropertyName);
        var databaseWebAgentName = GetValue<string>(recordForUpdate, _databaseWebAgentNamePropertyName);

        var databaseSet = GetValue<EDatabaseSet>(recordForUpdate, _databaseSetPropertyName);

        var backupType = EBackupType.Full;

        if (_databaseBackupParametersPropertyName is not null)
        {
            var databaseBackupParameters =
                GetValue<DatabaseBackupParametersModel>(recordForUpdate, _databaseBackupParametersPropertyName);
            if (databaseBackupParameters is not null)
                backupType = databaseBackupParameters.BackupType;
        }

        if (_parametersManager.Parameters is not IParametersWithDatabaseServerConnectionsAndApiClients parameters)
        {
            Console.WriteLine("Parameters is invalid");
            return;
        }

        List<DatabaseInfoModel> dbList;

        var agentClient = DatabaseAgentClientsFabric.CreateDatabaseManager(true, _logger, _httpClientFactory,
                databaseWebAgentName, new ApiClients(parameters.ApiClients), databaseServerConnectionName,
                new DatabaseServerConnections(parameters.DatabaseServerConnections), null, null, CancellationToken.None)
            .Result;

        if (agentClient is null)
        {
            StShared.WriteErrorLine($"DatabaseManagementClient does not created for webAgent {databaseWebAgentName}",
                true, _logger);
            dbList = [];
        }
        else
        {
            DatabasesListCreator databasesListCreator = new(databaseSet, agentClient, backupType);
            dbList = databasesListCreator.LoadDatabaseNames(CancellationToken.None).Result;
        }

        if (databaseSet != EDatabaseSet.DatabasesBySelection)
        {
            Console.WriteLine("Databases list is:");
            var i = 0;
            foreach (var databaseInfoModel in dbList.OrderBy(o => o.IsSystemDatabase)
                         .ThenBy(tb => tb.Name))
            {
                i++;
                Console.WriteLine($"{i}. {databaseInfoModel.Name}");
            }
        }
        else
        {
            var oldDatabaseNames = GetValue(recordForUpdate, []) ?? [];
            var oldDatabaseChecks = dbList.ToDictionary(
                databaseInfoModel => databaseInfoModel.Name,
                databaseInfoModel => oldDatabaseNames.Contains(databaseInfoModel.Name));
            SetValue(recordForUpdate, MenuInputer.MultipleInputFromList(FieldName, oldDatabaseChecks));
        }
    }

    public override string GetValueStatus(object? record)
    {
        var val = GetValue(record);
        return val is null ? "" : string.Join(",", val);
    }
}