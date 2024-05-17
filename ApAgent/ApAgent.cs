using System;
using System.Net.Http;
using ApAgent.Cruders;
using ApAgent.MenuCommands;
using ApAgent.StepCruders;
using CliMenu;
using CliParameters.CliMenuCommands;
using CliTools;
using CliTools.CliMenuCommands;
using LibApAgentData.Models;
using LibDataInput;
using LibParameters;
using LibToolActions.BackgroundTasks;
using Microsoft.Extensions.Logging;

namespace ApAgent;

public sealed class ApAgent : CliAppLoop
{
    private readonly ILogger _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ParametersManager _parametersManager;
    private readonly Processes _processes;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ApAgent(ILogger logger, IHttpClientFactory httpClientFactory, ParametersManager parametersManager,
        Processes processes) : base(null, processes)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _parametersManager = parametersManager;
        _processes = processes;
    }

    protected override void BuildMainMenu()
    {
        var parameters = (ApAgentParameters)_parametersManager.Parameters;

        //if (parameters == null)
        //{
        //    StShared.WriteErrorLine("minimal parameters not found", true, _logger);
        //    return false;
        //}

        CliMenuSet mainMenuSet = new("Main Menu");
        if (!AddChangeMenu(mainMenuSet))
            return;

        //ძირითადი პარამეტრების რედაქტირება
        ApAgentParametersEditor apAgentParametersEditor =
            new(_logger, _httpClientFactory, parameters, _parametersManager);
        mainMenuSet.AddMenuItem(new ParametersEditorListCliMenuCommand(apAgentParametersEditor),
            "ApAgent Parameters Editor");

        //მთლიანი ფაილის გასუფთავება
        SaveApAgentParametersForLocalReServerCommand saveApAgentParametersCommand = new(_parametersManager);
        mainMenuSet.AddMenuItem(saveApAgentParametersCommand);

        //სამუშაოების დროის დაგეგმვების სია
        CruderListCliMenuCommand jobSchedulesCommand =
            new(new JobScheduleCruder(_logger, _httpClientFactory, _parametersManager, _processes));
        mainMenuSet.AddMenuItem(jobSchedulesCommand);

        //მონაცემთა ბაზების ბექაპირების ნაბიჯების სია
        CruderListCliMenuCommand databaseBackupStepCommand =
            new(new DatabaseBackupStepCruder(_logger, _httpClientFactory, _processes, _parametersManager));
        mainMenuSet.AddMenuItem(databaseBackupStepCommand);

        //რამდენიმე ბაზის დამუშავების ბრძანებების სია
        CruderListCliMenuCommand multiDatabaseProcessStepCommand =
            new(new MultiDatabaseProcessStepCruder(_logger, _httpClientFactory, _processes, _parametersManager));
        mainMenuSet.AddMenuItem(multiDatabaseProcessStepCommand);

        //პროგრამის გაშვების ნაბიჯების სია
        CruderListCliMenuCommand runProgramStepCommand =
            new(new RunProgramStepCruder(_logger, _httpClientFactory, _processes, _parametersManager));
        mainMenuSet.AddMenuItem(runProgramStepCommand);

        //მონაცემთა ბაზების მხარეს გასაშვები ბრძანებების ნაბიჯების სია
        CruderListCliMenuCommand executeSqlCommandStepCommand =
            new(new ExecuteSqlCommandStepCruder(_logger, _httpClientFactory, _processes, _parametersManager));
        mainMenuSet.AddMenuItem(executeSqlCommandStepCommand);

        //ფაილების დაბეკაპების ნაბიჯების სია
        CruderListCliMenuCommand filesBackupStepCommand =
            new(new FilesBackupStepCruder(_logger, _httpClientFactory, _processes, _parametersManager));
        mainMenuSet.AddMenuItem(filesBackupStepCommand);

        //ფაილების დასინქრონიზების ნაბიჯების სია
        CruderListCliMenuCommand filesSyncStepCommand =
            new(new FilesSyncStepCruder(_logger, _httpClientFactory, _processes, _parametersManager));
        mainMenuSet.AddMenuItem(filesSyncStepCommand);

        //ფაილების გადაადგილების ნაბიჯების სია
        CruderListCliMenuCommand filesMoveStepCommand =
            new(new FilesMoveStepCruder(_logger, _httpClientFactory, _processes, _parametersManager));
        mainMenuSet.AddMenuItem(filesMoveStepCommand);

        //ფაილების გადაადგილების ნაბიჯების სია
        CruderListCliMenuCommand unZipOnPlaceStepCommand =
            new(new UnZipOnPlaceStepCruder(_logger, _httpClientFactory, _processes, _parametersManager));
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
        mainMenuSet.AddMenuItem(key, "Exit", new ExitCliMenuCommand(), key.Length);
    }
}