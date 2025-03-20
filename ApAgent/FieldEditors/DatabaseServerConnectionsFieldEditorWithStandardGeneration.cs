//using ApAgent.MenuCommands;
//using CliMenu;
//using CliParameters;
//using CliParametersDataEdit.FieldEditors;
//using Microsoft.Extensions.Logging;

//namespace ApAgent.FieldEditors;

//public sealed class DatabaseServerConnectionsFieldEditorWithStandardGeneration : DatabaseServerConnectionsFieldEditor
//{
//    public DatabaseServerConnectionsFieldEditorWithStandardGeneration(string propertyName, IParametersManager parametersManager, ILogger logger) : base(propertyName, parametersManager, logger)
//    {
//    }

//    public override CliMenuSet GetSubMenu(object record)
//    {
//        CliMenuSet menuSet = base.GetSubMenu(record);
//        GenerateStandardMaintenanceSchemaCommand genSchemaCommand =
//            new GenerateStandardMaintenanceSchemaCommand(Logger, ParametersManager, recordName);
//        menuSet.AddMenuItem(genSchemaCommand, "Generate standard maintenance schema...");
//        return menuSet;
//    }

//}

