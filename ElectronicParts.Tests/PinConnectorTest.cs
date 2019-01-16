using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicParts.Services.Implementations;
using ElectronicParts.Models;

namespace ElectronicParts.Tests
{ 
    [TestFixture]
    public class PinConnectorTest
    {
        [Test]
        public void TestPinConnectorNullValuePin()
        {
            var testInputPin = new Pin<bool>();
            var testOutputPin = new Pin<bool>();
            testInputPin.Value = null;
            var pinConnector = new PinConnectorService();
            pinConnector.TryConnectPins(testInputPin, testOutputPin, out Connector test);
        }
    }
}
