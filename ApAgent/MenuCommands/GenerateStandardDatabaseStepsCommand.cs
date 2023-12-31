﻿using System;
using ApAgent.Generators;
using CliMenu;
using CliParametersDataEdit.Cruders;
using LibApAgentData.Models;
using LibDataInput;
using LibParameters;
using Microsoft.Extensions.Logging;
using SystemToolsShared;

namespace ApAgent.MenuCommands;

public sealed class GenerateStandardDatabaseStepsCommand : CliMenuCommand
{
    private readonly ILogger _logger;
    private readonly ParametersManager _parametersManager;

    public GenerateStandardDatabaseStepsCommand(ILogger logger, ParametersManager parametersManager) : base(
        "Generate Standard Database Jobs...")
    {
        _parametersManager = parametersManager;
        _logger = logger;
    }


    protected override void RunAction()
    {
        try
        {
            MenuAction = EMenuAction.Reload;

            DatabaseServerConnectionCruder databaseServerConnectionCruder = new(_parametersManager, _logger);

            var databaseConnectionName =
                databaseServerConnectionCruder.GetNameWithPossibleNewName(
                    "Select local connection for Generate standard database maintenance schema", null);

            if (string.IsNullOrWhiteSpace(databaseConnectionName))
            {
                StShared.WriteErrorLine("databaseConnectionName does not specified", true);
                return;
            }

            var parameters = (ApAgentParameters)_parametersManager.Parameters;

            StandardJobsSchemaGenerator standardJobsSchemaGenerator =
                new(true, _logger, _parametersManager, databaseConnectionName, _parametersManager.ParametersFileName);
            standardJobsSchemaGenerator.Generate();


            //შენახვა
            _parametersManager.Save(parameters, "Maintain schema generated success");
        }
        catch (DataInputEscapeException)
        {
            Console.WriteLine();
            Console.WriteLine("Escape... ");
            StShared.Pause();
        }
        catch (Exception e)
        {
            StShared.WriteException(e, true);
        }
    }
}