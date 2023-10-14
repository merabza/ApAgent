using ApAgent.Counters;
using ApAgent.Cruders;
using ApAgent.Models;
using CliParameters.FieldEditors;
using LibDataInput;
using LibParameters;

namespace ApAgent.FieldEditors;

public sealed class FolderPathMaskFieldEditor : FieldEditor<string>
{
    private readonly string? _defaultValue;
    private readonly FileBackupFolderCruder _fileBackupFolderCruder;


    public FolderPathMaskFieldEditor(FileBackupFolderCruder fileBackupFolderCruder, string propertyName,
        string? defaultValue = default, bool enterFieldDataOnCreate = false) : base(propertyName,
        enterFieldDataOnCreate)
    {
        _fileBackupFolderCruder = fileBackupFolderCruder;
        _defaultValue = defaultValue;
    }

    public override void UpdateField(string? recordName, object recordForUpdate)
    {
        var forUpdate = (FolderPathMaskItemData)recordForUpdate;
        BackupFolderCruderMaskCounter backupFolderCruderMaskCounter = new(_fileBackupFolderCruder);
        var defVal = string.IsNullOrWhiteSpace(forUpdate.Path)
            ? null
            : backupFolderCruderMaskCounter.CountMask(forUpdate.Path);

        SetValue(recordForUpdate, Inputer.InputText(FieldName, GetValue(recordForUpdate, defVal)));
    }

    public override void SetDefault(ItemData currentItem)
    {
        SetValue(currentItem, _defaultValue);
    }
}