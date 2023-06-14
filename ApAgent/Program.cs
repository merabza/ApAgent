using System;
using ApAgent;
using CliParameters;
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

    //პროგრამის ატრიბუტების დაყენება 
    StatProgAttr.SetAttr();

    var key = ProgramAttributes.Instance.GetAttribute<string>("AppKey") + Environment.MachineName.Capitalize();

    IArgumentsParser argParser = new ArgumentsParser<ApAgentParameters>(args, "ApAgent", key);
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
    ServicesCreator servicesCreator = new(par.LogFolder, null, "ApAgent");
    var serviceProvider = servicesCreator.CreateServiceProvider(LogEventLevel.Information);

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

    var processes = new Processes(processesLogger);

    ApAgent.ApAgent apAgent = new(logger, new ParametersManager(parametersFileName, par, key), processes);
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