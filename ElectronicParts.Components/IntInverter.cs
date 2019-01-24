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
    public class IntInverter : IDisplayableNode
    {
        private DateTime lastChange;

        public IntInverter()
        {
            this.Inputs = new List<IPin>() { new Pin<bool>(), new Pin<int>() };

            this.Outputs = new List<IPin>() { new Pin<int>() };
        }
        public ICollection<IPin> Inputs { get; }

        public ICollection<IPin> Outputs { get; }

        public string Label => "IntInverter";

        public string Description => "Inverts the incoming integer if the incoming boolean is true.";

        public NodeType Type => NodeType.Logic;

        public Bitmap Picture => null;

        public event EventHandler PictureChanged;

        public void Activate()
        {
            return;
        }

        public void Execute()
        {
            if ((bool)this.Inputs.ElementAt(0).Value.Current)
            {
                this.Outputs.ElementAt(0).Value.Current = -(int)this.Inputs.ElementAt(1).Value.Current;
            }
            else
            {
                this.Outputs.ElementAt(0).Value.Current = (int)this.Inputs.ElementAt(1).Value.Current;
            }
        }
    }
}
