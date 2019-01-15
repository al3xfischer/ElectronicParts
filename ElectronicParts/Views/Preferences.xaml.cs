using ElectronicParts.DI;
using ElectronicParts.ViewModels;
using System.Windows;

namespace ElectronicParts.Views
{
    /// <summary>
    /// Interaction logic for Preferences.xaml
    /// </summary>
    public partial class Preferences : Window
    {
        public Preferences()
        {
            InitializeComponent();
            this.DataContext = this;
            this.ViewModel = Container.Resolve<PreferencesViewModel>();
        }

        public PreferencesViewModel ViewModel { get; }


    }
}
