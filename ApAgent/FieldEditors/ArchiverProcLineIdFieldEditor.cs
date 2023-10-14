using CliParameters.FieldEditors;
using LibDataInput;
using LibParameters;

namespace ApAgent.FieldEditors;

public sealed class ArchiverProcLineIdFieldEditor : FieldEditor<int>
{
    private readonly string _archiverNamePropertyName;
    private readonly int _defaultValue;

    public ArchiverProcLineIdFieldEditor(string propertyName, int defaultValue, string archiverNamePropertyName,
        bool enterFieldDataOnCreate = false) : base(propertyName, enterFieldDataOnCreate)
    {
        _defaultValue = defaultValue;
        _archiverNamePropertyName = archiverNamePropertyName;
    }

    public override void UpdateField(string? recordName, object recordForUpdate)
    {
        var archiverName = GetValue<string>(recordForUpdate, _archiverNamePropertyName);

        SetValue(recordForUpdate,
            Inputer.InputInt(FieldName, archiverName == null ? 1 : GetValue(recordForUpdate, _defaultValue)));
    }

    public override void SetDefault(ItemData currentItem)
    {
        SetValue(currentItem, _defaultValue);
    }
}