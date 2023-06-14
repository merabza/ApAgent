using System;
using LibApAgentData.Models;
using LibParameters;
using SystemToolsShared;

namespace ApAgent.Counters;

public /*open*/ class SCounter
{
    private readonly IParametersManager _parametersManager;

    protected SCounter(IParametersManager parametersManager)
    {
        _parametersManager = parametersManager;
    }

    protected bool IsFileStorageLocal(string? fileStorageName)
    {
        var parameters = (ApAgentParameters)_parametersManager.Parameters;

        if (fileStorageName == null)
        {
            StShared.WriteErrorLine("FileStorage with Name not specified. ", true);
            return true;
        }

        if (!parameters.FileStorages.ContainsKey(fileStorageName))
        {
            StShared.WriteErrorLine($"FileStorage with Name {fileStorageName} does not exists. ", true);
            return true;
        }

        var fileStorage = parameters.FileStorages[fileStorageName];

        if (fileStorage.FileStoragePath is null)
            throw new Exception("fileStorage.FileStoragePath is null");

        return FileStat.IsFileSchema(fileStorage.FileStoragePath);
    }
}