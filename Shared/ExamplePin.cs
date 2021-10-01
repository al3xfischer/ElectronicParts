// ***********************************************************************
// Assembly         : Shared
// Author           : 
// ***********************************************************************
// <copyright file="ExamplePin.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the ExamplePin class.</summary>
// ***********************************************************************

using System;
using System.Diagnostics;

namespace Shared
{
    /// <summary>
    ///     An example implementation of the <see cref="IPinGeneric{T}" /> interface.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    [Serializable]
    public class ExamplePin<T> : IPinGeneric<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ExamplePin{T}" /> class.
        /// </summary>
        public ExamplePin()
        {
            Value = new ExampleValue<T>();
        }

        /// <summary>
        ///     Gets or sets the label of the pin.
        /// </summary>
        /// <value>The label of the pin.</value>
        public string Label { get; set; }

        /// <summary>
        ///     Gets or sets the value of the pin.
        /// </summary>
        /// <value>The value of the pin.</value>
        public IValueGeneric<T> Value { get; set; }

        /// <summary>
        ///     Gets or sets the value of the pin.
        /// </summary>
        /// <value>The value of the pin.</value>
        IValue IPin.Value
        {
            get => Value;

            set
            {
                try
                {
                    Value = (IValueGeneric<T>) value;
                }
                catch (InvalidCastException e)
                {
                    Value = new ExampleValue<T>();
                    Debug.WriteLine(e.Message);
                }
            }
        }
    }
}