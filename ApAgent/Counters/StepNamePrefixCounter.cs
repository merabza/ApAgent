using System.Threading;
using DatabaseTools.DbTools.Models;
using Microsoft.Extensions.Logging;
using OneOf;
using ParametersManagement.LibDatabaseParameters;
using ParametersManagement.LibParameters;
using SystemTools.SystemToolsShared.Errors;
using ToolsManagement.DatabasesManagement;

namespace ApAgent.Counters;

public sealed class StepNamePrefixCounter
{
    private readonly string _databaseServerConnectionName;
    private readonly ILogger _logger;
    private readonly IParametersManager _parametersManager;

    // ReSharper disable once ConvertToPrimaryConstructor
    public StepNamePrefixCounter(ILogger logger, IParametersManager parametersManager,
        string databaseServerConnectionName)
    {
        _logger = logger;
        _parametersManager = parametersManager;
        _databaseServerConnectionName = databaseServerConnectionName;
    }

    public string Count()
    {
        var parameters = (IParametersWithDatabaseServerConnections)_parametersManager.Parameters;

        OneOf<IDatabaseManager, Err[]> createDatabaseManagerResult = DatabaseManagersFactory
            .CreateDatabaseManager(_logger, true, _databaseServerConnectionName,
                new DatabaseServerConnections(parameters.DatabaseServerConnections), CancellationToken.None).Result;

        if (createDatabaseManagerResult.IsT1)
        {
            Err.PrintErrorsOnConsole(createDatabaseManagerResult.AsT1);
        }

        OneOf<DbServerInfo, Err[]> getDatabaseServerInfoResult =
            createDatabaseManagerResult.AsT0.GetDatabaseServerInfo(CancellationToken.None).Result;
        if (getDatabaseServerInfoResult.IsT0)
        {
            return getDatabaseServerInfoResult.AsT0.ServerName ?? string.Empty;
        }

        return string.Empty;
    }
}
