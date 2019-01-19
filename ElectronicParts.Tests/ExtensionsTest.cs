using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicParts.ViewModels;

namespace ElectronicParts.Tests
{
    [TestFixture]
    public class ExtensionsTest
    {
        [Test]
        public void TestRoundToFunction()
        {
            var testValue = 11;
            var result = testValue.RoundTo(20);
            var referenz = 20;
            Assert.AreEqual(referenz, result);
        }
    }
}
