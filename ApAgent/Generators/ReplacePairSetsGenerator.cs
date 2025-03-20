//using System.Collections.Generic;
//using CliParameters;
//using LibApAgentData.Models;

//namespace ApAgent.Generators
//{

//    public sealed class ReplacePairSetsGenerator
//    {
//        private readonly IParametersManager _parametersManager;

//        public string ExcludeSetGitBinObjName { get; private set; } //GitBinObj

//        public ReplacePairSetsGenerator(IParametersManager parametersManager)
//        {
//            _parametersManager = parametersManager;
//        }

//        public void Generate()
//        {
//            ApAgentParameters parameters = (ApAgentParameters)_parametersManager.Parameters;

//            //თუ არ არსებობს შეიქმნას სტანდარტული FTP-ს პრობლემური ჩანაცვლებები
//            ExcludeSetGitBinObjName = CreateFtpFileRestrictsReplacePairSet(parameters); //GitBinObj
//        }

//        private string CreateFtpFileRestrictsReplacePairSet(ApAgentParameters parameters)
//        {
//            string replacePairSetName = "FTP File Restricts";
//            if (parameters.ReplacePairsSets.ContainsKey(replacePairSetName))
//                return replacePairSetName;

//            ReplacePairsSet replacePairsSet = new ReplacePairsSet
//                { PairsDict = new Dictionary<string, string> { { "...", "." } } };
//            if (!parameters.ReplacePairsSets.ContainsKey(replacePairSetName))
//                parameters.ReplacePairsSets.Add(replacePairSetName, replacePairsSet);
//            return replacePairSetName;
//        }

//    }

//}

