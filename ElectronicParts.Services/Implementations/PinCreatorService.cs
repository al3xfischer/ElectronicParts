// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Alexander Fischer
// ***********************************************************************
// <copyright file="PinCreatorService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the PinCreatorService.cs class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using ElectronicParts.Models;
    using ElectronicParts.Services.Interfaces;
    using Shared;

    /// <summary>
    /// A class used for the creation of <see cref="IPin"/> instances.
    /// </summary>
    public class PinCreatorService : IPinCreatorService
    {
        /// <summary>
        /// Creates a pin of the given type.
        /// </summary>
        /// <param name="type">The type of the pin.</param>
        /// <returns>The created pin.</returns>
        public IPin CreatePin(Type type)
        {
            if (type == typeof(string))
            {
                return new Pin<string>();
            }
            else if (type == typeof(int))
            {
                return new Pin<int>();
            }
            else if (type == typeof(bool))
            {
                return new Pin<bool>();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Creates a amount of pins of a given type.
        /// </summary>
        /// <param name="type">The type of the pin.</param>
        /// <param name="amount">The amount of pins being created.</param>
        /// <returns>A IEnumerable of <see cref="IPin"/> instances.</returns>
        public IEnumerable<IPin> CreatePins(Type type, int amount)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (amount <= 0)
            {
                throw new ArgumentException("The amout must not be less than or equal to zero.");
            }

            for (int i = 0; i < amount; i++)
            {
                yield return this.CreatePin(type);
            }
        }
    }
}
