using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rino.Forthic;

namespace Rino.ForthicTests
{
    [TestClass]
    public class DoubleLiteralTest
    {
        [TestMethod]
        public void TestConstruction()
        {
            DoubleItem val = new DoubleItem(100.5);
            Assert.IsNotNull(val);
            Assert.AreEqual(100.5, val.DoubleValue, 0.01);
        }

        DoubleItem AddDoubles(dynamic l, dynamic r) {
            double val = l.DoubleValue + r.DoubleValue;
            DoubleItem result = new DoubleItem(val);
            return result;
        }

        [TestMethod]
        public void TestAddTwoDoubleItems()
        {
            DoubleItem val1 = new DoubleItem(1);
            DoubleItem val2 = new DoubleItem(2);
            
            DoubleItem s = AddDoubles(val1, val2);
            Assert.AreEqual(3, s.DoubleValue, 0.1);
        }

        [TestMethod]
        public void TestAddIntItemAndDoubleItem()
        {
            IntItem val1 = new IntItem(1);
            DoubleItem val2 = new DoubleItem(3.4);
            
            DoubleItem s = AddDoubles(val1, val2);
            Assert.AreEqual(4.4, s.DoubleValue, 0.01);
        }
    }
}
