using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rino.Forthic;

namespace Rino.ForthicTests
{
    [TestClass]
    public class StringItemTest
    {
        [TestMethod]
        public void TestConstruction()
        {
            StringItem val = new StringItem("Howdy");
            Assert.IsNotNull(val);
            Assert.AreEqual("Howdy", val.StringValue);
        }
    }
}
