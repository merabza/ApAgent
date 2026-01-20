using System.Collections.Generic;
using System.Linq;
using AppCliTools.CliParameters.FieldEditors;
using AppCliTools.CliParametersEdit.Cruders;
using ParametersManagement.LibParameters;

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

    public override void UpdateField(string? recordKey, object recordForUpdate)
    {
        var archiverCruder = ArchiverCruder.Create(_parametersManager);
        List<string> keys = archiverCruder.GetKeys();
        string? def = keys.Count > 1 ? null : archiverCruder.GetKeys().SingleOrDefault();
        SetValue(recordForUpdate,
            archiverCruder.GetNameWithPossibleNewName(FieldName, GetValue(recordForUpdate, def), null, true));
    }
}
