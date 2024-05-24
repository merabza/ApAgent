using System;
using System.Threading;
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

    // ReSharper disable once ConvertToPrimaryConstructor
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

        var dac = DatabaseAgentClientsFabric.CreateDatabaseManager(true, _logger, _databaseServerConnectionName,
                new DatabaseServerConnections(parametersDsc.DatabaseServerConnections), null, null,
                CancellationToken.None)
            .Result;

        var isServerLocalResult = dac?.IsServerLocal(CancellationToken.None).Result;
        if (isServerLocalResult is not null && isServerLocalResult.Value.IsT0)
            return isServerLocalResult.Value.AsT0;
        return false;
    }

    public int Count(EProcLineCase procLineCase)
    {
        var isServerLocal = IsServerLocal();

        return procLineCase switch
        {
            EProcLineCase.Backup => 1,
            EProcLineCase.Download => isServerLocal || _downloadFileStorageName is null ||
                                      IsFileStorageLocal(_downloadFileStorageName)
                ? 1
                : 2,
            EProcLineCase.Archive => isServerLocal || _downloadFileStorageName is null ||
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