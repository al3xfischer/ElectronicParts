using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace ElectronicParts.Components
{
    [Serializable]
    public class Pin<T> : IPinGeneric<T>
    {
        public Pin()
        {
            this.Value = new Value<T>();
        }
        public IValueGeneric<T> Value { get; set; }

        public string Label { get; set; }

        IValue IPin.Value
        {
            get
            {
                return this.Value;
            }
            set
            {
                try
                {
                    this.Value = (IValueGeneric<T>)value;
                }
                catch (InvalidCastException e)
                {
                    this.Value = new Value<T>();
                    Debug.WriteLine(e.Message);
                }
            }
        }
    }
}
