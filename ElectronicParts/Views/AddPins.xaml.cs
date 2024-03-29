﻿// ***********************************************************************
// Assembly         : ElectronicParts
// Author           : Alexander Fischer
// ***********************************************************************
// <copyright file="AddPins.xaml.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the AddPins class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Views
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for AddPins.
    /// </summary>
    public partial class AddPins : Window
    {
        /// <summary>
        /// The amount of pins to add.
        /// </summary>
        private int amount;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddPins"/> class.
        /// </summary>
        public AddPins()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.Types = new List<Type> { typeof(string), typeof(int), typeof(bool) };
            this.SelectedType = this.Types[2];
            this.Amount = 1;
        }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount of pins to be added.</value>
        public int Amount
        {
            get => this.amount;

            set
            {
                if (value <= 0)
                {
                    throw new ValidationException();
                }

                this.amount = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected type.
        /// </summary>
        /// <value>The elected type.</value>
        public Type SelectedType { get; set; }

        /// <summary>
        /// Gets or sets the available types.
        /// </summary>
        /// <value>The available types.</value>
        public List<Type> Types { get; set; }

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
    }
}
