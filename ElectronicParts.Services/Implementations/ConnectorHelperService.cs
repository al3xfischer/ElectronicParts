// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="ConnectorHelperService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the ConnectorHelperService class of the ElectronicParts.Services project</summary>
// ***********************************************************************
namespace ElectronicParts.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ElectronicParts.Models;
    using ElectronicParts.Services.Extensions;
    using ElectronicParts.Services.Interfaces;
    using Microsoft.Extensions.Logging;
    using Shared;

    /// <summary>
    /// Represents the <see cref="ConnectorHelperService"/> class of the ElectronicParts.Services application.
    /// Implements the <see cref="ElectronicParts.Services.Interfaces.IConnectorHelperService" />
    /// </summary>
    /// <seealso cref="ElectronicParts.Services.Interfaces.IConnectorHelperService" />
    public class ConnectorHelperService : IConnectorHelperService
    {
        /// <summary>
        /// Represents the Logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectorHelperService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">Gets thrown if the injected logger is null.</exception>
        public ConnectorHelperService(ILogger<ConnectorHelperService> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets or sets the IEnumerable with which the helper service can iterate over all existing connections.
        /// </summary>
        /// <value>The existing connections.</value>
        public IEnumerable<Connector> ExistingConnections { get; set; }

        /// <summary>
        /// Gets or sets the IEnumerable with which the helper service can iterate over all existing Nodes.
        /// </summary>
        /// <value>The existing nodes.</value>
        public IEnumerable<IDisplayableNode> ExistingNodes { get; set; }

        /// <summary>
        /// Gets or sets a function which can be used to get the current top value of a pin.
        /// </summary>
        /// <value>The get height mapping.</value>
        public Func<IPin, int> GetHeightMapping { get; set; }

        /// <summary>
        /// Gets the multiple output offset which is used if the node has multiple outputs.
        /// </summary>
        /// <param name="pin">The pin to check.</param>
        /// <returns>The required offset as integer.</returns>
        public int GetMultipleOutputOffset(IPin pin)
        {
            try
            {
                return this.GetContainingNode(pin).Outputs.IndexOf(pin);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the offset which is used to space out the connections.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <param name="pinCount">The pin count.</param>
        /// <returns>The required offset as double.</returns>
        public double GetOffset(IPin input, IPin output, out int pinCount)
        {
            pinCount = 1;

            if (!this.IsSelfConnecting(input, output))
            {
                return 0;
            }

            var containingNode = this.GetContainingNode(input);
            int pinCountIn = containingNode.Inputs.Count;
            int pinCountOut = containingNode.Outputs.Count;

            if (pinCountIn > pinCountOut)
            {
                pinCount = pinCountIn;
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
                pinCount = pinCountOut;
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

        /// <summary>
        /// Determines whether the containing node has more inputs or outputs.
        /// </summary>
        /// <param name="pin">The pin to check the node of.</param>
        /// <returns>True if is inputs more the specified pin and otherwise, False.</returns>
        public bool IsInputsMore(IPin pin)
        {
            return this.GetContainingNode(pin).Inputs?.Count > this.GetContainingNode(pin).Outputs?.Count;
        }

        /// <summary>
        /// Determines whether the specified input and output pins are part of the same node.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <returns>True if the pins are part of the same node and otherwise, False.</returns>
        public bool IsSelfConnecting(IPin input, IPin output)
        {
            return this.ExistingNodes.Any(node => node.Inputs.Contains(input) && node.Outputs.Contains(output));
        }

        /// <summary>
        /// Gets the offset which is needed if one output pin has multiple connections.
        /// </summary>
        /// <param name="outputPin">The output pin.</param>
        /// <param name="con">The connector the pin is a part of.</param>
        /// <returns>The required offset as integer.</returns>
        public int MultipleConnectionsOffset(IPin outputPin, Connector con)
        {
            var existingConnections = this.ExistingConnections.Where(conn => conn.OutputPin == outputPin);
            var connectionsAmount = existingConnections.Count();
            Dictionary<IPin, int> heightMapping = new Dictionary<IPin, int>();
            foreach (var conn in existingConnections)
            {
                try
                {
                    heightMapping.Add(conn.InputPin, this.GetHeightMapping(conn.InputPin));
                }
                catch (Exception e)
                {
                    this.logger.LogError("had an error while adding value to heightmap: " + e.Message);
                }
            }

            heightMapping = heightMapping.OrderBy(map => map.Value).ToDictionary(dict => dict.Key, dict => dict.Value);
            return heightMapping.IndexOf(heightMapping.FirstOrDefault(pair => pair.Key == con.InputPin));
        }

        /// <summary>
        /// Gets the node which the specified pin is a part of.
        /// </summary>
        /// <param name="pin">The pin to get the node from.</param>
        /// <returns>The containing node.</returns>
        private IDisplayableNode GetContainingNode(IPin pin)
        {
            return this.ExistingNodes.FirstOrDefault(node => node.Inputs.Contains(pin) || node.Outputs.Contains(pin));
        }

        /// <summary>
        /// Gets the amount of output pins of a node.
        /// </summary>
        /// <param name="pin">The pin to check the amount for.</param>
        /// <returns>The amount of pins as integer.</returns>
        private int GetOutputPinAmount(IPin pin)
        {
            return this.GetContainingNode(pin).Outputs.Count;
        }
    }
}
