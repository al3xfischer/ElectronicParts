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
    public class NodeSnapShot
    {
        public IDisplayableNode Node { get; set; }

        public Point Position { get; set; }

        public NodeSnapShot(IDisplayableNode node, Point position)
        {
            this.Node = node;
            this.Position = position;
        }
    }
}
