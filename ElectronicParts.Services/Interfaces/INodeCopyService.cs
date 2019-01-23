// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="INodeCopyService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the INodeCopyService class of the ElectronicParts.Services project</summary>
// ***********************************************************************

namespace ElectronicParts.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ElectronicParts.Models;
    using Shared;

    /// <summary>
    /// Represents the <see cref="INodeCopyService"/> interface.
    /// </summary>
    public interface INodeCopyService
    {
        /// <summary>
        /// Gets the copied connectors.
        /// </summary>
        /// <value>The copied connectors.</value>
        IEnumerable<Connector> CopiedConnectors { get; }

        /// <summary>
        /// Gets the copied nodes.
        /// </summary>
        /// <value>The copied nodes.</value>
        IEnumerable<IDisplayableNode> CopiedNodes { get; }

        /// <summary>
        /// Exposes a Task which can be used to await the currently running copyProcess.
        /// </summary>
        /// <returns>A task which can be awaited.</returns>
        Task CopyTaskAwaiter();

        /// <summary>
        /// Initializes the copy process. Call this method when the user requested the copy process for example by pressing STRG-C
        /// This method will store the nodes and connector and start creating a copy of the elements.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="connectors">The connectors.</param>
        void InitializeCopyProcess(IEnumerable<IDisplayableNode> nodes, IEnumerable<Connector> connectors);

        /// <summary>
        /// This Method tries to start a new CopyProcess.
        /// </summary>
        /// <returns>true if there is no copyProcess running at the moment and a new one has been successfully created, false otherwise.</returns>
        bool TryBeginCopyTask();
    }
}
