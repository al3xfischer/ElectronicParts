using ElectronicParts.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Shared;
using System;

namespace ElectronicParts.Services.Implementations
{
    public class NodeValidationService : INodeValidationService
    {
        private readonly ILogger<NodeValidationService> logger;

        public NodeValidationService(ILogger<NodeValidationService> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool Validate(IDisplayableNode node)
        {
            try
            {
                node.Activate();
                node.Execute();
                var input = node.Inputs;
                var outputs = node.Outputs;
                var picture = node.Picture;
                var type = node.Type;
                var label = node.Label;
                var description = node.Description;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Node is not valid and will not be avaliable in the application.");
                return false;
            }

            return true;
        }
    }
}
