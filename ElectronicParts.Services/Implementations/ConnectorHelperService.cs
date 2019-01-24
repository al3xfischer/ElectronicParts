using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicParts.Services.Extensions;
using ElectronicParts.Services.Interfaces;
using ElectronicParts.Models;

namespace ElectronicParts.Services.Implementations
{
    public class ConnectorHelperService : IConnectorHelperService
    {
        public IEnumerable<IDisplayableNode> ExistingNodes { get; set; }
        public IEnumerable<Connector> ExistingConnections { get; set; }
        public Func<IPin, int> GetHeightMapping { get; set; }

        public bool IsSelfConnecting(IPin input, IPin output)
        {
            return this.ExistingNodes.Any(node => node.Inputs.Contains(input) && node.Outputs.Contains(output));
        }

        public double GetOffset(IPin input, IPin output, out int pinCount)
        {
            pinCount = 1;

            if (!this.IsSelfConnecting(input, output))
            {
                return 0;
            }

            var containingNode = this.GetContainingNode(input);
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
                    continue;
                }
            }

            heightMapping = heightMapping.OrderBy(map => map.Value).ToDictionary(dict => dict.Key, dict => dict.Value);
            return heightMapping.IndexOf(heightMapping.FirstOrDefault(pair => pair.Key == con.InputPin));
        }

        public int GetMultipleOutputOffset(IPin pin)
        {
            var x = this.GetContainingNode(pin);
            return this.GetContainingNode(pin).Outputs.IndexOf(pin);
        }

        private int GetOutputPinAmount(IPin pin)
        {
            return this.GetContainingNode(pin).Outputs.Count;
        }

        private IDisplayableNode GetContainingNode(IPin pin)
        {
            return ExistingNodes.FirstOrDefault(node => node.Inputs.Contains(pin) || node.Outputs.Contains(pin));
        }
    }
}
