using Microsoft.Extensions.DependencyInjection;
using SystemToolsShared;

namespace ApAgent;

public sealed class ApAgentServicesCreator : ServicesCreator
{
    public ApAgentServicesCreator(string logFolder, string logFileName, string appName) : base(logFolder, logFileName,
        appName)
    {
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);


        //services.AddHostedService<JobStepsRunnerQueuedHostedService>();
        //services.AddSingleton<IJobStepsRunnerBackgroundTaskQueue, BackgroundTaskQueue>();
    }
}