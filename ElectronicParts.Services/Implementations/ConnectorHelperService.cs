// ***********************************************************************
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="ConnectorHelperService.cs" company="FHWN">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>Represents the ConnectorHelperService class of the ElectronicParts Programm</summary>
// ***********************************************************************

namespace ElectronicParts.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ElectronicParts.Services.Extensions;
    using ElectronicParts.Services.Interfaces;
    using Shared;

    /// <summary>
    /// Class ConnectorHelperService.
    /// </summary>
    /// <seealso cref="ElectronicParts.Services.Interfaces.IConnectorHelperService" />
    public class ConnectorHelperService : IConnectorHelperService
    {
        /// <summary>
        /// Gets or sets the existing nodes.
        /// </summary>
        /// <value>The existing nodes.</value>
        public IEnumerable<IDisplayableNode> ExistingNodes { get; set; }

        /// <summary>
        /// Determines whether two given <see cref="IPin" /> belong to the same <see cref="IDisplayableNode" />.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <returns>Whether the two given pins belong to the same node.</returns>
        public bool IsSelfConnecting(IPin input, IPin output)
        {
            return this.ExistingNodes.Any(node => node.Inputs.Contains(input) && node.Outputs.Contains(output));
        }

        /// <summary>
        /// Gets the offset for the specified <see cref="IPin" />.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <param name="pinCount">The pin count.</param>
        /// <returns>The offset for the given pins.</returns>
        public double GetOffset(IPin input, IPin output, out int pinCount)
        {
            pinCount = 1;

            if (!this.IsSelfConnecting(input, output))
            {
                return 0;
            }

            var containingNode = this.ExistingNodes.First(node => node.Inputs.Contains(input));
            pinCount = containingNode.Inputs.Count;

            if (pinCount > containingNode.Outputs.Count)
            {
                if (containingNode.Inputs.IndexOf(input) < pinCount / 2)
                {
                    return -(pinCount + 1 - containingNode.Inputs.IndexOf(input) - 1) / 2.0;
                }
                else
                {
                    return containingNode.Inputs.IndexOf(input) / 2.0;
                }
            }
            else
            {
                if (containingNode.Outputs.IndexOf(output) < containingNode.Outputs.Count / 2)
                {
                    return -(containingNode.Outputs.Count + 1 - containingNode.Outputs.IndexOf(output) - 1) / 2.0;
                }
                else
                {
                    return containingNode.Outputs.IndexOf(output) / 2.0;
                }
            }
        }
    }
}