using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.Models
{
    public class Connector
    {
        public Connector(IPin input, IPin output, IValue commonVal)
        {
            this.OutputPin = output ?? throw new ArgumentNullException(nameof(output));
            this.InputPin = input ?? throw new ArgumentNullException(nameof(input));
            this.CommonValue = commonVal ?? throw new ArgumentNullException(nameof(commonVal));
        }

        public IPin OutputPin { get; private set; }
        public IPin InputPin { get; private set; }
        public IValue CommonValue { get; private set; }
    }
}
