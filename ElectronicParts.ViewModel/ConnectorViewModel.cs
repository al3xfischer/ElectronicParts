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
    using System.Windows;
    using System.Windows.Input;
    using ElectronicParts.Services.Interfaces;
    using ElectronicParts.Models;
    using Shared;

    /// <summary>
    /// Represents the <see cref="ConnectorViewModel"/> class.
    /// </summary>
    public class ConnectorViewModel : BaseViewModel
    {
        private IConnectorHelperService helperService;
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectorViewModel"/> class.
        /// </summary>
        /// <param name="connector">The connector represented by this view model.</param>
        /// <param name="input">The input pin as <see cref="PinViewModel"/>.</param>
        /// <param name="output">The output pin as <see cref="PinViewModel"/>.</param>
        /// <param name="deletionCommand">The <see cref="ICommand"/> to delete the connection.</param>
        public ConnectorViewModel(Connector connector, PinViewModel input, PinViewModel output, ICommand deletionCommand, IConnectorHelperService helperService)
        {

            this.Connector = connector ?? throw new ArgumentNullException(nameof(connector));
            this.Input = input ?? throw new ArgumentNullException(nameof(input));
            this.Output = output ?? throw new ArgumentNullException(nameof(output));
            this.DeleteCommand = deletionCommand ?? throw new ArgumentNullException(nameof(deletionCommand));
            this.helperService = helperService ?? throw new ArgumentNullException(nameof(helperService));
            this.Input.OnValueChanged += this.RefreshPins;
            this.Output.OnValueChanged += this.RefreshPins;
            this.Input.PropertyChanged += Input_PropertyChanged;
            this.Output.PropertyChanged += Output_PropertyChanged;
        }

        private void Output_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.Output.Left) || e.PropertyName == nameof(this.Output.Top))
            {
                this.FirePropertyChanged(string.Empty);
            }
        }

        private void Input_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.Input.Left) || e.PropertyName == nameof(this.Input.Top))
            {
                this.FirePropertyChanged(string.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the connector object.
        /// </summary>
        /// <value>The connector object.</value>
        public Connector Connector { get; set; }

        public Point CenterBottomPoint
        {
            get
            {
                if (this.Output.Left > (this.Input.Left + this.Output.Left) / 2)
                {
                    if (this.helperService.IsSelfConnecting(this.Input.Pin, this.Output.Pin))
                    {
                        var offset = this.helperService.GetOffset(this.Input.Pin, this.Output.Pin);
                        return new Point(this.Input.Left - Math.Abs(offset) * 10, this.Input.Top + offset * 35);
                    }
                    return new Point(this.Input.Left, ((this.Input.Top + this.Output.Top)) / 2);
                }
                return new Point((this.Input.Left + this.Output.Left) / 2, this.Input.Top);
            }
        }

        public Point CenterTopPoint
        {
            get
            {
                if (this.Output.Left > (this.Input.Left + this.Output.Left) / 2)
                {
                    if (this.helperService.IsSelfConnecting(this.Input.Pin, this.Output.Pin))
                    {
                        var offset = this.helperService.GetOffset(this.Input.Pin, this.Output.Pin);
                        return new Point(this.Output.Left + Math.Abs(offset) * 10, this.Input.Top + offset * 35);
                    }
                    return new Point((this.Output.Left), (this.Input.Top + this.Output.Top) / 2);
                }
                return new Point((this.Input.Left + this.Output.Left) / 2, this.Output.Top);
            }
        }

        public Point SelfConnectionInputPoint
        {
            get
            {
                if (!this.helperService.IsSelfConnecting(this.Input.Pin, this.Output.Pin))
                {
                    return new Point(this.Input.Left, this.Input.Top);
                }

                var offset = Math.Abs(this.helperService.GetOffset(this.Input.Pin, this.Output.Pin));
                return new Point(this.Input.Left - offset * 10, this.Input.Top);
            }
        }

        public Point SelfConnectionOutputPoint
        {
            get
            {
                if (!this.helperService.IsSelfConnecting(this.Input.Pin, this.Output.Pin))
                {
                    return new Point(this.Output.Left, this.Output.Top);
                }

                var offset = Math.Abs(this.helperService.GetOffset(this.Input.Pin, this.Output.Pin));
                return new Point(this.Output.Left + offset * 10, this.Output.Top);
            }
        }

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
            this.FirePropertyChanged(string.Empty);
        }
    }
}
