using ElectronicParts.Commands;
using System;
using System.Windows;
using System.Linq;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ElectronicParts.Models;
using ElectronicParts.Services.Interfaces;
using System.Threading.Tasks;

namespace ElectronicParts.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<NodeViewModel> nodes;
        private ObservableCollection<Connector> connections;
        private ObservableCollection<NodeViewModel> availableNodes;
        private readonly IExecutionService myExecutionService;

        public MainViewModel(IExecutionService executionService)
        {
            this.myExecutionService = executionService ?? throw new ArgumentNullException(nameof(executionService));

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

            this.Nodes = new ObservableCollection<NodeViewModel>
            {
                new NodeViewModel(new TestNode())
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
    }
}
