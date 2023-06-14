using System;
using System.Globalization;
using CliParameters.FieldEditors;
using LibDataInput;
using LibParameters;

namespace ApAgent.FieldEditors;

public sealed class DateFieldEditor : FieldEditor<DateTime>
{
    private readonly DateTime _defaultValue;

    public DateFieldEditor(string propertyName, DateTime defaultValue) : base(propertyName)
    {
        _defaultValue = defaultValue;
    }

    public override void UpdateField(string? recordName, object recordForUpdate)
    {
        SetValue(recordForUpdate, Inputer.InputDate(FieldName, GetValue(recordForUpdate, _defaultValue)));
    }

    public override string GetValueStatus(object? record)
    {
        var val = GetValue(record);
        return val.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        //სტანდარტული ფორმატის გადაყვანა custom ფორმატში
        //DateTime.Now.ToString("G", CultureInfo.InvariantCulture);
        //var v = DateTime.Now.ToString("G", CultureInfo.InvariantCulture);
        //ამ მასივი პირველი ელემენტი ემთხვევა სტანდარტულ ფორმატს. დანარჩენები ალბათ გამოიყენება სტრიქონის გაპარსვისას
    }

    public override void SetDefault(ItemData currentItem)
    {
        SetValue(currentItem, _defaultValue);
    }
}