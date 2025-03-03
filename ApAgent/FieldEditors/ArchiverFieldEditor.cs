using System.Linq;
using CliParameters.FieldEditors;
using CliParametersEdit.Cruders;
using LibParameters;

namespace ApAgent.FieldEditors;

public sealed class ArchiverFieldEditor : FieldEditor<string>
{
    private readonly IParametersManager _parametersManager;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ArchiverFieldEditor(string propertyName, IParametersManager parametersManager,
        bool enterFieldDataOnCreate = false) : base(propertyName, enterFieldDataOnCreate)
    {
        _parametersManager = parametersManager;
    }

    public override void UpdateField(string? recordName, object recordForUpdate)
    {
        var archiverCruder = new ArchiverCruder(_parametersManager);
        var keys = archiverCruder.GetKeys();
        var def = keys.Count > 1 ? null : archiverCruder.GetKeys().SingleOrDefault();
        SetValue(recordForUpdate,
            archiverCruder.GetNameWithPossibleNewName(FieldName, GetValue(recordForUpdate, def), null, true));
    }
}