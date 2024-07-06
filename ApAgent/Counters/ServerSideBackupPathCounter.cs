//using System.IO;
//using CliParameters;
//using CliToolsData.Models;
//using DatabaseAgentClients;
//using DbTools.Models;
//using LibApAgentData.Models;
//using Microsoft.Extensions.Logging;

//namespace ApAgent.Counters
//{
//  public sealed class ServerSideBackupPathCounter
//  {
//    private const string CDatabaseBackupFiles = "DatabaseBackupFiles";
//    private readonly ILogger _logger;
//    private readonly string _databaseServerConnectionName;
//    private readonly ParametersManager _parametersManager;
//    private readonly string _databaseWebAgentName;

//    public ServerSideBackupPathCounter(ILogger logger, ParametersManager parametersManager, string databaseWebAgentName,
//      string databaseServerConnectionName)
//    {
//      _logger = logger;
//      _databaseServerConnectionName = databaseServerConnectionName;
//      _parametersManager = parametersManager;
//      _databaseWebAgentName = databaseWebAgentName;
//    }

//    public string Count(string serverName)
//    {
//      ApAgentParameters parameters = (ApAgentParameters) _parametersManager.Parameters;

//      DatabaseAgentClient dac = DatabaseAgentClientsFabric.CreateAgentClient(_logger, _databaseWebAgentName,
//        new WebAgents(parameters.WebAgents), _databaseServerConnectionName,
//        new DatabaseServerConnections(parameters.DatabaseServerConnections));

//      DbServerInfo dbServerInfo = dac?.GetDatabaseServerInfo(serverName).Result;
//      if (dbServerInfo?.BackupDirectory != null)
//        return dbServerInfo.BackupDirectory;
//      bool isServerLocal = dac?.IsServerLocal() ?? false;
//      return isServerLocal ? GetWorkFolderCandidate(parameters) : null;
//    }


//    private string GetWorkFolderCandidate(ApAgentParameters parameters)
//    {
//      FileInfo pf = new FileInfo(_parametersManager.ParametersFileName);
//      string workFolder = parameters.WorkFolder ?? pf.Directory?.FullName;
//      string workFolderCandidate = workFolder == null ? string.Empty : Path.Combine(workFolder, CDatabaseBackupFiles);
//      return workFolderCandidate;
//    }

//  }
//}

