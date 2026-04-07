using System.Collections.Generic;
using System.Net.Http;
using ApAgent.FieldEditors;
using ApAgentData.LibApAgentData.Steps;
using AppCliTools.CliParameters.FieldEditors;
using AppCliTools.CliParametersEdit.FieldEditors;
using AppCliTools.CliParametersExcludeSetsEdit.FieldEditors;
using Microsoft.Extensions.Logging;
using ParametersManagement.LibParameters;
using SystemTools.BackgroundTasks;

namespace ApAgent.StepCruders;

public sealed class FilesMoveStepCruder : StepCruder<FilesMoveStep>
{
    public FilesMoveStepCruder(ILogger logger, IHttpClientFactory httpClientFactory, Processes processes,
        ParametersManager parametersManager, Dictionary<string, FilesMoveStep> currentValuesDictionary) : base(logger,
        httpClientFactory, processes, parametersManager, currentValuesDictionary, "Files Move Step", "Files Move Steps")
    {
        List<FieldEditor> tempFieldEditors = [.. FieldEditors];
        FieldEditors.Clear();

        FieldEditors.Add(new FileStorageNameFieldEditor(logger, nameof(FilesMoveStep.SourceFileStorageName),
            ParametersManager));
        FieldEditors.Add(new FileStorageNameFieldEditor(logger, nameof(FilesMoveStep.DestinationFileStorageName),
            ParametersManager));
        FieldEditors.Add(new TextFieldEditor(nameof(FilesMoveStep.MoveFolderMask), "yyyyMMddHHmmss"));
        FieldEditors.Add(new ExcludeSetNameFieldEditor(nameof(FilesMoveStep.ExcludeSet), parametersManager, true));
        FieldEditors.Add(new ExcludeSetNameFieldEditor(nameof(FilesMoveStep.DeleteDestinationFilesSet),
            ParametersManager, true));
        FieldEditors.Add(
            new ReplacePairsSetNameFieldEditor(nameof(FilesMoveStep.ReplacePairsSet), parametersManager, true));
        FieldEditors.Add(new IntFieldEditor(nameof(FilesMoveStep.MaxFolderCount), 2));
        FieldEditors.Add(new BoolFieldEditor(nameof(FilesMoveStep.CreateFolderWithDateTime), true));
        FieldEditors.Add(new FolderPathsSetFieldEditor(nameof(FilesMoveStep.PriorityPoints)));

        FieldEditors.AddRange(tempFieldEditors);
    }

    //protected override Dictionary<string, ItemData> GetCrudersDictionary()
    //{
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    return parameters.FilesMoveSteps.ToDictionary(p => p.Key, p => (ItemData)p.Value);
    //}

    //protected override void AddRecordWithKey(string recordName, ItemData newRecord)
    //{
    //    var newFilesMoveStep = (FilesMoveStep)newRecord;
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    parameters.FilesMoveSteps.Add(recordName, newFilesMoveStep);
    //}

    //protected override void RemoveRecordWithKey(string recordKey)
    //{
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    var filesMoveSteps = parameters.FilesMoveSteps;
    //    filesMoveSteps.Remove(recordKey);
    //}

    //public override void UpdateRecordWithKey(string recordName, ItemData newRecord)
    //{
    //    var newFilesMoveStep = (FilesMoveStep)newRecord;
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    parameters.FilesMoveSteps[recordName] = newFilesMoveStep;
    //}

    //protected override ItemData CreateNewItem(string? recordKey, ItemData? defaultItemData)
    //{
    //    return new FilesMoveStep();
    //}
}
