using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ElectronicParts.Components;
using Shared;
using System.Drawing;
using System.Threading;

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

            string a = source1.Outputs.ElementAt(0).Value.Current.ToString();
            string b = source2.Outputs.ElementAt(0).Value.Current.ToString();

            source1.Execute();
            string source1Val = source1.Outputs.ElementAt(0).Value.Current.ToString();

            source2.Execute();
            string source2Val = source2.Outputs.ElementAt(0).Value.Current.ToString();

            adder.Inputs.ElementAt(0).Value.Current = source1.Outputs.ElementAt(0).Value.Current;
            adder.Inputs.ElementAt(1).Value.Current = source2.Outputs.ElementAt(0).Value.Current;

            adder.Execute();

            display.Inputs.ElementAt(0).Value.Current = adder.Outputs.ElementAt(0).Value.Current;

            string s1 = display.Label;
            display.Execute();
            string s2 = display.Label;
        }
    }
}
