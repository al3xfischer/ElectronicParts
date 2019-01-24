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
    public class Inverter : IDisplayableNode
    {
        public Inverter()
        {
            this.Inputs = new List<IPin>() { new Pin<bool>() };

            this.Outputs = new List<IPin>() { new Pin<bool>() };
        }
        public ICollection<IPin> Inputs { get; }

        public ICollection<IPin> Outputs { get; }

        public string Label => "Inverter";

        public string Description => "Inverts the incoming boolean.";

        public NodeType Type => NodeType.Logic;

        public Bitmap Picture => Properties.Resources.Inverter;

        public event EventHandler PictureChanged;

        public void Activate()
        {
            return;
        }

        public void Execute()
        {
            this.Outputs.ElementAt(0).Value.Current = !(bool)this.Inputs.ElementAt(0).Value.Current;
        }
    }
}
