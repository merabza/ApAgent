using System.Collections.Generic;
using System.Linq;
using CliParameters.FieldEditors;
using LibApAgentData.Models;
using LibApAgentData.Steps;
using LibParameters;
using LibToolActions.BackgroundTasks;
using Microsoft.Extensions.Logging;

namespace ApAgent.StepCruders;

public sealed class UnZipOnPlaceStepCruder : StepCruder
{
    public UnZipOnPlaceStepCruder(ILogger logger, Processes processes, ParametersManager parametersManager) : base(
        logger, processes, parametersManager, "Unzip On Place Step", "Unzip On Place Steps")
    {
        List<FieldEditor> tempFieldEditors = new();
        tempFieldEditors.AddRange(FieldEditors);
        FieldEditors.Clear();

        FieldEditors.Add(new FolderPathFieldEditor(nameof(UnZipOnPlaceStep.PathWithZips)));
        FieldEditors.Add(new BoolFieldEditor(nameof(UnZipOnPlaceStep.WithSubFolders), true));

        FieldEditors.AddRange(tempFieldEditors);
    }

    protected override Dictionary<string, ItemData> GetCrudersDictionary()
    {
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        return parameters.UnZipOnPlaceSteps.ToDictionary(p => p.Key, p => (ItemData)p.Value);
    }

    protected override void AddRecordWithKey(string recordName, ItemData newRecord)
    {
        var newUnZipOnPlaceStep = (UnZipOnPlaceStep)newRecord;
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        parameters.UnZipOnPlaceSteps.Add(recordName, newUnZipOnPlaceStep);
    }

    protected override void RemoveRecordWithKey(string recordKey)
    {
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        var unZipOnPlaceSteps = parameters.UnZipOnPlaceSteps;
        unZipOnPlaceSteps.Remove(recordKey);
    }

    public override void UpdateRecordWithKey(string recordName, ItemData newRecord)
    {
        var newUnZipOnPlaceStep = (UnZipOnPlaceStep)newRecord;
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        parameters.UnZipOnPlaceSteps[recordName] = newUnZipOnPlaceStep;
    }

    protected override ItemData CreateNewItem(string recordName, ItemData? defaultItemData)
    {
        return new UnZipOnPlaceStep();
    }
}