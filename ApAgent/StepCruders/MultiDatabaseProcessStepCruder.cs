using System.Collections.Generic;
using System.Net.Http;
using ApAgent.FieldEditors;
using ApAgentData.LibApAgentData.Models;
using ApAgentData.LibApAgentData.Steps;
using AppCliTools.CliParameters.FieldEditors;
using AppCliTools.CliParametersApiClientsEdit.FieldEditors;
using AppCliTools.CliParametersDataEdit.FieldEditors;
using Microsoft.Extensions.Logging;
using ParametersManagement.LibParameters;
using SystemTools.BackgroundTasks;

namespace ApAgent.StepCruders;

public sealed class MultiDatabaseProcessStepCruder : StepCruder<MultiDatabaseProcessStep>
{
    public MultiDatabaseProcessStepCruder(ILogger logger, IHttpClientFactory httpClientFactory, Processes processes,
        ParametersManager parametersManager,
        Dictionary<string, MultiDatabaseProcessStep> currentValuesDictionary) : base(logger, httpClientFactory,
        processes, parametersManager, currentValuesDictionary, "Multi Database process Step",
        "Multi Database process Steps")
    {
        List<FieldEditor> tempFieldEditors = [.. FieldEditors];
        FieldEditors.Clear();

        FieldEditors.Add(new EnumFieldEditor<EMultiDatabaseActionType>(nameof(MultiDatabaseProcessStep.ActionType),
            EMultiDatabaseActionType.CheckRepairDataBase));

        //public string DatabaseServerConnectionName { get; set; }
        FieldEditors.Add(new DatabaseServerConnectionNameFieldEditor(logger, httpClientFactory,
            nameof(MultiDatabaseProcessStep.DatabaseServerConnectionName), ParametersManager, true));
        //public string DatabaseWebAgentName { get; set; }
        FieldEditors.Add(new ApiClientNameFieldEditor(nameof(MultiDatabaseProcessStep.DatabaseWebAgentName), logger,
            httpClientFactory, ParametersManager, true));

        FieldEditors.Add(new EnumFieldEditor<EDatabaseSet>(nameof(MultiDatabaseProcessStep.DatabaseSet),
            EDatabaseSet.AllDatabases));
        FieldEditors.Add(new DatabaseNamesFieldEditor(logger, httpClientFactory,
            nameof(MultiDatabaseProcessStep.DatabaseNames), ParametersManager,
            nameof(MultiDatabaseProcessStep.DatabaseServerConnectionName),
            nameof(MultiDatabaseProcessStep.DatabaseSet)));

        FieldEditors.AddRange(tempFieldEditors);
    }

    //protected override Dictionary<string, ItemData> GetCrudersDictionary()
    //{
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    return parameters.MultiDatabaseProcessSteps.ToDictionary(p => p.Key, p => (ItemData)p.Value);
    //}

    //public override void UpdateRecordWithKey(string recordName, ItemData newRecord)
    //{
    //    var newMultiDatabaseProcessStep = (MultiDatabaseProcessStep)newRecord;
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    parameters.MultiDatabaseProcessSteps[recordName] = newMultiDatabaseProcessStep;
    //}

    //protected override void AddRecordWithKey(string recordName, ItemData newRecord)
    //{
    //    var newMultiDatabaseProcessStep = (MultiDatabaseProcessStep)newRecord;
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    parameters.MultiDatabaseProcessSteps.Add(recordName, newMultiDatabaseProcessStep);
    //}

    //protected override void RemoveRecordWithKey(string recordKey)
    //{
    //    var parameters = (ApAgentParameters)ParametersManager.Parameters;
    //    var multiDatabaseProcessSteps = parameters.MultiDatabaseProcessSteps;
    //    multiDatabaseProcessSteps.Remove(recordKey);
    //}

    //protected override ItemData CreateNewItem(string? recordKey, ItemData? defaultItemData)
    //{
    //    return new MultiDatabaseProcessStep();
    //}
}
