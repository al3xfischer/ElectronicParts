using ElectronicParts.Models;
using Shared;
using System;
using System.Windows.Input;

namespace ElectronicParts.ViewModels
{
    public class ConnectorViewModel : BaseViewModel
    {
        public ConnectorViewModel(Connector connector, PinViewModel input, PinViewModel output, ICommand deletionCommand)
        {
            this.Connector = connector ?? throw new ArgumentNullException(nameof(connector));
            this.Input = input ?? throw new ArgumentNullException(nameof(input));
            this.Output = output ?? throw new ArgumentNullException(nameof(output));
            this.DeleteCommand = deletionCommand ?? throw new ArgumentNullException(nameof(deletionCommand));
            this.Input.OnValueChanged += this.RefreshPins;
            this.Output.OnValueChanged += this.RefreshPins;
        }

        public Connector Connector { get; set; }

        public PinViewModel Input { get; }

        public PinViewModel Output { get; }

        public ICommand DeleteCommand { get; }

        public IValue CurrentValue
        {
            get => this.Connector.CommonValue;
        }

        public void Update()
        {
            this.FirePropertyChanged(nameof(CurrentValue));
        }

        private void RefreshPins(object sender, EventArgs e)
        {
            this.Output?.Refresh();
            this.Input?.Refresh();
        }
    }
}
