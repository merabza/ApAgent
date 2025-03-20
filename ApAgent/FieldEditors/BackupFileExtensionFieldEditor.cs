using ApAgent.Counters;
using CliParameters.FieldEditors;
using DbTools;
using LibDataInput;

namespace ApAgent.FieldEditors;

public sealed class BackupFileExtensionFieldEditor : TextFieldEditor
{
    private readonly string _backupTypePropertyName;

    public BackupFileExtensionFieldEditor(string propertyName, string backupTypePropertyName) : base(propertyName)
    {
        _backupTypePropertyName = backupTypePropertyName;
    }

    public override void UpdateField(string? recordName, object recordForUpdate) //, object currentRecord
    {
        var backupType = GetValue<EBackupType>(recordForUpdate, _backupTypePropertyName);
        BackupFileExtensionCounter backupFileExtensionCounter = new(backupType);
        SetValue(recordForUpdate,
            Inputer.InputText(FieldName, GetValue(recordForUpdate, backupFileExtensionCounter.Count())));
    }
}