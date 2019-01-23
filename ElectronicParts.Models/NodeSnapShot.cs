// ***********************************************************************
// Assembly         : ElectronicParts.Models
// Author           : Peter Helf
// ***********************************************************************
// <copyright file="NodeSnapShot.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the NodeSnapShot class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Models
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Shared;

    /// <summary>
    /// Represents the NodeSnapShot class of the ElectronicParts program.
    /// This class is used to create serializable instances of the NodeViewModel class.
    /// </summary>
    [Serializable]
    public class NodeSnapShot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeSnapShot"/> class.
        /// </summary>
        /// <param name="node">The node which will be serialized.</param>
        /// <param name="position">The position of the node.</param>
        public NodeSnapShot(IDisplayableNode node, Point position)
        {
            this.Node = node;
            this.Position = position;
        }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <value>The node which will be serialized.</value>
        public IDisplayableNode Node { get; }

        /// <summary>
        /// Gets the position of the node.
        /// </summary>
        /// <value>The position of the node.</value>
        public Point Position { get; }
    }
}
