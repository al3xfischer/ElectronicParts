using ElectronicParts.DI;
using ElectronicParts.ViewModels;
using System.Text.RegularExpressions;
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

        private void PreviewNumberInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[^\\d]+$");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
