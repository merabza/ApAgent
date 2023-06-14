//using System;
//using SystemToolsShared;
//using CliParameters;
//using CliShared;
//using CliShared.DataInput;
//using LibApAgentData.Models;
//using LibApAgentData.PathCounters;

//namespace ApAgent.MenuCommands
//{

//  internal class ProcLogFileFolderCommand : CliMenuCommand
//  {
//    private readonly ParametersManager _parametersManager;

//    public ProcLogFileFolderCommand(ParametersManager parametersManager) : base("Process Log Files Folder")
//    {
//      _parametersManager = parametersManager;
//    }


//    public override bool Run()
//    {
//      try
//      {
//        FolderPathInput folderInput = new FolderPathInput(Name, GetProcLogFileFolder());
//        if (!folderInput.DoInput())
//          return false;

//        ApAgentParameters parameters = (ApAgentParameters)_parametersManager.Parameters;
//        parameters.WorkFolder = folderInput.FolderPath;
//        _parametersManager.Save(_parametersManager.Parameters, "Work Folder Updated success");

//        MenuAction = EMenuAction.Reload;
//        return true;
//      }
//      catch (DataInputEscapeException)
//      {
//        Console.WriteLine();
//        Console.WriteLine("Escape... ");
//        StShared.Pause();
//        return false;
//      }
//      catch (Exception e)
//      {
//        StShared.WriteException(e);
//        return false;
//      }
//    }

//    private string GetProcLogFileFolder()
//    {
//      ApAgentParameters parameters = (ApAgentParameters)_parametersManager.Parameters;
//      if (!string.IsNullOrWhiteSpace(parameters.ProcLogFilesFolder)) 
//        return parameters.ProcLogFilesFolder;

//      LocalPathCounter procLogFilesPathCounter =
//        LocalPathCounterFabric.CreateProcLogFilesPathCounter(parameters, _parametersManager.ParametersFileName);
//      return procLogFilesPathCounter.Count(null);

//    }

//    protected override string GetStatus()
//    {
//      return GetProcLogFileFolder();
//    }

//  }

//}

