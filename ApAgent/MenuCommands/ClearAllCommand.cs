using CliMenu;
using LibApAgentData.Models;
using LibDataInput;
using LibParameters;

namespace ApAgent.MenuCommands;

public sealed class ClearAllCommand : CliMenuCommand
{
    private readonly ParametersManager _parametersManager;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ClearAllCommand(ParametersManager parametersManager) : base("Clear All", EMenuAction.Reload)
    {
        _parametersManager = parametersManager;
    }


    protected override bool RunBody()
    {
        if (!Inputer.InputBool("Clear All, are you sure?", false, false))
            return false;

        var parameters = (ApAgentParameters)_parametersManager.Parameters;

        parameters.ClearAll();
        _parametersManager.Save(parameters, "Data cleared success");
        return true;
    }
}