////Created by ProjectMainClassCreator at 1/14/2021 15:15:01

//using System;
//using System.IO;
//using SystemToolsShared;
//using CliParameters;
//using CliShared;
//using CliShared.DataInput;
//using LibApAgentData.Models;

//namespace ApAgent.MenuCommands
//{
//  internal class WorkFolderCommand : CliMenuCommand
//  {
//    private readonly ParametersManager _parametersManager;

//    public WorkFolderCommand(ParametersManager parametersManager) : base("Work Folder")
//    {
//      _parametersManager = parametersManager;
//    }


//    public override bool Run()
//    {
//      try
//      {
//        FolderPathInput folderInput = new FolderPathInput(Name, GetWorkFolder());
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

//    private string GetWorkFolder()
//    {
//      ApAgentParameters parameters = (ApAgentParameters)_parametersManager.Parameters;
//      FileInfo pf = new FileInfo(_parametersManager.ParametersFileName);
//      return parameters.WorkFolder ?? pf.Directory?.FullName;
//    }

//    protected override string GetStatus()
//    {
//      return GetWorkFolder();
//    }

//  }
//}

