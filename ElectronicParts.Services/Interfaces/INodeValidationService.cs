// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="INodeValidationService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the INodeValidationService interface of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Services.Interfaces
{
    using Shared;

    /// <summary>
    /// A interface used to implement classes which allow to validate the implementation of a <see cref="IDisplayableNode"/>.
    /// </summary>
    public interface INodeValidationService
    {
        /// <summary>
        /// Checks if the implementation of a <see cref="IDisplayableNode"/> is correct.
        /// </summary>
        /// <param name="node">The node which is checked.</param>
        /// <returns>A value indicating whether the implementation of the node is correct.</returns>
        bool Validate(IDisplayableNode node);
    }
}