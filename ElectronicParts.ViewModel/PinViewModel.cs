using Shared;
using System;
using System.Windows.Input;

namespace ElectronicParts.ViewModels
{
    public class PinViewModel : BaseViewModel
    {
        private int left;

        private int top;

        public PinViewModel(IPin pin, ICommand connectCommand)
        {
            this.Pin = pin ?? throw new ArgumentNullException(nameof(pin));
            this.ConnectCommand = connectCommand ?? throw new ArgumentNullException(nameof(connectCommand));
        }

        public int Left { get => this.left; set { Set(ref this.left, value); } }

        public int Top { get => this.top; set { Set(ref this.top, value); } }

        public IPin Pin { get; }

        public ICommand ConnectCommand { get; }
    }
}
