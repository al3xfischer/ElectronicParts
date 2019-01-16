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
            IPin pin1 = new Pin<bool>();
            pin1.Value.Current = true;

            IPin pin2 = new Pin<bool>();
            pin2.Value.Current = false;

            IPin pin3 = new Pin<bool>();
            pin3.Value.Current = true;

            IDisplayableNode node = new AndGate();
            node.Inputs.Add(pin1);
            node.Inputs.Add(pin2);
            node.Inputs.Add(pin3);

            node.Execute();

            Assert.IsFalse((bool)node.Outputs.ElementAt(0).Value.Current);

            pin2.Value.Current = true;

            node.Execute();

            Assert.IsTrue((bool)node.Outputs.ElementAt(0).Value.Current);
        }

        [Test]
        public void OrGateTest()
        {
            IPin pin1 = new Pin<bool>();
            pin1.Value.Current = true;

            IPin pin2 = new Pin<bool>();
            pin2.Value.Current = false;

            IPin pin3 = new Pin<bool>();
            pin3.Value.Current = true;

            IDisplayableNode node = new OrGate();
            node.Inputs.Add(pin1);
            node.Inputs.Add(pin2);
            node.Inputs.Add(pin3);

            node.Execute();

            Assert.IsTrue((bool)node.Outputs.ElementAt(0).Value.Current);

            pin1.Value.Current = false;

            node.Execute();

            Assert.IsTrue((bool)node.Outputs.ElementAt(0).Value.Current);

            pin3.Value.Current = false;

            node.Execute();

            Assert.IsFalse((bool)node.Outputs.ElementAt(0).Value.Current);

            Bitmap bm = node.Picture;
        }
    }
}
