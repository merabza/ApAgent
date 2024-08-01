using System.Collections.Generic;
using System.Linq;
using ApAgent.Cruders;
using ApAgent.MenuCommands;
using CliMenu;
using CliParameters.FieldEditors;
using LibParameters;

namespace ApAgent.FieldEditors;

public sealed class BackupFolderPathsFieldEditor : FieldEditor<Dictionary<string, string>>
{
    private readonly IParametersManager _parametersManager;

    // ReSharper disable once ConvertToPrimaryConstructor
    public BackupFolderPathsFieldEditor(string propertyName, IParametersManager parametersManager,
        bool enterFieldDataOnCreate = false) : base(propertyName, enterFieldDataOnCreate, null, true)
    {
        _parametersManager = parametersManager;
    }

    public void Update(object recordForUpdate, Dictionary<string, string> data)
    {
        SetValue(recordForUpdate, data);
    }


    public override CliMenuSet GetSubMenu(object record)
    {
        var currentValuesDict = GetValue(record) ?? new Dictionary<string, string>();

        FileBackupFolderCruder fileBackupFolderCruder = new(currentValuesDict, _parametersManager, record, this);
        var foldersSet = fileBackupFolderCruder.GetListMenu();

        foldersSet.InsertMenuItem(1,
            new MultiSelectSubfoldersWithMasksCommand(currentValuesDict, fileBackupFolderCruder));
        return foldersSet;
    }

    public override string GetValueStatus(object? record)
    {
        var val = GetValue(record);

        if (val == null || val.Count == 0)
            return "No Folders";

        if (val.Count != 1)
            return $"{val.Count} folders";

        var kvp = val.Single();
        return $"{kvp.Key} - {kvp.Value}";
    }
}