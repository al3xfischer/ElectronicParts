using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ElectronicParts.Views
{
    /// <summary>
    /// Interaction logic for AddPins.xaml
    /// </summary>
    public partial class AddPins : Window
    {
        public AddPins()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Types = new List<Type> { typeof(string), typeof(int), typeof(bool) };
        }

        /// <summary>
        /// Gets or sets the available types.
        /// </summary>
        /// <value>The types.</value>
        public List<Type> Types { get; set; }

        /// <summary>
        /// Gets or sets the type of the selected.
        /// </summary>
        /// <value>The type of the selected.</value>
        public Type SelectedType { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public int Amount { get; set; }

        /// <summary>
        /// Handles the Click event of the Ok control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the Cancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
