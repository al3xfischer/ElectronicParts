﻿// ***********************************************************************
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
    using ElectronicParts.Models;
    using ElectronicParts.Services.Interfaces;
    using Shared;

    /// <summary>
    /// Represents the <see cref="ConnectorViewModel"/> class.
    /// </summary>
    public class ConnectorViewModel : BaseViewModel
    {
        /// <summary>
        /// Represents the Helper service.
        /// </summary>
        private readonly IConnectorHelperService helperService;

        /// <summary>
        /// Represents the offset needed in case of multiple outputs.
        /// </summary>
        private int multipleOutputOffset;

        /// <summary>
        /// Represents the offset needed in case of multiple output connections.
        /// </summary>
        private int multipleConnectionsOffset;

        /// <summary>
        /// Represents the offset needed always.
        /// </summary>
        private double defaultOffset;

        /// <summary>
        /// Represents the pin count of the containing node.
        /// </summary>
        private int pinCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectorViewModel" /> class.
        /// </summary>
        /// <param name="connector">The connector represented by this view model.</param>
        /// <param name="input">The input pin as <see cref="PinViewModel" />.</param>
        /// <param name="output">The output pin as <see cref="PinViewModel" />.</param>
        /// <param name="deletionCommand">The <see cref="ICommand" /> to delete the connection.</param>
        /// <param name="helperService">The helper service.</param>
        /// <exception cref="ArgumentNullException">
        /// Connector
        /// or
        /// input
        /// or
        /// output
        /// or
        /// deletionCommand
        /// or
        /// helperService.
        /// </exception>
        public ConnectorViewModel(Connector connector, PinViewModel input, PinViewModel output, ICommand deletionCommand, IConnectorHelperService helperService)
        {
            this.Connector = connector ?? throw new ArgumentNullException(nameof(connector));
            this.Input = input ?? throw new ArgumentNullException(nameof(input));
            this.Output = output ?? throw new ArgumentNullException(nameof(output));
            this.DeleteCommand = deletionCommand ?? throw new ArgumentNullException(nameof(deletionCommand));
            this.helperService = helperService ?? throw new ArgumentNullException(nameof(helperService));
            this.RecalculateOffsets();
            this.Input.OnValueChanged += this.RefreshPins;
            this.Output.OnValueChanged += this.RefreshPins;
            this.Input.PropertyChanged += this.Input_PropertyChanged;
            this.Output.PropertyChanged += this.Output_PropertyChanged;
        }

        /// <summary>
        /// Gets the center bottom point.
        /// </summary>
        /// <value>The center bottom point.</value>
        public Point CenterBottomPoint
        {
            get
            {
                if (this.Output.Left > (this.Input.Left + this.Output.Left) / 2)
                {
                    if (this.helperService.IsSelfConnecting(this.Input.Pin, this.Output.Pin))
                    {
                        var step = this.defaultOffset < 0 ? this.pinCount * -10 : this.pinCount * 10;

                        if (this.helperService.IsInputsMore(this.Input.Pin))
                        {
                            return new Point(this.Input.Left - (Math.Abs(this.defaultOffset) * 10), this.Input.Top + step);
                        }

                        return new Point(this.Input.Left - (Math.Abs(this.defaultOffset) * 10), this.Output.Top + step);
                    }

                    return new Point(this.Input.Left - (10 * this.multipleConnectionsOffset) - this.multipleOutputOffset, ((this.Input.Top + this.Output.Top) / 2) + this.multipleOutputOffset);
                }

                return new Point(((this.Input.Left + this.Output.Left) / 2) + (this.multipleConnectionsOffset * 10) + this.multipleOutputOffset, this.Input.Top);
            }
        }

        /// <summary>
        /// Gets the center top point.
        /// </summary>
        /// <value>The center top point.</value>
        public Point CenterTopPoint
        {
            get
            {
                if (this.Output.Left > (this.Input.Left + this.Output.Left) / 2)
                {
                    if (this.helperService.IsSelfConnecting(this.Input.Pin, this.Output.Pin))
                    {
                        var step = this.defaultOffset < 0 ? this.pinCount * -10 : this.pinCount * 10;
                        if (this.helperService.IsInputsMore(this.Input.Pin))
                        {
                            return new Point(this.Output.Left + (Math.Abs(this.defaultOffset) * 10), this.Input.Top + step);
                        }
                       
                        return new Point(this.Output.Left + (Math.Abs(this.defaultOffset) * 10), this.Output.Top + step);
                    }

                    return new Point(this.Output.Left + (10 * this.multipleConnectionsOffset) + this.multipleOutputOffset, ((this.Input.Top + this.Output.Top) / 2) + this.multipleOutputOffset);
                }

                return new Point(((this.Input.Left + this.Output.Left) / 2) + (this.multipleConnectionsOffset * 10) + this.multipleOutputOffset, this.Output.Top);
            }
        }

        /// <summary>
        /// Gets or sets the connector object.
        /// </summary>
        /// <value>The connector object.</value>
        public Connector Connector { get; set; }

        /// <summary>
        /// Gets the common value of the connection.
        /// </summary>
        /// <value>The the common value of the connection.</value>
        public IValue CurrentValue { get => this.Connector.CommonValue; }

        /// <summary>
        /// Gets the delete command.
        /// </summary>
        /// <value>The delete command.</value>
        public ICommand DeleteCommand { get; }

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
        /// Gets a point which is used for the line in case that the connection connects input and output of the same node.
        /// </summary>
        /// <value>The self connection input point.</value>
        public Point SelfConnectionInputPoint
        {
            get
            {
                if (!this.helperService.IsSelfConnecting(this.Input.Pin, this.Output.Pin))
                {
                    if (this.Output.Left > (this.Input.Left + this.Output.Left) / 2)
                    {
                        return new Point(this.Input.Left - (this.multipleConnectionsOffset * 10) - this.multipleOutputOffset, this.Input.Top);
                    }

                    return new Point(this.Input.Left, this.Input.Top);
                }

                return new Point(this.Input.Left - (Math.Abs(this.defaultOffset) * 10), this.Input.Top);
            }
        }

        /// <summary>
        /// Gets a point which is used for the line in case that the connection connects input and output of the same node.
        /// </summary>
        /// <value>The self connection output point.</value>
        public Point SelfConnectionOutputPoint
        {
            get
            {
                if (!this.helperService.IsSelfConnecting(this.Input.Pin, this.Output.Pin))
                {
                    if (this.Output.Left > (this.Input.Left + this.Output.Left) / 2)
                    {
                        return new Point(this.Output.Left + (this.multipleConnectionsOffset * 10) + this.multipleOutputOffset, this.Output.Top);
                    }

                    return new Point(this.Output.Left, this.Output.Top);
                }

                return new Point(this.Output.Left + (Math.Abs(this.defaultOffset) * 10), this.Output.Top);
            }
        }

        /// <summary>
        /// Updates the view by calling the INotifyPropertyChanged event of the base view model.
        /// </summary>
        public void Update()
        {
            this.FirePropertyChanged(nameof(this.CurrentValue));
        }

        /// <summary>
        /// Recalculates the offsets and updates the lines.
        /// </summary>
        public void RecalculateOffsets()
        {
            this.multipleOutputOffset = this.helperService.GetMultipleOutputOffset(this.Output.Pin);
            this.multipleConnectionsOffset = this.helperService.MultipleConnectionsOffset(this.Output.Pin, this.Connector);
            this.defaultOffset = this.helperService.GetOffset(this.Input.Pin, this.Output.Pin, out this.pinCount);
            this.UpdateLine();
        }

        /// <summary>
        /// Forces the line to check for new position points.
        /// </summary>
        public void UpdateLine()
        {
            this.FirePropertyChanged(nameof(this.CenterBottomPoint));
            this.FirePropertyChanged(nameof(this.CenterTopPoint));
            this.FirePropertyChanged(nameof(this.SelfConnectionInputPoint));
            this.FirePropertyChanged(nameof(this.SelfConnectionOutputPoint));
        }

        /// <summary>
        /// This method is called when the input changes.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void Input_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.Input.Left) || e.PropertyName == nameof(this.Input.Top))
            {
                this.FirePropertyChanged(string.Empty);
            }
        }

        /// <summary>
        /// This method is called when the output changes.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void Output_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.Output.Left) || e.PropertyName == nameof(this.Output.Top))
            {
                this.FirePropertyChanged(string.Empty);
            }
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
