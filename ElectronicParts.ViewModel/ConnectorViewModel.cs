// ***********************************************************************
// Author           : Roman Jahn
// ***********************************************************************
// <copyright file="ConnectorViewModel.cs" company="FHWN">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>Represents the ConnectorViewModel class of the ElectronicParts Programm</summary>
// ***********************************************************************
namespace ElectronicParts.ViewModels
{
    using System;
    using System.Windows.Input;
    using ElectronicParts.Models;
    using Shared;

    /// <summary>
    /// Represents the <see cref="ConnectorViewModel"/> class.
    /// </summary>
    public class ConnectorViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectorViewModel"/> class.
        /// </summary>
        /// <param name="connector">The connector object.</param>
        /// <param name="input">The input pin as <see cref="PinViewModel"/>.</param>
        /// <param name="output">The output pin as <see cref="PinViewModel"/>.</param>
        /// <param name="deletionCommand">The <see cref="ICommand"/> to delete the connection.</param>
        public ConnectorViewModel(Connector connector, PinViewModel input, PinViewModel output, ICommand deletionCommand)
        {
            this.Connector = connector ?? throw new ArgumentNullException(nameof(connector));
            this.Input = input ?? throw new ArgumentNullException(nameof(input));
            this.Output = output ?? throw new ArgumentNullException(nameof(output));
            this.DeleteCommand = deletionCommand ?? throw new ArgumentNullException(nameof(deletionCommand));
            this.Input.OnValueChanged += this.RefreshPins;
            this.Output.OnValueChanged += this.RefreshPins;
        }

        /// <summary>
        /// Gets or sets the connector object.
        /// </summary>
        /// <value>The connector object.</value>
        public Connector Connector { get; set; }

        /// <summary>
        /// Gets the input pin view model.
        /// </summary>
        /// <value>The input pin view model.</value>
        public PinViewModel Input { get; }

        /// <summary>
        /// Gets the output pin view model.
        /// </summary>
        /// <value>The output pin view model.</value>
        public PinViewModel Output { get; }

        /// <summary>
        /// Gets the delete command.
        /// </summary>
        /// <value>The delete command.</value>
        public ICommand DeleteCommand { get; }

        /// <summary>
        /// Gets the common value of the connection.
        /// </summary>
        /// <value>The the common value of the connection.</value>
        public IValue CurrentValue
        {
            get => this.Connector.CommonValue;
        }

        /// <summary>
        /// Updates the view by calling the INotifyPropertyChanged event of the base view model.
        /// </summary>
        public void Update()
        {
            this.FirePropertyChanged(nameof(this.CurrentValue));
        }

        /// <summary>
        /// Refreshes the input and output pins.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The EventArgs of the event.</param>
        private void RefreshPins(object sender, EventArgs e)
        {
            this.Output?.Refresh();
            this.Input?.Refresh();
        }
    }
}
