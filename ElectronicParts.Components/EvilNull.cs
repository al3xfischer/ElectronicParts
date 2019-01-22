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
    public class EvilNull : IDisplayableNode
    {
        public ICollection<IPin> Inputs => null;

        public ICollection<IPin> Outputs => null;

        public string Label => null;

        public string Description => null;

        public NodeType Type => (NodeType)5000;

        public Bitmap Picture => null;

        public event EventHandler PictureChanged;

        public void Activate()
        {
        }

        public void Execute()
        {
        }
    }
}
