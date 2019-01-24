using System.Collections.Generic;
using Shared;

namespace ElectronicParts.Services.Interfaces
{
    public interface IConnectorHelperService
    {
        IEnumerable<IDisplayableNode> ExistingNodes { get; set; }

        double GetOffset(IPin input, IPin output, out int pinCount);
        bool IsSelfConnecting(IPin input, IPin output);
    }
}