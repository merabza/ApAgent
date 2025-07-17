using System.Collections.Generic;
using ApAgent.FieldEditors;
using ApAgent.Models;
using CliParameters;
using CliParameters.FieldEditors;
using LibParameters;

namespace ApAgent.Cruders;

public sealed class FileBackupFolderCruder : ParCruder<FolderPathMaskItemData>
{
    //private readonly BackupFolderPathsFieldEditor _backupFolderPathsFieldEditor;

    //private readonly Dictionary<string, string> _currentValuesDict;
    //private readonly object _record;

    public FileBackupFolderCruder(IParametersManager parametersManager,
        Dictionary<string, FolderPathMaskItemData> currentValuesDictionary) : base(parametersManager,
        currentValuesDictionary, "Backup Folder Path", "Backup Folder Paths")
    {
        //_currentValuesDict = currentValuesDict;
        //_record = record;
        //_backupFolderPathsFieldEditor = backupFolderPathsFieldEditor;

        FieldEditors.Add(new FolderPathFieldEditor(nameof(FolderPathMaskItemData.Path)));
        FieldEditors.Add(new FolderPathMaskFieldEditor(this, nameof(FolderPathMaskItemData.Mask)));
    }

    //protected override Dictionary<string, ItemData> GetCrudersDictionary()
    //{
    //    return _currentValuesDict.ToDictionary(p => p.Key,
    //        p => (ItemData)new FolderPathMaskItemData { Path = p.Value, Mask = p.Key });
    //}

    //public override bool ContainsRecordWithKey(string recordKey)
    //{
    //    return _currentValuesDict.ContainsKey(recordKey);
    //}

    //protected override void AddRecordWithKey(string recordName, ItemData newRecord)
    //{
    //    if (newRecord is not FolderPathMaskItemData sid)
    //        return;

    //    if (!string.IsNullOrWhiteSpace(sid.Path) && recordName == sid.Mask)
    //        _currentValuesDict.Add(recordName, sid.Path);
    //}

    //protected override void RemoveRecordWithKey(string recordKey)
    //{
    //    _currentValuesDict.Remove(recordKey);
    //}

    //public override void UpdateRecordWithKey(string recordName, ItemData newRecord)
    //{
    //    if (newRecord is not FolderPathMaskItemData sid)
    //        return;
    //    if (string.IsNullOrWhiteSpace(sid.Mask))
    //        return;
    //    if (string.IsNullOrWhiteSpace(sid.Path))
    //        return;
    //    if (recordName == sid.Mask)
    //    {
    //        _currentValuesDict[recordName] = sid.Path;
    //    }
    //    else
    //    {
    //        _currentValuesDict.Remove(recordName);
    //        _currentValuesDict.Add(sid.Mask, sid.Path);
    //    }
    //}

    //protected override ItemData CreateNewItem(string? recordKey, ItemData? defaultItemData)
    //{
    //    return new FolderPathMaskItemData();
    //}

    //public override List<string> GetKeys()
    //{
    //    return [.. _currentValuesDict.Keys];
    //}

    //public override string? GetStatusFor(string name)
    //{
    //    return _currentValuesDict.GetValueOrDefault(name);
    //}

    //public override void Save(string message)
    //{
    //    _backupFolderPathsFieldEditor.Update(_record, _currentValuesDict);
    //    base.Save(message);
    //}
}