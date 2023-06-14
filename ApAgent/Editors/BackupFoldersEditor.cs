//using System.Collections.Generic;
//using ApAgent.Commands;
//using ApAgent.Cruders;
//using CliTools;
//using CliTools.Menu;

//namespace ApAgent.Editors
//{
//  public sealed class BackupFoldersEditor : CliAppLoop
//  {

//    private readonly Dictionary<string, string> _currentValuesDict;

//    public BackupFoldersEditor(Dictionary<string, string> currentValuesDict) : base("Backup Folders")
//    {
//      _currentValuesDict = currentValuesDict;
//    }

//    protected override bool BuildMainMenu()
//    {

//      FileBackupFolderCruder fileBackupFolderCruder = new FileBackupFolderCruder(_currentValuesDict);
//      CliMenuSet foldersSet = fileBackupFolderCruder.GetListMenu();

//      foldersSet.InsertMenuItem(1, "Multi Select Subfolders", new MultiSelectSubfoldersCommand(_currentValuesDict));

//      return AddChangeMenu(foldersSet);
//    }

//  }
//}

