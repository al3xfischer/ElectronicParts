// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Alexander Fischer
// ***********************************************************************
// <copyright file="NodeValidationService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the NodeValidationService class of the ElectronicParts.Services project</summary>
// ***********************************************************************
namespace ElectronicParts.Services.Implementations
{
    using System;
    using ElectronicParts.Services.Interfaces;
    using Microsoft.Extensions.Logging;
    using Shared;

    /// <summary>
    /// Represents the <see cref="NodeValidationService"/> class of the ElectronicParts.Services application.
    /// Implements the <see cref="ElectronicParts.Services.Interfaces.INodeValidationService" />
    /// </summary>
    /// <seealso cref="ElectronicParts.Services.Interfaces.INodeValidationService" />
    public class NodeValidationService : INodeValidationService
    {
        /// <summary>
        /// Represents the logger instance.
        /// </summary>
        private readonly ILogger<NodeValidationService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeValidationService"/> class.
        /// </summary>
        /// <param name="logger">The injected <see cref="ILogger"/> instance.</param>
        /// <exception cref="ArgumentNullException">Throws if the injected <see cref="ILogger"/> instance is null.</exception>
        public NodeValidationService(ILogger<NodeValidationService> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Checks if the implementation of a <see cref="IDisplayableNode" /> is correct.
        /// </summary>
        /// <param name="node">The node which is checked.</param>
        /// <returns>A value indicating whether the implementation of the node is correct.</returns>
        public bool Validate(IDisplayableNode node)
        {
            try
            {
                node.Activate();
                node.Execute();
                var inputs = node.Inputs;

                foreach (var input in inputs)
                {
                    if (input is null)
                    {
                        throw new NullReferenceException(nameof(input));
                    }
                }

                var outputs = node.Outputs;

                foreach (var output in outputs)
                {
                    if (output is null)
                    {
                        throw new NullReferenceException(nameof(output));
                    }
                }

                var picture = node.Picture;
                var type = node.Type;
                var label = node.Label;
                var description = node.Description;

                node.PictureChanged += NodePictureChanged;
                node.PictureChanged -= NodePictureChanged;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Node is not valid and will not be avaliable in the application.");
                return false;
            }

            return true;
        }

        private void NodePictureChanged(object sender, EventArgs e)
        {
            return;
        }
    }
}
