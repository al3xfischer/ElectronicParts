using ElectronicParts.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace ElectronicParts.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            this.SaveCommand = new RelayCommand(arg => { });
            this.LoadCommand = new RelayCommand(arg => { });
            this.ExitCommand = new RelayCommand(arg => Environment.Exit(0));
        }

        public ICommand SaveCommand { get; }

        public ICommand LoadCommand { get; }

        public ICommand ExitCommand { get; }
    }
}
