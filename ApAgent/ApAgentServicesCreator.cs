using LibApAgentData.Models;
using Microsoft.Extensions.DependencyInjection;
using SystemToolsShared;

namespace ApAgent;

public sealed class ApAgentServicesCreator : ServicesCreator
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public ApAgentServicesCreator(ApAgentParameters par) : base(par.LogFolder, null, "ApAgent")
    {

    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddHttpClient();
    }
}