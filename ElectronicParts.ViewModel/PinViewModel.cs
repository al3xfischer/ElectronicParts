using ElectronicParts.Services.Interfaces;
using Shared;
using System;
using System.Timers;
using System.Windows.Input;

namespace ElectronicParts.ViewModels
{
    public class PinViewModel : BaseViewModel
    {
        private readonly IExecutionService executionService;

        private int left;

        private int top;

        private Timer timer;


        public PinViewModel(IPin pin, ICommand connectCommand, IExecutionService executionService)
        {
            this.timer = new Timer();
            this.timer.Interval = 10;
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

        public event EventHandler OnValueChanged;

        public int Left { get => this.left; set { Set(ref this.left, value); } }

        public int Top { get => this.top; set { Set(ref this.top, value); } }

        public IValue CurrentValue { get => this.Pin.Value; }

        public IPin Pin { get; }

        public ICommand ConnectCommand { get; }

        public bool CanBeConnected { get; set; }

        public bool Executing
        {
            get => this.executionService.IsEnabled;
        }

        public void Refresh()
        {
            // To update all bindings
            this.FirePropertyChanged(string.Empty);
        }

        public void Update()
        {
            this.OnValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
