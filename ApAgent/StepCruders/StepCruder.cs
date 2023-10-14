using System;
using System.Linq;
using ApAgent.FieldEditors;
using ApAgent.MenuCommands;
using CliMenu;
using CliParameters;
using CliParameters.FieldEditors;
using LibApAgentData.Models;
using LibApAgentData.Steps;
using LibParameters;
using LibToolActions.BackgroundTasks;
using Microsoft.Extensions.Logging;
using SystemToolsShared;

namespace ApAgent.StepCruders;

public /*open*/ class StepCruder : ParCruder
{
    private readonly ILogger _logger;
    private readonly string? _parametersFileName;
    private readonly Processes _processes;

    protected StepCruder(ILogger logger, Processes processes, ParametersManager parametersManager, string crudName,
        string crudNamePlural) : base(parametersManager, crudName, crudNamePlural)
    {
        _parametersFileName = parametersManager.ParametersFileName;
        _logger = logger;
        _processes = processes;
        //რიგითი ნომერი უნდა დგინდება არსებულ ნომრებში მაქსიმუმს 1-ით მეტი, ან არსებული ნაბიჯების რაოდენობაზე 1-ით მეტი. (მაქსიმუმი ამ 2 რიცხვს შორის)
        FieldEditors.Add(new IntFieldEditor(nameof(JobStep.ProcLineId), 1));
        FieldEditors.Add(new IntFieldEditor(nameof(JobStep.DelayMinutesBeforeStep)));
        FieldEditors.Add(new IntFieldEditor(nameof(JobStep.DelayMinutesAfterStep)));
        FieldEditors.Add(new TimeSpanFieldEditor(nameof(JobStep.HoleStartTime), new TimeSpan(0, 0, 0)));
        FieldEditors.Add(new TimeSpanFieldEditor(nameof(JobStep.HoleEndTime), new TimeSpan(23, 59, 59)));
        FieldEditors.Add(new EnumFieldEditor<EPeriodType>(nameof(JobStep.PeriodType), EPeriodType.Day));
        FieldEditors.Add(new IntFieldEditor(nameof(JobStep.FreqInterval), 1));
        FieldEditors.Add(new DateTimeFieldEditor(nameof(JobStep.StartAt), DateTime.Today));
        FieldEditors.Add(new BoolFieldEditor(nameof(JobStep.Enabled), true));
    }

    protected override void FillDetailsSubMenu(CliMenuSet itemSubMenuSet, string recordName)
    {
        base.FillDetailsSubMenu(itemSubMenuSet, recordName);
        RunThisStepNowCommand runThisStepNowCommand =
            new(_logger, _processes, ParametersManager, this, recordName, _parametersFileName);
        itemSubMenuSet.AddMenuItem(runThisStepNowCommand, "Run this step now...");

        var parameters = (ApAgentParameters)ParametersManager.Parameters;

        //if (parameters == null)
        //    return;

        var scheduleNamesList = parameters.JobsBySchedules.Where(w => w.JobStepName == recordName)
            .Select(s => s.ScheduleName).ToList();
        foreach (var kvp in parameters.JobSchedules)
            itemSubMenuSet.AddMenuItem(
                new SelectScheduleNamesCommand(ParametersManager, recordName, kvp.Key,
                    scheduleNamesList.Contains(kvp.Key)),
                $"{(scheduleNamesList.Contains(kvp.Key) ? "√" : "×")} {kvp.Key}");
    }

    public override bool ContainsRecordWithKey(string recordKey)
    {
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        var steps = parameters.GetSteps();
        return steps.ContainsKey(recordKey);
    }
}