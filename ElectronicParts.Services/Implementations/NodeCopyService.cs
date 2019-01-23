// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Kevin Janisch
// ***********************************************************************
// <copyright file="NodeCopyService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the NodeCopyService class of the ElectronicParts.Services project</summary>
// ***********************************************************************
namespace ElectronicParts.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ElectronicParts.Models;
    using ElectronicParts.Services.Extensions;
    using ElectronicParts.Services.Interfaces;
    using Shared;

    /// <summary>
    /// Represents the <see cref="NodeCopyService"/> class of the ElectronicParts.Services application.
    /// Implements the <see cref="ElectronicParts.Services.Interfaces.INodeCopyService" />
    /// </summary>
    /// <seealso cref="ElectronicParts.Services.Interfaces.INodeCopyService" />
    public class NodeCopyService : INodeCopyService
    {
        /// <summary>
        /// Represents the pin connector service.
        /// </summary>
        private readonly IPinConnectorService connectorService;

        /// <summary>
        /// Represents the connectors which are to be copied.
        /// </summary>
        private IEnumerable<Connector> connectorsToCopy;

        /// <summary>
        /// Represents the nodes which are to be copied.
        /// </summary>
        private IEnumerable<IDisplayableNode> nodesToCopy;

        /// <summary>
        /// Represents the connectors which were copied last.
        /// </summary>
        private IEnumerable<Connector> copiedConnectors;

        /// <summary>
        /// Represents the nodes which were copied last.
        /// </summary>
        private IEnumerable<IDisplayableNode> copiedNodes;

        /// <summary>
        /// Represents the currently running copy task.
        /// </summary>
        private Task copyTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeCopyService"/> class.
        /// </summary>
        /// <param name="connectorService">The connector service.</param>
        /// <exception cref="ArgumentNullException">Gets throws if the injected <see cref="PinConnectorService"/> is null.</exception>
        public NodeCopyService(IPinConnectorService connectorService)
        {
            this.connectorService = connectorService ?? throw new ArgumentNullException(nameof(connectorService));
        }

        /// <summary>
        /// Gets the copied connectors.
        /// </summary>
        /// <value>The copied connectors.</value>
        public IEnumerable<Connector> CopiedConnectors
        {
            get
            {
                if (!this.copyTask.IsCompleted || this.copyTask is null)
                {
                    return Enumerable.Empty<Connector>();
                }

                return this.CopiedConnectors;
            }
        }

        /// <summary>
        /// Gets the copied nodes.
        /// </summary>
        /// <value>The copied nodes.</value>
        public IEnumerable<IDisplayableNode> CopiedNodes
        {
            get
            {
                if (!this.copyTask.IsCompleted || this.copyTask is null)
                {
                    return Enumerable.Empty<IDisplayableNode>();
                }

                return this.CopiedNodes;
            }
        }

        /// <summary>
        /// This asynchronous method returns a Task which can be used to await the currently running copy process.
        /// </summary>
        /// <returns>Returns a task used for waiting for the copy process to finish without exposing the actual task.</returns>
        public async Task CopyTaskAwaiter()
        {
            if (!this.copyTask.IsCompleted && !(this.copyTask is null))
            {
                await this.copyTask;
            }
        }

        /// <summary>
        /// Initializes the copy process. Call this method when the user requested the copy process for example by pressing STRG-C
        /// This method will store the nodes and connector and start creating a copy of the elements.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="connectors">The connectors.</param>
        public void InitializeCopyProcess(IEnumerable<IDisplayableNode> nodes, IEnumerable<Connector> connectors)
        {
            this.nodesToCopy = nodes ?? throw new ArgumentNullException(nameof(nodes));
            this.connectorsToCopy = connectors ?? throw new ArgumentNullException(nameof(connectors));
            this.TryBeginCopyTask();
        }

        /// <summary>
        /// This Method tries to start a new CopyProcess.
        /// </summary>
        /// <returns>true if there is no copyProcess running at the moment and a new one has been successfully created, false otherwise.</returns>
        public bool TryBeginCopyTask()
        {
            if (this.copyTask.IsCompleted || this.copyTask is null)
            {
                this.copyTask = this.MakeCopyAsync();
                return true;
            }

            return false;
        }

        /// <summary>
        /// This is a private task which contains the logic to create a full deep copy of all nodes supplied during initialization phase.
        /// This task will also make all the necessary connection via the <see cref="PinConnectorService"/>.
        /// </summary>
        /// <returns>An Task used to wait for the copy process to finish.</returns>
        private async Task MakeCopyAsync()
        {
            await Task.Run(() =>
            {
                List<IDisplayableNode> copiedNodes = new List<IDisplayableNode>();
                List<Connector> copiedConnectors = new List<Connector>();
                var inputPinsSource = nodesToCopy.SelectMany(node => node.Inputs);
                var outputPinsSource = nodesToCopy.SelectMany(node => node.Outputs);
                var inputPinsDest = copiedNodes.SelectMany(node => node.Inputs);
                var outputPinsDest = copiedNodes.SelectMany(node => node.Outputs);

                foreach (var node in this.nodesToCopy)
                {
                    copiedNodes.Add(Activator.CreateInstance(node?.GetType()) as IDisplayableNode);
                    this.copiedNodes = copiedNodes;
                }

                foreach (var connS in this.connectorsToCopy)
                {
                    var inputSourceIndex = inputPinsSource.IndexOf(connS.InputPin);
                    var outputSourceIndex = inputPinsSource.IndexOf(connS.InputPin);

                    this.connectorService.TryConnectPins(inputPinsDest.ElementAt(inputSourceIndex), outputPinsSource.ElementAt(outputSourceIndex), out Connector newConn, false);
                    copiedConnectors.Add(newConn);
                }

                this.copiedConnectors = copiedConnectors;
            });
        }
    }
}
