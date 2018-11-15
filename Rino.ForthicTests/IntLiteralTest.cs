using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rino.Forthic;

namespace Rino.ForthicTests
{
    [TestClass]
    public class IntLiteralTest
    {
        [TestMethod]
        public void TestConstruction()
        {
            IntLiteral val = new IntLiteral(100);
            Assert.IsNotNull(val);
            Assert.AreEqual(100, val.IntValue);
        }

        IntLiteral AddInts(dynamic l, dynamic r) {
            int val = l.IntValue + r.IntValue;
            IntLiteral result = new IntLiteral(val);
            return result;
        }

        [TestMethod]
        public void TestAddTwoIntItems()
        {
            IntLiteral val1 = new IntLiteral(1);
            IntLiteral val2 = new IntLiteral(2);
            
            IntLiteral s = AddInts(val1, val2);
            Assert.AreEqual(3, s.IntValue);
        }

        [TestMethod]
        public void TestAddIntItemAndDoubleItem()
        {
            IntLiteral val1 = new IntLiteral(1);
            DoubleItem val2 = new DoubleItem(3.4);
            
            IntLiteral s = AddInts(val1, val2);
            Assert.AreEqual(4, s.IntValue);
        }
    }
}
