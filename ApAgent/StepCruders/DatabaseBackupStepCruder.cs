using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using ApAgent.FieldEditors;
using CliParameters.FieldEditors;
using CliParametersDataEdit.FieldEditors;
using CliParametersEdit.FieldEditors;
using LibApAgentData.Models;
using LibApAgentData.Steps;
using LibParameters;
using LibToolActions.BackgroundTasks;
using Microsoft.Extensions.Logging;

namespace ApAgent.StepCruders;

public sealed class DatabaseBackupStepCruder : StepCruder
{
    public DatabaseBackupStepCruder(ILogger logger, IHttpClientFactory httpClientFactory, Processes processes,
        ParametersManager parametersManager) : base(logger, httpClientFactory, processes, parametersManager,
        "Database Backup Step", "Database Backup Steps")
    {
        var parametersFileName = parametersManager.ParametersFileName;

        var tempFieldEditors = new List<FieldEditor>();
        tempFieldEditors.AddRange(FieldEditors);
        FieldEditors.Clear();

        FieldEditors.Add(new DatabaseServerConnectionNameFieldEditor(logger, httpClientFactory,
            nameof(DatabaseBackupStep.DatabaseServerConnectionName), ParametersManager, true));

        //FieldEditors.Add(new ApiClientNameFieldEditor(logger, httpClientFactory,
        //    nameof(DatabaseBackupStep.DatabaseWebAgentName), ParametersManager, true));

        FieldEditors.Add(new DatabaseBackupParametersFieldEditor(logger,
            nameof(DatabaseBackupStep.DatabaseBackupParameters), ParametersManager));

        FieldEditors.Add(new EnumFieldEditor<EDatabaseSet>(nameof(DatabaseBackupStep.DatabaseSet),
            EDatabaseSet.AllDatabases));

        FieldEditors.Add(new DbServerFoldersSetNameFieldEditor(logger, httpClientFactory,
            nameof(DatabaseBackupStep.DbServerFoldersSetName), parametersManager,
            nameof(DatabaseBackupStep.DatabaseServerConnectionName)));


        FieldEditors.Add(new DatabaseNamesFieldEditor(logger, httpClientFactory,
            nameof(DatabaseBackupStep.DatabaseNames), ParametersManager,
            nameof(DatabaseBackupStep.DatabaseServerConnectionName), nameof(DatabaseBackupStep.DatabaseSet),
            nameof(DatabaseBackupStep.DatabaseBackupParameters)));

        //FieldEditors.Add(new DbServerSideBackupPathFieldEditor(nameof(DatabaseBackupStep.DbServerSideBackupPath),
        //    parametersManager, nameof(DatabaseBackupStep.DatabaseWebAgentName),
        //    nameof(DatabaseBackupStep.DatabaseBackupParameters)));

        FieldEditors.Add(new SmartSchemaNameFieldEditor(nameof(DatabaseBackupStep.SmartSchemaName), ParametersManager));
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

    protected override ItemData CreateNewItem(string? recordKey, ItemData? defaultItemData)
    {
        return new DatabaseBackupStep();
    }
}