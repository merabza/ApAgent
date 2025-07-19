using ApAgent.Generators;
using CliMenu;
using CliParametersDataEdit.Cruders;
using LibApAgentData.Models;
using LibParameters;
using Microsoft.Extensions.Logging;
using SystemToolsShared;

namespace ApAgent.MenuCommands;

public sealed class GenerateStandardDatabaseStepsCommand : CliMenuCommand
{
    private readonly ILogger _logger;
    private readonly ParametersManager _parametersManager;

    // ReSharper disable once ConvertToPrimaryConstructor
    public GenerateStandardDatabaseStepsCommand(ILogger logger, ParametersManager parametersManager) : base(
        "Generate Standard Database Jobs...", EMenuAction.Reload)
    {
        _parametersManager = parametersManager;
        _logger = logger;
    }

    protected override bool RunBody()
    {
        var databaseServerConnectionCruder = DatabaseServerConnectionCruder.Create(_logger, null, _parametersManager);

        var databaseConnectionName =
            databaseServerConnectionCruder.GetNameWithPossibleNewName(
                "Select local connection for Generate standard database maintenance schema", null);

        if (string.IsNullOrWhiteSpace(databaseConnectionName))
        {
            StShared.WriteErrorLine("databaseConnectionName does not specified", true);
            return false;
        }

        var parameters = (ApAgentParameters)_parametersManager.Parameters;

        StandardJobsSchemaGenerator standardJobsSchemaGenerator = new(true, _logger, _parametersManager,
            databaseConnectionName, _parametersManager.ParametersFileName);
        standardJobsSchemaGenerator.Generate();

        //შენახვა
        _parametersManager.Save(parameters, "Maintain schema generated success");
        return true;
    }
}