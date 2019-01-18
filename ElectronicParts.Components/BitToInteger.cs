using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace ElectronicParts.Components
{
    public class BitToInteger : IDisplayableNode
    {
        public BitToInteger()
        {
            this.Inputs = new List<IPin>();

            for (int i = 0; i < 8; i++)
            {
                this.Inputs.Add(new Pin<bool>());
            }

            this.Outputs = new List<IPin>() { new Pin<bool>() };
        }
        public ICollection<IPin> Inputs { get; }

        public ICollection<IPin> Outputs { get; }

        public string Label => "BitToInteger";

        public string Description => "Converts 8 bits to an integer between 0 and 255";

        public NodeType Type => NodeType.Logic;

        public Bitmap Picture => Properties.Resources.Converter;

        public event EventHandler PictureChanged;

        public void Activate()
        {
            return;
        }

        public void Execute()
        {
            bool[] arr = new bool[8];

            for (int i = 0; i < this.Inputs.Count; i++)
            {
                arr[i] = (bool)this.Inputs.ElementAt(i).Value.Current;
            }

            this.Outputs.ElementAt(0).Value.Current = this.BoolArrayToByteConverter(arr);
        }

        private byte BoolArrayToByteConverter(bool[] arr)
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
