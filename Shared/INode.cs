using System.Collections.Generic;

namespace Shared
{
    public interface INode
    {
        ICollection<IPin> Inputs { get; }

        ICollection<IPin> Outputs { get; }

        string Label { get; }

        string Description { get; }

        NodeType Type { get; }

        void Execute();
        
        void Activate();
    }
}
