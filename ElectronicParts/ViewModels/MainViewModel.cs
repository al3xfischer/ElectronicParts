using ElectronicParts.Commands;
using System;
using System.Linq;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ElectronicParts.Models;
using ElectronicParts.Services.Interfaces;
using System.Threading.Tasks;
using ElectronicParts.Converter;
using System.Collections.Generic;

namespace ElectronicParts.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<NodeViewModel> nodes;
        private ObservableCollection<Connector> connections;
        private ObservableCollection<NodeViewModel> availableNodes;
        private readonly IExecutionService myExecutionService;

        public MainViewModel(IExecutionService executionService, INodeSerializerService nodeSerializerService)
        {
            this.myExecutionService = executionService ?? throw new ArgumentNullException(nameof(executionService));

            this.SaveCommand = new RelayCommand(arg => 
            {
                SnapShot snapShot = SnapShotConverter.Convert(this.nodes, this.connections);
                nodeSerializerService.Serialize(snapShot);
            });

            this.LoadCommand = new RelayCommand(arg => 
            {
                SnapShot snapShot = nodeSerializerService.Deserialize();

                List<NodeViewModel> nodes = new List<NodeViewModel>();
                List<Connector> connections = new List<Connector>();

                foreach (NodeSnapShot node in snapShot.Nodes)
                {
                    NodeViewModel nodeViewModel = new NodeViewModel(node.Node, this.DeleteCommand, this.InputPinCommand, this.OutputPinCommand);
                    nodeViewModel.Left = node.Position.X;
                    nodeViewModel.Top = node.Position.Y;

                    nodes.Add(nodeViewModel);
                }

                foreach (Connector connection in snapShot.Connections)
                {
                    connections.Add(connection);
                }

                this.nodes.Clear();

                foreach (NodeViewModel node in nodes)
                {
                    this.nodes.Add(node);
                }

                this.connections.Clear();

                foreach (Connector connection in connections)
                {
                    this.connections.Add(connection);
                }
            });
            
            this.ExitCommand = new RelayCommand(arg => Environment.Exit(0));
            this.ExecutionStepCommand = new RelayCommand(async arg =>
            {
                var nodeList = this.Nodes.Select(nodeVM => nodeVM.node);
                await this.myExecutionService.ExecuteOnce(nodeList);
            }, arg => !this.myExecutionService.IsEnabled);

            this.ExecutionStartLoopCommand = new RelayCommand(async arg =>
            {
                var nodeList = this.Nodes.Select(nodeVM => nodeVM.node);
                await this.myExecutionService.StartExecutionLoop(nodeList);

            }, arg => !this.myExecutionService.IsEnabled);

            this.ExecutionStopLoopCommand = new RelayCommand(arg =>
            {
                this.myExecutionService.StopExecutionLoop();

            }, arg => this.myExecutionService.IsEnabled);

            this.ResetAllConnectionsCommand = new RelayCommand(async arg =>
            {
                await this.ResetAllConnections();

            });

            this.ExecutionStopLoopAndResetCommand = new RelayCommand(async arg =>
            {
                this.myExecutionService.StopExecutionLoop();
                await this.ResetAllConnections();

            }, arg => this.myExecutionService.IsEnabled);

            this.InputPinCommand = new RelayCommand(arg => { });
            this.OutputPinCommand = new RelayCommand(arg => { });
            this.DeleteCommand = new RelayCommand(arg =>
            {
                var nodeVm = arg as NodeViewModel;

                if (nodeVm is null)
                {
                    return;
                }

                this.Nodes.Remove(nodeVm);
            });
            this.AddNodeCommand = new RelayCommand(arg =>
            {
                var node = arg as IDisplayableNode;
                if (node is null)
                {
                    return;
                }

                var copy = Activator.CreateInstance(node?.GetType()) as IDisplayableNode;
                var vm = new NodeViewModel(copy, this.DeleteCommand, this.InputPinCommand, this.OutputPinCommand);
                this.Nodes.Add(vm);
                this.FirePropertyChanged(nameof(Nodes));
            });
            this.Nodes = new ObservableCollection<NodeViewModel>
            {
                new NodeViewModel(new TestNode(),this.DeleteCommand,this.InputPinCommand,this.OutputPinCommand)
            };

            this.AvailableNodes = new ObservableCollection<NodeViewModel>
            {
                new NodeViewModel(new TestNode(),this.DeleteCommand,this.InputPinCommand,this.OutputPinCommand)
            };

            this.Connections = new ObservableCollection<Connector>();
        }

        public ObservableCollection<NodeViewModel> Nodes
        {
            get => this.nodes;

            set
            {
                Set(ref this.nodes, value);
            }
        }

        public ObservableCollection<Connector> Connections
        {
            get => this.connections;

            set
            {
                Set(ref this.connections, value);
            }
        }

        public ObservableCollection<NodeViewModel> AvailableNodes
        {
            get => this.availableNodes;

            set
            {
                Set(ref this.availableNodes, value);
            }
        }

        public NodeViewModel SelectedNode { get; set; }

        public NodeViewModel SelectedNodeInformation { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand ExecutionStepCommand { get; }
        public ICommand ExecutionStartLoopCommand { get; }
        public ICommand ExecutionStopLoopCommand { get; }
        public ICommand ExecutionStopLoopAndResetCommand { get; }
        public ICommand ResetAllConnectionsCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand ReloadAssembliesCommand { get; }
        public ICommand ExitCommand { get; }

        private async Task ResetAllConnections()
        {
            await Task.Run(() =>
            {
                foreach (var connection in this.Connections)
                {
                    connection.ResetValue();
                }
            });
        }
        public ICommand AddNodeCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand InputPinCommand { get; }

        public ICommand OutputPinCommand { get; }
    }
}
