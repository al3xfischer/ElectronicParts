using System.Collections.Generic;

namespace Shared
{
    public interface INode
    {
        ICollection<IPin> Inputs { get; }

        ICollection<IPin> Outputs { get; }

        void Execute();

        void Activate();

        string Label { get; }

        string Description { get; }
    }
}
