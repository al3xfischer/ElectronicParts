// ***********************************************************************
// Assembly         : Shared
// Author           : 
// ***********************************************************************
// <copyright file="IValueGeneric.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the IValueGeneric interface.</summary>
// ***********************************************************************

namespace Shared
{
    /// <summary>
    ///     A generic implementation of the <see cref="IValue" /> interface.
    /// </summary>
    /// <typeparam name="T">The generic type of the value.</typeparam>
    public interface IValueGeneric<T> : IValue
    {
        /// <summary>
        ///     Gets or sets the current value.
        /// </summary>
        /// <value>The current value.</value>
        new T Current { get; set; }
    }
}