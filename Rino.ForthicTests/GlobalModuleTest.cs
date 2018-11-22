using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rino.Forthic;

namespace Rino.ForthicTests
{
    [TestClass]
    public class GlobalModuleTest
    {
        [TestMethod]
        public void TestConstruction()
        {
            GlobalModule m = new GlobalModule();
            Assert.IsNotNull(m);
        }

        [TestMethod]
        public void TestHandleIntLiteral()
        {
            GlobalModule m = new GlobalModule();

            Word w;
            bool found = m.TryFindWord("20", out w);
            Assert.AreEqual(true, found);
        }
    }
}
