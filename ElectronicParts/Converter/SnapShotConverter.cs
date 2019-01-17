using ElectronicParts.Models;
using ElectronicParts.ViewModels;
using Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.Converter
{
    public static class SnapShotConverter
    {
        public static SnapShot Convert(IEnumerable<NodeViewModel> nodes, IEnumerable<Connector> connections)
        {
            List<NodeSnapShot> nodeSnapShots = new List<NodeSnapShot>();

            foreach (NodeViewModel nodeVM in nodes)
            {
                Point position = new Point(nodeVM.Left, nodeVM.Top);

                nodeSnapShots.Add(new NodeSnapShot(nodeVM.Node, position));
            }

            return new SnapShot(nodeSnapShots, connections);
        }
    }
}
