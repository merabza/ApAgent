using System;
using System.Collections.Generic;
using System.Net.Http;
using ApAgent.Counters;
using ApAgent.Cruders;
using ApAgent.FieldEditors;
using CliParameters;
using CliParameters.FieldEditors;
using CliParametersEdit.FieldEditors;
using CliParametersExcludeSetsEdit.FieldEditors;
using LibApAgentData.Steps;
using LibParameters;
using LibToolActions.BackgroundTasks;
using Microsoft.Extensions.Logging;
using SystemToolsShared;

namespace ApAgent.StepCruders;

public sealed class FilesBackupStepCruder : StepCruder<FilesBackupStep>
{
    public FilesBackupStepCruder(ILogger logger, IHttpClientFactory httpClientFactory, Processes processes,
        ParametersManager parametersManager, Dictionary<string, FilesBackupStep> currentValuesDictionary) : base(logger,
        httpClientFactory, processes, parametersManager, currentValuesDictionary, "Files Backup Step",
        "Files Backup Steps")
    {
        var parametersFileName = parametersManager.ParametersFileName;
        DateMaskCounter dateMaskCounter = new();
        var dateMask = dateMaskCounter.Count();

        List<FieldEditor> tempFieldEditors = [];
        tempFieldEditors.AddRange(FieldEditors);
        FieldEditors.Clear();

        FieldEditors.Add(new TextFieldEditor(nameof(FilesBackupStep.MaskName),
            $"{Environment.MachineName.Capitalize()}_"));
        FieldEditors.Add(new TextFieldEditor(nameof(FilesBackupStep.DateMask), dateMask));
        FieldEditors.Add(new LocalPathFieldEditor(nameof(FilesBackupStep.LocalPath), ParametersManager, null,
            parametersFileName));
        FieldEditors.Add(new ArchiverFieldEditor(nameof(FilesBackupStep.ArchiverName), ParametersManager));
        FieldEditors.Add(
            new SmartSchemaNameFieldEditor(nameof(FilesBackupStep.LocalSmartSchemaName), ParametersManager));
        FieldEditors.Add(new FileStorageNameFieldEditor(logger, nameof(FilesBackupStep.UploadFileStorageName),
            ParametersManager));
        FieldEditors.Add(new IntFieldEditor(nameof(FilesBackupStep.UploadProcLineId), 1));
        FieldEditors.Add(new SmartSchemaNameFieldEditor(nameof(FilesBackupStep.UploadSmartSchemaName),
            ParametersManager));
        FieldEditors.Add(new BoolFieldEditor(nameof(FilesBackupStep.BackupSeparately), true));
        FieldEditors.Add(new ExcludeSetNameFieldEditor(nameof(FilesBackupStep.ExcludeSetName), ParametersManager));
        //FieldEditors.Add(new BackupFolderPathsFieldEditor(nameof(FilesBackupStep.BackupFolderPaths),
        //    ParametersManager));

        FieldEditors.Add(
            new SimpleNamesWithDescriptionsFieldEditor<FileBackupFolderCruder>(
                nameof(FilesBackupStep.BackupFolderPaths)));

        FieldEditors.AddRange(tempFieldEditors);
    }

    //protected override Dictionary<string, ItemData> GetCrudersDictionary()
    //{
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    return parameters.FilesBackupSteps.ToDictionary(p => p.Key, p => (ItemData)p.Value);
    //}

    //protected override void AddRecordWithKey(string recordName, ItemData newRecord)
    //{
    //    var newFilesBackupStep = (FilesBackupStep)newRecord;
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    parameters.FilesBackupSteps.Add(recordName, newFilesBackupStep);
    //}

    //protected override void RemoveRecordWithKey(string recordKey)
    //{
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    var filesBackupSteps = parameters.FilesBackupSteps;
    //    filesBackupSteps.Remove(recordKey);
    //}

    //public override void UpdateRecordWithKey(string recordName, ItemData newRecord)
    //{
    //    var newFilesBackupStep = (FilesBackupStep)newRecord;
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    parameters.FilesBackupSteps[recordName] = newFilesBackupStep;
    //}

    //protected override ItemData CreateNewItem(string? recordKey, ItemData? defaultItemData)
    //{
    //    return new FilesBackupStep();
    //}
}