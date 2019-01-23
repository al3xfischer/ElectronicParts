using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicParts.Views
{
    /// <summary>
    /// Interaction logic for AddPins.xaml
    /// </summary>
    public partial class AddPins : Window
    {
        /// <summary>
        /// The amount of pins to add.
        /// </summary>
        private int amount;

        public AddPins()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Types = new List<Type> { typeof(string), typeof(int), typeof(bool) };
            this.SelectedType = this.Types[2];
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
        public int Amount
        {
            get => this.amount;

            set
            {
                if(value <= 0)
                {
                    throw new ValidationException();
                }

                this.amount = value;
            }
        }

        /// <summary>
        /// Handles the Click event of the Ok control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(this.amoutTextBox))
            {
                return;
            }

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
