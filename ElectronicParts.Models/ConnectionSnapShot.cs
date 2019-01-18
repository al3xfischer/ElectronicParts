// ***********************************************************************
// Assembly         : ElectronicParts.Models
// Author           : Peter Helf
// ***********************************************************************
// <copyright file="ConnectionSnapShot.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the ConnectionSnapShot class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Models
{
    using System;
    using Shared;

    /// <summary>
    /// Represents the ConnectionSnapShot class of the ElectronicParts program.
    /// This class is used to create serializable instances of the ConnectorViewModel class.
    /// </summary>
    [Serializable]
    public class ConnectionSnapShot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionSnapShot"/> class.
        /// </summary>
        /// <param name="connector">The connection between two pins.</param>
        /// <param name="inputPin">The input pin of the connection.</param>
        /// <param name="outputPin">The output pin of the connection.</param>
        /// <param name="value">The current value transmitted by the connection.</param>
        public ConnectionSnapShot(Connector connector, PinSnapShot inputPin, PinSnapShot outputPin, IValue value)
        {
            this.Connector = connector;
            this.InputPin = inputPin;
            this.OutputPin = outputPin;
            this.Value = value;
        }

        /// <summary>
        /// Gets the connection between two pins.
        /// </summary>
        /// <value>The connection between two pins.</value>
        public Connector Connector { get; }

        /// <summary>
        /// Gets the input pin of the connection.
        /// </summary>
        /// <value>The input pin of the connection.</value>
        public PinSnapShot InputPin { get; }

        /// <summary>
        /// Gets the output pin of the connection.
        /// </summary>
        /// <value>The output pin of the connection.</value>
        public PinSnapShot OutputPin { get; }

        /// <summary>
        /// Gets the current value transmitted by the connection.
        /// </summary>
        /// <value>The current value transmitted by the connection.</value>
        public IValue Value { get; }
    }
}
