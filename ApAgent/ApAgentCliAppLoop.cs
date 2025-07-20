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

public sealed class ApAgentCliAppLoop : CliAppLoop
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;
    private readonly ParametersManager _parametersManager;
    private readonly Processes _processes;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ApAgentCliAppLoop(ILogger logger, IHttpClientFactory httpClientFactory, ParametersManager parametersManager,
        Processes processes) : base(null, null, processes)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _parametersManager = parametersManager;
        _processes = processes;
    }

    public override CliMenuSet BuildMainMenu()
    {
        var parameters = (ApAgentParameters)_parametersManager.Parameters;

        var mainMenuSet = new CliMenuSet("Main Menu");

        //ძირითადი პარამეტრების რედაქტირება
        var apAgentParametersEditor =
            new ApAgentParametersEditor(_logger, _httpClientFactory, parameters, _parametersManager);
        mainMenuSet.AddMenuItem(new ParametersEditorListCliMenuCommand(apAgentParametersEditor));

        //მთლიანი ფაილის გასუფთავება
        var saveApAgentParametersCommand = new SaveApAgentParametersForLocalReServerCommand(_parametersManager);
        mainMenuSet.AddMenuItem(saveApAgentParametersCommand);

        //სამუშაოების დროის დაგეგმვების სია
        var jobSchedulesCommand = new CruderListCliMenuCommand(new JobScheduleCruder(_logger, _httpClientFactory,
            _parametersManager, parameters.JobSchedules, _processes));
        mainMenuSet.AddMenuItem(jobSchedulesCommand);

        //მონაცემთა ბაზების ბექაპირების ნაბიჯების სია
        var databaseBackupStepCommand = new CruderListCliMenuCommand(new DatabaseBackupStepCruder(_logger,
            _httpClientFactory, _processes, _parametersManager, parameters.DatabaseBackupSteps));
        mainMenuSet.AddMenuItem(databaseBackupStepCommand);

        //რამდენიმე ბაზის დამუშავების ბრძანებების სია
        var multiDatabaseProcessStepCommand = new CruderListCliMenuCommand(new MultiDatabaseProcessStepCruder(_logger,
            _httpClientFactory, _processes, _parametersManager, parameters.MultiDatabaseProcessSteps));
        mainMenuSet.AddMenuItem(multiDatabaseProcessStepCommand);

        //პროგრამის გაშვების ნაბიჯების სია
        var runProgramStepCommand = new CruderListCliMenuCommand(new RunProgramStepCruder(_logger, _httpClientFactory,
            _processes, _parametersManager, parameters.RunProgramSteps));
        mainMenuSet.AddMenuItem(runProgramStepCommand);

        //მონაცემთა ბაზების მხარეს გასაშვები ბრძანებების ნაბიჯების სია
        var executeSqlCommandStepCommand = new CruderListCliMenuCommand(new ExecuteSqlCommandStepCruder(_logger,
            _httpClientFactory, _processes, _parametersManager, parameters.ExecuteSqlCommandSteps));
        mainMenuSet.AddMenuItem(executeSqlCommandStepCommand);

        //ფაილების დაბეკაპების ნაბიჯების სია
        var filesBackupStepCommand = new CruderListCliMenuCommand(new FilesBackupStepCruder(_logger, _httpClientFactory,
            _processes, _parametersManager, parameters.FilesBackupSteps));
        mainMenuSet.AddMenuItem(filesBackupStepCommand);

        //ფაილების დასინქრონიზების ნაბიჯების სია
        var filesSyncStepCommand = new CruderListCliMenuCommand(new FilesSyncStepCruder(_logger, _httpClientFactory,
            _processes, _parametersManager, parameters.FilesSyncSteps));
        mainMenuSet.AddMenuItem(filesSyncStepCommand);

        //ფაილების გადაადგილების ნაბიჯების სია
        var filesMoveStepCommand = new CruderListCliMenuCommand(new FilesMoveStepCruder(_logger, _httpClientFactory,
            _processes, _parametersManager, parameters.FilesMoveSteps));
        mainMenuSet.AddMenuItem(filesMoveStepCommand);

        //ფაილების გადაადგილების ნაბიჯების სია
        var unZipOnPlaceStepCommand = new CruderListCliMenuCommand(new UnZipOnPlaceStepCruder(_logger,
            _httpClientFactory, _processes, _parametersManager, parameters.UnZipOnPlaceSteps));
        mainMenuSet.AddMenuItem(unZipOnPlaceStepCommand);

        //ნაბიჯების გასუფთავება
        var generateStandardDatabaseStepsCommand =
            new GenerateStandardDatabaseStepsCommand(_logger, _parametersManager);
        mainMenuSet.AddMenuItem(generateStandardDatabaseStepsCommand);

        //ნაბიჯების გასუფთავება
        var clearStepsCommand = new ClearStepsCommand(_parametersManager);
        mainMenuSet.AddMenuItem(clearStepsCommand);

        //მთლიანი ფაილის გასუფთავება
        var clearAllCommand = new ClearAllCommand(_parametersManager);
        mainMenuSet.AddMenuItem(clearAllCommand);

        //გასასვლელი
        var key = ConsoleKey.Escape.Value().ToLower();
        mainMenuSet.AddMenuItem(key, new ExitCliMenuCommand(), key.Length);

        return mainMenuSet;
    }
}