using System.Collections.Generic;
using System.Threading.Tasks;
using Shared;

namespace ElectronicParts.Services.Assemblies
{
    public interface IAssemblyService
    {
        List<IDisplayableNode> AvailableNodes { get; }

        Task LoadAssemblies();
    }
}