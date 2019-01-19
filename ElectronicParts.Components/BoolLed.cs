using Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ElectronicParts.Components
{
    public class BoolLed : IDisplayableNode
    {
        public BoolLed()
        {
            this.Inputs = new List<IPin> { new Pin<bool>() };
            this.Outputs = new List<IPin>();
        }

        public ICollection<IPin> Inputs { get; private set; }

        public ICollection<IPin> Outputs { get; private set; }

        public string Label { get => "Bool Led"; }

        public string Description { get => "A led with two states (active or not)."; }

        public NodeType Type { get => NodeType.Display; }

        public Bitmap Picture
        {
            get
            {
                var current = this.Inputs.FirstOrDefault()?.Value?.Current as bool?;

                if (current == true)
                {
                    return Properties.Resources.green_led;
                }
                else
                {
                    return Properties.Resources.red_led;
                }
            }
        }

        public event EventHandler PictureChanged;

        public void Activate()
        {
        }

        public void Execute()
        {
            this.PictureChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
