using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using ElectronicParts.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ElectronicParts.Services.Implementations
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
            try
            {
                this.configuration = new ConfigurationBuilder()
                    .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                    .AddJsonFile("appsettings.json")
                    .Build();

                this.Configuration = new Configuration(this.configuration);
            }
            catch
            {
                this.Configuration = new Configuration();
            }
        }

        public void SaveConfiguration()
        {
            using (FileStream fileStream = new FileStream(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\appsettings.json", FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Configuration));
                ser.WriteObject(fileStream, this.Configuration);
            }
        }
    }
}
