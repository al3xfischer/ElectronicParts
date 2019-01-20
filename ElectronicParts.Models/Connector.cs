using Shared;
using System;
using System.Linq;

namespace ElectronicParts.Models
{
    [Serializable]
    public class Connector
    {
        public Connector(IPin input, IPin output, IValue commonVal)
        {
            this.OutputPin = output ?? throw new ArgumentNullException(nameof(output));
            this.InputPin = input ?? throw new ArgumentNullException(nameof(input));
            this.CommonValue = commonVal ?? throw new ArgumentNullException(nameof(commonVal));
        }

        public void ResetValue()
        {
            Type valueType = this.CommonValue.GetType();
            Type[] genericTypeArgs = valueType.GetGenericArguments();
            if (genericTypeArgs.Count() == 0 || genericTypeArgs.Count() > 1)
            {
                throw new InvalidOperationException("The node containing the output pin is not implemented correctly. There have been 0 or more than one generic type arguments.");
            }

            Type genericType = genericTypeArgs.First();

            if (genericType == typeof(string))
            {
                this.CommonValue.Current = Activator.CreateInstance(genericType, string.Empty.ToCharArray());
            }
            else
            {
                this.CommonValue.Current = Activator.CreateInstance(genericType);
            }
        }
        public IPin OutputPin { get; private set; }
        public IPin InputPin { get; private set; }
        public IValue CommonValue { get; private set; }
    }
}
