using System.Collections.Generic;
using Shared;

namespace ElectronicParts.Services.Interfaces
{
    public interface IConnectorHelperService
    {
        IEnumerable<IDisplayableNode> ExistingNodes { get; set; }

        double GetOffset(IPin input, IPin output);
        bool IsSelfConnecting(IPin input, IPin output);
    }
}