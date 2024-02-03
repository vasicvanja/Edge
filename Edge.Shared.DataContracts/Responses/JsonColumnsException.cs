using System.Diagnostics.CodeAnalysis;

namespace Edge.Shared.DataContracts.Responses
{
    /// <summary>
    /// This is exception thrown by Ats security components to diferentiate from generic exceptions
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class JsonColumnsException : Exception
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AtsException"/> class.
        /// </summary>
        public JsonColumnsException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtsException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public JsonColumnsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtsException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public JsonColumnsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}
