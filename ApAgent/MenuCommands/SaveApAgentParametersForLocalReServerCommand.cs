using CliMenu;
using LibApAgentData.Models;
using LibDataInput;
using LibParameters;
using System.IO;
using SystemToolsShared;

namespace ApAgent.MenuCommands;

public sealed class SaveApAgentParametersForLocalReServerCommand : CliMenuCommand
{
    private readonly ParametersManager _parametersManager;

    // ReSharper disable once ConvertToPrimaryConstructor
    public SaveApAgentParametersForLocalReServerCommand(ParametersManager parametersManager) : base(
        "Save ApAgent Parameters For Local ReServer", EMenuAction.Reload)
    {
        _parametersManager = parametersManager;
    }

    protected override bool RunBody()
    {

        var parameters = (ApAgentParameters)_parametersManager.Parameters;

        //შევამოწმოთ პარამეტრებში გვაქვს თუ არა შევსებული პარამეტრების ფაილის სახელი რესერვერისათვის ApAgentParametersFileNameForLocalReServer
        //თუ პარამეტრი არ არსებობს, გამოვიტანოთ შესაბამისი შეტყობინება და გავჩერდეთ
        if (string.IsNullOrWhiteSpace(parameters.ApAgentParametersFileNameForLocalReServer))
        {
            StShared.WriteErrorLine("file name for local reServer Parameters is empty. please enter it first", true);
            return false;
        }

        //შევამოწმოთ არსებობს თუ არა უკვე ეს ფაილი.
        //თუ უკვე არსებობს გამოვიტანოთ შეკითხვა იმის შესახებ, გადავაწეროთ თუ არა
        //თუ პასუხი უარყოფითი იქნება, გავჩერდეთ
        if (File.Exists(parameters.ApAgentParametersFileNameForLocalReServer) &&
            !Inputer.InputBool("Parameters file already exists, Rewrite?", false, false))
            return false;

        //შევინახოთ პარამეტრების ფაილი
        _parametersManager.Save(parameters, "Parameters for ReServer Saved",
            parameters.ApAgentParametersFileNameForLocalReServer);

        return true;
    }
}