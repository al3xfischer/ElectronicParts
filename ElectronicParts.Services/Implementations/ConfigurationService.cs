// ***********************************************************************
// Assembly         : ElectronicParts.Services
// Author           : Peter Helf
// ***********************************************************************
// <copyright file="ConfigurationService.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the ConfigurationService class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.Services.Implementations
{
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization.Json;
    using ElectronicParts.Models;
    using ElectronicParts.Services.Interfaces;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Represents the ConfigurationService class of the ElectronicParts program.
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        /// <summary>
        /// The configurations which are read from a file.
        /// </summary>
        private IConfiguration configuration;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationService"/> class.
        /// </summary>
        public ConfigurationService()
        {
            this.SetupConfiguration();
        }

        /// <summary>
        /// Gets the configurations which include all configurations needed in other classes of the program.
        /// </summary>
        /// <value>The Configuration which contains all needed configurations.</value>
        public Configuration Configuration { get; private set; }

        /// <summary>
        /// Saves the configuration to a file.
        /// </summary>
        public void SaveConfiguration()
        {
            using (FileStream fileStream = new FileStream(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\appsettings.json", FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Configuration));
                ser.WriteObject(fileStream, this.Configuration);
            }
        }

        /// <summary>
        /// Sets up the configuration by first reading it out of a file.
        /// </summary>
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
    }
}
