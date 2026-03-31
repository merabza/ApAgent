using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ApAgentData.LibApAgentData;
using ApAgentData.LibApAgentData.Models;
using AppCliTools.CliParameters.FieldEditors;
using AppCliTools.LibMenuInput;
using DatabaseTools.DbTools;
using DatabaseTools.DbTools.Models;
using Microsoft.Extensions.Logging;
using OneOf;
using ParametersManagement.LibApiClientParameters;
using ParametersManagement.LibDatabaseParameters;
using ParametersManagement.LibParameters;
using SystemTools.SystemToolsShared;
using SystemTools.SystemToolsShared.Errors;
using ToolsManagement.DatabasesManagement;

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

    public override async ValueTask UpdateField(string? recordKey, object recordForUpdate,
        CancellationToken cancellationToken = default)
    {
        string? databaseServerConnectionName =
            GetValue<string>(recordForUpdate, _databaseServerConnectionNamePropertyName);
        string? databaseWebAgentName = GetValue<string>(recordForUpdate, _databaseWebAgentNamePropertyName);

        var databaseSet = GetValue<EDatabaseSet>(recordForUpdate, _databaseSetPropertyName);

        var backupType = EBackupType.Full;

        if (_databaseBackupParametersPropertyName is not null)
        {
            var databaseBackupParameters =
                GetValue<DatabaseBackupParametersDomain>(recordForUpdate, _databaseBackupParametersPropertyName);
            if (databaseBackupParameters is not null)
            {
                backupType = databaseBackupParameters.BackupType;
            }
        }

        if (_parametersManager.Parameters is not IParametersWithDatabaseServerConnectionsAndApiClients parameters)
        {
            Console.WriteLine("Parameters is invalid");
            return;
        }

        List<DatabaseInfoModel> dbList;

        OneOf<IDatabaseManager, Error[]> createDatabaseManagerResult =
            await DatabaseManagersFactory.CreateDatabaseManager(_logger, true, databaseServerConnectionName,
                new DatabaseServerConnections(parameters.DatabaseServerConnections),
                new ApiClients(parameters.ApiClients), _httpClientFactory, null, null, cancellationToken);

        if (createDatabaseManagerResult.IsT1)
        {
            Error.PrintErrorsOnConsole(createDatabaseManagerResult.AsT1);
            StShared.WriteErrorLine($"DatabaseManagementClient does not created for webAgent {databaseWebAgentName}",
                true, _logger);
            dbList = [];
        }
        else
        {
            var databasesListCreator =
                new DatabasesListCreator(databaseSet, createDatabaseManagerResult.AsT0, backupType);
            dbList = await databasesListCreator.LoadDatabaseNames(cancellationToken);
        }

        if (databaseSet != EDatabaseSet.DatabasesBySelection)
        {
            Console.WriteLine("Databases list is:");
            int i = 0;
            foreach (DatabaseInfoModel databaseInfoModel in dbList.OrderBy(o => o.IsSystemDatabase)
                         .ThenBy(tb => tb.Name))
            {
                i++;
                Console.WriteLine($"{i}. {databaseInfoModel.Name}");
            }
        }
        else
        {
            List<string> oldDatabaseNames = GetValue(recordForUpdate, []) ?? [];
            Dictionary<string, bool> oldDatabaseChecks = dbList.ToDictionary(
                databaseInfoModel => databaseInfoModel.Name,
                databaseInfoModel => oldDatabaseNames.Contains(databaseInfoModel.Name));
            SetValue(recordForUpdate, MenuInputer.MultipleInputFromList(FieldName, oldDatabaseChecks));
        }
    }

    public override string GetValueStatus(object? record)
    {
        List<string>? val = GetValue(record);
        return val is null ? string.Empty : string.Join(",", val);
    }
}
