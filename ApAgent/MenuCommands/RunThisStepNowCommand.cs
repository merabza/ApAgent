using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
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
        _parametersFileName = parametersFileName;
    }

    protected override ValueTask<bool> RunBody(CancellationToken cancellationToken = default)
    {
        var parameters = (ApAgentParameters)_parametersManager.Parameters;

        string? procLogFilesFolder =
            parameters.CountLocalPath(parameters.ProcLogFilesFolder, _parametersFileName, "ProcLogFiles");

        if (string.IsNullOrWhiteSpace(procLogFilesFolder))
        {
            StShared.WriteErrorLine("procLogFilesFolder does not counted. step does not started", true, _logger);
            return new ValueTask<bool>(false);
        }

        // ReSharper disable once using
        using ProcessManager processManager = _processes.GetNewProcessManager();

        ProcessesToolAction? stepToolAction = _jobStep.GetToolAction(_logger, _httpClientFactory, true, processManager,
            parameters, procLogFilesFolder);

        if (stepToolAction is null)
        {
            StShared.WriteErrorLine("stepToolAction does not found. step does not started", true, _logger);
            return new ValueTask<bool>(false);
        }

        processManager.Run(stepToolAction);
        return new ValueTask<bool>(true);
    }
}
