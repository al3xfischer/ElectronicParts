using Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.Components
{
    [Serializable]
    public class Throwing : IDisplayableNode
    {
        public ICollection<IPin> Inputs => throw new NotImplementedException();

        public ICollection<IPin> Outputs => throw new NotImplementedException();

        public string Label => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();

        public NodeType Type => throw new NotImplementedException();

        public Bitmap Picture => throw new NotImplementedException();

        public event EventHandler PictureChanged;

        public void Activate()
        {
            throw new NotImplementedException();
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
