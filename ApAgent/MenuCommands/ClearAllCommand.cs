using System.Threading;
using System.Threading.Tasks;
using ApAgentData.LibApAgentData.Models;
using AppCliTools.CliMenu;
using AppCliTools.LibDataInput;
using ParametersManagement.LibParameters;

namespace ApAgent.MenuCommands;

public sealed class ClearAllCommand : CliMenuCommand
{
    private readonly ParametersManager _parametersManager;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ClearAllCommand(ParametersManager parametersManager) : base("Clear All", EMenuAction.Reload)
    {
        _parametersManager = parametersManager;
    }

    protected override async ValueTask<bool> RunBody(CancellationToken cancellationToken = default)
    {
        if (!Inputer.InputBool("Clear All, are you sure?", false, false))
        {
            return false;
        }

        var parameters = (ApAgentParameters)_parametersManager.Parameters;

        parameters.ClearAll();
        await _parametersManager.Save(parameters, "Data cleared success");
        return true;
    }
}
