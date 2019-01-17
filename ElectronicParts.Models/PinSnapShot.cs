using Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.Models
{
    [Serializable]
    public class PinSnapShot
    {
        public IPin Pin { get; set; }

        public Point Position { get; set; }

        public PinSnapShot(IPin pin, Point point)
        {
            this.Pin = pin;
            this.Position = point;
        }
    }
}
