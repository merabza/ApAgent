using System;
using CliMenu;
using LibApAgentData.Models;
using LibDataInput;
using LibParameters;
using SystemToolsShared;

namespace ApAgent.MenuCommands;

public sealed class ClearStepsCommand : CliMenuCommand
{
    private readonly ParametersManager _parametersManager;

    public ClearStepsCommand(ParametersManager parametersManager) : base("Clear Steps")
    {
        _parametersManager = parametersManager;
    }


    protected override void RunAction()
    {
        try
        {
            MenuAction = EMenuAction.Reload;
            if (!Inputer.InputBool("Clear Steps, are you sure?", false, false))
                return;

            var parameters = (ApAgentParameters)_parametersManager.Parameters;

            parameters.ClearSteps();
            _parametersManager.Save(parameters, "Steps cleared success");
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