// ***********************************************************************
// Assembly         : Shared
// Author           : 
// ***********************************************************************
// <copyright file="ExampleValue.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the ExampleValue class.</summary>
// ***********************************************************************

using System;

namespace Shared
{
    /// <summary>
    ///     An example implementation of the <see cref="IValueGeneric{T}" /> interface.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    [Serializable]
    public class ExampleValue<T> : IValueGeneric<T>
    {
        /// <summary>
        ///     Gets or sets the current value.
        /// </summary>
        /// <value>The current value.</value>
        public T Current { get; set; }

        /// <summary>
        ///     Gets or sets the current value.
        /// </summary>
        /// <value>The current value.</value>
        object IValue.Current
        {
            get => Current;

            set => Current = (T) value;
        }
    }
}