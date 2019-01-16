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
        private Connector myConnector;

        public ConnectorViewModel(Connector connector)
        {
            this.myConnector = connector;
        }

        public IValue MyCurrentValue
        {
            get => this.myConnector.CommonValue;
        }
    }
}
