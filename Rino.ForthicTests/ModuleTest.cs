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
            Module m = new Module();
            Assert.IsNotNull(m);
        }

        [TestMethod]
        public void TestAddWord()
        {
            Module m = new Module();
            m.AddWord(new PushIntItemWord("101", 101));
        }

        [TestMethod]
        public void TestAddVariable()
        {
            Module m = new Module();
            m.AddVariableIfMissing("x");
        }
    }
}
