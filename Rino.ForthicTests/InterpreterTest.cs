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
            words.Add(new PushStackItemWord("101", new IntItem(101)));

            Assert.AreEqual(0, interp.stack.Count);
            foreach(Word w in words)
            {
                w.Execute(interp);
            }
            Assert.AreEqual(1, interp.stack.Count);
            dynamic item = interp.stack.Peek();
            Assert.AreEqual(101, item.IntValue);
        }

        [TestMethod]
        public void TestVariableItem()
        {
            Interpreter interp = new Interpreter();

            VariableItem varItem = new VariableItem();
            varItem.VariableValue = new IntItem(100);

            // Simulate creation of finding a variable
            var word = new PushStackItemWord("x", varItem);
            word.Execute(interp);

            Assert.AreEqual(1, interp.stack.Count);
            dynamic item = interp.stack.Peek();
            dynamic val = item.VariableValue;
            Assert.AreEqual(100, val.IntValue);

            // Change variable value and check that the original changed, too
            item.VariableValue = new IntItem(21);
            dynamic val2 = varItem.VariableValue;
            Assert.AreEqual(21, val2.IntValue);
        }
    }
}
