using Edge.Shared.DataContracts.Constants;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace Edge.Shared.DataContracts.Responses
{
    /// <summary>
    /// This is a base response class which should be used as a base class for
    /// all return types of web services public methods. It includes response code, which will help the
    /// cosuming application to easily understand the result of web service call. If some errors occur,
    /// use ErrorParameters collection to add them
    /// <remarks>
    /// Restricting the generic type to Enum type is tricky in .NET, because where T : Enum
    /// simply doesn't work. The best option is to use where T : struck, IConvertible and
    /// then in constructor check, wheter the type is enum type.
    /// Source: http://stackoverflow.com/questions/79126/create-generic-method-constraining-t-to-an-enum
    /// </remarks>
    /// </summary>
    /// <typeparam name="TResponseEnum">The response code enumeration.</typeparam>
    /// <exception cref="ArgumentException">Thrown during construction, if the generic parameter TResponseEnum is not of clr type Enum</exception>
    [ExcludeFromCodeCoverage]
    public abstract class BaseResponse<TResponseEnum> : IResponseWithErrors where TResponseEnum : struct, IConvertible
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResponse{TResponseEnum}"/> class.
        /// </summary>
        protected BaseResponse()
        {
            //we need to check that the TResponseEnum is really an enum type, because we cannot restrict it with the generic parameter
            if (!typeof(TResponseEnum).GetTypeInfo().IsEnum)
                throw new InvalidGenericTypeParameterException(AppMessageConstants.ResponseEnumMustBeOfTypeEnum);
            _responseCodeValue = 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the error parameter collection if error occurred, otherwise null.
        /// </summary>
        /// <value>
        /// The error parameters.
        /// </value>
        public List<ErrorParameter> ErrorParameters { get; set; }

        /// <summary>
        /// The response code enumeration stored as integer, which is then passed over wire.
        /// </summary>
        private int _responseCodeValue;

        /// <summary>
        /// Always! Always set the status code of the result, otherwise getting the StatusCode
        /// will throw an exception.
        /// Gets or sets the status code enum. This methods internally holds the enum type
        /// as a Int32 value, which is passed through the wire, instead of the enum type itself
        /// to prevent well known problem of WCF sharing Enums.
        /// P.S.: Always start your enums with 1, instead of zero, because zero is ints default
        /// value or use zero as "Undefined" enumeration.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <value>
        /// The status code.
        /// </value>
        public TResponseEnum ResponseCode
        {
            get
            {
                return (TResponseEnum)Enum.ToObject(typeof(TResponseEnum), _responseCodeValue);
            }
            set
            {
                _responseCodeValue = (int)Convert.ChangeType(value, typeof(int));
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get the value for certain error name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetErrorParameterValue(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(AppMessageConstants.ValueNameCannotBeNullOrEmpty);

            if (ErrorParameters == null || ErrorParameters.Count == 0)
            {
                return null;
            }

            var errorParam = ErrorParameters.FirstOrDefault(x => x.Name == name);
            return errorParam?.Value;
        }

        /// <summary>
        /// Get the list of errors in reportable one string version
        /// </summary>
        public string GetErrorParameters
        {
            get
            {
                if (ErrorParameters == null || ErrorParameters.Count == 0)
                    return string.Empty;

                return string.Join("\n", ErrorParameters.Select(r => r.Name + " [" + r.Value + "]"));
            }
        }

        /// <summary>
        /// Adds the error parameter to an error parameters collection. Creates the collection if it doesnt exist
        /// </summary>
        /// <param name="errParam">The error parameter.</param>
        /// <returns>
        /// Returns instance of the <see cref="IResponseWithErrors"/> on which the AddErrorParameter was called
        /// to support method call chaining.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when ErrorParameter is null</exception>
        public IResponseWithErrors AddErrorParameter(ErrorParameter errParam)
        {
            if (errParam == null) throw new ArgumentNullException("errParam",
               AppMessageConstants.TheErrorParameterCannotBeNull);

            if (ErrorParameters == null)
            {
                ErrorParameters = new List<ErrorParameter> { errParam };
            }
            else
            {
                ErrorParameters.Add(errParam);
            }

            //return this object to support add error parameter method call chaining
            return this;
        }

        /// <summary>
        /// Adds the error parameter to the error parameters collection. Creates the collection if it doesnt exist.
        /// The value will be converted to string using invariant culture.
        /// </summary>
        /// <param name="name">The name of the error parameter.</param>
        /// <param name="value">The value of the error parameter - will be converted to invariant string.</param>
        /// <returns>
        /// Returns instance of the <see cref="IResponseWithErrors"/> on which the AddErrorParameter was called
        /// to support method call chaining.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when name is null</exception>
        /// <exception cref="System.ArgumentException">Thrown when name is empty or whitespace</exception>
        public IResponseWithErrors AddErrorParameter(string name, object value)
        {
            return AddErrorParameter(new ErrorParameter(name, Convert.ToString(value, CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Adds the error parameter to the error parameters collection. Creates the collection if it doesnt exist.
        /// </summary>
        /// <param name="name">The name of the error parameter.</param>
        /// <param name="value">The value of the error parameter.</param>
        /// <returns>
        /// Returns instance of the <see cref="IResponseWithErrors"/> on which the AddErrorParameter was called
        /// to support method call chaining.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when name is null</exception>
        /// <exception cref="System.ArgumentException">Thrown when name is empty or whitespace</exception>
        public IResponseWithErrors AddErrorParameter(string name, string value)
        {
            if (name == null)
                throw new ArgumentNullException("name", AppMessageConstants.TheNameCannotBeNull);
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(AppMessageConstants.TheNameCannotBeEmpty, "name");

            return AddErrorParameter(new ErrorParameter(name, value));
        }



        #endregion
    }
}
