using ElectronicParts.Commands;
using System;
using System.Windows;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ElectronicParts.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<NodeViewModel> nodes;

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

        public NodeViewModel SelectedNode { get; set; }

        public ICommand SaveCommand { get; }

        public ICommand LoadCommand { get; }

        public ICommand ExitCommand { get; }
    }
}
