using System;
using System.Net.Http;
using ApAgent;
using ApAgentData.LibApAgentData;
using ApAgentData.LibApAgentData.Models;
using AppCliTools.CliParameters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ParametersManagement.LibParameters;
using Serilog;
using Serilog.Events;
using SystemTools.SystemToolsShared;
using ToolsManagement.LibToolActions.BackgroundTasks;

ILogger<Program>? logger = null;
try
{
    Console.WriteLine("Loading...");

    const string appName = "ApAgent";

    //პროგრამის ატრიბუტების დაყენება 
    ProgramAttributes.Instance.AppName = appName;

    string key = StringExtension.AppAgentAppKey + Environment.MachineName.Capitalize();

    var argParser = new ArgumentsParser<ApAgentParameters>(args, appName, key);
    EParseResult parseResult = argParser.Analysis();

    if (parseResult != EParseResult.Ok)
    {
        return 1;
    }

    var par = (ApAgentParameters?)argParser.Par;
    if (par is null)
    {
        StShared.WriteErrorLine("ApAgentParameters is null", true);
        return 3;
    }

    string? parametersFileName = argParser.ParametersFileName;
    var apAgentServicesCreator = new ApAgentServicesCreator(par);

    // ReSharper disable once using
    await using ServiceProvider? serviceProvider = apAgentServicesCreator.CreateServiceProvider(LogEventLevel.Information);

    if (serviceProvider == null)
    {
        Console.WriteLine("Logger not created");
        return 8;
    }

    logger = serviceProvider.GetService<ILogger<Program>>();
    if (logger is null)
    {
        StShared.WriteErrorLine("logger is null", true);
        return 3;
    }

    var processesLogger = serviceProvider.GetService<ILogger<Processes>>();
    if (processesLogger is null)
    {
        StShared.WriteErrorLine("processesLogger is null", true);
        return 3;
    }

    var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
    if (httpClientFactory is null)
    {
        StShared.WriteErrorLine("httpClientFactory is null", true);
        return 6;
    }

    var processes = new Processes(processesLogger);

    var apAgent = new ApAgentCliAppLoop(logger, httpClientFactory, new ParametersManager(parametersFileName, par, key),
        processes);
    return (await apAgent.Run()) ? 0 : 1;
}
catch (Exception e)
{
    StShared.WriteException(e, true, logger);
    return 7;
}
finally
{
    await Log.CloseAndFlushAsync();
}
