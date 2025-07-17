using System.Net.Http;
using ApAgent.FieldEditors;
using CliParameters;
using CliParameters.FieldEditors;
using CliParametersApiClientsEdit;
using CliParametersDataEdit.Cruders;
using CliParametersEdit.Cruders;
using CliParametersExcludeSetsEdit.Cruders;
using LibApAgentData.Models;
using LibApiClientParameters;
using LibDatabaseParameters;
using LibFileParameters.Models;
using LibParameters;
using Microsoft.Extensions.Logging;

namespace ApAgent;

public sealed class ApAgentParametersEditor : ParametersEditor
{
    public ApAgentParametersEditor(ILogger logger, IHttpClientFactory httpClientFactory, IParameters parameters,
        ParametersManager parametersManager) : base("ApAgent Parameters Editor", parameters, parametersManager)
    {
        FieldEditors.Add(new FolderPathFieldEditor(nameof(ApAgentParameters.LogFolder)));
        FieldEditors.Add(new FolderPathFieldEditor(nameof(ApAgentParameters.WorkFolder)));
        FieldEditors.Add(new FolderPathFieldEditor(nameof(ApAgentParameters.ProcLogFilesFolder)));
        FieldEditors.Add(new FilePathFieldEditor(nameof(ApAgentParameters.ApAgentParametersFileNameForLocalReServer)));

        FieldEditors.Add(new TextFieldEditor(nameof(ApAgentParameters.UploadFileTempExtension),
            ApAgentParameters.DefaultUploadFileTempExtension));
        FieldEditors.Add(new TextFieldEditor(nameof(ApAgentParameters.DownloadFileTempExtension),
            ApAgentParameters.DefaultDownloadFileTempExtension));
        FieldEditors.Add(new TextFieldEditor(nameof(ApAgentParameters.ArchivingFileTempExtension),
            ApAgentParameters.DefaultArchivingFileTempExtension));
        FieldEditors.Add(new TextFieldEditor(nameof(ApAgentParameters.DateMask), ApAgentParameters.DefaultDateMask));

        //FieldEditors.Add(new DatabaseServerConnectionsFieldEditor(logger, httpClientFactory, parametersManager,
        //    nameof(ApAgentParameters.DatabaseServerConnections)));
        FieldEditors.Add(new DictionaryFieldEditor<DatabaseServerConnectionCruder, DatabaseServerConnectionData>(
            nameof(ApAgentParameters.DatabaseServerConnections), logger, httpClientFactory, parametersManager));

        //FieldEditors.Add(new ApiClientsFieldEditor(logger, httpClientFactory, nameof(ApAgentParameters.ApiClients),
        //    parametersManager));
        FieldEditors.Add(new DictionaryFieldEditor<ApiClientCruder, ApiClientSettings>(
            nameof(ApAgentParameters.ApiClients), logger, httpClientFactory, parametersManager));

        //FieldEditors.Add(new FileStoragesFieldEditor(logger, nameof(ApAgentParameters.FileStorages),
        //    parametersManager));
        FieldEditors.Add(
            new DictionaryFieldEditor<FileStorageCruder, FileStorageData>(nameof(ApAgentParameters.FileStorages),
                logger, parametersManager));

        //FieldEditors.Add(new ExcludeSetsFieldEditor(nameof(ApAgentParameters.ExcludeSets), parametersManager));
        FieldEditors.Add(new DictionaryFieldEditor<ExcludeSetCruder, ExcludeSet>(nameof(ApAgentParameters.ExcludeSets),
            logger, parametersManager));

        FieldEditors.Add(new ReplacePairsSetFieldEditor(nameof(ApAgentParameters.ReplacePairsSets), parametersManager));

        //FieldEditors.Add(new SmartSchemasFieldEditor(nameof(ApAgentParameters.SmartSchemas), parametersManager));
        FieldEditors.Add(
            new DictionaryFieldEditor<SmartSchemaCruder, SmartSchema>(nameof(ApAgentParameters.SmartSchemas),
                parametersManager));

        //FieldEditors.Add(new ArchiversFieldEditor(nameof(ApAgentParameters.Archivers), parametersManager));
        FieldEditors.Add(
            new DictionaryFieldEditor<ArchiverCruder, ArchiverData>(nameof(ApAgentParameters.Archivers),
                parametersManager));
    }
}