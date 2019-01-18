// ***********************************************************************
// Assembly         : ElectronicParts.Models
// Author           : Peter Helf
// ***********************************************************************
// <copyright file="PinSnapShot.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the PinSnapShot class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Models
{
    using System;
    using System.Drawing;
    using Shared;

    /// <summary>
    /// Represents the PinSnapShot class of the ElectronicParts program.
    /// This class is used to create serializable instances of the PinViewModel class.
    /// </summary>
    [Serializable]
    public class PinSnapShot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PinSnapShot"/> class.
        /// </summary>
        /// <param name="pin">The pin which will be serialized.</param>
        /// <param name="point">The position of the pin.</param>
        public PinSnapShot(IPin pin, Point point)
        {
            this.Pin = pin;
            this.Position = point;
        }

        /// <summary>
        /// Gets the pin which will be serialized.
        /// </summary>
        /// <value>The pin which will be serialized.</value>
        public IPin Pin { get; }

        /// <summary>
        /// Gets the position of the pin.
        /// </summary>
        /// <value>The position of the pin.</value>
        public Point Position { get; }
    }
}
