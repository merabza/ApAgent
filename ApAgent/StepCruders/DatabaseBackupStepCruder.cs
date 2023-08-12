using System.Collections.Generic;
using System.Linq;
using ApAgent.FieldEditors;
using CliParameters.FieldEditors;
using CliParametersApiClientsEdit.FieldEditors;
using CliParametersDataEdit.FieldEditors;
using CliParametersEdit.FieldEditors;
using Installer.AgentClients;
using LibApAgentData.Models;
using LibApAgentData.Steps;
using LibParameters;
using LibToolActions.BackgroundTasks;
using Microsoft.Extensions.Logging;

namespace ApAgent.StepCruders;

public sealed class DatabaseBackupStepCruder : StepCruder
{
    public DatabaseBackupStepCruder(ILogger logger, Processes processes, ParametersManager parametersManager) : base(
        logger, processes, parametersManager, "Database Backup Step", "Database Backup Steps")
    {
        var parametersFileName = parametersManager.ParametersFileName;
        WebAgentClientFabric webAgentClientFabric = new();

        List<FieldEditor> tempFieldEditors = new();
        tempFieldEditors.AddRange(FieldEditors);
        FieldEditors.Clear();

        FieldEditors.Add(new DatabaseServerConnectionNameFieldEditor(logger,
            nameof(DatabaseBackupStep.DatabaseServerConnectionName), ParametersManager, true));

        FieldEditors.Add(new ApiClientNameFieldEditor(logger, nameof(DatabaseBackupStep.DatabaseWebAgentName),
            ParametersManager, webAgentClientFabric, true));

        //FieldEditors.Add(new ApiClientDbServerNameFieldEditor(logger, nameof(DatabaseBackupStep.DatabaseServerName),
        //    ParametersManager, webAgentClientFabric, nameof(DatabaseBackupStep.DatabaseWebAgentName)));

        FieldEditors.Add(new DatabaseBackupParametersFieldEditor(
            nameof(DatabaseBackupStep.DatabaseBackupParameters),
            ParametersManager));

        FieldEditors.Add(
            new EnumFieldEditor<EDatabaseSet>(nameof(DatabaseBackupStep.DatabaseSet), EDatabaseSet.AllDatabases));
        FieldEditors.Add(new DatabaseNamesFieldEditor(logger, nameof(DatabaseBackupStep.DatabaseNames),
            ParametersManager, nameof(DatabaseBackupStep.DatabaseServerConnectionName),
            nameof(DatabaseBackupStep.DatabaseWebAgentName), nameof(DatabaseBackupStep.DatabaseSet),
            nameof(DatabaseBackupStep.DatabaseBackupParameters)));

        FieldEditors.Add(new DbServerSideBackupPathFieldEditor(nameof(DatabaseBackupStep.DbServerSideBackupPath),
            parametersManager, nameof(DatabaseBackupStep.DatabaseWebAgentName),
            nameof(DatabaseBackupStep.DatabaseBackupParameters)));

        FieldEditors.Add(new SmartSchemaNameFieldEditor(nameof(DatabaseBackupStep.SmartSchemaName),
            ParametersManager));
        FieldEditors.Add(new FileStorageNameFieldEditor(logger, nameof(DatabaseBackupStep.FileStorageName),
            ParametersManager));
        FieldEditors.Add(new IntFieldEditor(nameof(DatabaseBackupStep.DownloadProcLineId), 1));
        FieldEditors.Add(new LocalPathFieldEditor(nameof(DatabaseBackupStep.LocalPath), ParametersManager,
            nameof(DatabaseBackupStep.DatabaseBackupParameters), parametersFileName));
        FieldEditors.Add(new SmartSchemaNameFieldEditor(nameof(DatabaseBackupStep.LocalSmartSchemaName),
            ParametersManager));
        FieldEditors.Add(new ArchiverFieldEditor(nameof(DatabaseBackupStep.ArchiverName), ParametersManager));
        FieldEditors.Add(new ArchiverProcLineIdFieldEditor(nameof(DatabaseBackupStep.CompressProcLineId), 1,
            nameof(DatabaseBackupStep.ArchiverName)));
        FieldEditors.Add(new FileStorageNameFieldEditor(logger, nameof(DatabaseBackupStep.UploadFileStorageName),
            ParametersManager));
        FieldEditors.Add(new IntFieldEditor(nameof(DatabaseBackupStep.UploadProcLineId), 1));
        FieldEditors.Add(new SmartSchemaNameFieldEditor(nameof(DatabaseBackupStep.UploadSmartSchemaName),
            ParametersManager));
        FieldEditors.AddRange(tempFieldEditors);
    }

    protected override Dictionary<string, ItemData> GetCrudersDictionary()
    {
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        return parameters.DatabaseBackupSteps.ToDictionary(p => p.Key, p => (ItemData)p.Value);
    }

    public override void UpdateRecordWithKey(string recordName, ItemData newRecord)
    {
        var newDatabaseBackupStep = (DatabaseBackupStep)newRecord;
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        parameters.DatabaseBackupSteps[recordName] = newDatabaseBackupStep;
    }

    protected override void AddRecordWithKey(string recordName, ItemData newRecord)
    {
        var newDatabaseBackupStep = (DatabaseBackupStep)newRecord;
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        parameters.DatabaseBackupSteps.Add(recordName, newDatabaseBackupStep);
    }

    protected override void RemoveRecordWithKey(string recordKey)
    {
        var parameters = (ApAgentParameters)ParametersManager.Parameters;
        var databaseBackupSteps = parameters.DatabaseBackupSteps;
        databaseBackupSteps.Remove(recordKey);
    }

    protected override ItemData CreateNewItem(ItemData? defaultItemData)
    {
        return new DatabaseBackupStep();
    }
}