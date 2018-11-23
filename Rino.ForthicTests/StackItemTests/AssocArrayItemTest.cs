using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using Rino.Forthic;

namespace Rino.ForthicTests
{
    [TestClass]
    public class AssocArrayItemTest
    {
        [TestMethod]
        public void TestConstruction()
        {
            AssocArrayItem val = new AssocArrayItem();
            Assert.IsNotNull(val);
        }

        [TestMethod]
        public void TestIteration()
        {
            RecordItem rec = new RecordItem();
            rec.SetValue("age", new IntItem(42));

            AssocArrayItem aa = new AssocArrayItem();
            aa.SetValue("Alpha", rec);

            List<RecordItem> items = aa.Items();
            Assert.AreEqual(1, items.Count);

            dynamic k = items[0].GetValue("key");
            dynamic v = items[0].GetValue("value");
            Assert.AreEqual("Alpha", k.StringValue);
            Assert.AreEqual(42, v.GetValue("age").IntValue);
        }

    }
}
