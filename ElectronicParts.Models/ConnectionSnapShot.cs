using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.Models
{
    [Serializable]
    public class ConnectionSnapShot
    {
        public Connector Connector { get; set; }
        public PinSnapShot InputPin { get; set; }
        public PinSnapShot OutputPin { get; set; }
        public IValue Value { get; set; }

        public ConnectionSnapShot(Connector connector, PinSnapShot inputPin, PinSnapShot outputPin, IValue value)
        {
            this.Connector = connector;
            this.InputPin = inputPin;
            this.OutputPin = outputPin;
            this.Value = value;
        }
    }
}
