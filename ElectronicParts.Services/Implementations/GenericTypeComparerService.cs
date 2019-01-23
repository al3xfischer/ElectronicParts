// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="GenericTypeComparerService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the GenericTypeComparerService class of the ElectronicParts.Services project</summary>
// ***********************************************************************
namespace ElectronicParts.Services.Implementations
{
    using System;
    using System.Linq;
    using ElectronicParts.Services.Interfaces;

    /// <summary>
    /// Represents the <see cref="GenericTypeComparerService"/> class of the ElectronicParts.Services application.
    /// Implements the <see cref="ElectronicParts.Services.Interfaces.IGenericTypeComparerService" />
    /// </summary>
    /// <seealso cref="ElectronicParts.Services.Interfaces.IGenericTypeComparerService" />
    public class GenericTypeComparerService : IGenericTypeComparerService
    {
        /// <summary>
        /// Checks if two objects have the same generic type.
        /// </summary>
        /// <param name="first">The object with the first type.</param>
        /// <param name="second">The object with the second type.</param>
        /// <returns>A value indicating whether the two objects have the same generic type.</returns>
        public bool IsSameGenericType(object first, object second)
        {
            return first.GetType().GetGenericArguments().SequenceEqual(second.GetType().GetGenericArguments());
        }
    }
}
