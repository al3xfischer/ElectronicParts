using System;
using System.Collections.Generic;
using ElectronicParts.Models;
using Shared;

namespace ElectronicParts.Services.Interfaces
{
    public interface IConnectorHelperService
    {
        IEnumerable<IDisplayableNode> ExistingNodes { get; set; }
        IEnumerable<Connector> ExistingConnections { get; set; }
        Func<IPin, int> GetHeightMapping { get; set; }
        int GetMultipleOutputOffset(IPin pin);
        int MultipleConnectionsOffset(IPin outputPin, Connector con);

        double GetOffset(IPin input, IPin output, out int pinCount);
        bool IsSelfConnecting(IPin input, IPin output);
    }
}