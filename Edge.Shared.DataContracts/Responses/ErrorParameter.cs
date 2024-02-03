using Edge.Shared.DataContracts.Constants;
using System.Diagnostics.CodeAnalysis;

namespace Edge.Shared.DataContracts.Responses
{
    /// <summary>
    /// Represents an error parameter in service responses if error occurs
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ErrorParameter
    {
        #region Properties
        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>
        /// The name of the error parameter.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the value of the error parameter.
        /// </summary>
        /// <value>
        /// The value of the error parameter as string or null if there is no value.
        /// </value>
        public string Value { get; }
        #endregion

        #region Constructor

        public ErrorParameter() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorParameter"/> with specified name and value.
        /// </summary>
        /// <param name="name">The name of the error parameter.</param>
        /// <param name="value">The value of the error parameter.</param>
        public ErrorParameter(string name, string value)
        {
            if (name == null)
                throw new ArgumentNullException("name", AppMessageConstants.TheNameCannotBeNull);
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(AppMessageConstants.TheNameCannotBeEmpty, "name");

            Name = name;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorParameter"/> with no value.
        /// </summary>
        /// <param name="name">The name.</param>
        public ErrorParameter(string name) : this(name, null)
        {
        }

        #endregion

        #region Equality

        /// <summary>
        /// Checks whether the instances are equal
        /// </summary>
        /// <param name="other">The error parameter to compare equality.</param>
        /// <returns>
        ///  <c>True</c> if name and value are same, otherwise <c>false</c>
        /// </returns>
        protected bool Equals(ErrorParameter other)
        {
            return string.Equals(Name, other.Name) && string.Equals(Value, other.Value);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is of type <see cref="ErrorParameter"/> and <see cref="Equals(ErrorParameter)"/> returns true; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ErrorParameter)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }

        #endregion
    }
}
