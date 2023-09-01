using System;
using DatabasesManagement;
using LibDatabaseParameters;
using LibParameters;
using Microsoft.Extensions.Logging;

namespace ApAgent.Counters;

public sealed class ProcLineCounter : SCounter
{
    private readonly string _databaseServerConnectionName;
    private readonly string? _downloadFileStorageName;
    private readonly ILogger _logger;
    private readonly IParametersManager _parametersManager;
    private readonly string? _uploadFileStorageName;

    public ProcLineCounter(ILogger logger, IParametersManager parametersManager, string databaseServerConnectionName,
        string? downloadFileStorageName, string? uploadFileStorageName) : base(
        parametersManager)
    {
        _logger = logger;
        _parametersManager = parametersManager;
        _databaseServerConnectionName = databaseServerConnectionName;
        _downloadFileStorageName = downloadFileStorageName;
        _uploadFileStorageName = uploadFileStorageName;
    }

    private bool IsServerLocal()
    {
        if (string.IsNullOrWhiteSpace(_databaseServerConnectionName))
            return false;
        if (_parametersManager.Parameters is not IParametersWithDatabaseServerConnections parametersDsc)
            return false;

        var dac = DatabaseAgentClientsFabric.CreateDatabaseManagementClient(true, _logger,
            _databaseServerConnectionName, new DatabaseServerConnections(parametersDsc.DatabaseServerConnections), null,
            null);

        return dac?.IsServerLocal() ?? false;
    }

    public int Count(EProcLineCase procLineCase)
    {
        return procLineCase switch
        {
            EProcLineCase.Backup => 1,
            EProcLineCase.Download => IsServerLocal() || _downloadFileStorageName is null ||
                                      IsFileStorageLocal(_downloadFileStorageName)
                ? 1
                : 2,
            EProcLineCase.Archive => IsServerLocal() || _downloadFileStorageName is null ||
                                     IsFileStorageLocal(_downloadFileStorageName)
                ? 1
                : 3,
            EProcLineCase.Upload => _uploadFileStorageName is null || IsFileStorageLocal(_uploadFileStorageName)
                ? 1
                : 4,
            _ => throw new ArgumentOutOfRangeException(nameof(procLineCase), procLineCase, null)
        };
    }
}