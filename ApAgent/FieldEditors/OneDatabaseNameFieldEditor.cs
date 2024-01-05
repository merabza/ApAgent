using System.Collections.Generic;
using System.Linq;
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

    private readonly string _databaseWebAgentNamePropertyName;
    private readonly ILogger _logger;
    private readonly IParametersManager _parametersManager;

    // ReSharper disable once ConvertToPrimaryConstructor
    public OneDatabaseNameFieldEditor(ILogger logger, string propertyName, IParametersManager parametersManager,
        string databaseServerConnectionNamePropertyName, string databaseWebAgentNamePropertyName,
        bool enterFieldDataOnCreate = false) : base(propertyName, enterFieldDataOnCreate)
    {
        _logger = logger;
        _parametersManager = parametersManager;
        _databaseServerConnectionNamePropertyName = databaseServerConnectionNamePropertyName;
        _databaseWebAgentNamePropertyName = databaseWebAgentNamePropertyName;
    }

    public override void UpdateField(string? recordName, object recordForUpdate)
    {
        var databaseServerConnectionName =
            GetValue<string>(recordForUpdate, _databaseServerConnectionNamePropertyName);
        var databaseWebAgentName = GetValue<string>(recordForUpdate, _databaseWebAgentNamePropertyName);

        var agentClient =
            _parametersManager.Parameters is not IParametersWithDatabaseServerConnectionsAndApiClients parameters
                ? null
                : DatabaseAgentClientsFabric.CreateDatabaseManagementClient(true, _logger, databaseWebAgentName,
                    new ApiClients(parameters.ApiClients), databaseServerConnectionName,
                    new DatabaseServerConnections(parameters.DatabaseServerConnections), null, null,
                    CancellationToken.None).Result;

        List<DatabaseInfoModel> dbList;

        if (agentClient is null)
        {
            StShared.WriteErrorLine($"DatabaseManagementClient does not created for webAgent {databaseWebAgentName}",
                true, _logger);
            dbList = [];
        }
        else
        {
            DatabasesListCreator databasesListCreator = new(EDatabaseSet.AllDatabases, agentClient, EBackupType.Full);
            dbList = databasesListCreator.LoadDatabaseNames();
        }

        var currentDatabaseName = GetValue(recordForUpdate);

        CliMenuSet listSet = new();
        foreach (var listItem in dbList.Select(s => s.Name)) listSet.AddMenuItem(new CliMenuCommand(listItem));

        var selectedId = MenuInputer.InputIdFromMenuList(PropertyName.Pluralize(), listSet, currentDatabaseName);
        if (selectedId >= 0 && selectedId < dbList.Count)
            SetValue(recordForUpdate, dbList[selectedId].Name);
    }
}