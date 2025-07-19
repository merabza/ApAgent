using System.Collections.Generic;
using System.Net.Http;
using ApAgent.FieldEditors;
using CliParameters.FieldEditors;
using CliParametersEdit.FieldEditors;
using CliParametersExcludeSetsEdit.FieldEditors;
using LibApAgentData.Steps;
using LibParameters;
using LibToolActions.BackgroundTasks;
using Microsoft.Extensions.Logging;

namespace ApAgent.StepCruders;

public sealed class FilesSyncStepCruder : StepCruder<FilesSyncStep>
{
    public FilesSyncStepCruder(ILogger logger, IHttpClientFactory httpClientFactory, Processes processes,
        ParametersManager parametersManager, Dictionary<string, FilesSyncStep> currentValuesDictionary) : base(logger,
        httpClientFactory, processes, parametersManager, currentValuesDictionary, "Files Sync Step", "Files Sync Steps")
    {
        List<FieldEditor> tempFieldEditors = [.. FieldEditors];
        FieldEditors.Clear();

        FieldEditors.Add(new FileStorageNameFieldEditor(logger, nameof(FilesSyncStep.SourceFileStorageName),
            ParametersManager));
        FieldEditors.Add(new FileStorageNameFieldEditor(logger, nameof(FilesSyncStep.DestinationFileStorageName),
            ParametersManager));
        FieldEditors.Add(new ExcludeSetNameFieldEditor(nameof(FilesSyncStep.ExcludeSet), ParametersManager, true));
        FieldEditors.Add(new ExcludeSetNameFieldEditor(nameof(FilesSyncStep.DeleteDestinationFilesSet),
            ParametersManager, true));
        FieldEditors.Add(new ReplacePairsSetNameFieldEditor(nameof(FilesSyncStep.ReplacePairsSet),
            ParametersManager, true));

        FieldEditors.AddRange(tempFieldEditors);
    }

    //protected override Dictionary<string, ItemData> GetCrudersDictionary()
    //{
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    return parameters.FilesSyncSteps.ToDictionary(p => p.Key, p => (ItemData)p.Value);
    //}

    //protected override void AddRecordWithKey(string recordName, ItemData newRecord)
    //{
    //    var newFilesSyncStep = (FilesSyncStep)newRecord;
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    parameters.FilesSyncSteps.Add(recordName, newFilesSyncStep);
    //}

    //protected override void RemoveRecordWithKey(string recordKey)
    //{
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    var filesSyncSteps = parameters.FilesSyncSteps;
    //    filesSyncSteps.Remove(recordKey);
    //}

    //public override void UpdateRecordWithKey(string recordName, ItemData newRecord)
    //{
    //    var newFilesSyncStep = (FilesSyncStep)newRecord;
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    parameters.FilesSyncSteps[recordName] = newFilesSyncStep;
    //}

    //protected override ItemData CreateNewItem(string? recordKey, ItemData? defaultItemData)
    //{
    //    return new FilesSyncStep();
    //}
}