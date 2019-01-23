using ElectronicParts.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicParts.Services.Extensions;
using ElectronicParts.Services.Interfaces;

namespace ElectronicParts.Services.Implementations
{
    public class NodeCopyService : INodeCopyService
    {
        private IEnumerable<IDisplayableNode> nodesToCopy;
        private IEnumerable<Connector> connectorsToCopy;
        private IEnumerable<IDisplayableNode> copiedNodes;
        private IEnumerable<Connector> copiedConnectors;
        private readonly IPinConnectorService connectorService;
        private Task copyTask;


        /// <summary>
        /// Gets the copied nodes.
        /// </summary>
        /// <value>The copied nodes.</value>
        public IEnumerable<IDisplayableNode> CopiedNodes
        {
            get
            {
                if (!this.copyTask?.IsCompleted == true)
                {
                    return Enumerable.Empty<IDisplayableNode>();
                }

                return this.copiedNodes;
            }
        }


        /// <summary>
        /// Gets the copied connectors.
        /// </summary>
        /// <value>The copied connectors.</value>
        public IEnumerable<Connector> CopiedConnectors
        {
            get
            {
                if (!this.copyTask?.IsCompleted == true)
                {
                    return Enumerable.Empty<Connector>();
                }

                return this.copiedConnectors;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="NodeCopyService"/> class.
        /// </summary>
        /// <param name="connectorService">The connector service.</param>
        /// <exception cref="System.ArgumentNullException">connectorService</exception>
        public NodeCopyService(IPinConnectorService connectorService)
        {
            this.connectorService = connectorService ?? throw new ArgumentNullException(nameof(connectorService));
        }

        /// <summary>
        /// Initializes the copy process. Call this method when the user requested the copy process for example by pressing STRG-C
        /// This method will store the nodes and connector and start creating a copy of the elements.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="connectors">The connectors.</param>
        public void InitializeCopyProcess(IEnumerable<IDisplayableNode> nodes, IEnumerable<Connector> connectors)
        {
            this.nodesToCopy = nodes.ToList();
            this.connectorsToCopy = connectors.ToList();
            this.copyTask = this.MakeCopyAsync();
        }


        /// <summary>
        /// This Method tries to start a new CopyProcess.
        /// </summary>
        /// <returns>true if there is no copyProces running at the moment and a new one has been successfully created, false otherwise.</returns>
        public bool TryBeginCopyTask()
        {
            if (this.copyTask?.IsCompleted == true)
            {
                this.copyTask = this.MakeCopyAsync();
                return true;
            }

            return false;
        }


        /// <summary>
        /// This is a private task which contains the logic to create a full deep copy of all nodes suplied during initialization phase.
        /// This task will also make all the necessary connection via the pinconnectorservice.
        /// </summary>
        /// <returns>An awaitable Task.</returns>
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
                    var outputSourceIndex = outputPinsSource.IndexOf(connS.OutputPin);

                    this.connectorService.TryConnectPins(inputPinsDest.ElementAt(inputSourceIndex), outputPinsDest.ElementAt(outputSourceIndex), out Connector newConn, false);
                    copiedConnectors.Add(newConn);
                }

                this.copiedConnectors = copiedConnectors;
            });
        }

        /// <summary>
        /// This asynchronous method returns a Task which can be used to await the currently running copy process.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task CopyTaskAwaiter()
        {
            if (!this.copyTask?.IsCompleted == true)
            {
                await this.copyTask;
            }
        }
    }
}