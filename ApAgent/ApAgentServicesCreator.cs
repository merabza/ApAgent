using SystemToolsShared;

namespace ApAgent;

public sealed class ApAgentServicesCreator : ServicesCreator
{
    public ApAgentServicesCreator(string logFolder, string logFileName, string appName) : base(logFolder, logFileName,
        appName)
    {
    }
}