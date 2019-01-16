using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ElectronicParts.Components;
using Shared;

namespace ElectronicParts.Tests
{
    [TestFixture]
    public class ComponentsTests
    {
        [Test]
        public void CompTest()
        {
            IPin pin = new Pin<bool>();
            pin.Value.Current = true;

            INode node = new LogicAnd();
            node.Inputs.Add(pin);

        }

    }
}
