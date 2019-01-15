using ElectronicParts.Commands;
using Shared;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ElectronicParts.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<IDisplayableNode> nodes;

        public MainViewModel()
        {
            this.SaveCommand = new RelayCommand(arg => { });
            this.LoadCommand = new RelayCommand(arg => { });
            this.ExitCommand = new RelayCommand(arg => Environment.Exit(0));
            this.Nodes = new ObservableCollection<IDisplayableNode>
            {
                new TestNode()
            };
        }

        public ObservableCollection<IDisplayableNode> Nodes
        {
            get => this.nodes;

            set
            {
                Set(ref this.nodes, value);
            }
        }

        public ICommand SaveCommand { get; }

        public ICommand LoadCommand { get; }

        public ICommand ExitCommand { get; }
    }
}
