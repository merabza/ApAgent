using System;
using ApAgent.Cruders;
using ApAgent.MenuCommands;
using ApAgent.StepCruders;
using CliMenu;
using CliParameters.MenuCommands;
using CliTools;
using CliTools.Commands;
using LibApAgentData.Models;
using LibDataInput;
using LibParameters;
using LibToolActions.BackgroundTasks;
using Microsoft.Extensions.Logging;

namespace ApAgent;

public sealed class ApAgent : CliAppLoop
{
    private readonly ILogger _logger;
    private readonly ParametersManager _parametersManager;
    private readonly Processes _processes;

    public ApAgent(ILogger logger, ParametersManager parametersManager, Processes processes) : base(null, processes)
    {
        _logger = logger;
        _parametersManager = parametersManager;
        _processes = processes;
    }

    protected override bool BuildMainMenu()
    {
        var parameters = (ApAgentParameters)_parametersManager.Parameters;

        //if (parameters == null)
        //{
        //    StShared.WriteErrorLine("minimal parameters not found", true, _logger);
        //    return false;
        //}

        CliMenuSet mainMenuSet = new("Main Menu");
        if (!AddChangeMenu(mainMenuSet))
            return false;

        //ძირითადი პარამეტრების რედაქტირება
        ApAgentParametersEditor apAgentParametersEditor = new(parameters, _parametersManager, _logger);
        mainMenuSet.AddMenuItem(new ParametersEditorListCommand(apAgentParametersEditor),
            "ApAgent Parameters Editor");

        //მთლიანი ფაილის გასუფთავება
        SaveApAgentParametersForLocalReServerCommand saveApAgentParametersCommand = new(_parametersManager);
        mainMenuSet.AddMenuItem(saveApAgentParametersCommand);

        //სამუშაოების დროის დაგეგმვების სია
        CruderListCommand jobSchedulesCommand = new(new JobScheduleCruder(_logger, _parametersManager, _processes));
        mainMenuSet.AddMenuItem(jobSchedulesCommand);

        //მონაცემთა ბაზების ბექაპირების ნაბიჯების სია
        CruderListCommand databaseBackupStepCommand =
            new(new DatabaseBackupStepCruder(_logger, _processes, _parametersManager));
        mainMenuSet.AddMenuItem(databaseBackupStepCommand);

        //რამდენიმე ბაზის დამუშავების ბრძანებების სია
        CruderListCommand multiDatabaseProcessStepCommand =
            new(new MultiDatabaseProcessStepCruder(_logger, _processes, _parametersManager));
        mainMenuSet.AddMenuItem(multiDatabaseProcessStepCommand);

        //პროგრამის გაშვების ნაბიჯების სია
        CruderListCommand runProgramStepCommand =
            new(new RunProgramStepCruder(_logger, _processes, _parametersManager));
        mainMenuSet.AddMenuItem(runProgramStepCommand);

        //მონაცემთა ბაზების მხარეს გასაშვები ბრძანებების ნაბიჯების სია
        CruderListCommand executeSqlCommandStepCommand =
            new(new ExecuteSqlCommandStepCruder(_logger, _processes, _parametersManager));
        mainMenuSet.AddMenuItem(executeSqlCommandStepCommand);

        //ფაილების დაბეკაპების ნაბიჯების სია
        CruderListCommand filesBackupStepCommand =
            new(new FilesBackupStepCruder(_logger, _processes, _parametersManager));
        mainMenuSet.AddMenuItem(filesBackupStepCommand);

        //ფაილების დასინქრონიზების ნაბიჯების სია
        CruderListCommand filesSyncStepCommand = new(new FilesSyncStepCruder(_logger, _processes, _parametersManager));
        mainMenuSet.AddMenuItem(filesSyncStepCommand);

        //ფაილების გადაადგილების ნაბიჯების სია
        CruderListCommand filesMoveStepCommand = new(new FilesMoveStepCruder(_logger, _processes, _parametersManager));
        mainMenuSet.AddMenuItem(filesMoveStepCommand);

        //ფაილების გადაადგილების ნაბიჯების სია
        CruderListCommand unZipOnPlaceStepCommand =
            new(new UnZipOnPlaceStepCruder(_logger, _processes, _parametersManager));
        mainMenuSet.AddMenuItem(unZipOnPlaceStepCommand);

        //ნაბიჯების გასუფთავება
        GenerateStandardDatabaseStepsCommand generateStandardDatabaseStepsCommand = new(_logger, _parametersManager);
        mainMenuSet.AddMenuItem(generateStandardDatabaseStepsCommand);

        //ნაბიჯების გასუფთავება
        ClearStepsCommand clearStepsCommand = new(_parametersManager);
        mainMenuSet.AddMenuItem(clearStepsCommand);

        //მთლიანი ფაილის გასუფთავება
        ClearAllCommand clearAllCommand = new(_parametersManager);
        mainMenuSet.AddMenuItem(clearAllCommand);

        //გასასვლელი
        var key = ConsoleKey.Escape.Value().ToLower();
        mainMenuSet.AddMenuItem(key, "Exit", new ExitCommand(), key.Length);

        return true;
    }
}