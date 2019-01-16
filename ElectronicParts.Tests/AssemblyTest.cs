using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicParts.Services.Implementations;
using NUnit.Framework;



namespace ElectronicParts.Tests
{
    [TestFixture]
    public class AssemblyTest
    {
        [Test]
        public void TestAssemblyCtor()
        {
            var testService = new AssemblyService();
            var x = testService.LoadAssemblies();
            x.Wait();
            var test = testService.AvailableNodes;
            var y = 0;
        }
    }
}
