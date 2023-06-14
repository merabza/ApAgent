//using System;
//using System.IO;
//using SystemToolsShared;
//using CliParameters;
//using CliShared;
//using CliShared.DataInput;
//using LibApAgentData.Models;

//namespace ApAgent.MenuCommands
//{

//  internal class LogFolderCommand : CliMenuCommand
//  {
//    private readonly ParametersManager _parametersManager;

//    public LogFolderCommand(ParametersManager parametersManager) : base("Log Folder")
//    {
//      _parametersManager = parametersManager;
//    }


//    public override bool Run()
//    {
//      try
//      {
//        FolderPathInput folderInput = new FolderPathInput(Name, GetLogFolder());
//        if (!folderInput.DoInput())
//          return false;

//        ApAgentParameters parameters = (ApAgentParameters)_parametersManager.Parameters;
//        parameters.LogFolder = folderInput.FolderPath;
//        _parametersManager.Save(_parametersManager.Parameters, "Log Folder Updated success");

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

//    public override CliMenuSet GetSubmenu()
//    {
//      return null;
//    }


//    private string GetLogFolder()
//    {
//      ApAgentParameters parameters = (ApAgentParameters)_parametersManager.Parameters;
//      FileInfo pf = new FileInfo(_parametersManager.ParametersFileName);
//      return parameters.LogFolder ?? pf.Directory?.FullName;
//    }


//    protected override string GetStatus()
//    {
//      return GetLogFolder();
//    }

//  }


//}

