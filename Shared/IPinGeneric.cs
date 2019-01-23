// ***********************************************************************
// Assembly         : Shared
// Author           : 
// ***********************************************************************
// <copyright file="IPinGeneric.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the IPinGeneric interface.</summary>
// ***********************************************************************

namespace Shared
{
    /// <summary>
    /// A generic implementation of the <see cref="IPin"/> interface.
    /// </summary>
    /// <typeparam name="T">The generic type of the pin.</typeparam>
    public interface IPinGeneric<T> : IPin
    {
        /// <summary>
        /// Gets or sets the value of the pin.
        /// </summary>
        /// <value>The value of the pin.</value>
        new IValueGeneric<T> Value { get; set; }
    }
}
