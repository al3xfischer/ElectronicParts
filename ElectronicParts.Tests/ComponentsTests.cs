using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ElectronicParts.Components;
using Shared;
using System.Drawing;

namespace ElectronicParts.Tests
{
    [TestFixture]
    public class ComponentsTests
    {
        [Test]
        public void AndGateTest()
        {

            IDisplayableNode node = new AndGate();
            node.Inputs.ElementAt(0).Value.Current = true;
            node.Inputs.ElementAt(1).Value.Current = false;

            node.Inputs.Add(new Pin<bool>());
            node.Inputs.ElementAt(2).Value.Current = true;
            
            node.Execute();

            Assert.IsFalse((bool)node.Outputs.ElementAt(0).Value.Current);

            node.Inputs.ElementAt(1).Value.Current = true;

            node.Execute();

            Assert.IsTrue((bool)node.Outputs.ElementAt(0).Value.Current);
        }

        [Test]
        public void OrGateTest()
        {
            IDisplayableNode node = new OrGate();
            node.Inputs.ElementAt(0).Value.Current = true;
            node.Inputs.ElementAt(1).Value.Current = false;

            node.Inputs.Add(new Pin<bool>());
            node.Inputs.ElementAt(2).Value.Current = true;

            node.Execute();

            Assert.IsTrue((bool)node.Outputs.ElementAt(0).Value.Current);

            node.Inputs.ElementAt(0).Value.Current = false;

            node.Execute();

            Assert.IsTrue((bool)node.Outputs.ElementAt(0).Value.Current);

            node.Inputs.ElementAt(2).Value.Current = false;

            node.Execute();

            Assert.IsFalse((bool)node.Outputs.ElementAt(0).Value.Current);
        }

        [Test]
        public void IntegerSourceAdderDisplayTest()
        {
            IDisplayableNode source1 = new IntegerSource();
            IDisplayableNode source2 = new IntegerSource();
            IDisplayableNode adder = new IntegerAdder();
            IDisplayableNode display = new IntegerDisplay();

            source1.Execute();
            source2.Execute();

            
        }
    }
}
