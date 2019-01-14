using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IPinGeneric<T> : IPin
    {
        new IValueGeneric<T> Value { get; set; }
    }
}
