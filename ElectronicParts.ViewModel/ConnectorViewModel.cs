using ElectronicParts.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.ViewModels
{
    public class ConnectorViewModel : BaseViewModel
    {
        public ConnectorViewModel(Connector connector, PinViewModel input, PinViewModel output)
        {
            this.Connector = connector ?? throw new ArgumentNullException(nameof(connector));
            this.Input = input ?? throw new ArgumentNullException(nameof(input));
            this.Output = output ?? throw new ArgumentNullException(nameof(output));
        }

        public Connector Connector { get; }

        public PinViewModel Input { get; }

        public PinViewModel Output { get; }

        public IValue CurrentValue
        {
            get => this.Connector.CommonValue;
        }

        public void Update()
        {
            this.FirePropertyChanged(nameof(CurrentValue));
        }
    }
}
