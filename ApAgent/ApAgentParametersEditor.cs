using ApAgent.FieldEditors;
using CliParameters;
using CliParameters.FieldEditors;
using CliParametersApiClientsEdit.FieldEditors;
using CliParametersDataEdit.FieldEditors;
using CliParametersEdit.FieldEditors;
using CliParametersExcludeSetsEdit.FieldEditors;
using Installer.AgentClients;
using LibApAgentData.Models;
using LibParameters;
using Microsoft.Extensions.Logging;

namespace ApAgent;

public sealed class ApAgentParametersEditor : ParametersEditor
{
    public ApAgentParametersEditor(IParameters parameters, ParametersManager parametersManager,
        ILogger logger) : base("InsuranceReport Parameters Editor", parameters, parametersManager)
    {
        WebAgentClientFabric webAgentClientFabric = new();

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
        FieldEditors.Add(new TextFieldEditor(nameof(ApAgentParameters.DateMask),
            ApAgentParameters.DefaultDateMask));

        FieldEditors.Add(
            new DatabaseServerConnectionsFieldEditor(nameof(ApAgentParameters.DatabaseServerConnections),
                parametersManager, logger));
        FieldEditors.Add(new ApiClientsFieldEditor(logger, nameof(ApAgentParameters.ApiClients), parametersManager,
            webAgentClientFabric));
        FieldEditors.Add(new FileStoragesFieldEditor(logger, nameof(ApAgentParameters.FileStorages),
            parametersManager));
        FieldEditors.Add(new ExcludeSetsFieldEditor(nameof(ApAgentParameters.ExcludeSets), parametersManager));
        FieldEditors.Add(new ReplacePairsSetFieldEditor(nameof(ApAgentParameters.ReplacePairsSets),
            parametersManager));
        FieldEditors.Add(new SmartSchemasFieldEditor(nameof(ApAgentParameters.SmartSchemas), parametersManager));
        FieldEditors.Add(new ArchiversFieldEditor(nameof(ApAgentParameters.Archivers), parametersManager));
    }
}