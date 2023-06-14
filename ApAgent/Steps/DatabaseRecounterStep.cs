//using LibApAgentData.Models;
//using LibApAgentData.Steps;
//using LibToolActions.BackgroundTasks;
//using Microsoft.Extensions.Logging;

//namespace ApAgent.Steps
//{
//    //FIXME ბაზის გადათვლის ნაბიჯი არ გამოიყენება
//    public sealed class DatabaseReCounterStep : JobStep
//    {
//        //თუ ბაზასთან დასაკავშირებლად ვიყენებთ პირდაპირ კავშირს, მაშინ ვებაგენტი აღარ გამოიყენება და პირიქით
//        public string DatabaseServerConnectionName { get; set; } //ბაზასთან დაკავშირების პარამეტრების ჩანაწერის სახელი
//        public string DatabaseWebAgentName { get; set; } //შეიძლება ბაზასთან დასაკავშირებლად გამოვიყენოთ ვებაგენტი
//        public string DatabaseServerName { get; set; } //გამოიყენება მხოლოდ იმ შემთხვევაში თუ ვიყენებთ WebAgent-ს

//        public string DatabaseName { get; set; } //მონაცემთა ბაზის სახელი
//        public string ReCounterTypeName { get; set; } //გადათვლის ტიპის სახელი


//        public override ProcessesToolAction GetToolAction(ILogger logger, ProcessManager processManager,
//            ApAgentParameters parameters, string procLogFilesFolder)
//        {
//            //FIXME მეთოდი არ არის რეალიზებული
//            return null;
//        }

//    }

//}

