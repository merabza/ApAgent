using System.Net.Http;
using ApAgentData.LibApAgentData.Models;
using AppCliTools.CliMenu;
using Microsoft.Extensions.Logging;
using ParametersManagement.LibParameters;
using SystemTools.SystemToolsShared;
using ToolsManagement.LibToolActions.BackgroundTasks;

namespace ApAgent.MenuCommands;

public sealed class RunAllStepsNowCommand : CliMenuCommand
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _jobScheduleName;
    private readonly ILogger _logger;
    private readonly string? _parametersFileName;
    private readonly IParametersManager _parametersManager;
    private readonly Processes _processes;

    // ReSharper disable once ConvertToPrimaryConstructor
    public RunAllStepsNowCommand(ILogger logger, IHttpClientFactory httpClientFactory, Processes processes,
        IParametersManager parametersManager, string jobScheduleName, string? parametersFileName) : base(
        "Run All steps from this schedule now...", EMenuAction.Reload)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _processes = processes;
        _parametersManager = parametersManager;
        _jobScheduleName = jobScheduleName;
        _parametersFileName = parametersFileName;
    }

    protected override bool RunBody()
    {
        var parameters = (ApAgentParameters)_parametersManager.Parameters;

        string? procLogFilesFolder =
            parameters.CountLocalPath(parameters.ProcLogFilesFolder, _parametersFileName, "ProcLogFiles");

        if (!string.IsNullOrWhiteSpace(procLogFilesFolder))
        {
            return parameters.RunAllSteps(_logger, _httpClientFactory, true, _jobScheduleName, _processes,
                procLogFilesFolder);
        }

        StShared.WriteErrorLine("procLogFilesFolder does not counted. cannot run steps", true, _logger);
        return false;
    }
}
