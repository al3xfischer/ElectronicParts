using ElectronicParts.DI;
using ElectronicParts.ViewModels;
using System.Text.RegularExpressions;
using System.Windows;
using Xceed.Wpf.Toolkit;

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
            this.BoolSelection = new bool[] { true, false };
        }

        public PreferencesViewModel ViewModel { get; }
        
        public bool[] BoolSelection { get; set; }

        private void PreviewNumberInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[^\\d-]+$");
            e.Handled = regex.IsMatch(e.Text);            
        }

        private void GridViewValueColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.SortByValue();
        }
    }
}
