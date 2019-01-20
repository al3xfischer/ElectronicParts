using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace ElectronicParts.Components
{
    [Serializable]
    public class BitToInteger : IDisplayableNode
    {
        public BitToInteger()
        {
            this.Inputs = new List<IPin>();

            for (int i = 0; i < 8; i++)
            {
                this.Inputs.Add(new Pin<bool>());
            }

            this.Outputs = new List<IPin>() { new Pin<int>() };
        }
        public ICollection<IPin> Inputs { get; }

        public ICollection<IPin> Outputs { get; }

        public string Label => "BitToInteger";

        public string Description => "Converts 8 bits to an integer between 0 and 255";

        public NodeType Type => NodeType.Logic;

        public Bitmap Picture => Properties.Resources.Converter;

        [field: NonSerialized]
        public event EventHandler PictureChanged;

        public void Activate()
        {
            return;
        }

        public void Execute()
        {
            this.Outputs.ElementAt(0).Value.Current = this.BoolArrayToByteConverter(this.Inputs.Select(pin => (bool)pin.Value.Current));
        }

        private int BoolArrayToByteConverter(IEnumerable<bool> arr)
        {
            byte result = 0;
            foreach (bool b in arr)
            {
                result <<= 1;
                if (b) result |= 1;
            }

            return result;
        }
    }
}
