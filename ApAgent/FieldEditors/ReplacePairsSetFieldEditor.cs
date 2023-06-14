using System.Collections.Generic;
using System.Linq;
using ApAgent.Cruders;
using CliMenu;
using CliParameters.FieldEditors;
using LibApAgentData.Models;
using LibParameters;

namespace ApAgent.FieldEditors;

public sealed class ReplacePairsSetFieldEditor : FieldEditor<Dictionary<string, ReplacePairsSet>>
{
    private readonly ParametersManager _parametersManager;

    public ReplacePairsSetFieldEditor(string propertyName, ParametersManager parametersManager) : base(propertyName,
        null, true)
    {
        _parametersManager = parametersManager;
    }

    public override CliMenuSet GetSubMenu(object record)
    {
        ReplacePairsSetCruder replacePairsSetCruder = new(_parametersManager);
        var menuSet = replacePairsSetCruder.GetListMenu();
        return menuSet;
    }


    public override string GetValueStatus(object? record)
    {
        var val = GetValue(record);

        if (val == null || val.Count <= 0)
            return "No Details";

        if (val.Count > 1)
            return $"{val.Count} Details";

        var kvp = val.Single();
        return kvp.Key;
    }
}