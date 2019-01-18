using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicParts.Models;
using Microsoft.Extensions.Configuration;

namespace ElectronicParts.Services.Interfaces
{
    public interface IConfigurationService
    {
        Configuration Configuration { get; }

        void SaveConfiguration();
    }
}
