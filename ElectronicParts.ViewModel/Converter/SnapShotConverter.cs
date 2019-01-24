// ***********************************************************************
// Assembly         : ElectronicParts.ViewModels
// Author           : Peter Helf
// ***********************************************************************
// <copyright file="SnapShotConverter.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the SnapShotConverter class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.ViewModels.Converter
{
    using System.Collections.Generic;
    using System.Drawing;
    using ElectronicParts.Models;

    /// <summary>
    /// Used for converting the view model of the ElectronicParts program to a snapshot.
    /// </summary>
    public static class SnapShotConverter
    {
        /// <summary>
        /// Converts all nodes and connections to a snapshot.
        /// </summary>
        /// <param name="nodes">The nodes which will be converted.</param>
        /// <param name="connections">The connections which will be converted.</param>
        /// <returns>A snapshot of the view model.</returns>
        public static SnapShot Convert(IEnumerable<NodeViewModel> nodes, IEnumerable<ConnectorViewModel> connections)
        {
            List<NodeSnapShot> nodeSnapShots = new List<NodeSnapShot>();
            List<ConnectionSnapShot> connectionSnapShots = new List<ConnectionSnapShot>();

            foreach (NodeViewModel nodeVM in nodes)
            {
                nodeVM.RemoveDelegate();
                Point position = new Point((int)nodeVM.Left, (int)nodeVM.Top);

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
