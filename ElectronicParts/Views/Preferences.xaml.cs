// ***********************************************************************
// Assembly         : ElectronicParts
// Author           : 
// ***********************************************************************
// <copyright file="Preferences.xaml.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the Preferences class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Views
{
    using System.Text.RegularExpressions;
    using System.Windows;
    using ElectronicParts.DI;
    using ElectronicParts.ViewModels;
    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for Preferences.
    /// </summary>
    public partial class Preferences : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Preferences"/> class.
        /// </summary>
        public Preferences()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.ViewModel = Container.Resolve<PreferencesViewModel>();
        }

        /// <summary>
        /// Gets the view model of the <see cref="Preferences"/> class.
        /// </summary>
        /// <value>The view model of the <see cref="Preferences"/> class.</value>
        public PreferencesViewModel ViewModel { get; }

        /// <summary>
        /// This method gets called when value column of a grid view is clicked.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void GridViewValueColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.SortByValue();
        }

        /// <summary>
        /// This method gets called when the user tries to write text into the integer.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void PreviewNumberInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[^\\d-]+$");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
