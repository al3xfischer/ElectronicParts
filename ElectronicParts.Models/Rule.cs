// ***********************************************************************
// Assembly         : ElectronicParts.Models
// Author           : Peter Helf
// ***********************************************************************
// <copyright file="Rule.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the Rule class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Models
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents the Rule class of the ElectronicParts program.
    /// </summary>
    /// <typeparam name="T">The type of connection that the rule applies to.</typeparam>
    [DataContract]
    public class Rule<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rule{T}"/> class.
        /// </summary>
        /// <param name="value">The value which decides whether the rule is active or not.</param>
        /// <param name="color">The color of the connection when the rule is active.</param>
        public Rule(T value, string color, Func<T,bool> valueValidation)
        {
            this.valueValidation = valueValidation;
            this.Value = value;
            this.Color = color;
        }

        /// <summary>
        /// Gets or sets the value which decides whether the rule is active or not.
        /// </summary>
        /// <value>The value which decides whether the rule is active or not.</value>
        [DataMember]
        public T Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (this.valueValidation(value))
                {
                    this.value = value;
                }
                else
                {
                    throw new ArgumentException(nameof(value));
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of the connection when the rule is active.
        /// </summary>
        /// <value>The color of the connection when the rule is active.</value>
        [DataMember]
        public string Color { get; set; }

        private readonly Func<T,bool> valueValidation;
        private T value;
    }
}
