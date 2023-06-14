using LibApAgentData.Models;
using LibFileParameters.Models;
using LibParameters;

namespace ApAgent.Generators;

public sealed class FileStorageGenerator
{
    private readonly IParametersManager _parametersManager;

    public FileStorageGenerator(IParametersManager parametersManager)
    {
        _parametersManager = parametersManager;
    }

    public void GenerateForLocalPath(string fileStorageName, string fileStoragePath)
    {
        var parameters = (ApAgentParameters)_parametersManager.Parameters;

        var fileStorage = new FileStorageData { FileStoragePath = fileStoragePath };

        if (!parameters.FileStorages.ContainsKey(fileStorageName))
            parameters.FileStorages.Add(fileStorageName, fileStorage);
    }
}