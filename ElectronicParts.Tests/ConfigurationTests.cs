using ElectronicParts.Services;
using ElectronicParts.Services.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.Tests
{
    [TestFixture]
    class ConfigurationTests
    {
        [Test]
        public void TestConfigChange()
        {
            IConfigurationService configurationManager = new ConfigurationManager();

            configurationManager.Configuration.GetSection("MySection1")["Key1"] = "123";

            Assert.That(configurationManager.Configuration.GetSection("MySection1")["Key1"] == "123");
        }

        [Test]
        public void TestConfigSave()
        {
            IConfigurationService configurationManager = new ConfigurationManager();

            configurationManager.Configuration.GetSection("MySection1")["Key1"] = "123";

            configurationManager.SaveConfiguration();


            IConfigurationService configurationManager2 = new ConfigurationManager();

            Assert.That(configurationManager2.Configuration.GetSection("MySection1")["Key1"] == "123");
        }
    }
}
