using System.Collections.Generic;
using System.Linq;
using ApAgent.Cruders;
using ApAgent.MenuCommands;
using CliMenu;
using CliParameters.FieldEditors;
using LibParameters;

namespace ApAgent.FieldEditors;

public sealed class FolderPathsSetFieldEditor : FieldEditor<List<string>>
{
    private readonly IParametersManager _parametersManager;

    // ReSharper disable once ConvertToPrimaryConstructor
    public FolderPathsSetFieldEditor(string propertyName, IParametersManager parametersManager,
        bool enterFieldDataOnCreate = false) : base(propertyName, enterFieldDataOnCreate, null, true)
    {
        _parametersManager = parametersManager;
    }

    public void Update(object recordForUpdate, List<string> data)
    {
        SetValue(recordForUpdate, data);
    }


    public override CliMenuSet GetSubMenu(object record)
    {
        var currentValuesList = GetValue(record) ?? new List<string>();

        FolderPathsSetCruder folderPathsSetCruder = new(currentValuesList, _parametersManager, record, this);
        var foldersSet = folderPathsSetCruder.GetListMenu();

        foldersSet.InsertMenuItem(1, new MultiSelectSubfoldersCommand(currentValuesList, folderPathsSetCruder));
        return foldersSet;
    }

    public override string GetValueStatus(object? record)
    {
        var val = GetValue(record);

        if (val == null || val.Count == 0)
            return "No Folders";

        return val.Count != 1 ? $"{val.Count} folders" : val.Single();
    }
}