namespace ElectronicParts.Tests
{
    using ElectronicParts.Services;
    using ElectronicParts.Services.Interfaces;
    using NUnit.Framework;
    using System.Windows.Media;

    [TestFixture]
    class ConfigurationTests
    {
        [Test]
        public void TestConfigSave()
        {
            IConfigurationService configuration1 = new ConfigurationService();

            configuration1.Configuration.BoolColor = (Color)ColorConverter.ConvertFromString("Purple");
            
            configuration1.SaveConfiguration();

            IConfigurationService configuration2 = new ConfigurationService();

            Assert.That(configuration2.Configuration.BoolColor == (Color)ColorConverter.ConvertFromString("purple"));
        }
    }
}
