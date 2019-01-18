using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace ElectronicParts.Components
{
    public class Timer : IDisplayableNode
    {
        private DateTime lastChange;

        public Timer()
        {
            this.Inputs = new List<IPin>() { new Pin<int>() };

            this.Outputs = new List<IPin>() { new Pin<bool>() };

            this.lastChange = DateTime.Now;
        }
        public ICollection<IPin> Inputs { get; }

        public ICollection<IPin> Outputs { get; }

        public string Label => "Timer";

        public string Description => "Switches on and off at intervall of given input integer in seconds";

        public NodeType Type => NodeType.Logic;

        public Bitmap Picture => Properties.Resources.Timer;

        public event EventHandler PictureChanged;

        public void Activate()
        {
            return;
        }

        public void Execute()
        {
            var now = DateTime.Now;

            if (int.TryParse(this.Inputs.ElementAt(0).Value.Current.ToString(), out int intervall))
            {
                if (now - this.lastChange > TimeSpan.FromSeconds(intervall))
                {
                    this.Outputs.ElementAt(0).Value.Current = !(bool)this.Outputs.ElementAt(0).Value.Current;
                    this.lastChange = now;
                }
            }
        }
    }
}
