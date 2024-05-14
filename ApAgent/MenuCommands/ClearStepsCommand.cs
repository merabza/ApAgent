using CliMenu;
using LibApAgentData.Models;
using LibDataInput;
using LibParameters;

namespace ApAgent.MenuCommands;

public sealed class ClearStepsCommand : CliMenuCommand
{
    private readonly ParametersManager _parametersManager;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ClearStepsCommand(ParametersManager parametersManager) : base("Clear Steps")
    {
        _parametersManager = parametersManager;
    }


    protected override void RunAction()
    {
        MenuAction = EMenuAction.Reload;
        if (!Inputer.InputBool("Clear Steps, are you sure?", false, false))
            return;

        var parameters = (ApAgentParameters)_parametersManager.Parameters;

        parameters.ClearSteps();
        _parametersManager.Save(parameters, "Steps cleared success");
    }
}