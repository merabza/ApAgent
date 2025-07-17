using System.Collections.Generic;
using System.Linq;
using ApAgent.Cruders;
using ApAgent.MenuCommands;
using CliMenu;
using CliParameters.FieldEditors;

namespace ApAgent.FieldEditors;

public sealed class FolderPathsSetFieldEditor : FieldEditor<List<string>>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public FolderPathsSetFieldEditor(string propertyName, bool enterFieldDataOnCreate = false) : base(propertyName,
        enterFieldDataOnCreate, null, true)
    {
    }

    public void Update(object recordForUpdate, List<string> data)
    {
        SetValue(recordForUpdate, data);
    }

    public override CliMenuSet GetSubMenu(object record)
    {
        var currentValuesList = GetValue(record) ?? [];

        var folderPathsSetCruder = new FolderPathsSetCruder(currentValuesList, record, this);
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