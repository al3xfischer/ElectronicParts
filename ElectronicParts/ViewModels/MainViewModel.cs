using ElectronicParts.Commands;
<<<<<<< HEAD
using System;
using System.Windows;
=======
using Shared;
using System;
using System.Collections.ObjectModel;
>>>>>>> NodeTemplate
using System.Windows.Input;

namespace ElectronicParts.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
<<<<<<< HEAD
=======
        private ObservableCollection<NodeViewModel> nodes;

>>>>>>> NodeTemplate
        public MainViewModel()
        {
            this.SaveCommand = new RelayCommand(arg => { });
            this.LoadCommand = new RelayCommand(arg => { });
            this.ExitCommand = new RelayCommand(arg => Environment.Exit(0));
<<<<<<< HEAD
        }

=======
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

>>>>>>> NodeTemplate
        public ICommand SaveCommand { get; }

        public ICommand LoadCommand { get; }

        public ICommand ExitCommand { get; }
    }
}
