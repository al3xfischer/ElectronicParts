// ***********************************************************************
// Author           : Alexander Fischer
// ***********************************************************************
// <copyright file="NodeViewModel.cs" company="FHWN">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>Represents the NodeViewModel class of the ElectronicParts Programm</summary>
// ***********************************************************************

namespace ElectronicParts.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Input;
    using ElectronicParts.Services.Interfaces;
    using ElectronicParts.ViewModels.Commands;
    using Shared;

    /// <summary>
    /// Represents the <see cref="NodeViewModel"/> class.
    /// </summary>
    public class NodeViewModel : BaseViewModel
    {
        /// <summary>
        /// Contains the execution service.
        /// </summary>
        private readonly IExecutionService executionService;

        /// <summary>
        /// The input pin  connection command.
        /// </summary>
        private readonly ICommand inputPinCommand;

        /// <summary>
        /// The output pin connection command.
        /// </summary>
        private readonly ICommand outputPinCommand;

        /// <summary>
        /// Contains the left value of the node.
        /// </summary>
        private double left;

        /// <summary>
        /// Contains the top value of the node.
        /// </summary>
        private double top;

        /// <summary>
        /// Contains the width value of the node.
        /// </summary>
        private int width;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeViewModel"/> class.
        /// </summary>
        /// <param name="node">The node represented by this view model.</param>
        /// <param name="deleteCommand">The command to delete this view model.</param>
        /// <param name="inputPinCommand">The command to be invoked connecting an input pin.</param>
        /// <param name="outputPinCommand">The command to be invoked connecting an output pin.</param>
        /// <param name="executionService">The execution service.</param>
        public NodeViewModel(IDisplayableNode node, ICommand deleteCommand, ICommand inputPinCommand, ICommand outputPinCommand, IExecutionService executionService)
        {
            this.Node = node ?? throw new ArgumentNullException(nameof(node));
            this.DeleteCommand = deleteCommand ?? throw new ArgumentNullException(nameof(deleteCommand));
            this.inputPinCommand = inputPinCommand ?? throw new ArgumentNullException(nameof(inputPinCommand));
            this.outputPinCommand = outputPinCommand ?? throw new ArgumentNullException(nameof(outputPinCommand));
            this.executionService = executionService ?? throw new ArgumentNullException(nameof(executionService));
            this.Inputs = node.Inputs?.Select(n => new PinViewModel(n, inputPinCommand, this.executionService)).ToObservableCollection();
            this.Outputs = node.Outputs?.Select(n => new PinViewModel(n, outputPinCommand, this.executionService)).ToObservableCollection();
            this.Top = 18;
            this.Left = 20;
            this.Width = 50;
            this.executionService.OnIsEnabledChanged += (sender, e) => this.FirePropertyChanged(nameof(CanAddPin));

            this.IncreaseWidthCommand = new RelayCommand(arg =>
            {
                this.Width += 20;
                this.UpdatePosition();
                this.FirePropertyChanged(string.Empty);
            },arg => !this.executionService.IsEnabled);

            this.DecreaseWidthCommand = new RelayCommand(arg =>
            {
                this.Width -= 20;
                this.UpdatePosition();
                this.FirePropertyChanged(string.Empty);
            },arg => !this.executionService.IsEnabled);

            this.ActivateCommand = new RelayCommand(arg =>
            {
                this.Node.Activate();
                if (!(this.Inputs is null))
                {
                    foreach (var input in this.Inputs)
                    {
                        input.Update();
                    }
                }

                if (!(this.Outputs is null))
                {
                    foreach (var output in this.Outputs)
                    {
                        output.Update();
                    }
                }
            },arg => this.executionService.IsEnabled);

            try
            {
                this.Node.PictureChanged += this.NodePictureChanged;
            }
            catch (Exception)
            {
                Debug.WriteLine("An exception occured while subscribing to the PictureChanged event of the node.");
            }
        }

        /// <summary>
        /// Gets the command to activate the node.
        /// </summary>
        /// <value>The command to activate the node.</value>
        public ICommand ActivateCommand { get; }

        /// <summary>
        /// Gets the command to decrease the width of the node.
        /// </summary>
        /// <value>The command to decrease the width of the node.</value>
        public ICommand DecreaseWidthCommand { get; }

        /// <summary>
        /// Gets the command to delete the node.
        /// </summary>
        /// <value>The command to delete the node.</value>
        public ICommand DeleteCommand { get; }

        /// <summary>
        /// Gets the description of the node.
        /// </summary>
        /// <value>The description of the node.</value>
        public string Description { get => this.Node.Description; }

        /// <summary>
        /// Gets the command to increase the width of the node.
        /// </summary>
        /// <value>The command to increase the width of the node.</value>
        public ICommand IncreaseWidthCommand { get; }

        /// <summary>
        /// Gets the input pins of the node.
        /// </summary>
        /// <value>The input pins of the node.</value>
        public ObservableCollection<PinViewModel> Inputs { get; }

        /// <summary>
        /// Gets the label of the node.
        /// </summary>
        /// <value>The label of the node.</value>
        public string Label { get => this.Node.Label; }

        /// <summary>
        /// Gets a value indicating whether pins can be added.
        /// </summary>
        /// <value>Whether pins can be added.</value>
        public bool CanAddPin { get => !this.executionService.IsEnabled; }

        /// <summary>
        /// Gets or sets the left of the node.
        /// </summary>
        /// <value >The left of the node.</value>
        public double Left
        {
            get => this.left;

            set
            {
                if (value < 0)
                {
                    this.Set(ref this.left, 0);
                }
                else
                {
                    this.Set(ref this.left, value);
                }

                this.UpdateLeft(this.Inputs, (int)this.left + 16);

                if (this.Inputs is null || this.Inputs.Count == 0)
                {
                    this.UpdateLeft(this.Outputs, (int)this.Left + this.Width + 12);
                }
                else
                {
                    this.UpdateLeft(this.Outputs, (int)this.Left + this.Width + 36);
                }
            }
        }

        /// <summary>
        /// Gets the maximum number of input or output pins.
        /// Returns number of input pins if greater than output pins, otherwise returns number of output pins. 
        /// </summary>
        /// <value>The maximum number of input or output pins.</value>
        public int MaxPins
        {
            get
            {
                return (this.Inputs?.Count >= this.Outputs?.Count ? this.Inputs?.Count : this.Outputs?.Count) ?? 0;
            }
        }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <value>The node of this view model.</value>
        public IDisplayableNode Node { get; }

        /// <summary>
        /// Gets the output pins of the node.
        /// </summary>
        /// <value>The output pins of the node.</value>
        public ObservableCollection<PinViewModel> Outputs { get; }

        /// <summary>
        /// Gets the picture of the node.
        /// </summary>
        /// <value>The picture of the node.</value>
        public Bitmap Picture { get => this.Node.Picture; }

        /// <summary>
        /// Gets or sets the top of the node.
        /// </summary>
        /// <value >The top of the node.</value>
        public double Top
        {
            get => this.top;

            set
            {
                if (value < 0)
                {
                    this.Set(ref this.top, 0);
                }
                else
                {
                    this.Set(ref this.top, value);
                }

                this.UpdateTop(this.Inputs?.Select((p, i) => Tuple.Create(p, i)), (int)this.Top);
                this.UpdateTop(this.Outputs?.Select((p, i) => Tuple.Create(p, i)), (int)this.Top);
            }
        }

        /// <summary>
        /// Gets the type of the node.
        /// </summary>
        /// <value>The type of the node.</value>
        public NodeType Type { get => this.Node.Type; }

        /// <summary>
        /// Gets the width of the node.
        /// </summary>
        /// <value >The width of the node.</value>
        public int Width
        {
            get => this.width;

            private set
            {
                if (value < 50)
                {
                    this.width = 50;
                    return;
                }

                if (value > 200)
                {
                    this.width = 200;
                    return;
                }

                this.width = value;
            }
        }

        /// <summary>
        /// Adds the delegate NodePictureChanged to the PictureChanged event of the node.
        /// </summary>
        public void AddDelegate()
        {
            this.Node.PictureChanged -= this.NodePictureChanged;
            this.Node.PictureChanged += this.NodePictureChanged;
        }

        /// <summary>
        /// Adds the input pins.
        /// </summary>
        /// <param name="pins">The input pins.</param>
        public void AddInputPins(IEnumerable<IPin> pins)
        {
            if (this.Inputs is null)
            {
                return;
            }

            foreach (var pin in pins)
            {
                this.Inputs.Add(new PinViewModel(pin, this.inputPinCommand, this.executionService));
                this.Node.Inputs.Add(pin);
            }

            this.Top = this.Top;
            this.Left = this.Left;
            this.FirePropertyChanged(string.Empty);
        }

        /// <summary>
        /// Adds the output pins.
        /// </summary>
        /// <param name="pins">The output pins.</param>
        public void AddOutputPins(IEnumerable<IPin> pins)
        {
            if (this.Outputs is null)
            {
                return;
            }

            foreach (var pin in pins)
            {
                this.Outputs.Add(new PinViewModel(pin, this.outputPinCommand, this.executionService));
                this.Node.Outputs.Add(pin);
            }

            this.Top = this.Top;
            this.Left = this.Left;
            this.FirePropertyChanged(string.Empty);
        }

        /// <summary>
        /// Removes the delegate NodePictureChanged from the PictureChanged event of the node.
        /// </summary>
        public void RemoveDelegate()
        {
            this.Node.PictureChanged -= this.NodePictureChanged;
        }

        /// <summary>
        /// Snaps to grid.
        /// Will round to the next possible value.
        /// </summary>
        /// <param name="gridSize">Size of the grid.</param>
        public void SnapToNewGrid(int gridSize)
        {
            int leftOffset;
            leftOffset = this.Inputs.Count > 0 ? 10 : 0;

            if ((this.Left + leftOffset) % gridSize != 0)
            {
                this.Left = Math.Max(this.Left.RoundTo(gridSize) - leftOffset, 0);
            }

            if (this.Top % gridSize != 0)
            {
                this.Top = Math.Max(this.Top.RoundTo(gridSize), 0);
            }
        }

        /// <summary>
        /// Snaps to grid.
        /// </summary>
        /// <param name="gridSize">Size of the grid.</param>
        /// <param name="floor">If set to true will floor value else will ceil value.</param>
        public void SnapToNewGrid(int gridSize, bool floor)
        {
            int leftOffset = this.Inputs?.Count > 0 ? 10 : 0;

            if (floor)
            {
                if ((this.Left + leftOffset) % gridSize != 0)
                {
                    this.Left = Math.Max(this.Left.FloorTo(gridSize) - leftOffset, 0);
                }

                if (this.Top % gridSize != 0)
                {
                    this.Top = Math.Max(this.Top.FloorTo(gridSize), 0);
                }
            }
            else
            {
                if ((this.Left + leftOffset) % gridSize != 0)
                {
                    this.Left = Math.Max(this.Left.CeilingTo(gridSize) - leftOffset, 0);
                }

                if (this.Top % gridSize != 0)
                {
                    this.Top = Math.Max(this.Top.CeilingTo(gridSize), 0);
                }
            }
        }

        /// <summary>
        /// Updates the (picture of the) node.
        /// </summary>
        public void Update()
        {
            this.FirePropertyChanged(nameof(this.Picture));
        }

        /// <summary>
        /// Updates the position of the outputs pins.
        /// </summary>
        public void UpdatePosition()
        {
            if (this.Inputs is null || this.Inputs.Count == 0)
            {
                this.UpdateLeft(this.Outputs, (int)this.Left + this.Width + 13);
            }
            else
            {
                this.UpdateLeft(this.Outputs, (int)this.Left + this.Width + 23);
            }
        }

        /// <summary>
        /// Invokes the INotifyPropertyChanged event of the <see cref="BaseViewModel"/>
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> for this event.</param>
        private void NodePictureChanged(object sender, EventArgs e)
        {
            this.FirePropertyChanged(nameof(this.Picture));
        }

        /// <summary>
        /// Updates the left value of each given pin to the given value.
        /// </summary>
        /// <param name="pins">The pins to be updated.</param>
        /// <param name="value">The new value for left.</param>
        private void UpdateLeft(IEnumerable<PinViewModel> pins, int value)
        {
            if (pins is null)
            {
                return;
            }

            foreach (var pin in pins)
            {
                pin.Left = value;
            }
        }

        /// <summary>
        /// Updates the top value of each given pin to the given value.
        /// </summary>
        /// <param name="pins">The pins to be updated.</param>
        /// <param name="value">The new value for top.</param>
        private void UpdateTop(IEnumerable<Tuple<PinViewModel, int>> pins, int value)
        {
            if (pins is null)
            {
                return;
            }

            foreach (var pin in pins)
            {
                pin.Item1.Top = (pin.Item2 * 19) + value + 16;
            }
        }
    }
}
