using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rino.Forthic;

namespace Rino.ForthicTests
{
    [TestClass]
    public class RecordItemTest
    {
        [TestMethod]
        public void TestConstruction()
        {
            RecordItem val = new RecordItem();
            Assert.IsNotNull(val);
        }

        [TestMethod]
        public void TestSetGetValue()
        {
            RecordItem rec = new RecordItem();
            rec.SetValue("count", new IntItem(44));
            dynamic val = rec.GetValue("count");
            Assert.AreEqual(44, val.IntValue);
        }

    }
}
