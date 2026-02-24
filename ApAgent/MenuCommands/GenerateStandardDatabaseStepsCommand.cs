using System.Threading;
using System.Threading.Tasks;
using ApAgent.Generators;
using ApAgentData.LibApAgentData.Models;
using AppCliTools.CliMenu;
using AppCliTools.CliParametersDataEdit.Cruders;
using Microsoft.Extensions.Logging;
using ParametersManagement.LibParameters;
using SystemTools.SystemToolsShared;

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

    protected override async ValueTask<bool> RunBody(CancellationToken cancellationToken = default)
    {
        var databaseServerConnectionCruder = DatabaseServerConnectionCruder.Create(_logger, null, _parametersManager);

        string? databaseConnectionName = await databaseServerConnectionCruder.GetNameWithPossibleNewName(
            "Select local connection for Generate standard database maintenance schema", null, null, false,
            cancellationToken);

        if (string.IsNullOrWhiteSpace(databaseConnectionName))
        {
            StShared.WriteErrorLine("databaseConnectionName does not specified", true);
            return false;
        }

        var parameters = (ApAgentParameters)_parametersManager.Parameters;

        var standardJobsSchemaGenerator = new StandardJobsSchemaGenerator(true, _logger, _parametersManager,
            databaseConnectionName, _parametersManager.ParametersFileName);
        await standardJobsSchemaGenerator.Generate(cancellationToken);

        //შენახვა
        await _parametersManager.Save(parameters, "Maintain schema generated success", null, cancellationToken);
        return true;
    }
}
