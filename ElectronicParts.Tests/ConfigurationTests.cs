using ElectronicParts.Services;
using ElectronicParts.Services.Implementations;
using ElectronicParts.Services.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ElectronicParts.Tests
{
    [TestFixture]
    class ConfigurationTests
    {
        [Test]
        public void TestConfigSave()
        {
            IConfigurationService configuration1 = new ConfigurationService();

            configuration1.Configuration.BoolColor = "Purple";
            
            configuration1.SaveConfiguration();

            IConfigurationService configuration2 = new ConfigurationService();

            Assert.That(configuration2.Configuration.BoolColor == "Purple");
        }
    }
}
