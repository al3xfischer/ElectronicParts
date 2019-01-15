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
    public class ConfigurationService : IConfigurationService
    {
        public Configuration Configuration { get; private set; }

        private IConfiguration configuration;

        public ConfigurationService()
        {
            this.SetupConfiguration();
        }

        private void SetupConfiguration()
        {
            this.configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            this.Configuration = new Configuration(this.configuration);
        }

        public void SaveConfiguration()
        {
            using (FileStream fileStream = new FileStream(Directory.GetCurrentDirectory() + @"\appsettings.json", FileMode.Open))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Configuration));
                ser.WriteObject(fileStream, this.Configuration);
            }
        }
    }
}
