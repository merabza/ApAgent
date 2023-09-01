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

    public StepNamePrefixCounter(ILogger logger, IParametersManager parametersManager,
        string databaseServerConnectionName)
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
            null);

        var dbServerInfo = dac?.GetDatabaseServerInfo().Result;
        return dbServerInfo?.ServerName ?? string.Empty;
    }
}