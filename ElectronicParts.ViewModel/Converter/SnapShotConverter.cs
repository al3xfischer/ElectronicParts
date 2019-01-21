using ElectronicParts.Models;
using ElectronicParts.ViewModels;
using Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.ViewModels.Converter
{
    public static class SnapShotConverter
    {
        public static SnapShot Convert(IEnumerable<NodeViewModel> nodes, IEnumerable<ConnectorViewModel> connections)
        {
            List<NodeSnapShot> nodeSnapShots = new List<NodeSnapShot>();
            List<ConnectionSnapShot> connectionSnapShots = new List<ConnectionSnapShot>();

            foreach (NodeViewModel nodeVM in nodes)
            {
                nodeVM.RemoveDelegate();
                Point position = new Point(nodeVM.Left, nodeVM.Top);

                nodeSnapShots.Add(new NodeSnapShot(nodeVM.Node, position));
            }

            foreach (ConnectorViewModel connection in connections)
            {
                Point inputPinPosition = new Point(connection.Input.Left, connection.Input.Top);
                Point outputPinPosition = new Point(connection.Output.Left, connection.Output.Top);

                PinSnapShot inputPinSnapShot = new PinSnapShot(connection.Input.Pin, inputPinPosition);
                PinSnapShot outputPinSnapShot = new PinSnapShot(connection.Output.Pin, outputPinPosition);

                connectionSnapShots.Add(new ConnectionSnapShot(connection.Connector, inputPinSnapShot, outputPinSnapShot, connection.CurrentValue));
            }

            return new SnapShot(nodeSnapShots, connectionSnapShots);
        }
    }
}
