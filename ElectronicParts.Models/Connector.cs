// ***********************************************************************
// Assembly         : ElectronicParts.Models
// Author           : 
// ***********************************************************************
// <copyright file="Connector.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the Connector class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Models
{
    using System;
    using System.Linq;
    using Shared;

    /// <summary>
    /// Represents a connection between two pins. 
    /// </summary>
    [Serializable]
    public class Connector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Connector"/> class.
        /// </summary>
        /// <param name="input">The input pin.</param>
        /// <param name="output">The output pin.</param>
        /// <param name="commonVal">The value shared between the two pins.</param>
        public Connector(IPin input, IPin output, IValue commonVal)
        {
            this.OutputPin = output ?? throw new ArgumentNullException(nameof(output));
            this.InputPin = input ?? throw new ArgumentNullException(nameof(input));
            this.CommonValue = commonVal ?? throw new ArgumentNullException(nameof(commonVal));
        }
        
        /// <summary>
        /// Gets the output pin.
        /// </summary>
        /// <value>The output pin.</value>
        public IPin OutputPin { get; private set; }

        /// <summary>
        /// Gets the input pin.
        /// </summary>
        /// <value>The input pin.</value>
        public IPin InputPin { get; private set; }

        /// <summary>
        /// Gets the shared value of the two pins.
        /// </summary>
        /// <value>The value shared between the two pins.</value>
        public IValue CommonValue { get; private set; }

        /// <summary>
        /// Resets the values of both pins to a default value.
        /// </summary>
        public void ResetValue()
        {
            Type valueType = this.CommonValue.GetType();
            Type[] genericTypeArgs = valueType.GetGenericArguments();
            if (genericTypeArgs.Count() == 0 || genericTypeArgs.Count() > 1)
            {
                throw new InvalidOperationException("The node containing the output pin is not implemented correctly. There have been 0 or more than one generic type arguments.");
            }

            Type genericType = genericTypeArgs.First();

            if (genericType == typeof(string))
            {
                this.CommonValue.Current = Activator.CreateInstance(genericType, string.Empty.ToCharArray());
            }
            else
            {
                this.CommonValue.Current = Activator.CreateInstance(genericType);
            }
        }
    }
}
