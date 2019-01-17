using Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.Models
{
    public class NodeSnapShot
    {
        public IDisplayableNode Node { get; set; }

        public Point Position { get; set; }
    }
}
