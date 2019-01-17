using ElectronicParts.Commands;
using System;
using System.Linq;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ElectronicParts.Services.Interfaces;
using System.Threading.Tasks;
using ElectronicParts.Services.Implementations;

namespace ElectronicParts.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<NodeViewModel> nodes;

        private ObservableCollection<ConnectorViewModel> connections;

        private ObservableCollection<NodeViewModel> availableNodes;

        private readonly IExecutionService myExecutionService;

        private readonly IPinConnectorService pinConnectorService;

        private PinViewModel inputPin;

        private PinViewModel outputPin;

        public MainViewModel(IExecutionService executionService, IPinConnectorService pinConnectorService)
        {
            this.myExecutionService = executionService ?? throw new ArgumentNullException(nameof(executionService));
            this.pinConnectorService = pinConnectorService ?? throw new ArgumentNullException(nameof(pinConnectorService));
            this.SaveCommand = new RelayCommand(arg => { });
            this.LoadCommand = new RelayCommand(arg => { });
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

            this.InputPinCommand = new RelayCommand(arg =>
            {
                this.inputPin = arg as PinViewModel;
                this.Connect();
            }, arg => !this.myExecutionService.IsEnabled);

            this.OutputPinCommand = new RelayCommand(arg =>
            {
                this.outputPin = arg as PinViewModel;
                this.Connect();
            }, arg => !this.myExecutionService.IsEnabled);

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

            this.Connections = new ObservableCollection<ConnectorViewModel>();
        }

        public ObservableCollection<NodeViewModel> Nodes
        {
            get => this.nodes;

            set
            {
                Set(ref this.nodes, value);
            }
        }

        public ObservableCollection<ConnectorViewModel> Connections
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
                foreach (var connectionVM in this.Connections)
                {
                    connectionVM.Connector.ResetValue();
                }
            });
        }
        public ICommand AddNodeCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand InputPinCommand { get; }

        public ICommand OutputPinCommand { get; }

        private void Connect()
        {
            if (!(this.inputPin is null) && !(this.outputPin is null))
            {
                if (this.pinConnectorService.TryConnectPins(this.inputPin.Pin, this.outputPin.Pin, out var connection))
                {
                    this.Connections.Add(new ConnectorViewModel(connection, this.inputPin, this.outputPin));
                }

                this.inputPin = null;
                this.outputPin = null;
            }
        }
    }
}
