// ***********************************************************************
// Assembly         : Shared
// Author           : 
// ***********************************************************************
// <copyright file="IPin.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the IPin interface.</summary>
// ***********************************************************************

namespace Shared
{
    /// <summary>
    ///     An interface used for pins.
    /// </summary>
    public interface IPin
    {
        /// <summary>
        ///     Gets or sets the label of the pin.
        /// </summary>
        /// <value>The label of the pin.</value>
        string Label { get; set; }

        /// <summary>
        ///     Gets or sets the value of the pin.
        /// </summary>
        /// <value>The value of the pin.</value>
        IValue Value { get; set; }
    }
}