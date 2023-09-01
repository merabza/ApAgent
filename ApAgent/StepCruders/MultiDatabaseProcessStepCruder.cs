using System.Collections.Generic;
using System.Linq;
using ApAgent.FieldEditors;
using CliParameters.FieldEditors;
using CliParametersApiClientsEdit.FieldEditors;
using CliParametersDataEdit.FieldEditors;
using LibApAgentData.Models;
using LibApAgentData.Steps;
using LibParameters;
using LibToolActions.BackgroundTasks;
using Microsoft.Extensions.Logging;

namespace ApAgent.StepCruders;

public sealed class MultiDatabaseProcessStepCruder : StepCruder
{
    public MultiDatabaseProcessStepCruder(ILogger logger, Processes processes,
        ParametersManager parametersManager) :
        base(logger, processes, parametersManager, "Multi Database process Step", "Multi Database process Steps")
    {
        List<FieldEditor> tempFieldEditors = new();
        tempFieldEditors.AddRange(FieldEditors);
        FieldEditors.Clear();

        FieldEditors.Add(new EnumFieldEditor<EMultiDatabaseActionType>(nameof(MultiDatabaseProcessStep.ActionType),
            EMultiDatabaseActionType.CheckRepairDataBase));

        //public string DatabaseServerConnectionName { get; set; }
        FieldEditors.Add(new DatabaseServerConnectionNameFieldEditor(logger,
            nameof(MultiDatabaseProcessStep.DatabaseServerConnectionName), ParametersManager, true));
        //public string DatabaseWebAgentName { get; set; }
        FieldEditors.Add(new ApiClientNameFieldEditor(logger, nameof(MultiDatabaseProcessStep.DatabaseWebAgentName),
            ParametersManager, true));

        FieldEditors.Add(
            new EnumFieldEditor<EDatabaseSet>(nameof(MultiDatabaseProcessStep.DatabaseSet),
                EDatabaseSet.AllDatabases));
        FieldEditors.Add(new DatabaseNamesFieldEditor(logger, nameof(MultiDatabaseProcessStep.DatabaseNames),
            ParametersManager, nameof(MultiDatabaseProcessStep.DatabaseServerConnectionName),
            nameof(MultiDatabaseProcessStep.DatabaseWebAgentName),
            nameof(MultiDatabaseProcessStep.DatabaseServerName),
            nameof(MultiDatabaseProcessStep.DatabaseSet)));

        FieldEditors.AddRange(tempFieldEditors);
    }

    protected override Dictionary<string, ItemData> GetCrudersDictionary()
    {
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        return parameters.MultiDatabaseProcessSteps.ToDictionary(p => p.Key, p => (ItemData)p.Value);
    }

    public override void UpdateRecordWithKey(string recordName, ItemData newRecord)
    {
        var newMultiDatabaseProcessStep = (MultiDatabaseProcessStep)newRecord;
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        parameters.MultiDatabaseProcessSteps[recordName] = newMultiDatabaseProcessStep;
    }

    protected override void AddRecordWithKey(string recordName, ItemData newRecord)
    {
        var newMultiDatabaseProcessStep = (MultiDatabaseProcessStep)newRecord;
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        parameters.MultiDatabaseProcessSteps.Add(recordName, newMultiDatabaseProcessStep);
    }

    protected override void RemoveRecordWithKey(string recordKey)
    {
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        var multiDatabaseProcessSteps = parameters.MultiDatabaseProcessSteps;
        multiDatabaseProcessSteps.Remove(recordKey);
    }

    protected override ItemData CreateNewItem(ItemData? defaultItemData)
    {
        return new MultiDatabaseProcessStep();
    }
}