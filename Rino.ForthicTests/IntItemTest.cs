using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rino.Forthic;

namespace Rino.ForthicTests
{
    [TestClass]
    public class IntItemTest
    {
        [TestMethod]
        public void TestConstruction()
        {
            IntItem val = new IntItem(100);
            Assert.IsNotNull(val);
            Assert.AreEqual(100, val.IntValue);
        }

        IntItem AddInts(dynamic l, dynamic r) {
            int val = l.IntValue + r.IntValue;
            IntItem result = new IntItem(val);
            return result;
        }

        [TestMethod]
        public void TestAddTwoIntItems()
        {
            IntItem val1 = new IntItem(1);
            IntItem val2 = new IntItem(2);
            
            IntItem s = AddInts(val1, val2);
            Assert.AreEqual(3, s.IntValue);
        }

        [TestMethod]
        public void TestAddIntItemAndDoubleItem()
        {
            IntItem val1 = new IntItem(1);
            DoubleItem val2 = new DoubleItem(3.4);
            
            IntItem s = AddInts(val1, val2);
            Assert.AreEqual(4, s.IntValue);
        }
    }
}
