using System.Net.Http;
using ApAgent.FieldEditors;
using CliParameters;
using CliParameters.FieldEditors;
using CliParametersApiClientsEdit.FieldEditors;
using CliParametersDataEdit.FieldEditors;
using CliParametersEdit.FieldEditors;
using CliParametersExcludeSetsEdit.FieldEditors;
using LibApAgentData.Models;
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

        FieldEditors.Add(new DatabaseServerConnectionsFieldEditor(logger, httpClientFactory, parametersManager,
            nameof(ApAgentParameters.DatabaseServerConnections)));
        FieldEditors.Add(new ApiClientsFieldEditor(logger, httpClientFactory, nameof(ApAgentParameters.ApiClients),
            parametersManager));
        FieldEditors.Add(new FileStoragesFieldEditor(logger, nameof(ApAgentParameters.FileStorages),
            parametersManager));
        FieldEditors.Add(new ExcludeSetsFieldEditor(nameof(ApAgentParameters.ExcludeSets), parametersManager));
        FieldEditors.Add(new ReplacePairsSetFieldEditor(nameof(ApAgentParameters.ReplacePairsSets), parametersManager));
        FieldEditors.Add(new SmartSchemasFieldEditor(nameof(ApAgentParameters.SmartSchemas), parametersManager));
        FieldEditors.Add(new ArchiversFieldEditor(nameof(ApAgentParameters.Archivers), parametersManager));
    }
}