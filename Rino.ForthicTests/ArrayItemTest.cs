using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using Rino.Forthic;

namespace Rino.ForthicTests
{
    [TestClass]
    public class ArrayItemTest
    {
        [TestMethod]
        public void TestConstruction()
        {
            ArrayItem val = new ArrayItem();
            Assert.IsNotNull(val);
        }

        [TestMethod]
        public void TestIteration()
        {
            RecordItem rec = new RecordItem();
            rec.SetValue("age", new IntItem(42));

            ArrayItem aa = new ArrayItem();
            aa.AddItem(rec);

            List<StackItem> items = aa.Items();
            Assert.AreEqual(1, items.Count);

            dynamic item = items[0];
            Assert.AreEqual(42, item.GetValue("age").IntValue);
        }

    }
}
