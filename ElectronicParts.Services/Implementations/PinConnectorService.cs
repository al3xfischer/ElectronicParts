using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.Services.Implementations
{
    public class PinConnectorService
    {
        public IValue ConnectPins(IPin firstPin, IPin secondPin)
        {
            var firstType = firstPin.GetType().GetGenericTypeDefinition();

        }
    }
}
