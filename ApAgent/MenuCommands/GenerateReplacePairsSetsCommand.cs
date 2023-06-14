//using System;
//using SystemToolsShared;
//using ApAgent.Generators;
//using CliMenu;
//using CliParameters;
//using LibApAgentData.Models;
//using LibDataInput;

//namespace ApAgent.MenuCommands
//{

//    public sealed class GenerateReplacePairsSetsCommand : CliMenuCommand
//    {

//        private readonly IParametersManager _parametersManager;

//        public GenerateReplacePairsSetsCommand(IParametersManager parametersManager)
//        {
//            _parametersManager = parametersManager;
//        }

//        public override bool Run()
//        {
//            MenuAction = EMenuAction.Reload;
//            ApAgentParameters parameters = (ApAgentParameters)_parametersManager.Parameters;
//            try
//            {

//                if (!Inputer.InputBool("This process will change Replace Pair Sets, are you sure?", false, false))
//                    return false;

//                ReplacePairSetsGenerator standardReplacePairSetsGenerator = new ReplacePairSetsGenerator(_parametersManager); 
//                standardReplacePairSetsGenerator.Generate();

//                //შენახვა
//                _parametersManager.Save(parameters, "Replace Pair Sets generated success");

//                return true;
//            }
//            catch (DataInputEscapeException)
//            {
//                Console.WriteLine();
//                Console.WriteLine("Escape... ");
//                StShared.Pause();
//            }
//            catch (Exception e)
//            {
//                StShared.WriteException(e, true);
//            }

//            return false;
//        }

//    }

//}

