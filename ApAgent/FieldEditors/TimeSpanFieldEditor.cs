﻿using System;
using CliParameters.FieldEditors;
using LibDataInput;
using LibParameters;

namespace ApAgent.FieldEditors;

public sealed class TimeSpanFieldEditor : FieldEditor<TimeSpan>
{
    private readonly TimeSpan _defaultValue;

    public TimeSpanFieldEditor(string propertyName, TimeSpan defaultValue) : base(propertyName)
    {
        _defaultValue = defaultValue;
    }

    public override void UpdateField(string? recordName, object recordForUpdate) //, object currentRecord
    {
        SetValue(recordForUpdate, Inputer.InputTimeSpan(FieldName, GetValue(recordForUpdate, _defaultValue)));
    }

    public override void SetDefault(ItemData currentItem)
    {
        SetValue(currentItem, _defaultValue);
    }
}