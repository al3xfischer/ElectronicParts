using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicParts.Tests
{
    [TestFixture]
    public class BoolArrayToByteConverterTest
    {
        [Test]
        public void TestBoolArrayToByteMethod()
        {
            bool[] arr = { true, true, true, true, true, true, true, false };
            byte val = 0;
            foreach (bool b in arr)
            {
                val <<= 1;
                if (b) val |= 1;
            }

            var x = val;
        }
    }
}
