using CliMenu;
using LibApAgentData.Models;
using LibDataInput;
using LibParameters;

namespace ApAgent.MenuCommands;

public sealed class ClearAllCommand : CliMenuCommand
{
    private readonly ParametersManager _parametersManager;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ClearAllCommand(ParametersManager parametersManager) : base("Clear All")
    {
        _parametersManager = parametersManager;
    }


    protected override void RunAction()
    {
        MenuAction = EMenuAction.Reload;
        if (!Inputer.InputBool("Clear All, are you sure?", false, false))
            return;

        var parameters = (ApAgentParameters)_parametersManager.Parameters;

        parameters.ClearAll();
        _parametersManager.Save(parameters, "Data cleared success");
    }
}