namespace Edge.Shared.DataContracts.Responses
{
    public interface IResponseWithErrors
    {
        /// <summary>
        /// Gets or sets the error parameter collection if error occurred, otherwise null.
        /// </summary>
        /// <value>
        /// The error parameters.
        /// </value>
        List<ErrorParameter> ErrorParameters { get; set; }

        /// <summary>
        /// Gets the error parameter value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        string GetErrorParameterValue(string name);

        /// <summary>
        /// Adds the error parameter to an error parameters collection. Creates the collection if it doesn't exist
        /// </summary>
        /// <param name="errParam">The error parameter.</param>
        /// <returns>
        /// Returns instance of the <see cref="IResponseWithErrors"/> on which the AddErrorParameter was called
        /// to support method call chaining.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ErrorParameter is null</exception>
        IResponseWithErrors AddErrorParameter(ErrorParameter errParam);

        /// <summary>
        /// Adds the error parameter to the error parameters collection. Creates the collection if it doesn't exist.
        /// The value> will be converted to string using invariant culture.
        /// </summary>
        /// <param name="name">The name of the error parameter.</param>
        /// <param name="value">The value of the error parameter - will be converted to invariant string.</param>
        /// <returns>
        /// Returns instance of the <see cref="IResponseWithErrors"/> on which the AddErrorParameter was called
        /// to support method call chaining.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when name is null</exception>
        /// <exception cref="System.ArgumentException">Thrown when name is empty or whitespace</exception>
        IResponseWithErrors AddErrorParameter(string name, object value);

        /// <summary>
        /// Adds the error parameter to the error parameters collection. Creates the collection if it doesn't exist.
        /// </summary>
        /// <param name="name">The name of the error parameter.</param>
        /// <param name="value">The value of the error parameter.</param>
        /// <returns>
        /// Returns instance of the <see cref="IResponseWithErrors"/> on which the AddErrorParameter was called
        /// to support method call chaining.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when name is null</exception>
        /// <exception cref="System.ArgumentException">Thrown when name is empty or whitespace</exception>
        IResponseWithErrors AddErrorParameter(string name, string value);
    }
}
