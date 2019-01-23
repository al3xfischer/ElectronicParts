// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Alexander Fischer
// ***********************************************************************
// <copyright file="IPinCreatorService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the IPinCreatorService interface of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Shared;

    /// <summary>
    /// A interface used for the creation of <see cref="IPin"/> instances.
    /// </summary>
    public interface IPinCreatorService
    {
        /// <summary>
        /// Creates a pin of the given type.
        /// </summary>
        /// <param name="type">The type of the pin.</param>
        /// <returns>The created pin.</returns>
        IPin CreatePin(Type type);

        /// <summary>
        /// Creates a amount of pins of a given type.
        /// </summary>
        /// <param name="type">The type of the pin.</param>
        /// <param name="amount">The amount of pins being created.</param>
        /// <returns>A IEnumerable of <see cref="IPin"/> instances.</returns>
        IEnumerable<IPin> CreatePins(Type type, int amount);
    }
}
