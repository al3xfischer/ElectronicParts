// ***********************************************************************
// Assembly         : Shared
// Author           : 
// ***********************************************************************
// <copyright file="IValue.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the IValue interface</summary>
// ***********************************************************************

namespace Shared
{
    /// <summary>
    /// An interface used for values.
    /// </summary>
    public interface IValue
    {
        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        /// <value>The current value.</value>
        object Current { get; set; }
    }
}
