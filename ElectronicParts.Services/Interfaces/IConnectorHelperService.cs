// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="IConnectorHelperService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the IConnectorHelperService class of the ElectronicParts.Services project</summary>
// ***********************************************************************
namespace ElectronicParts.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using ElectronicParts.Models;
    using Shared;

    /// <summary>
    /// Represents the <see cref="IConnectorHelperService"/> interface.
    /// </summary>
    public interface IConnectorHelperService
    {
        /// <summary>
        /// Gets or sets the IEnumerable with which the helper service can iterate over all existing connections.
        /// </summary>
        /// <value>The existing connections.</value>
        IEnumerable<Connector> ExistingConnections { get; set; }

        /// <summary>
        /// Gets or sets the IEnumerable with which the helper service can iterate over all existing Nodes.
        /// </summary>
        /// <value>The existing nodes.</value>
        IEnumerable<IDisplayableNode> ExistingNodes { get; set; }

        /// <summary>
        /// Gets or sets a function which can be used to get the current top value of a pin.
        /// </summary>
        /// <value>The get height mapping.</value>
        Func<IPin, int> GetHeightMapping { get; set; }

        /// <summary>
        /// Gets the multiple output offset which is used if the node has multiple outputs.
        /// </summary>
        /// <param name="pin">The pin to check.</param>
        /// <returns>The required offset as integer.</returns>
        int GetMultipleOutputOffset(IPin pin);

        /// <summary>
        /// Gets the offset which is used to space out the connections.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <param name="pinCount">The pin count.</param>
        /// <returns>The required offset as double.</returns>
        double GetOffset(IPin input, IPin output, out int pinCount);

        /// <summary>
        /// Determines whether the specified input and output pins are part of the same node.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <returns>True if the pins are part of the same node and otherwise, False.</returns>
        bool IsSelfConnecting(IPin input, IPin output);

        /// <summary>
        /// Gets the offset which is needed if one output pin has multiple connections.
        /// </summary>
        /// <param name="outputPin">The output pin.</param>
        /// <param name="con">The connector the pin is a part of.</param>
        /// <returns>The required offset as integer.</returns>
        int MultipleConnectionsOffset(IPin outputPin, Connector con);

        /// <summary>
        /// Determines whether the containing node has more inputs or outputs.
        /// </summary>
        /// <param name="pin">The pin to check the node of.</param>
        /// <returns>True if is inputs more the specified pin and otherwise, False.</returns>
        bool IsInputsMore(IPin pin);
    }
}
