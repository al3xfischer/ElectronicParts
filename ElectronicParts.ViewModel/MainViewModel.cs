// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : 
// ***********************************************************************
// <copyright file="MainViewModel.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the MainViewModel class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;
    using System.Timers;
    using System.Windows;
    using System.Windows.Input;
    using ElectronicParts.Models;
    using ElectronicParts.Services.Implementations;
    using ElectronicParts.Services.Interfaces;
    using ElectronicParts.ViewModels.Commands;
    using ElectronicParts.ViewModels.Converter;
    using GuiLabs.Undo;
    using Microsoft.Extensions.Logging;
    using Shared;

    public class MainViewModel : BaseViewModel
    {
        private readonly IExecutionService executionService;

        private readonly IAssemblyService assemblyService;

        private readonly IPinConnectorService pinConnectorService;

        private readonly INodeSerializerService nodeSerializerService;

        private readonly ILogger<MainViewModel> logger;

        private readonly IAssemblyNameExtractorService assemblyNameExtractorService;

        private readonly IGenericTypeComparerService genericTypeComparerService;

        private readonly IConfigurationService configurationService;

        private readonly Timer updateMillisecondsPerLoopUpdateTimer;

        private readonly Timer reSnappingTimer;

        private readonly ActionManager actionManager;

        private ObservableCollection<NodeViewModel> nodes;

        private ObservableCollection<ConnectorViewModel> connections;

        private ObservableCollection<NodeViewModel> availableNodes;

        private int framesPerSecond;

        private long milisecondsPerLoop;

        private int gridSize;

        private bool gridSnappingEnabled;

        private string selectedCategory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="executionService"></param>
        /// <param name="assemblyService"></param>
        /// <param name="pinConnectorService">A service for the connection of pins.</param>
        /// <param name="nodeSerializerService">A service which serializes all given nodes.</param>
        /// <param name="logger">The logger for the main view model.</param>
        /// <param name="configurationService">A service including all configurations for the application.</param>
        /// <param name="assemblyNameExtractorService"></param>
        /// <param name="actionManager"></param>
        /// <param name="genericTypeComparerService">A service which checks if two classes implement the same generic types.</param>
        public MainViewModel(
            IExecutionService executionService,
            IAssemblyService assemblyService,
            IPinConnectorService pinConnectorService,
            INodeSerializerService nodeSerializerService,
            ILogger<MainViewModel> logger,
            IConfigurationService configurationService,
            IAssemblyNameExtractorService assemblyNameExtractorService, 
            ActionManager actionManager,
            IGenericTypeComparerService genericTypeComparerService)
        {
            this.ClearedNodes = new Stack<IEnumerable<NodeViewModel>>();
            this.ClearedConnections = new Stack<IEnumerable<ConnectorViewModel>>();
            this.actionManager = actionManager ?? throw new ArgumentNullException(nameof(actionManager));
            this.executionService = executionService ?? throw new ArgumentNullException(nameof(executionService));
            this.pinConnectorService = pinConnectorService ?? throw new ArgumentNullException(nameof(pinConnectorService));
            this.nodeSerializerService = nodeSerializerService ?? throw new ArgumentNullException(nameof(nodeSerializerService));
            this.assemblyService = assemblyService ?? throw new ArgumentNullException(nameof(assemblyService));
            this.AvailableNodes = new ObservableCollection<NodeViewModel>();
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.assemblyNameExtractorService = assemblyNameExtractorService ?? throw new ArgumentNullException(nameof(assemblyNameExtractorService));
            this.genericTypeComparerService = genericTypeComparerService;
            
            this.configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
            this.updateMillisecondsPerLoopUpdateTimer = new Timer(2000);
            this.updateMillisecondsPerLoopUpdateTimer.Elapsed += this.UpdateMillisecondsPerLoopUpdateTimer_Elapsed;
            this.updateMillisecondsPerLoopUpdateTimer.Start();
            this.reSnappingTimer = new Timer(500);
            this.reSnappingTimer.Elapsed += this.ReSnappingTimer_Elapsed;
            this.reSnappingTimer.AutoReset = false;
            this.FramesPerSecond = 50;
            this.SelectedCategory = this.NodeCategories.First();

            this.GridSnappingEnabled = true;
            this.GridSize = 10;

            this.IncreaseGridSize = new RelayCommand(
                arg =>
            {
                this.GridSize++;
                this.reSnappingTimer.Stop();
                this.reSnappingTimer.Start();
            }, 
            arg => this.GridSize < 30);

            this.DecreaseGridSize = new RelayCommand(
                arg =>
            {
                this.GridSize--;
                this.reSnappingTimer.Stop();
                this.reSnappingTimer.Start();
            }, 
            arg => this.GridSize > 5);

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
                this.actionManager.Clear();
                SnapShot snapShot = default(SnapShot);

                try
                {
                    snapShot = nodeSerializerService.Deserialize();
                }
                catch (SerializationException e)
                {
                    this.logger.LogError(e, $"An error occurred while deserializing ({nameof(this.LoadCommand)}).");
                    Debug.WriteLine("Failed deserialization");

                    var missingAssembly = this.assemblyNameExtractorService.ExtractAssemblyNameFromErrorMessage(e);
                    var result = MessageBox.Show($"There are missing assemblies: {missingAssembly}\nDo you want to add new assemblies?", "Loading Failed", MessageBoxButton.YesNo, MessageBoxImage.Error);

                    if (result == MessageBoxResult.Yes)
                    {
                        this.AddAssembly?.Invoke();
                    }
                }

                if (snapShot is null)
                {
                    return;
                }

                List<NodeViewModel> nodes = new List<NodeViewModel>();
                List<ConnectorViewModel> connections = new List<ConnectorViewModel>();

                foreach (NodeSnapShot node in snapShot.Nodes)
                {
                    NodeViewModel nodeViewModel = new NodeViewModel(node.Node, this.DeleteNodeCommand, this.InputPinCommand, this.OutputPinCommand, this.executionService);
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

                this.RepositionNodes();
            });

            this.ExitCommand = new RelayCommand(arg => Environment.Exit(0));

            this.ExecutionStepCommand = new RelayCommand(
                async arg =>
            {
                var nodeList = this.Nodes.Select(nodeVM => nodeVM.Node);
                await this.executionService.ExecuteOnce(nodeList);
                foreach (var connection in this.connections)
                {
                    connection.Update();
                }

                this.FirePropertyChanged(nameof(CanAddNode));
            }, 
            arg => !this.executionService.IsEnabled);

            this.ExecutionStartLoopCommand = new RelayCommand(
                async arg =>
            {
                var nodeList = this.Nodes.Select(nodeVM => nodeVM.Node);
                await this.executionService.StartExecutionLoop(
                    nodeList, 
                    () =>
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
            }, 
            arg => !this.executionService.IsEnabled);

            this.ExecutionStopLoopCommand = new RelayCommand(
                arg =>
            {
                this.executionService.StopExecutionLoop();
                this.FirePropertyChanged(nameof(CanAddNode));
            }, 
            arg => this.executionService.IsEnabled);

            this.ResetAllConnectionsCommand = new RelayCommand(
                async arg =>
            {
                await this.ResetAllConnections();
                this.FirePropertyChanged(nameof(CanAddNode));
            });

            this.ReloadAssembliesCommand = new RelayCommand(
                async arg =>
            {
                await this.ReloadAssemblies();
            });

            this.ExecutionStopLoopAndResetCommand = new RelayCommand(
                async arg =>
            {
                this.executionService.StopExecutionLoop();
                await this.ResetAllConnections();
                this.FirePropertyChanged(nameof(CanAddNode));

            }, 
            arg => this.executionService.IsEnabled);

            this.InputPinCommand = new RelayCommand(
                arg =>
            {
                this.InputPin = arg as PinViewModel;
                this.Connect();
            }, 
            arg => !this.executionService.IsEnabled);

            this.OutputPinCommand = new RelayCommand(
                arg =>
            {
                this.OutputPin = arg as PinViewModel;
                this.Connect();
            }, 
            arg => !this.executionService.IsEnabled);

            this.DeleteConnectionCommand = new RelayCommand(
                arg =>
            {
                var connectionVm = arg as ConnectorViewModel;

                if (connectionVm is null)
                {
                    return;
                }

                this.pinConnectorService.TryRemoveConnection(connectionVm.Connector);

                this.connections.Remove(connectionVm);
            }, 
            arg => !this.executionService.IsEnabled);

            this.DeleteNodeCommand = new RelayCommand(
                arg =>
            {
                var nodeVm = arg as NodeViewModel;

                if (nodeVm is null)
                {
                    return;
                }

                this.InputPin = null;
                this.OutputPin = null;

                var connectionsMarkedForDeletion = this.Connections.Where(connection => nodeVm.Inputs.Contains(connection.Input) || nodeVm.Outputs.Contains(connection.Output)).ToList();
                var nodeToRemove = nodeVm;

                this.actionManager.Execute(new CallMethodAction(
                    () =>
                    {
                        connectionsMarkedForDeletion.ForEach(c =>
                        {
                            this.pinConnectorService.TryRemoveConnection(c.Connector);
                            this.connections.Remove(c);
                        });
                        this.Nodes.Remove(nodeVm);
                    },
                    () =>
                    {
                        connectionsMarkedForDeletion.ForEach(c =>
                        {
                            this.pinConnectorService.TryConnectPins(c.Input.Pin, c.Output.Pin, out Connector newConnection, true);
                            this.pinConnectorService.ManuallyAddConnectionToExistingConnections(c.Connector);
                            this.Connections.Add(c);
                        });
                        this.Nodes.Add(nodeVm);
                    })); 
            }, 
            arg => !this.executionService.IsEnabled);

            this.UndoCommand = new RelayCommand(
                arg =>
            {
                this.actionManager.Undo();
            }, 
            arg => this.actionManager.CanUndo && !this.executionService.IsEnabled);

            this.RedoCommand = new RelayCommand(
                arg =>
            {
                this.actionManager.Redo();
            }, 
            arg => this.actionManager.CanRedo && !this.executionService.IsEnabled);

            this.ClearAllNodesCommand = new RelayCommand(
                arg =>
            {
                if (this.Nodes.Count > 0)
                {
                    this.actionManager.Execute(new CallMethodAction(
                        async () => await this.ClearCanvas(),
                        async () => await this.UndoClearCanvas()));
                }
            }, 
            arg => !this.executionService.IsEnabled && this.Nodes.Count > 0);

            this.AddNodeCommand = new RelayCommand(
                arg =>
            {
                this.AddNode(arg as IDisplayableNode);
            }, 
            arg => !this.executionService.IsEnabled);

            this.UpdateBoardSize = new RelayCommand(
                arg =>
            {
                this.RepositionNodes();

                this.FirePropertyChanged(nameof(BoardHeight));
                this.FirePropertyChanged(nameof(BoardWidth));
            });

            this.Nodes = new ObservableCollection<NodeViewModel>();
            this.PreviewLines = new ObservableCollection<PreviewLineViewModel>() { new PreviewLineViewModel() };
            this.AvailableNodes = new ObservableCollection<NodeViewModel>();
            this.Connections = new ObservableCollection<ConnectorViewModel>();
            var reloadingTask = this.ReloadAssemblies();
            this.logger.LogInformation("Ctor MainVM done");
        }
        
        public PinViewModel InputPin { get; set; }

        public PinViewModel OutputPin { get; set; }

        public ObservableCollection<PreviewLineViewModel> PreviewLines { get; set; }
        
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
                    this.Set(ref this.gridSize, value);
                    this.reSnappingTimer.Stop();
                    this.reSnappingTimer.Start();
                }
            }
        }

        public int VerticalScrollerOffset { get; set; }

        public int HorizontalScrollerOffset { get; set; }

        public ObservableCollection<NodeViewModel> Nodes
        {
            get => this.nodes;

            set
            {
                this.Set(ref this.nodes, value);
            }
        }

        public long MilisecondsPerLoop
        {
            get => this.milisecondsPerLoop;
            set
            {
                this.Set(ref this.milisecondsPerLoop, value);
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
            get => this.selectedCategory;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.Set(ref this.selectedCategory, value);
                    this.FirePropertyChanged(nameof(this.AvailableNodes));
                }
            }
        }

        public Action AddAssembly { get; set; }

        public ObservableCollection<ConnectorViewModel> Connections
        {
            get => this.connections;

            set
            {
                this.Set(ref this.connections, value);
            }
        }

        public ObservableCollection<NodeViewModel> AvailableNodes
        {
            get
            {
                return this.availableNodes.Where(
                    nodeVM =>
                   (Enum.GetName(typeof(NodeType), nodeVM.Type) == this.SelectedCategory || this.SelectedCategory == this.NodeCategories.First()))
                   .ToObservableCollection();
            }

            set
            {
                this.Set(ref this.availableNodes, value);
            }
        }

        public NodeViewModel SelectedNode { get; set; }

        public NodeViewModel SelectedNodeInformation { get; set; }

        public bool CanAddNode
        {
            get => !this.executionService.IsEnabled;
        }

        public int BoardWidth
        {
            get => this.configurationService.Configuration.BoardWidth;
        }

        public int BoardHeight
        {
            get => this.configurationService.Configuration.BoardHeight;
        }

        public ICommand UndoCommand { get; }

        public ICommand RedoCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand ExecutionStepCommand { get; }

        public ICommand ExecutionStartLoopCommand { get; }

        public ICommand ExecutionStopLoopCommand { get; }

        public ICommand ExecutionStopLoopAndResetCommand { get; }

        public ICommand ResetAllConnectionsCommand { get; }

        public ICommand ClearAllNodesCommand { get; }

        public ICommand LoadCommand { get; }

        public ICommand ReloadAssembliesCommand { get; }

        public ICommand ExitCommand { get; }

        public ICommand IncreaseGridSize { get; }

        public ICommand DecreaseGridSize { get; }

        public ICommand UpdateBoardSize { get; }

        public ICommand AddNodeCommand { get; }

        public ICommand DeleteNodeCommand { get; }

        public ICommand DeleteConnectionCommand { get; }

        public ICommand InputPinCommand { get; }

        public ICommand OutputPinCommand { get; }

        public int FramesPerSecond
        {
            get => this.framesPerSecond;
            set
            {
                if (value > 0 && value <= 100)
                {
                    this.Set(ref this.framesPerSecond, value);
                    this.executionService.FramesPerSecond = value;
                }
            }
        }

        private Stack<IEnumerable<NodeViewModel>> ClearedNodes { get; set; }

        private Stack<IEnumerable<ConnectorViewModel>> ClearedConnections { get; set; }

        public async Task ReloadAssemblies()
        {
            await this.assemblyService.LoadAssemblies();
            this.availableNodes.Clear();
            foreach (var assembly in this.assemblyService.AvailableNodes.Select(node => new NodeViewModel(node, this.DeleteNodeCommand, this.InputPinCommand, this.OutputPinCommand, this.executionService)))
            {
                this.availableNodes.Add(assembly);
            }

            this.FirePropertyChanged(nameof(this.AvailableNodes));
        }

        public void ResetPreviewLine()
        {
            this.PreviewLines[0].Visible = false;
            this.OutputPin = null;
            this.InputPin = null;
            this.ResetPossibleConnections();
        }

        public void ResetPossibleConnections()
        {
            foreach (var pinList in this.nodes.Select(node => node.Outputs))
            {
                foreach (var pin in pinList)
                {
                    pin.CanBeConnected = false;
                }
            }

            foreach (var pinList in this.nodes.Select(node => node.Inputs))
            {
                foreach (var pin in pinList)
                {
                    pin.CanBeConnected = false;
                }
            }
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

        private void AddNode(IDisplayableNode node)
        {
            if (node is null)
            {
                return;
            }

            var copy = Activator.CreateInstance(node?.GetType()) as IDisplayableNode;
            var vm = new NodeViewModel(copy, this.DeleteNodeCommand, this.InputPinCommand, this.OutputPinCommand, this.executionService);
            vm.Top = this.VerticalScrollerOffset + 20;
            vm.Left = this.HorizontalScrollerOffset + 20;
            this.actionManager.Execute(new CallMethodAction(() => this.Nodes.Add(vm), () => this.Nodes.Remove(vm)));
            vm.SnapToNewGrid(this.GridSize, false);
        }

        private async Task ClearCanvas()
        {
            await Task.Run(() =>
            {
                this.ClearedConnections.Push(this.Connections.ToList());
                this.ClearedNodes.Push(this.Nodes.ToList());

                foreach (var connectionVM in this.Connections)
                {
                    this.pinConnectorService.TryRemoveConnection(connectionVM.Connector);
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.Connections.Clear();
                    this.Nodes.Clear();
                });
            });
        }

        private async Task UndoClearCanvas()
        {
            await Task.Run(() =>
            {
                foreach (var nodeVM in this.ClearedNodes.Pop())
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        this.Nodes.Add(nodeVM);
                    });
                }

                foreach (var connector in this.ClearedConnections.Pop())
                {
                    this.pinConnectorService.TryConnectPins(connector.Input.Pin, connector.Output.Pin, out Connector newConnection, true);

                    this.pinConnectorService.ManuallyAddConnectionToExistingConnections(connector.Connector);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        this.Connections.Add(connector);
                    });
                }
            });
        }

        private void RepositionNodes()
        {
            foreach (var node in this.Nodes)
            {
                if (node.Left + node.Width + 20 >= this.BoardWidth)
                {
                    node.Left = this.BoardWidth - node.Width - 20;
                }

                if (node.Top + (20 * node.MaxPins) >= this.BoardHeight)
                {
                    node.Top = this.BoardHeight - (20 * node.MaxPins);
                }
            }
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
                        catch (Exception e)
                        {
                            this.logger.LogError(e, $"Unexpected error in {nameof(this.SnapAllNodesToGrid)}");
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
                        catch (Exception e)
                        {
                            this.logger.LogError(e, $"Unexpected error in {nameof(this.SnapAllNodesToGrid)}");
                        }
                    }
                }
            });
        }

        private void Connect()
        {
            this.Connect(this.InputPin, this.OutputPin);
        }
        
        private void CheckPossibleConnections(IEnumerable<IEnumerable<PinViewModel>> pinLists, IPin selectedPin)
        {
            foreach (var pinList in pinLists)
            {
                foreach (var pin in pinList)
                {
                    if (this.genericTypeComparerService.IsSameGenericType(pin.Pin, selectedPin))
                    {
                        pin.CanBeConnected = true;
                    }
                }
            }
        }

        private void Connect(PinViewModel inputPin, PinViewModel outputPin)
        {
            if (!(inputPin is null) && !(outputPin is null) && this.pinConnectorService.IsConnectable(inputPin.Pin, outputPin.Pin))
            {
                Connector createdConnector = null;
                ConnectorViewModel createdConnectorVM = null;
                var inPin = inputPin;
                var outPin = outputPin;
                Action creationAction = () =>
                {
                    this.MakeConnection(inPin, outPin, out createdConnectorVM, out createdConnector);
                };
                Action deleteAction = () => this.RemoveConnection(createdConnectorVM, createdConnector);

                this.actionManager.Execute(new CallMethodAction(creationAction, deleteAction));
                this.ResetPossibleConnections();
                return;
            }

            if (!(this.InputPin is null))
            {
                this.CheckPossibleConnections(this.nodes.Select(node => node.Outputs), this.InputPin.Pin);
                PreviewLineViewModel previewLineViewModel = this.PreviewLines[0];
                previewLineViewModel.PointOneX = this.InputPin.Left;
                previewLineViewModel.PointOneY = this.InputPin.Top;
            }

            if (!(this.OutputPin is null))
            {
                this.CheckPossibleConnections(this.nodes.Select(node => node.Inputs), this.OutputPin.Pin);
                PreviewLineViewModel previewLineViewModel = this.PreviewLines[0];
                previewLineViewModel.PointOneX = this.OutputPin.Left;
                previewLineViewModel.PointOneY = this.OutputPin.Top;
            }
        }

        private void MakeConnection(PinViewModel input, PinViewModel output, out ConnectorViewModel connectionVM, out Connector connection)
        {
            connectionVM = null;
            connection = null;

            Connector newConnection;

            if (this.pinConnectorService.TryConnectPins(input.Pin, output.Pin, out newConnection, false))
            {
                connection = newConnection;
                connectionVM = new ConnectorViewModel(newConnection, input, output, this.DeleteConnectionCommand);
                this.Connections.Add(connectionVM);
            }

            this.InputPin = null;
            this.OutputPin = null;
            return;
        }

        private void RemoveConnection(ConnectorViewModel connectionVM, Connector connection)
        {
            if (!(connectionVM is null) && !(connection is null))
            {
                if (this.pinConnectorService.TryRemoveConnection(connection))
                {
                    this.Connections.Remove(connectionVM);
                }
            }
        }
    }
}
