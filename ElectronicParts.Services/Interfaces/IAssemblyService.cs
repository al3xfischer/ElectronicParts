using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Shared;

namespace ElectronicParts.Services.Interfaces
{
    public interface IAssemblyService
    {
        IEnumerable<IDisplayableNode> AvailableNodes { get; }

        Task LoadAssemblies();
    }
}