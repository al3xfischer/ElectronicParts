using ElectronicParts.Commands;
using System;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ElectronicParts.Models;

namespace ElectronicParts.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<NodeViewModel> nodes;
        private ObservableCollection<Connector> connections;
        private ObservableCollection<NodeViewModel> availableNodes;

        public MainViewModel()
        {
            this.SaveCommand = new RelayCommand(arg => { });
            this.LoadCommand = new RelayCommand(arg => { });
            this.ExitCommand = new RelayCommand(arg => Environment.Exit(0));
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

        public ICommand LoadCommand { get; }

        public ICommand ReloadAssembliesCommand { get; }

        public ICommand ExitCommand { get; }

        public ICommand AddNodeCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand InputPinCommand { get; }

        public ICommand OutputPinCommand { get; }
    }
}
