namespace CustomValidation.Interface
{
    public interface IEmailValidator
    {
        /// <summary>
        /// Checks whether a supplied string is a valid email address
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        bool IsValidEmail(string email);
    }
}
