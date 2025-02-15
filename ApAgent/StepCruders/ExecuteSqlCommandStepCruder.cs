﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

public sealed class ExecuteSqlCommandStepCruder : StepCruder
{
    public ExecuteSqlCommandStepCruder(ILogger logger, IHttpClientFactory httpClientFactory, Processes processes,
        ParametersManager parametersManager) : base(logger, httpClientFactory, processes, parametersManager,
        "Execute SQL Command Step", "Execute SQL Command Steps")
    {
        List<FieldEditor> tempFieldEditors = [..FieldEditors];

        FieldEditors.Clear();

        //public string DatabaseServerConnectionName { get; set; }
        FieldEditors.Add(new DatabaseServerConnectionNameFieldEditor(logger, httpClientFactory,
            nameof(ExecuteSqlCommandStep.DatabaseServerConnectionName), ParametersManager, true));
        //public string DatabaseWebAgentName { get; set; }
        FieldEditors.Add(new ApiClientNameFieldEditor(logger, httpClientFactory,
            nameof(ExecuteSqlCommandStep.DatabaseWebAgentName), ParametersManager, true));

        FieldEditors.Add(new OneDatabaseNameFieldEditor(logger, httpClientFactory,
            nameof(ExecuteSqlCommandStep.DatabaseName), ParametersManager,
            nameof(ExecuteSqlCommandStep.DatabaseServerConnectionName)));

        FieldEditors.Add(new TextFieldEditor(nameof(ExecuteSqlCommandStep.ExecuteQueryCommand)));
        FieldEditors.Add(new IntFieldEditor(nameof(ExecuteSqlCommandStep.CommandTimeOut), 1));
        FieldEditors.AddRange(tempFieldEditors);
    }

    protected override Dictionary<string, ItemData> GetCrudersDictionary()
    {
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        return parameters.ExecuteSqlCommandSteps.ToDictionary(p => p.Key, p => (ItemData)p.Value);
    }

    public override void UpdateRecordWithKey(string recordName, ItemData newRecord)
    {
        var newExecuteSqlCommandStep = (ExecuteSqlCommandStep)newRecord;
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        parameters.ExecuteSqlCommandSteps[recordName] = newExecuteSqlCommandStep;
    }

    protected override void AddRecordWithKey(string recordName, ItemData newRecord)
    {
        var newExecuteSqlCommandStep = (ExecuteSqlCommandStep)newRecord;
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        parameters.ExecuteSqlCommandSteps.Add(recordName, newExecuteSqlCommandStep);
    }

    protected override void RemoveRecordWithKey(string recordKey)
    {
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        var executeSqlCommandSteps = parameters.ExecuteSqlCommandSteps;
        executeSqlCommandSteps.Remove(recordKey);
    }

    protected override ItemData CreateNewItem(string? recordKey, ItemData? defaultItemData)
    {
        return new ExecuteSqlCommandStep();
    }
}