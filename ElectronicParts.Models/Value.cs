// ***********************************************************************
// Assembly         : ElectronicParts.Models
// Author           : Kevin Janisch, Roman Jahn
// ***********************************************************************
// <copyright file="Value.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the Value class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Models
{
    using System;
    using Shared;

    /// <summary>
    /// Represents the <see cref="Value{T}"/> class.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    [Serializable]
    public class Value<T> : IValueGeneric<T>
    {
        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        /// <value>The current value.</value>
        public T Current { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IValue.Current"/>
        /// </summary>
        /// <value>The <see cref="IValue.Current"/>.</value>
        object IValue.Current
        {
            get
            {
                return this.Current;
            }

            set
            {
                this.Current = (T)value;
            }
        }
    }
}
