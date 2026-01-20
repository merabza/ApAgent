using System;
using AppCliTools.CliParameters.FieldEditors;
using AppCliTools.LibDataInput;
using ParametersManagement.LibParameters;

namespace ApAgent.FieldEditors;

public sealed class TimeSpanFieldEditor : FieldEditor<TimeSpan>
{
    private readonly TimeSpan _defaultValue;

    public TimeSpanFieldEditor(string propertyName, TimeSpan defaultValue, bool enterFieldDataOnCreate = false) : base(
        propertyName, enterFieldDataOnCreate)
    {
        _defaultValue = defaultValue;
    }

    public override void UpdateField(string? recordKey, object recordForUpdate)
    {
        SetValue(recordForUpdate, Inputer.InputTimeSpan(FieldName, GetValue(recordForUpdate, _defaultValue)));
    }

    public override void SetDefault(ItemData currentItem)
    {
        SetValue(currentItem, _defaultValue);
    }
}
