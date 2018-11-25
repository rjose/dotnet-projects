using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Rino.Forthic;

namespace Rino.ForthicTests
{
    [TestClass]
    public class ModuleManagementTest
    {

        [TestMethod]
        public void SimulateUseModules()
        {
            Interpreter interp = new Interpreter();

            // Register modules
            interp.RegisterModule(new ModuleA("test.A"));
            interp.RegisterModule(new ModuleB("test.B"));

            // Simulate using modules
            Module module_A;
            Assert.IsTrue(interp.TryFindModule("test.A", out module_A));
            interp.UseModule(module_A);

            // Verify that we can find "A" but not "B"
            Word word_A, word_B;
            Assert.IsTrue(interp.TryFindWord("A", out word_A));
            Assert.IsFalse(interp.TryFindWord("B", out word_B));
        }

        [TestMethod]
        public void TestModuleStack()
        {
            Interpreter interp = new Interpreter();

            // Register modules
            interp.RegisterModule(new ModuleA("test.A"));
            interp.RegisterModule(new ModuleB("test.B"));

            // Push module_A
            Module module_A;
            Assert.IsTrue(interp.TryFindModule("test.A", out module_A));
            interp.ModuleStackPush(module_A);
            
            // Verify that we can find "A" but not "B"
            Word word_A, word_B;
            Assert.IsTrue(interp.TryFindWord("A", out word_A));
            Assert.IsFalse(interp.TryFindWord("B", out word_B));

            // Push module_B and verify that we can find both A and B
            Module module_B;
            Assert.IsTrue(interp.TryFindModule("test.B", out module_B));
            interp.ModuleStackPush(module_B);
            Assert.IsTrue(interp.TryFindWord("A", out word_A));
            Assert.IsTrue(interp.TryFindWord("B", out word_B));

            // Pop module_B and verify that we can find A but not B
            interp.ModuleStackPop();
            Assert.IsTrue(interp.TryFindWord("A", out word_A));
            Assert.IsFalse(interp.TryFindWord("B", out word_B));
        }

    }


    // Support classes
    class ModuleA : Module
    {
        public ModuleA(string name) : base(name)
        {
            AddLiteralHandler(TryHandleALiteral);
        }

        protected bool TryHandleALiteral(string text, out Word result)
        {
            bool found = false;
            result = null;

            if (text == "A")
            {
                result = new PushIntItemWord(text, 1);
                found = true;
            }

            return found;
        }
    }

    class ModuleB : Module
    {
        public ModuleB(string name) : base(name)
        {
            AddLiteralHandler(TryHandleBLiteral);
        }

        protected bool TryHandleBLiteral(string text, out Word result)
        {
            bool found = false;
            result = null;

            if (text == "B")
            {
                result = new PushIntItemWord(text, 2);
                found = true;
            }

            return found;
        }
    }
}
