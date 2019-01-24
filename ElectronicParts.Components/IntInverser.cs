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
    public class IntInverser : IDisplayableNode
    {
        private DateTime lastChange;

        public IntInverser()
        {
            this.Inputs = new List<IPin>() { new Pin<bool>(), new Pin<int>() };

            this.Outputs = new List<IPin>() { new Pin<int>() };
        }
        public ICollection<IPin> Inputs { get; }

        public ICollection<IPin> Outputs { get; }

        public string Label => "IntInverser";

        public string Description => "Inverses the incoming integer if the incoming boolean is true.";

        public NodeType Type => NodeType.Logic;

        public Bitmap Picture => Properties.Resources.Timer;

        public event EventHandler PictureChanged;

        public void Activate()
        {
            return;
        }

        public void Execute()
        {
            if ((bool)this.Inputs.ElementAt(0).Value.Current)
            {

            }
        }
    }
}
