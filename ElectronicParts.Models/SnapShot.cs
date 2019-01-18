// ***********************************************************************
// Assembly         : ElectronicParts.Models
// Author           : Peter Helf
// ***********************************************************************
// <copyright file="SnapShot.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the PinSnapShot class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the SnapShot class of the ElectronicParts program.
    /// This class is used to create serializable instances of the ViewModel.
    /// </summary>
    [Serializable]
    public class SnapShot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SnapShot"/> class.
        /// </summary>
        /// <param name="nodes">The nodes which will be serialized.</param>
        /// <param name="connections">All connections between pins.</param>
        public SnapShot(IEnumerable<NodeSnapShot> nodes, IEnumerable<ConnectionSnapShot> connections)
        {
            this.Nodes = nodes;
            this.Connections = connections;
        }

        /// <summary>
        /// Gets all nodes saved in the snapshot.
        /// </summary>
        /// <value>All nodes of the snapshot.</value>
        public IEnumerable<NodeSnapShot> Nodes { get; }

        /// <summary>
        /// Gets all connections saved in the snapshot.
        /// </summary>
        /// <value>All connections saved in the snapshot.</value>
        public IEnumerable<ConnectionSnapShot> Connections { get; }
    }
}
