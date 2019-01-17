using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.Models
{
    [Serializable]
    public class SnapShot
    {
        public IEnumerable<NodeSnapShot> Nodes { get; set; }

        public IEnumerable<ConnectionSnapShot> Connections { get; set; }

        public SnapShot(IEnumerable<NodeSnapShot> nodes, IEnumerable<ConnectionSnapShot> connections)
        {
            this.Nodes = nodes;
            this.Connections = connections;
        }
    }
}
