using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using CliMenu;
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

public sealed class OneDatabaseNameFieldEditor : FieldEditor<string>
{
    private readonly string _databaseServerConnectionNamePropertyName;

    //private readonly string _databaseWebAgentNamePropertyName;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;
    private readonly IParametersManager _parametersManager;

    // ReSharper disable once ConvertToPrimaryConstructor
    public OneDatabaseNameFieldEditor(ILogger logger, IHttpClientFactory httpClientFactory, string propertyName,
        IParametersManager parametersManager, string databaseServerConnectionNamePropertyName,
        bool enterFieldDataOnCreate = false) : base(propertyName, enterFieldDataOnCreate)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _parametersManager = parametersManager;
        _databaseServerConnectionNamePropertyName = databaseServerConnectionNamePropertyName;
    }

    public override void UpdateField(string? recordName, object recordForUpdate)
    {
        var databaseServerConnectionName = GetValue<string>(recordForUpdate, _databaseServerConnectionNamePropertyName);

        var dbList = CreateDbList(databaseServerConnectionName);

        var currentDatabaseName = GetValue(recordForUpdate);

        CliMenuSet listSet = new();
        foreach (var listItem in dbList.Select(s => s.Name)) listSet.AddMenuItem(new CliMenuCommand(listItem));

        var selectedId = MenuInputer.InputIdFromMenuList(PropertyName.Pluralize(), listSet, currentDatabaseName);
        if (selectedId >= 0 && selectedId < dbList.Count)
            SetValue(recordForUpdate, dbList[selectedId].Name);
    }

    private List<DatabaseInfoModel> CreateDbList(string? databaseServerConnectionName)
    {
        var dbList = new List<DatabaseInfoModel>();

        if (_parametersManager.Parameters is not IParametersWithDatabaseServerConnectionsAndApiClients parameters)
        {
            Console.WriteLine("Parameters is invalid");
            return dbList;
        }

        var createDatabaseManagerResult = DatabaseManagersFabric.CreateDatabaseManager(_logger, true,
            databaseServerConnectionName, new DatabaseServerConnections(parameters.DatabaseServerConnections),
            new ApiClients(parameters.ApiClients), _httpClientFactory, null, null, CancellationToken.None).Result;

        if (createDatabaseManagerResult.IsT1)
        {
            StShared.WriteErrorLine(
                $"DatabaseManagementClient does not created for webAgent {databaseServerConnectionName}", true,
                _logger);
            dbList = [];
        }
        else
        {
            DatabasesListCreator databasesListCreator =
                new(EDatabaseSet.AllDatabases, createDatabaseManagerResult.AsT0, EBackupType.Full);
            dbList = databasesListCreator.LoadDatabaseNames(CancellationToken.None).Result;
        }

        return dbList;
    }
}