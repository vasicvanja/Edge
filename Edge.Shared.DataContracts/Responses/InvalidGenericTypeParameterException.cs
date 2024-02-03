using System.Diagnostics.CodeAnalysis;

namespace Edge.Shared.DataContracts.Responses
{
    /// <summary>
    /// Thrown when the generic type parameter is invalid. For example in <c>"ATS.Bus.DataContracts.Responses.BaseResponse"</c> we have
    /// no chance to restrict the type parameter to Enum type, thus we check it in constructor. The same goes for restricting the generic param to delegate.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class InvalidGenericTypeParameterException : JsonColumnsException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidGenericTypeParameterException"/> class.
        /// </summary>
        public InvalidGenericTypeParameterException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidGenericTypeParameterException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidGenericTypeParameterException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidGenericTypeParameterException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public InvalidGenericTypeParameterException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion
    }
}
