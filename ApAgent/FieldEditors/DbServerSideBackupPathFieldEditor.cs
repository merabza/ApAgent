using CliParameters.FieldEditors;
using LibApAgentData.Models;
using LibDatabaseParameters;
using LibDataInput;
using LibMenuInput;
using LibParameters;
using SystemToolsShared;

namespace ApAgent.FieldEditors;

public sealed class DbServerSideBackupPathFieldEditor : FieldEditor<string>
{
    private readonly string _databaseBackupParametersPropertyName;

    private readonly string _databaseWebAgentNamePropertyName;
    private readonly ParametersManager _parametersManager;

    public DbServerSideBackupPathFieldEditor(string propertyName, ParametersManager parametersManager,
        string databaseWebAgentNamePropertyName, string databaseBackupParametersPropertyName) : base(propertyName)
    {
        _parametersManager = parametersManager;
        _databaseWebAgentNamePropertyName = databaseWebAgentNamePropertyName;
        _databaseBackupParametersPropertyName = databaseBackupParametersPropertyName;
    }

    public override void UpdateField(string? recordName, object recordForUpdate)
    {
        var databaseWebAgentName = GetValue<string>(recordForUpdate, _databaseWebAgentNamePropertyName);
        var databaseBackupParameters =
            GetValue<DatabaseBackupParametersModel>(recordForUpdate, _databaseBackupParametersPropertyName);
        var currentPath = GetValue(recordForUpdate);

        if (currentPath != null)
            if (Inputer.InputBool("Clear?", false, false))
            {
                SetValue(recordForUpdate, null);
                return;
            }

        if (!string.IsNullOrWhiteSpace(databaseWebAgentName))
        {
            StShared.WriteWarningLine("Cannot set Db Server Side Backup Path, because Web Agent is used", true,
                null, true);
            return;
        }

        var parameters = (ApAgentParameters)_parametersManager.Parameters;

        var workFolderCandidateForLocalPath = databaseBackupParameters is null
            ? null
            : parameters.CountLocalPath(currentPath, _parametersManager.ParametersFileName,
                $"Database{databaseBackupParameters.BackupType}Backups");

        var newValue = MenuInputer.InputFolderPath(FieldName, workFolderCandidateForLocalPath);

        SetValue(recordForUpdate, string.IsNullOrWhiteSpace(newValue) ? null : newValue);
    }
}