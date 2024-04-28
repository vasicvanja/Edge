namespace Edge.Shared.DataContracts.Resources
{
    public class ResponseMessages
    {
        public const string InvalidModelState = "Invalid parameters.";
        public const string NonExistingRole = "The role does not exist!";
        public const string SuccessfulUserCreation = "User successfully registered!";
        public const string UnsuccessfulUserCreation = "User can't be registered!";
        public const string UserDoesNotExist = "User does not exist, please register in order to login!";
        public const string UserExists = "User already exists!";
        public const string InvalidLoginPassword = "Wrong password, please try again!";
        public const string SuccessfulLogin = "Welcome to Edge!";
        public const string SuccessfulUserLogout = "Successfully logged out!";
        public const string NoDataFoundForKey = "No data found for entity of type {0} with key {1}!";
        public const string NoDataFound = "No data found!";
        public const string GetEntityFailed = "Getting entity of type {0} by its key {1} failed!";
        public const string GettingEntitiesFailed = "Getting entites of type {0} failed!";
        public const string DeletionFailed = "Deletion of entity of type {0} by its key {1} failed!";
        public const string UnsuccessfulCreationOfEntity = "Unuccessful creation of entity of type {0}";
        public const string EntityAlreadyExists = "Entity of type {0} already exists with key {1}";
        public const string InvalidInputParameter = "Enter valid parameters for entity of type {0}";
        public const string UnsuccessfulUpdateOfEntity = "Unsuccessful update of entity of type {0}";
        public const string ChangingArtworkTypeNotAllowed = "Changing artwork type not allowed!";
        public const string UnsuccessfulEmailSend = "Sending Mail failed!";
        public const string SmtpSettingsDisabled = "SMTP Settings are currently disabled. Mails cannot be sent!";
        public const string UnsuccessfulCreationOfPasswordResetToken = "Unsuccessful creation of Password Reset Token.";
        public const string UnsuccessfulPasswordReset = "Unsuccessful password reset.";
        public const string InvalidToken = "Invalid Token!";
        public const string SmtpSettingsNotDefined = "SMTP Settings are not defined. Mails cannot be sent!";
        public const string UnsucessfulCheckoutSessionCreation = "Unsucessful creation of Stripe Checkout Session!";
        public const string EmptyStripeWebhookJson = "Empty Stripe Webhook JSON!";
        public const string UnhandledStripeEvent = "Stripe Checkout Session not completed.";
    }
}