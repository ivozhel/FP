namespace PearlyWhites.Models.Models.Tools
{
    public static class ResponseMessages
    {
        public static string SomethingWentWrong = "Something went wrong try again";
        public static string FalseId = "Id cannot be 0 or less";
        public static string Success = "Successfull";

        public static string NotFound(string type) => $"{type} not found";
        public static string Successfull(string operation, string type) => $"Successfully {operation}ed {type}";
        public static string CantLoad(string type, string forType, int forTypeID) => $"{type} can not be loaded for {forType} with ID {forTypeID}";
        public static string DoseNotHave(string type, string doseNotHaveType) => $"{type} have no {doseNotHaveType}";
        public static string AlreadyExists(string type) => $"{type} already exists";
    }
}
