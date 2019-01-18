using ElectronicParts.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.Services.Implementations
{
    public class GenericTypeComparerService : IGenericTypeComparerService
    {
        public bool IsSameGenericType(object first, object second)
        {
            return first.GetType().GetGenericArguments().SequenceEqual(second.GetType().GetGenericArguments());
        }
    }
}
