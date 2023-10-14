using ApAgent.Cruders;
using CliParameters.FieldEditors;
using LibParameters;

namespace ApAgent.FieldEditors;

public sealed class ReplacePairsSetNameFieldEditor : FieldEditor<string>
{
    private readonly IParametersManager _parametersManager;
    private readonly bool _useNone;

    public ReplacePairsSetNameFieldEditor(string propertyName, IParametersManager parametersManager, bool useNone,
        bool enterFieldDataOnCreate = false) : base(propertyName, enterFieldDataOnCreate)
    {
        _parametersManager = parametersManager;
        _useNone = useNone;
    }

    public override void UpdateField(string? recordName, object recordForUpdate) //, object currentRecord
    {
        ReplacePairsSetCruder replacePairsSetCruderCruder = new(_parametersManager);
        SetValue(recordForUpdate,
            replacePairsSetCruderCruder.GetNameWithPossibleNewName(FieldName, GetValue(recordForUpdate), null,
                _useNone));
    }
}