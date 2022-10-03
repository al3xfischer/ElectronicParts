// ***********************************************************************
// Author           : Alexander Fischer, Kevin Janisch, Peter Helf, Roman Jahn
// ***********************************************************************
// <copyright file="PinViewModel.cs" company="FHWN">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>Represents the PinViewModel class of the ElectronicParts Programm</summary>
// ***********************************************************************

namespace ElectronicParts.ViewModels
{
    using System;
    using System.Timers;
    using System.Windows.Input;
    using ElectronicParts.Services.Interfaces;
    using Shared;
    
    /// <summary>
    /// Represents the <see cref="PinViewModel"/> class.
    /// </summary>
    public class PinViewModel : BaseViewModel
    {
        /// <summary>
        /// Contains the execution service.
        /// </summary>
        private readonly IExecutionService executionService;

        /// <summary>
        /// Contains the left value of the pin.
        /// </summary>
        private int left;

        /// <summary>
        /// Contains the top value of the pin.
        /// </summary>
        private int top;

        /// <summary>
        /// Contains the timer.
        /// </summary>
        private readonly Timer timer;

        /// <summary>
        /// Contains a value indicating whether the pin can be connected.
        /// </summary>
        private bool canBeConnected;

        /// <summary>
        /// Initializes a new instance of the <see cref="PinViewModel"/> class.
        /// </summary>
        /// <param name="pin">The pin represented by this view model.</param>
        /// <param name="connectCommand">The command to be executed if pin gets connected.</param>
        /// <param name="executionService">The execution service.</param>
        public PinViewModel(IPin pin, ICommand connectCommand, IExecutionService executionService)
        {
            this.timer = new Timer
            {
                Interval = 10
            };
            this.timer.Elapsed += (sender, e) => this.Refresh();
            this.Pin = pin ?? throw new ArgumentNullException(nameof(pin));
            this.ConnectCommand = connectCommand ?? throw new ArgumentNullException(nameof(connectCommand));
            this.executionService = executionService ?? throw new ArgumentNullException(nameof(executionService));
            this.executionService.OnIsEnabledChanged += (sender, e) =>
            {
                if (this.Executing)
                {
                    this.timer.Start();
                }
                else
                {
                    this.timer.Stop();
                }

                this.Refresh();
            };
        }

        /// <summary>
        /// This event gets invoked when value of pin gets changed.
        /// </summary>
        public event EventHandler OnValueChanged;

        /// <summary>
        /// Gets or sets the left value of the pin view model.
        /// </summary>
        /// <value>The left value of the pin view model.</value>
        public int Left { get => this.left; set { this.Set(ref this.left, value); } }

        /// <summary>
        /// Gets or sets the top value of the pin view model.
        /// </summary>
        /// <value>The top value of the pin view model.</value>
        public int Top { get => this.top; set { this.Set(ref this.top, value); } }

        /// <summary>
        /// Gets the current value of the pin.
        /// </summary>
        /// <value>The current value of the pin.</value>
        public IValue CurrentValue { get => this.Pin.Value; }

        /// <summary>
        /// Gets the pin of the view model.
        /// </summary>
        /// <value>The pin of the view model.</value>
        public IPin Pin { get; }

        /// <summary>
        /// Gets the command to be executed if pin gets connected.
        /// </summary>
        /// <value>The command to be executed if pin gets connected.</value>
        public ICommand ConnectCommand { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the pin can be connected.
        /// </summary>
        /// <value>The value indicating whether the pin can be connected.</value>
        public bool CanBeConnected
        {
            get
            {
                return this.canBeConnected;
            }

            set
            {
                this.canBeConnected = value;
                this.FirePropertyChanged(nameof(this.CanBeConnected));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the execution is running.
        /// </summary>
        /// <value>The value indicating whether the execution is running.</value>
        public bool Executing
        {
            get => this.executionService.IsEnabled;
        }

        /// <summary>
        /// Invokes the INotifyPropertyChanged event to update all bindings in the view.
        /// </summary>
        public void Refresh()
        {
            // To update all bindings
            this.FirePropertyChanged(string.Empty);
        }

        /// <summary>
        /// Invokes the <see cref="OnValueChanged"/> event.
        /// </summary>
        public void Update()
        {
            this.OnValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}