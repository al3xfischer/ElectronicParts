using ElectronicParts.Commands;
using System;
using System.Windows;
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
            this.Nodes = new ObservableCollection<NodeViewModel>
            {
                new NodeViewModel(new TestNode())
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

        public ICommand SaveCommand { get; }

        public ICommand LoadCommand { get; }

        public ICommand ReloadAssembliesCommand { get; }

        public ICommand ExitCommand { get; }
    }
}
