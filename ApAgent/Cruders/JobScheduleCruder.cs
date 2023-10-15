using System;
using System.Collections.Generic;
using System.Linq;
using ApAgent.FieldEditors;
using ApAgent.MenuCommands;
using CliMenu;
using CliParameters;
using CliParameters.FieldEditors;
using LibApAgentData.Models;
using LibParameters;
using LibToolActions.BackgroundTasks;
using Microsoft.Extensions.Logging;

namespace ApAgent.Cruders;

public sealed class JobScheduleCruder : ParCruder
{
    private readonly ILogger _logger;
    private readonly string? _parametersFileName;
    private readonly Processes _processes;

    public JobScheduleCruder(ILogger logger, ParametersManager parametersManager, Processes processes) : base(
        parametersManager, "Job Schedule", "Job Schedules")
    {
        _parametersFileName = parametersManager.ParametersFileName;
        _logger = logger;
        _processes = processes;

        FieldEditors.Add(new EnumFieldEditor<EScheduleType>(nameof(JobSchedule.ScheduleType), EScheduleType.Daily));
        FieldEditors.Add(new DateTimeFieldEditor(nameof(JobSchedule.RunOnceDateTime), DateTime.Today.AddDays(1)));
        FieldEditors.Add(new IntFieldEditor(nameof(JobSchedule.FreqInterval), 1));
        FieldEditors.Add(new EnumFieldEditor<EDailyFrequency>(nameof(JobSchedule.DailyFrequencyType),
            EDailyFrequency.OccursOnce));
        FieldEditors.Add(
            new EnumFieldEditor<EEveryMeasure>(nameof(JobSchedule.FreqSubDayType), EEveryMeasure.Hour));
        FieldEditors.Add(new IntFieldEditor(nameof(JobSchedule.FreqSubDayInterval), 1));
        FieldEditors.Add(new TimeSpanFieldEditor(nameof(JobSchedule.ActiveStartDayTime), new TimeSpan(0, 0, 0)));
        FieldEditors.Add(new TimeSpanFieldEditor(nameof(JobSchedule.ActiveEndDayTime), new TimeSpan(23, 59, 59)));
        FieldEditors.Add(new DateFieldEditor(nameof(JobSchedule.DurationStartDate), DateTime.Today));
        FieldEditors.Add(new DateFieldEditor(nameof(JobSchedule.DurationEndDate), DateTime.MaxValue.Date));
    }

    protected override Dictionary<string, ItemData> GetCrudersDictionary()
    {
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        return parameters.JobSchedules.ToDictionary(p => p.Key, p => (ItemData)p.Value);
    }

    public override bool ContainsRecordWithKey(string recordKey)
    {
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        var jobSchedules = parameters.JobSchedules;
        return jobSchedules.ContainsKey(recordKey);
    }

    public override void UpdateRecordWithKey(string recordName, ItemData newRecord)
    {
        var newJobSchedule = (JobSchedule)newRecord;
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        parameters.JobSchedules[recordName] = newJobSchedule;
    }

    protected override void AddRecordWithKey(string recordName, ItemData newRecord)
    {
        var newJobSchedule = (JobSchedule)newRecord;
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        parameters.JobSchedules.Add(recordName, newJobSchedule);
    }

    protected override void RemoveRecordWithKey(string recordKey)
    {
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        var jobSchedules = parameters.JobSchedules;
        jobSchedules.Remove(recordKey);
    }

    protected override void CheckFieldsEnables(ItemData itemData, string? lastEditedFieldName = null)
    {
        var jobSchedule = (JobSchedule)itemData;

        var enableDuration = jobSchedule.ScheduleType != EScheduleType.Once;
        EnableFieldByName(nameof(JobSchedule.DurationStartDate), enableDuration);
        EnableFieldByName(nameof(JobSchedule.DurationEndDate), enableDuration);

        var enableOnce = jobSchedule.ScheduleType == EScheduleType.Once;
        EnableFieldByName(nameof(JobSchedule.RunOnceDateTime), enableOnce);

        var enableDaily = jobSchedule.ScheduleType == EScheduleType.Daily;
        EnableFieldByName(nameof(JobSchedule.FreqInterval), enableDaily);
        EnableFieldByName(nameof(JobSchedule.DailyFrequencyType), enableDaily);
        EnableFieldByName(nameof(JobSchedule.ActiveStartDayTime), enableDaily);

        var enableDailyOccursManyTimes =
            enableDaily && jobSchedule.DailyFrequencyType == EDailyFrequency.OccursManyTimes;
        EnableFieldByName(nameof(JobSchedule.FreqSubDayType), enableDailyOccursManyTimes);
        EnableFieldByName(nameof(JobSchedule.FreqSubDayInterval), enableDailyOccursManyTimes);
        EnableFieldByName(nameof(JobSchedule.ActiveEndDayTime), enableDailyOccursManyTimes);
    }

    protected override ItemData CreateNewItem(ItemData? defaultItemData)
    {
        return new JobSchedule();
    }

    //public საჭიროა ApAgent პროექტისათვის
    public override void FillDetailsSubMenu(CliMenuSet itemSubMenuSet, string recordName)
    {
        base.FillDetailsSubMenu(itemSubMenuSet, recordName);


        RunAllStepsNowCommand runAllStepsNowCommand =
            new(_logger, _processes, ParametersManager, recordName, _parametersFileName);
        itemSubMenuSet.AddMenuItem(runAllStepsNowCommand, "Run All steps from this schedule now...");
    }
}