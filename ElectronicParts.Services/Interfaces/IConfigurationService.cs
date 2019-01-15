using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ElectronicParts.Services.Interfaces
{
    public interface IConfigurationService
    {
        IConfiguration Configuration { get; }

        void SaveConfiguration();
    }
}
