//using ApAgent.MenuCommands;
//using CliMenu;
//using CliParameters;
//using CliParametersDataEdit.Cruders;
//using Microsoft.Extensions.Logging;

//namespace ApAgent.Cruders;

//public sealed class DatabaseServerConnectionCruderWithStandardGeneration : DatabaseServerConnectionCruder
//{
//    public DatabaseServerConnectionCruderWithStandardGeneration(ParametersManager parametersManager, ILogger logger)
//        :
//        base(parametersManager, logger)
//    {
//    }

//    public override void FillDetailsSubMenu(CliMenuSet itemSubMenuSet, string recordName)
//    {
//        base.FillDetailsSubMenu(itemSubMenuSet, recordName);
//        var genSchemaCommand =
//            new GenerateStandardMaintenanceSchemaCommand(Logger, ParametersManager, recordName);
//        itemSubMenuSet.AddMenuItem(genSchemaCommand, "Generate standard maintenance schema...");
//    }
//}

