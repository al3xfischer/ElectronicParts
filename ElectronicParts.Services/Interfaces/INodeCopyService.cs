using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronicParts.Models;
using Shared;

namespace ElectronicParts.Services.Interfaces
{
    public interface INodeCopyService
    {
        IEnumerable<Connector> CopiedConnectors { get; }
        IEnumerable<IDisplayableNode> CopiedNodes { get; }

        Task CopyTaskAwaiter();
        void InitializeCopyProcess(IEnumerable<IDisplayableNode> nodes, IEnumerable<Connector> connectors);
        bool TryBeginCopyTask();
    }
}