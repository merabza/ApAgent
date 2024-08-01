//using ApAgent.Generators;
//using CliMenu;
//using LibApAgentData.Models;
//using LibDataInput;
//using LibParameters;
//using Microsoft.Extensions.Logging;
//using SystemToolsShared;

//namespace ApAgent.MenuCommands;

//public sealed class GenerateStandardMaintenanceSchemaCommand : CliMenuCommand
//{
//    private readonly ILogger _logger;
//    private readonly string? _parametersFileName;
//    private readonly IParametersManager _parametersManager;
//    private readonly string _recordName;

//    // ReSharper disable once ConvertToPrimaryConstructor
//    public GenerateStandardMaintenanceSchemaCommand(ILogger logger, IParametersManager parametersManager,
//        string recordName, string? parametersFileName) : base(null, EMenuAction.Reload)
//    {
//        _logger = logger;
//        _parametersManager = parametersManager;
//        _recordName = recordName;
//        _parametersFileName = parametersFileName;
//    }

//    protected override bool RunBody()
//    {
//        var parameters = (ApAgentParameters)_parametersManager.Parameters;
//        var databaseServerConnections =
//            parameters.DatabaseServerConnections;

//        if (!databaseServerConnections.ContainsKey(_recordName))
//        {
//            StShared.WriteErrorLine($"Database connection with name {_recordName} does not exists. ", true, _logger);
//            return false;
//        }

//        if (!Inputer.InputBool("This process will change jobs, are you sure?", false, false))
//            return false;

//        //აქ ხდება პირდაპირ მონაცემთა ბაზისთვის სტანდარტული მომსახურების გენერირება.
//        //ამიტომ ვებაგენტის სახელი და სერვერის სახელი საჭირო არ არის.
//        //თანაც ApAgent-ში ვებაგენტის გამოყენება საერთოდ არ ხდება
//        StandardJobsSchemaGenerator standardJobsSchemaGenerator =
//            new(true, _logger, _parametersManager, _recordName, _parametersFileName);
//        standardJobsSchemaGenerator.Generate();

//        //შენახვა
//        _parametersManager.Save(parameters, "Maintain schema generated success");
//        return true;
//    }
//}