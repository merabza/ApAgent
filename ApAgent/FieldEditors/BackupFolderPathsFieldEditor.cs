//using System.Collections.Generic;
//using System.Linq;
//using ApAgent.Cruders;
//using ApAgent.MenuCommands;
//using AppCliTools.CliMenu;
//using AppCliTools.CliParameters.FieldEditors;

//namespace ApAgent.FieldEditors;

//public sealed class BackupFolderPathsFieldEditor : FieldEditor<Dictionary<string, string>>
//{
//    // ReSharper disable once ConvertToPrimaryConstructor
//    public BackupFolderPathsFieldEditor(string propertyName, bool enterFieldDataOnCreate = false) : base(propertyName,
//        enterFieldDataOnCreate, null, true)
//    {
//    }

//    public void Update(object recordForUpdate, Dictionary<string, string> data)
//    {
//        SetValue(recordForUpdate, data);
//    }

//    public override CliMenuSet GetSubMenu(object record)
//    {
//        var currentValuesDict = GetValue(record) ?? new Dictionary<string, string>();

//        var fileBackupFolderCruder = new FileBackupFolderCruder(currentValuesDict);
//        var foldersSet = fileBackupFolderCruder.GetListMenu();

//        foldersSet.InsertMenuItem(1,
//            new MultiSelectSubfoldersWithMasksCommand(currentValuesDict, fileBackupFolderCruder));
//        return foldersSet;
//    }

//    public override string GetValueStatus(object? record)
//    {
//        var val = GetValue(record);

//        if (val == null || val.Count == 0)
//        {
//            return "No Folders";
//        }

//        if (val.Count != 1)
//        {
//            return $"{val.Count} folders";
//        }

//        var kvp = val.Single();
//        return $"{kvp.Key} - {kvp.Value}";
//    }
//}
