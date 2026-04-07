using System.Net.Http;
using ApAgent.Cruders;
using ApAgentData.LibApAgentData.Models;
using AppCliTools.CliParameters;
using AppCliTools.CliParameters.FieldEditors;
using AppCliTools.CliParametersDataEdit.Cruders;
using AppCliTools.CliParametersEdit.Cruders;
using AppCliTools.CliParametersExcludeSetsEdit.Cruders;
using Microsoft.Extensions.Logging;
using ParametersManagement.LibDatabaseParameters;
using ParametersManagement.LibFileParameters.Models;
using ParametersManagement.LibParameters;

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
        //FieldEditors.Add(new DictionaryFieldEditor<ApiClientCruder, ApiClientSettings>(
        //    nameof(ApAgentParameters.ApiClients), logger, httpClientFactory, parametersManager));

        //FieldEditors.Add(new FileStoragesFieldEditor(logger, nameof(ApAgentParameters.FileStorages),
        //    parametersManager));
        FieldEditors.Add(
            new DictionaryFieldEditor<FileStorageCruder, FileStorageData>(nameof(ApAgentParameters.FileStorages),
                logger, parametersManager));

        //FieldEditors.Add(new ExcludeSetsFieldEditor(nameof(ApAgentParameters.ExcludeSets), parametersManager));
        FieldEditors.Add(new DictionaryFieldEditor<ExcludeSetCruder, ExcludeSet>(nameof(ApAgentParameters.ExcludeSets),
            parametersManager));

        //FieldEditors.Add(new ReplacePairsSetFieldEditor(nameof(ApAgentParameters.ReplacePairsSets), parametersManager));
        FieldEditors.Add(new DictionaryFieldEditor<ReplacePairsSetCruder, ReplacePairsSet>(
            nameof(ApAgentParameters.ReplacePairsSets), logger, httpClientFactory, parametersManager));

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
