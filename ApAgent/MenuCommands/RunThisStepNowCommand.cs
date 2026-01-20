using System.Net.Http;
using ApAgentData.LibApAgentData.Models;
using ApAgentData.LibApAgentData.Steps;
using AppCliTools.CliMenu;
using Microsoft.Extensions.Logging;
using ParametersManagement.LibParameters;
using SystemTools.SystemToolsShared;
using ToolsManagement.LibToolActions.BackgroundTasks;

namespace ApAgent.MenuCommands;

public sealed class RunThisStepNowCommand : CliMenuCommand
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JobStep _jobStep;
    private readonly ILogger _logger;
    private readonly string? _parametersFileName;
    private readonly IParametersManager _parametersManager;

    private readonly Processes _processes;
    //private readonly StepCruder _stepCruder;
    //private readonly string _stepName;

    // ReSharper disable once ConvertToPrimaryConstructor
    public RunThisStepNowCommand(ILogger logger, IHttpClientFactory httpClientFactory, Processes processes,
        IParametersManager parametersManager, JobStep jobStep, string? parametersFileName) : base(
        "Run this step now...", EMenuAction.Reload)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _processes = processes;
        _parametersManager = parametersManager;
        _jobStep = jobStep;
        //_stepName = stepName;
        _parametersFileName = parametersFileName;
    }

    protected override bool RunBody()
    {
        var parameters = (ApAgentParameters)_parametersManager.Parameters;

        string? procLogFilesFolder =
            parameters.CountLocalPath(parameters.ProcLogFilesFolder, _parametersFileName, "ProcLogFiles");

        if (string.IsNullOrWhiteSpace(procLogFilesFolder))
        {
            StShared.WriteErrorLine("procLogFilesFolder does not counted. step does not started", true, _logger);
            return false;
        }

        //JobStep? jobStep = (JobStep?)_stepCruder.GetItemByName(_stepName);

        //if (jobStep is null)
        //{
        //    StShared.WriteErrorLine("jobStep does not found. step does not started", true, _logger);
        //    return false;
        //}

        // ReSharper disable once using
        using ProcessManager processManager = _processes.GetNewProcessManager();

        ProcessesToolAction? stepToolAction = _jobStep.GetToolAction(_logger, _httpClientFactory, true, processManager,
            parameters, procLogFilesFolder);

        if (stepToolAction is null)
        {
            StShared.WriteErrorLine("stepToolAction does not found. step does not started", true, _logger);
            return false;
        }

        processManager.Run(stepToolAction);
        return true;
    }
}
