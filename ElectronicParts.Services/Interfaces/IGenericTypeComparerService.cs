// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="IGenericTypeComparerService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the IGenericTypeComparerService interface of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Services.Interfaces
{
    /// <summary>
    ///  A interface used to implement classes which allow to check if two types are the same generic type.
    /// </summary>
    public interface IGenericTypeComparerService
    {
        /// <summary>
        /// Checks if two objects have the same generic type.
        /// </summary>
        /// <param name="first">The object with the first type.</param>
        /// <param name="second">The object with the second type.</param>
        /// <returns>A value indicating whether the two objects have the same generic type.</returns>
        bool IsSameGenericType(object first, object second);
    }
}