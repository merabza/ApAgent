using System.Collections.Generic;
using System.Linq;
using CliParameters.FieldEditors;
using LibApAgentData.Models;
using LibApAgentData.Steps;
using LibParameters;
using LibToolActions.BackgroundTasks;
using Microsoft.Extensions.Logging;

namespace ApAgent.StepCruders;

public sealed class RunProgramStepCruder : StepCruder
{
    public RunProgramStepCruder(ILogger logger, Processes processes, ParametersManager parametersManager) : base(
        logger,
        processes, parametersManager, "Run Program Step", "Run Program Steps")
    {
        List<FieldEditor> tempFieldEditors = new();
        tempFieldEditors.AddRange(FieldEditors);
        FieldEditors.Clear();

        FieldEditors.Add(new TextFieldEditor(nameof(RunProgramStep.Program)));
        FieldEditors.Add(new TextFieldEditor(nameof(RunProgramStep.Arguments)));

        FieldEditors.AddRange(tempFieldEditors);
    }

    protected override Dictionary<string, ItemData> GetCrudersDictionary()
    {
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        return parameters.RunProgramSteps.ToDictionary(p => p.Key, p => (ItemData)p.Value);
    }

    public override void UpdateRecordWithKey(string recordName, ItemData newRecord)
    {
        var newRunProgramStep = (RunProgramStep)newRecord;
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        parameters.RunProgramSteps[recordName] = newRunProgramStep;
    }

    protected override void AddRecordWithKey(string recordName, ItemData newRecord)
    {
        var newRunProgramStep = (RunProgramStep)newRecord;
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        parameters.RunProgramSteps.Add(recordName, newRunProgramStep);
    }

    protected override void RemoveRecordWithKey(string recordKey)
    {
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        var runProgramSteps = parameters.RunProgramSteps;
        runProgramSteps.Remove(recordKey);
    }

    protected override ItemData CreateNewItem(ItemData? defaultItemData)
    {
        return new RunProgramStep();
    }
}