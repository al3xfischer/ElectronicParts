using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicParts.Services.Assemblies;
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
        }
    }
}
