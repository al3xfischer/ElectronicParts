using ElectronicParts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.Services.Interfaces
{
    public interface INodeSerializer
    {
        void Serialize(SnapShot snapShot, string path);

        SnapShot Deserialize(string path);
    }
}
