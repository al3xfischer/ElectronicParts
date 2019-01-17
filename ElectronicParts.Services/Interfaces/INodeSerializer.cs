using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.Services.Interfaces
{
    public interface INodeSerializer
    {
        void Serialize();

        void Deserialize();
    }
}
