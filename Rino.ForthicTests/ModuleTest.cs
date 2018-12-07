using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rino.Forthic;

namespace Rino.ForthicTests
{
    [TestClass]
    public class ModuleTest
    {
        [TestMethod]
        public void TestConstruction()
        {
            Module m = new Module("");
            Assert.IsNotNull(m);
        }

        [TestMethod]
        public void TestAddWord()
        {
            Module m = new Module("");
            m.AddWord(new PushStackItemWord("101", new IntItem(101)));
            Word w;
            bool found = m.TryFindWord("101", out w);
            Assert.AreEqual(true, found);

            found = m.TryFindWord("garbage", out w);
            Assert.AreEqual(false, found);
        }

        [TestMethod]
        public void TestAddVariable()
        {
            Module m = new Module("");
            m.AddVariableIfMissing("x");

            Word w;
            bool found = m.TryFindWord("x", out w);
            Assert.AreEqual(true, found);
        }
    }
}
