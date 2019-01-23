using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.Services.Interfaces
{
    public interface IPinCreatorService
    {
        IPin CreatePin(Type type);

        IEnumerable<IPin> CreatePins(Type type, int amount);
    }
}
