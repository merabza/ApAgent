using System.Collections.Generic;
using System.Linq;
using ApAgent.FieldEditors;
using ApAgent.Models;
using CliParameters;
using CliParameters.Cruders;
using CliParameters.FieldEditors;
using LibParameters;

namespace ApAgent.Cruders;

public sealed class FolderPathsSetCruder : Cruder
{
    private readonly List<string> _currentValuesList;
    private readonly FolderPathsSetFieldEditor _folderPathsSetFieldEditor;
    private readonly object _record;

    public FolderPathsSetCruder(List<string> currentValuesList, object record,
        FolderPathsSetFieldEditor folderPathsSetFieldEditor) : base("Priority Folder Path", "Priority Folder Paths",
        true, false)
    {
        _currentValuesList = currentValuesList;
        _record = record;
        _folderPathsSetFieldEditor = folderPathsSetFieldEditor;
        FieldEditors.Add(new FolderPathFieldEditor(nameof(FolderPathItemData.Path)));
    }

    protected override Dictionary<string, ItemData> GetCrudersDictionary()
    {
        return _currentValuesList.ToDictionary(p => p, ItemData (p) => new FolderPathItemData { Path = p });
    }

    protected override ItemData CreateNewItem(string? recordKey, ItemData? defaultItemData)
    {
        return new FolderPathItemData();
    }

    public override bool ContainsRecordWithKey(string recordKey)
    {
        return _currentValuesList.Contains(recordKey);
    }

    protected override void RemoveRecordWithKey(string recordKey)
    {
        _currentValuesList.Remove(recordKey);
    }

    protected override void AddRecordWithKey(string recordName, ItemData newRecord)
    {
        var newPath = ((FolderPathItemData)newRecord).Path;
        if (!string.IsNullOrWhiteSpace(newPath))
            _currentValuesList.Add(newPath);
    }

    public override void Save(string message)
    {
        _folderPathsSetFieldEditor.Update(_record, _currentValuesList);
        base.Save(message);
    }
}