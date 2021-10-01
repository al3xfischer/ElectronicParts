// ***********************************************************************
// Assembly         : Shared
// Author           : 
// ***********************************************************************
// <copyright file="INode.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the INode interface.</summary>
// ***********************************************************************

using System.Collections.Generic;

namespace Shared
{
    /// <summary>
    ///     An interface used for logic nodes.
    /// </summary>
    public interface INode
    {
        /// <summary>
        ///     Gets the description of the node.
        /// </summary>
        /// <value>The description of the node.</value>
        string Description { get; }

        /// <summary>
        ///     Gets a collection of input pins represented as <see cref="IPin" /> instances.
        /// </summary>
        /// <value>A collection of <see cref="IPin" /> instances.</value>
        ICollection<IPin> Inputs { get; }

        /// <summary>
        ///     Gets the label of the node.
        /// </summary>
        /// <value>The label of the node.</value>
        string Label { get; }

        /// <summary>
        ///     Gets a collection of output pins represented as <see cref="IPin" /> instances.
        /// </summary>
        /// <value>A collection of <see cref="IPin" /> instances.</value>
        ICollection<IPin> Outputs { get; }

        /// <summary>
        ///     Gets the <see cref="NodeType" /> of the pin.
        /// </summary>
        /// <value>The <see cref="NodeType" /> of the pin.</value>
        NodeType Type { get; }

        /// <summary>
        ///     Activates the node.
        /// </summary>
        void Activate();

        /// <summary>
        ///     Executes the node.
        /// </summary>
        void Execute();
    }
}