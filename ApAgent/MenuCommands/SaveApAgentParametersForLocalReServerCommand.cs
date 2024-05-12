using System;
using System.IO;
using CliMenu;
using LibApAgentData.Models;
using LibDataInput;
using LibParameters;
using SystemToolsShared;

namespace ApAgent.MenuCommands;

public sealed class SaveApAgentParametersForLocalReServerCommand : CliMenuCommand
{
    private readonly ParametersManager _parametersManager;

    public SaveApAgentParametersForLocalReServerCommand(ParametersManager parametersManager) : base(
        "Save ApAgent Parameters For Local ReServer")
    {
        _parametersManager = parametersManager;
    }


    protected override void RunAction()
    {
        try
        {
            MenuAction = EMenuAction.Reload;

            var parameters = (ApAgentParameters)_parametersManager.Parameters;

            //შევამოწმოთ პარამეტრებში გვაქვს თუ არა შევსებული პარამეტრების ფაილის სახელი რესერვერისათვის ApAgentParametersFileNameForLocalReServer
            //თუ პარამეტრი არ არსებობს, გამოვიტანოთ შესაბამისი შეტყობინება და გავჩერდეთ
            if (string.IsNullOrWhiteSpace(parameters.ApAgentParametersFileNameForLocalReServer))
            {
                StShared.WriteErrorLine("file name for local reServer Parameters is empty. please enter it first",
                    true);
                return;
            }

            //შევამოწმოთ არსებობს თუ არა უკვე ეს ფაილი.
            //თუ უკვე არსებობს გამოვიტანოთ შეკითხვა იმის შესახებ, გადავაწეროთ თუ არა
            //თუ პასუხი უარყოფითი იქნება, გავჩერდეთ
            if (File.Exists(parameters.ApAgentParametersFileNameForLocalReServer))
                if (!Inputer.InputBool("Parameters file already exists, Rewrite?", false, false))
                    return;

            //შევინახოთ პარამეტრების ფაილი
            _parametersManager.Save(parameters, "Parameters for ReServer Saved",
                parameters.ApAgentParametersFileNameForLocalReServer);
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