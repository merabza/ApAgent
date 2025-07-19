using System.Collections.Generic;
using System.Net.Http;
using CliParameters.FieldEditors;
using LibApAgentData.Steps;
using LibParameters;
using LibToolActions.BackgroundTasks;
using Microsoft.Extensions.Logging;

namespace ApAgent.StepCruders;

public sealed class RunProgramStepCruder : StepCruder<RunProgramStep>
{
    public RunProgramStepCruder(ILogger logger, IHttpClientFactory httpClientFactory, Processes processes,
        ParametersManager parametersManager, Dictionary<string, RunProgramStep> currentValuesDictionary) : base(logger,
        httpClientFactory, processes, parametersManager, currentValuesDictionary, "Run Program Step",
        "Run Program Steps")
    {
        List<FieldEditor> tempFieldEditors = [.. FieldEditors];
        FieldEditors.Clear();

        FieldEditors.Add(new TextFieldEditor(nameof(RunProgramStep.Program)));
        FieldEditors.Add(new TextFieldEditor(nameof(RunProgramStep.Arguments)));

        FieldEditors.AddRange(tempFieldEditors);
    }

    //protected override Dictionary<string, ItemData> GetCrudersDictionary()
    //{
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    return parameters.RunProgramSteps.ToDictionary(p => p.Key, p => (ItemData)p.Value);
    //}

    //public override void UpdateRecordWithKey(string recordName, ItemData newRecord)
    //{
    //    var newRunProgramStep = (RunProgramStep)newRecord;
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    parameters.RunProgramSteps[recordName] = newRunProgramStep;
    //}

    //protected override void AddRecordWithKey(string recordName, ItemData newRecord)
    //{
    //    var newRunProgramStep = (RunProgramStep)newRecord;
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    parameters.RunProgramSteps.Add(recordName, newRunProgramStep);
    //}

    //protected override void RemoveRecordWithKey(string recordKey)
    //{
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    var runProgramSteps = parameters.RunProgramSteps;
    //    runProgramSteps.Remove(recordKey);
    //}

    //protected override ItemData CreateNewItem(string? recordKey, ItemData? defaultItemData)
    //{
    //    return new RunProgramStep();
    //}
}