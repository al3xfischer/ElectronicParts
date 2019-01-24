// ***********************************************************************
// Author           : Kevin Janisch, Roman Jahn
// ***********************************************************************
// <copyright file="Pin.cs" company="FHWN">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>Represents the Pin class of the ElectronicParts Programm</summary>
// ***********************************************************************

namespace ElectronicParts.Models
{
    using System;
    using System.Diagnostics;
    using Shared;

    /// <summary>
    /// Represents the <see cref="Pin{T}"/> class.
    /// </summary>
    /// <typeparam name="T">The value type of the pin.</typeparam>
    [Serializable]
    public class Pin<T> : IPinGeneric<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pin{T}"/> class.
        /// </summary>
        public Pin()
        {
            this.Value = new Value<T>();
        }

        /// <summary>
        /// Gets or sets the value of the pin.
        /// </summary>
        /// <value>The value of the pin.</value>
        public IValueGeneric<T> Value { get; set; }

        /// <summary>
        /// Gets or sets the label of the pin.
        /// </summary>
        /// <value>The label of the pin.</value>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the value of <see cref="IPin.Value"/>.
        /// </summary>
        /// <value>The value of <see cref="IPin.Value"/>.</value>
        IValue IPin.Value
        {
            get
            {
                return this.Value;
            }

            set
            {
                try
                {
                    this.Value = (IValueGeneric<T>)value;
                }
                catch (InvalidCastException e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }
    }
}
