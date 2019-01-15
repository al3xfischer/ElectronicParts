using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace ElectronicParts.Components
{
    public class MyValue<T> : IValueGeneric<T>
    {
        public T Current { get; set; }

        object IValue.Current
        {
            get
            {
                return this.Current;
            }
            set
            {
                this.Current = (T)value;
            }
        }
    }
}
