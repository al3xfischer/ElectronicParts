// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="IConnectorHelperService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the IConnectorHelperService interface of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Services.Interfaces
{
    using System.Collections.Generic;
    using ElectronicParts.Models;
    using Shared;

    /// <summary>
    /// A interface used to implement a class to interact with <see cref="Connector"/>.
    /// </summary>
    public interface IConnectorHelperService
    {
        /// <summary>
        /// Gets or sets the existing nodes.
        /// </summary>
        /// <value>The existing nodes.</value>
        IEnumerable<IDisplayableNode> ExistingNodes { get; set; }

        /// <summary>
        /// Gets the offset for the specified <see cref="IPin"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <param name="pinCount">The pin count.</param>
        /// <returns>The offset for the given pins.</returns>
        double GetOffset(IPin input, IPin output, out int pinCount);

        /// <summary>
        /// Determines whether two given <see cref="IPin"/> belong to the same <see cref="IDisplayableNode"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <returns>Whether the two given pins belong to the same node.</returns>
        bool IsSelfConnecting(IPin input, IPin output);
    }
}