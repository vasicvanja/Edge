namespace Edge.Shared.DataContracts.Resources
{
    public class ResponseMessages
    {
        public static string InvalidModelState = "Invalid parameters.";
        public static string NonExistingRole = "The role does not exist!";
        public static string SuccessfulUserCreation = "User successfully registered!";
        public static string UnsuccessfulUserCreation = "User can't be registered!";
        public static string UserDoesNotExist = "User does not exist, please register in order to login!";
        public static string UserExists = "User already exists!";
        public static string InvalidLoginPassword = "Wrong password, please try again!";
        public static string SuccessfulLogin = "Welcome to Edge!";
        public static string SuccessfulUserLogout = "Successfully logged out!";
        public static string SuccessfullyRetrievedEntity = "Successfully retrieved an entity of type {0}";
        public static string NoDataFoundForKey = "No data found for entity of type {0} with key {1}!";
        public static string NoDataFound = "No data found!";
        public static string GetEntityFailed = "Getting entity of type {0} by its key {1} failed!";
        public static string GettingEntitiesFailed = "Getting entites of type {0} failed!";
        public static string DeletionFailed = "Deletion of entity of type {0} by its key {1} failed!";
        public static string SuccessfulCreationOfEntity = "Successful creation of entity of type {0}";
        public static string UnsuccessfulCreationOfEntity = "Unuccessful creation of entity of type {0}";
        public static string EntityAlreadyExists = "Entity of type {0} already exists with key {1}";
        public static string InvalidInputParameter = "Enter valid parameters for entity of type {0}";
        public static string SuccessfulUpdateOfEntity = "Successful update of entity of type {0}";
        public static string UnsuccessfulUpdateOfEntity = "Unsuccessful update of entity of type {0}";
        public static string ChangingArtworkTypeNotAllowed = "Changing artwork type not allowed!";
        public static string SuccessfullyRetrievedEntities = "Successfully retrieved entities!";
        public static string InvalidRegisterPassword = "Your registration was unsuccessful, please make sure that your password contains one capital letter, one number and one symbol!";
    }
}
