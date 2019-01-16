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
        }

        public PreferencesViewModel ViewModel { get; }

        private void PreviewNumberInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[^\\d]+$");
            e.Handled = regex.IsMatch(e.Text);            
        }

        private void StringColorPickerSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            ViewModel.SelectedStringColorString = this.StringColorPicker.SelectedColorText;
        }

        private void IntColorPickerSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            ViewModel.SelectedIntColorString = this.IntColorPicker.SelectedColorText;
        }

        private void BoolColorPickerSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            ViewModel.SelectedBoolColorString = this.BoolColorPicker.SelectedColorText;
        }
    }
}
