using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using ElectronicParts.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ElectronicParts.Services
{
    [DataContractAttribute]
    public class ConfigurationManager : IConfigurationService
    {
        [DataMemberAttribute]
        public IConfiguration Configuration { get; private set; }

        public ConfigurationManager()
        {
            this.SetupConfiguration();
        }

        private void SetupConfiguration()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public void SaveConfiguration()
        {
            using (FileStream fileStream = new FileStream(Directory.GetCurrentDirectory() + @"\appsettings.json", FileMode.Open))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(IConfiguration));
                ser.WriteObject(fileStream, this.Configuration);
            }
        }
    }
}
