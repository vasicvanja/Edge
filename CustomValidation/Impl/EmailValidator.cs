using CustomValidation.Interface;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CustomValidation.Impl
{
    public class EmailValidator : IEmailValidator
    {
        bool _invalid;

        private const string EmailValidationRexEx = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-_\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

        /// <summary>
        /// Checks whether a supplied string is a valid email address
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public bool IsValidEmail(string strIn)
        {
            _invalid = false;
            if (string.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper);
            if (_invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn, EmailValidationRexEx, RegexOptions.IgnoreCase);
        }

        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                _invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
    }
}