using Shared;
using System;
using System.Windows.Input;

namespace ElectronicParts.ViewModels
{
    public class PinViewModel
    {
        public PinViewModel(IPin pin, ICommand connectCommand)
        {
            this.Pin = pin ?? throw new ArgumentNullException(nameof(pin));
            this.ConnectCommand = connectCommand ?? throw new ArgumentNullException(nameof(connectCommand));
        }

        public IPin Pin { get; }

        public ICommand ConnectCommand { get; }
    }
}
