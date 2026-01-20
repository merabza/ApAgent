using System.Collections.Generic;
using System.Net.Http;
using ApAgentData.LibApAgentData.Steps;
using AppCliTools.CliParameters.FieldEditors;
using Microsoft.Extensions.Logging;
using ParametersManagement.LibParameters;
using ToolsManagement.LibToolActions.BackgroundTasks;

namespace ApAgent.StepCruders;

public sealed class UnZipOnPlaceStepCruder : StepCruder<UnZipOnPlaceStep>
{
    public UnZipOnPlaceStepCruder(ILogger logger, IHttpClientFactory httpClientFactory, Processes processes,
        ParametersManager parametersManager, Dictionary<string, UnZipOnPlaceStep> currentValuesDictionary) : base(
        logger, httpClientFactory, processes, parametersManager, currentValuesDictionary, "Unzip On Place Step",
        "Unzip On Place Steps")
    {
        List<FieldEditor> tempFieldEditors = [];
        tempFieldEditors.AddRange(FieldEditors);
        FieldEditors.Clear();

        FieldEditors.Add(new FolderPathFieldEditor(nameof(UnZipOnPlaceStep.PathWithZips)));
        FieldEditors.Add(new BoolFieldEditor(nameof(UnZipOnPlaceStep.WithSubFolders), true));

        FieldEditors.AddRange(tempFieldEditors);
    }

    //protected override Dictionary<string, ItemData> GetCrudersDictionary()
    //{
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    return parameters.UnZipOnPlaceSteps.ToDictionary(p => p.Key, p => (ItemData)p.Value);
    //}

    //protected override void AddRecordWithKey(string recordName, ItemData newRecord)
    //{
    //    var newUnZipOnPlaceStep = (UnZipOnPlaceStep)newRecord;
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    parameters.UnZipOnPlaceSteps.Add(recordName, newUnZipOnPlaceStep);
    //}

    //protected override void RemoveRecordWithKey(string recordKey)
    //{
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    var unZipOnPlaceSteps = parameters.UnZipOnPlaceSteps;
    //    unZipOnPlaceSteps.Remove(recordKey);
    //}

    //public override void UpdateRecordWithKey(string recordName, ItemData newRecord)
    //{
    //    var newUnZipOnPlaceStep = (UnZipOnPlaceStep)newRecord;
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    parameters.UnZipOnPlaceSteps[recordName] = newUnZipOnPlaceStep;
    //}

    //protected override ItemData CreateNewItem(string? recordKey, ItemData? defaultItemData)
    //{
    //    return new UnZipOnPlaceStep();
    //}
}
