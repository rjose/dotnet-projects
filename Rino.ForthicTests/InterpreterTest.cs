using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Rino.Forthic;

namespace Rino.ForthicTests
{
    [TestClass]
    public class InterpreterTest
    {
        [TestMethod]
        public void TestConstruction()
        {
            Interpreter interp = new Interpreter();
            Assert.IsNotNull(interp);
        }

        [TestMethod]
        public void TestWordExecution()
        {
            Interpreter interp = new Interpreter();

            List<Word> words = new List<Word>();
            words.Add(new PushIntItemWord(101));

            Assert.AreEqual(0, interp.stack.Count);
            foreach(Word w in words)
            {
                w.Execute(interp);
            }
            Assert.AreEqual(1, interp.stack.Count);
            dynamic item = interp.stack.Peek();
            Assert.AreEqual(101, item.IntValue);
        }
    }
}
