using System.Threading;
using DatabasesManagement;
using LibDatabaseParameters;
using LibParameters;
using Microsoft.Extensions.Logging;

namespace ApAgent.Counters;

public sealed class StepNamePrefixCounter
{
    private readonly string _databaseServerConnectionName;
    private readonly ILogger _logger;
    private readonly IParametersManager _parametersManager;

    // ReSharper disable once ConvertToPrimaryConstructor
    public StepNamePrefixCounter(ILogger logger, IParametersManager parametersManager, string databaseServerConnectionName)
    {
        _logger = logger;
        _parametersManager = parametersManager;
        _databaseServerConnectionName = databaseServerConnectionName;
    }

    public string Count()
    {
        var parameters =
            (IParametersWithDatabaseServerConnections)_parametersManager.Parameters;

        var dac = DatabaseAgentClientsFabric.CreateDatabaseManagementClient(true, _logger,
            _databaseServerConnectionName, new DatabaseServerConnections(parameters.DatabaseServerConnections), null,
            null, CancellationToken.None).Result;

        var getDatabaseServerInfoResult = dac?.GetDatabaseServerInfo(CancellationToken.None).Result;
        if (getDatabaseServerInfoResult is not null && getDatabaseServerInfoResult.Value.IsT0)
            return getDatabaseServerInfoResult.Value.AsT0?.ServerName ?? string.Empty;
        return string.Empty;
    }
}