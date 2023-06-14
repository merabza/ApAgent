using System.Linq;
using CliParameters.FieldEditors;
using CliParametersEdit.Cruders;
using LibParameters;

namespace ApAgent.FieldEditors;

public sealed class ArchiverFieldEditor : FieldEditor<string>
{
    private readonly IParametersManager _parametersManager;

    public ArchiverFieldEditor(string propertyName, IParametersManager parametersManager) : base(propertyName)
    {
        _parametersManager = parametersManager;
    }

    public override void UpdateField(string? recordName, object recordForUpdate)
    {
        ArchiverCruder archiverCruder = new(_parametersManager);
        var keys = archiverCruder.GetKeys();
        var def = keys.Count > 1 ? null : archiverCruder.GetKeys().SingleOrDefault();
        SetValue(recordForUpdate,
            archiverCruder.GetNameWithPossibleNewName(FieldName, GetValue(recordForUpdate, def), null, true));
    }
}