using System;
using System.Net.Http;
using ApAgent;
using CliParameters;
using LibApAgentData;
using LibApAgentData.Models;
using LibParameters;
using LibToolActions.BackgroundTasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using SystemToolsShared;

ILogger<Program>? logger = null;
try
{
    Console.WriteLine("Loading...");

    const string appName = "ApAgent";

    //პროგრამის ატრიბუტების დაყენება 
    ProgramAttributes.Instance.AppName = appName;

    var key = StringExtension.AppAgentAppKey + Environment.MachineName.Capitalize();

    var argParser = new ArgumentsParser<ApAgentParameters>(args, appName, key);
    switch (argParser.Analysis())
    {
        case EParseResult.Ok: break;
        case EParseResult.Usage: return 1;
        case EParseResult.Error: return 2;
        default: throw new ArgumentOutOfRangeException();
    }

    var par = (ApAgentParameters?)argParser.Par;
    if (par is null)
    {
        StShared.WriteErrorLine("ApAgentParameters is null", true);
        return 3;
    }

    var parametersFileName = argParser.ParametersFileName;
    var apAgentServicesCreator = new ApAgentServicesCreator(par);

    // ReSharper disable once using
    using var serviceProvider = apAgentServicesCreator.CreateServiceProvider(LogEventLevel.Information);

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

    var apAgent = new ApAgent.ApAgent(logger, httpClientFactory, new ParametersManager(parametersFileName, par, key),
        processes);
    return apAgent.Run() ? 0 : 1;
}
catch (Exception e)
{
    StShared.WriteException(e, true, logger);
    return 7;
}
finally
{
    Log.CloseAndFlush();
}