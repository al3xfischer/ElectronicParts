using ElectronicParts.ViewModels.Commands;
using System;
using System.Linq;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using ElectronicParts.Services.Implementations;
using System.Windows;
using ElectronicParts.Services.Interfaces;
using ElectronicParts.Models;
using ElectronicParts.ViewModels.Converter;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using Microsoft.Extensions.Logging;
using System.Timers;

namespace ElectronicParts.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<NodeViewModel> nodes;

        private ObservableCollection<ConnectorViewModel> connections;

        private ObservableCollection<NodeViewModel> availableNodes;

        private readonly IExecutionService executionService;

        private readonly IAssemblyService assemblyService;

        private readonly IPinConnectorService pinConnectorService;

        private readonly INodeSerializerService nodeSerializerService;

        private readonly ILogger<MainViewModel> logger;

        private PinViewModel inputPin;

        private PinViewModel outputPin;

        private readonly Timer updateMillisecondsPerLoopUpdateTimer;

        private readonly Timer reSnappingTimer;



        public MainViewModel(IExecutionService executionService, IAssemblyService assemblyService, IPinConnectorService pinConnectorService, INodeSerializerService nodeSerializerService, ILogger<MainViewModel> logger)
        {
            this.executionService = executionService ?? throw new ArgumentNullException(nameof(executionService));
            this.pinConnectorService = pinConnectorService ?? throw new ArgumentNullException(nameof(pinConnectorService));
            this.nodeSerializerService = nodeSerializerService ?? throw new ArgumentNullException(nameof(nodeSerializerService));
            this.assemblyService = assemblyService ?? throw new ArgumentNullException(nameof(assemblyService));
            this.AvailableNodes = new ObservableCollection<NodeViewModel>();
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.updateMillisecondsPerLoopUpdateTimer = new Timer(2000);
            this.updateMillisecondsPerLoopUpdateTimer.Elapsed += UpdateMillisecondsPerLoopUpdateTimer_Elapsed;
            this.updateMillisecondsPerLoopUpdateTimer.Start();
            this.reSnappingTimer = new Timer(500);
            this.reSnappingTimer.Elapsed += ReSnappingTimer_Elapsed;
            this.reSnappingTimer.AutoReset = false;
            this.FramesPerSecond = 50;
            this.SelectedCategory = this.NodeCategories.First();

            this.GridSnappingEnabled = true;
            this.GridSize = 10;

            this.IncreaseGridSize = new RelayCommand(arg =>
            {
                this.GridSize++;
                this.reSnappingTimer.Stop();
                this.reSnappingTimer.Start();
            }, arg => this.GridSize < 30);

            this.DecreaseGridSize = new RelayCommand(arg =>
            {
                this.GridSize--;
                this.reSnappingTimer.Stop();
                this.reSnappingTimer.Start();
            }, arg => this.GridSize > 5);

            this.SaveCommand = new RelayCommand(arg =>
            {
                SnapShot snapShot = SnapShotConverter.Convert(this.Nodes, this.connections);
                this.nodeSerializerService.Serialize(snapShot);

                foreach (NodeViewModel nodeVM in this.Nodes)
                {
                    nodeVM.AddDeleage();
                }
            });

            this.LoadCommand = new RelayCommand(arg =>
            {
                SnapShot snapShot = default(SnapShot);

                try
                {

                    snapShot = nodeSerializerService.Deserialize();
                }
                catch (SerializationException e)
                {
                    // TODO proper exception Handeling
                    Debug.WriteLine("Failed deserialiszation");

                    var missingAssembly = new AssemblyNameExtractorService().ExtractAssemblyNameFromErrorMessage(e);
                    var result = MessageBox.Show($"There are missing assemblies: {missingAssembly}\nDo you want to add new assemblies?", "Loading Failed", MessageBoxButton.YesNo, MessageBoxImage.Error);

                }

                if (snapShot is null)
                {
                    return;
                }

                List<NodeViewModel> nodes = new List<NodeViewModel>();
                List<ConnectorViewModel> connections = new List<ConnectorViewModel>();

                foreach (NodeSnapShot node in snapShot.Nodes)
                {
                    NodeViewModel nodeViewModel = new NodeViewModel(node.Node, this.DeleteNodeCommand, this.InputPinCommand, this.OutputPinCommand);
                    nodeViewModel.Left = node.Position.X;
                    nodeViewModel.Top = node.Position.Y;

                    nodes.Add(nodeViewModel);
                }

                foreach (ConnectionSnapShot connection in snapShot.Connections)
                {
                    PinViewModel outputPinViewModel = null;

                    foreach (NodeViewModel node in nodes)
                    {
                        if (node.Outputs.Any(outputPin => outputPin.Pin == connection.OutputPin.Pin))
                        {
                            outputPinViewModel = node.Outputs.First(outputPin => outputPin.Pin == connection.OutputPin.Pin);
                            break;
                        }
                    }

                    outputPinViewModel.Left = connection.OutputPin.Position.X;
                    outputPinViewModel.Top = connection.OutputPin.Position.Y;

                    PinViewModel inputPinViewModel = null;

                    foreach (NodeViewModel node in nodes)
                    {
                        if (node.Inputs.Any(inputPin => inputPin.Pin == connection.InputPin.Pin))
                        {
                            inputPinViewModel = node.Inputs.First(inputPin => inputPin.Pin == connection.InputPin.Pin);
                            break;
                        }
                    }

                    inputPinViewModel.Left = connection.InputPin.Position.X;
                    inputPinViewModel.Top = connection.InputPin.Position.Y;

                    ConnectorViewModel connectorViewModel = new ConnectorViewModel(connection.Connector, inputPinViewModel, outputPinViewModel, this.DeleteConnectionCommand);

                    connections.Add(connectorViewModel);
                }

                this.nodes.Clear();

                foreach (NodeViewModel node in nodes)
                {
                    this.nodes.Add(node);
                }

                this.connections.Clear();

                foreach (ConnectorViewModel connection in connections)
                {
                    this.connections.Add(connection);
                }
            });

            this.ExitCommand = new RelayCommand(arg => Environment.Exit(0));

            this.ExecutionStepCommand = new RelayCommand(async arg =>
            {
                var nodeList = this.Nodes.Select(nodeVM => nodeVM.Node);
                await this.executionService.ExecuteOnce(nodeList);
                foreach (var connection in this.connections)
                {
                    connection.Update();
                }
                this.FirePropertyChanged(nameof(CanAddNode));
            }, arg => !this.executionService.IsEnabled);

            this.ExecutionStartLoopCommand = new RelayCommand(async arg =>
            {
                var nodeList = this.Nodes.Select(nodeVM => nodeVM.Node);
                await this.executionService.StartExecutionLoop(nodeList, () =>
                {
                    this.FirePropertyChanged(nameof(CanAddNode));
                    Task.Run(() =>
                    {
                        foreach (var connection in this.Connections)
                        {
                            connection.Update();
                        }
                    });
                });
            }, arg => !this.executionService.IsEnabled);

            this.ExecutionStopLoopCommand = new RelayCommand(arg =>
            {
                this.executionService.StopExecutionLoop();
                this.FirePropertyChanged(nameof(CanAddNode));
            }, arg => this.executionService.IsEnabled);

            this.ResetAllConnectionsCommand = new RelayCommand(async arg =>
            {
                await this.ResetAllConnections();
                this.FirePropertyChanged(nameof(CanAddNode));
            });

            this.ReloadAssembliesCommand = new RelayCommand(async arg =>
            {
                await this.ReloadAssemblies();
            });

            this.ExecutionStopLoopAndResetCommand = new RelayCommand(async arg =>
            {
                this.executionService.StopExecutionLoop();
                await this.ResetAllConnections();
                this.FirePropertyChanged(nameof(CanAddNode));

            }, arg => this.executionService.IsEnabled);

            this.InputPinCommand = new RelayCommand(arg =>
            {
                this.inputPin = arg as PinViewModel;
                this.Connect();
            }, arg => !this.executionService.IsEnabled);

            this.OutputPinCommand = new RelayCommand(arg =>
            {
                this.outputPin = arg as PinViewModel;
                this.Connect();
            }, arg => !this.executionService.IsEnabled);

            this.DeleteConnectionCommand = new RelayCommand(arg =>
            {
                var connectionVm = arg as ConnectorViewModel;

                if (connectionVm is null)
                {
                    return;
                }

                this.pinConnectorService.TryRemoveConnection(connectionVm.Connector);

                this.connections.Remove(connectionVm);
            }, arg => !this.executionService.IsEnabled);

            this.DeleteNodeCommand = new RelayCommand(arg =>
            {
                var nodeVm = arg as NodeViewModel;

                if (nodeVm is null)
                {
                    return;
                }

                var connectionsMarkedForDeletion = this.Connections.Where(connection => nodeVm.Inputs.Contains(connection.Input) || nodeVm.Outputs.Contains(connection.Output)).ToList();
                connectionsMarkedForDeletion.ForEach(c => this.connections.Remove(c));
                this.Nodes.Remove(nodeVm);
            }, arg => !this.executionService.IsEnabled);

            this.AddNodeCommand = new RelayCommand(arg =>
            {
                var node = arg as IDisplayableNode;
                if (node is null)
                {
                    return;
                }

                var copy = Activator.CreateInstance(node?.GetType()) as IDisplayableNode;
                var vm = new NodeViewModel(copy, this.DeleteNodeCommand, this.InputPinCommand, this.OutputPinCommand);
                this.Nodes.Add(vm);
                vm.SnapToNewGrid(this.GridSize, false);
                this.FirePropertyChanged(nameof(Nodes));
            }, arg => !this.executionService.IsEnabled);

            this.Nodes = new ObservableCollection<NodeViewModel>();
            this.AvailableNodes = new ObservableCollection<NodeViewModel>();
            this.Connections = new ObservableCollection<ConnectorViewModel>();
            var reloadingTask = this.ReloadAssemblies();
            this.logger.LogInformation("Ctor MainVM done");
        }

        private async void ReSnappingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await this.SnapAllNodesToGrid();
        }

        private void UpdateMillisecondsPerLoopUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.executionService.IsEnabled)
            {
                this.MilisecondsPerLoop = this.executionService.MillisecondsPerLoop;
            }
        }

        public bool GridSnappingEnabled
        {
            get => this.gridSnappingEnabled;
            set
            {
                if (value != this.gridSnappingEnabled)
                {
                    this.gridSnappingEnabled = value;
                    var snappingTask = this.SnapAllNodesToGrid();
                }
            }
        }
        public int GridSize
        {
            get => this.gridSize;
            set
            {
                if (value >= 5 && value <= 30)
                {
                    Set(ref this.gridSize, value);
                    this.reSnappingTimer.Stop();
                    this.reSnappingTimer.Start();
                }
            }
        }

        public ObservableCollection<NodeViewModel> Nodes
        {
            get => this.nodes;

            set
            {
                Set(ref this.nodes, value);
            }
        }

        public long MilisecondsPerLoop
        {
            get => this.milisecondsPerLoop;
            set
            {
                Set(ref this.milisecondsPerLoop, value);
            }
        }

        public IEnumerable<string> NodeCategories
        {
            get
            {
                return Enum.GetNames(typeof(Shared.NodeType)).Prepend("All");
            }
        }

        public string SelectedCategory
        {
            get => selectedCategory;
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    Set(ref this.selectedCategory, value);
                    this.FirePropertyChanged(nameof(this.AvailableNodes));
                }
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
            get
            {
                
                return this.availableNodes.Where(nodeVM =>
                
                   (Enum.GetName(typeof(NodeType), nodeVM.Type) == this.SelectedCategory || this.SelectedCategory == this.NodeCategories.First())

                ).ToObservableCollection();
            }
            set
            {
                Set(ref this.availableNodes, value);
            }
        }

        public NodeViewModel SelectedNode { get; set; }

        public NodeViewModel SelectedNodeInformation { get; set; }

        public bool CanAddNode
        {
            get => !this.executionService.IsEnabled;
        }

        public ICommand SaveCommand { get; }
        public ICommand ExecutionStepCommand { get; }
        public ICommand ExecutionStartLoopCommand { get; }
        public ICommand ExecutionStopLoopCommand { get; }
        public ICommand ExecutionStopLoopAndResetCommand { get; }
        public ICommand ResetAllConnectionsCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand ReloadAssembliesCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand IncreaseGridSize { get; }
        public ICommand DecreaseGridSize { get; }

        private async Task ResetAllConnections()
        {
            await Task.Run(() =>
            {
                foreach (var connectionVM in this.Connections)
                {
                    connectionVM.Connector.ResetValue();
                }

                foreach (var connection in this.Connections)
                {
                    connection.Update();
                }

                foreach (var nodeVm in this.Nodes)
                {
                    nodeVm.Update();
                }
            });
        }

        public async Task ReloadAssemblies()
        {
            await this.assemblyService.LoadAssemblies();
            this.availableNodes.Clear();
            foreach (var assembly in this.assemblyService.AvailableNodes.Select(node => new NodeViewModel(node, this.DeleteNodeCommand, this.InputPinCommand, this.OutputPinCommand)))
            {
                this.availableNodes.Add(assembly);
            }
            this.FirePropertyChanged(nameof(this.AvailableNodes));
        }

        private async Task SnapAllNodesToGrid(bool floor)
        {
            await Task.Run(() =>
            {
                if (!(this.Nodes is null) && this.GridSnappingEnabled)
                {
                    foreach (var nodeVm in this.Nodes)
                    {
                        try
                        {
                            nodeVm.SnapToNewGrid(this.GridSize, floor);
                        }
                        catch
                        {
                            //TODO exception handeling
                        }
                    }
                }
            });
        }

        private async Task SnapAllNodesToGrid()
        {
            await Task.Run(() =>
            {
                if (!(this.Nodes is null) && this.GridSnappingEnabled)
                {
                    foreach (var nodeVm in this.Nodes)
                    {
                        try
                        {
                            nodeVm.SnapToNewGrid(this.GridSize);
                        }
                        catch
                        {
                            //TODO exception handeling
                        }
                    }
                }
            });
        }

        private int framesPerSecond;
        private long milisecondsPerLoop;
        private int gridSize;
        private bool gridSnappingEnabled;
        private string selectedCategory;

        public int FramesPerSecond
        {
            get => this.framesPerSecond;
            set
            {
                if (value > 0 && value <= 100)
                {
                    Set(ref this.framesPerSecond, value);
                    this.executionService.FramesPerSecond = value;
                }
            }
        }


        public ICommand AddNodeCommand { get; }

        public ICommand DeleteNodeCommand { get; }
        public ICommand DeleteConnectionCommand { get; }

        public ICommand InputPinCommand { get; }

        public ICommand OutputPinCommand { get; }


        private void Connect()
        {
            if (!(this.inputPin is null) && !(this.outputPin is null))
            {
                if (this.pinConnectorService.TryConnectPins(this.inputPin.Pin, this.outputPin.Pin, out var connection))
                {
                    this.Connections.Add(new ConnectorViewModel(connection, this.inputPin, this.outputPin, this.DeleteConnectionCommand));
                }

                this.inputPin = null;
                this.outputPin = null;
            }
        }
    }
}
