using ElectronicParts.Services.Interfaces;
using Shared;
using System;
using System.Windows.Input;

namespace ElectronicParts.ViewModels
{
    public class PinViewModel : BaseViewModel
    {
        private readonly IExecutionService executionService;

        private int left;

        private int top;

        public PinViewModel(IPin pin, ICommand connectCommand, IExecutionService executionService)
        {
            this.Pin = pin ?? throw new ArgumentNullException(nameof(pin));
            this.ConnectCommand = connectCommand ?? throw new ArgumentNullException(nameof(connectCommand));
            this.executionService = executionService ?? throw new ArgumentNullException(nameof(executionService));
        }

        public int Left { get => this.left; set { Set(ref this.left, value); } }

        public int Top { get => this.top; set { Set(ref this.top, value); } }

        public IValue CurrentValue { get => this.Pin.Value; }

        public IPin Pin { get; }

        public ICommand ConnectCommand { get; }

        public bool Executing
        {
            get => this.executionService.IsEnabled;
        }
    }
}
