using System;
using CliMenu;
using LibApAgentData.Models;
using LibDataInput;
using LibParameters;
using SystemToolsShared;

namespace ApAgent.MenuCommands;

public sealed class ClearAllCommand : CliMenuCommand
{
    private readonly ParametersManager _parametersManager;

    public ClearAllCommand(ParametersManager parametersManager) : base("Clear All")
    {
        _parametersManager = parametersManager;
    }


    protected override void RunAction()
    {
        try
        {
            MenuAction = EMenuAction.Reload;
            if (!Inputer.InputBool("Clear All, are you sure?", false, false))
                return;

            var parameters = (ApAgentParameters)_parametersManager.Parameters;

            parameters.ClearAll();
            _parametersManager.Save(parameters, "Data cleared success");
        }
        catch (DataInputEscapeException)
        {
            Console.WriteLine();
            Console.WriteLine("Escape... ");
            StShared.Pause();
        }
        catch (Exception e)
        {
            StShared.WriteException(e, true);
        }
    }
}