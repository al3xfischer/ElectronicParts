using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace ElectronicParts.Models
{
    public class Value<T> : IValueGeneric<T>
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