using ApAgent.StepCruders;
using CliMenu;
using LibApAgentData.Models;
using LibApAgentData.Steps;
using LibParameters;
using LibToolActions.BackgroundTasks;
using Microsoft.Extensions.Logging;
using SystemToolsShared;

namespace ApAgent.MenuCommands;

public sealed class RunThisStepNowCommand : CliMenuCommand
{
    private readonly ILogger _logger;
    private readonly string? _parametersFileName;
    private readonly IParametersManager _parametersManager;
    private readonly Processes _processes;
    private readonly StepCruder _stepCruder;
    private readonly string _stepName;

    // ReSharper disable once ConvertToPrimaryConstructor
    public RunThisStepNowCommand(ILogger logger, Processes processes, IParametersManager parametersManager,
        StepCruder stepCruder, string stepName, string? parametersFileName)
    {
        _logger = logger;
        _processes = processes;
        _parametersManager = parametersManager;
        _stepCruder = stepCruder;
        _stepName = stepName;
        _parametersFileName = parametersFileName;
    }

    protected override void RunAction()
    {
        var parameters = (ApAgentParameters)_parametersManager.Parameters;

        var procLogFilesFolder =
            parameters.CountLocalPath(parameters.ProcLogFilesFolder, _parametersFileName, "ProcLogFiles");

        if (string.IsNullOrWhiteSpace(procLogFilesFolder))
        {
            StShared.WriteErrorLine("procLogFilesFolder does not counted. step does not started", true, _logger);
            return;
        }

        var jobStep = (JobStep?)_stepCruder.GetItemByName(_stepName);

        if (jobStep is null)
        {
            StShared.WriteErrorLine("jobStep does not found. step does not started", true, _logger);
            return;
        }

        // ReSharper disable once using
        using var processManager = _processes.GetNewProcessManager();

        var stepToolAction =
            jobStep.GetToolAction(_logger, true, processManager, parameters, procLogFilesFolder);

        if (stepToolAction is null)
        {
            StShared.WriteErrorLine("stepToolAction does not found. step does not started", true, _logger);
            return;
        }

        processManager.Run(stepToolAction);
    }
}