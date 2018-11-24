using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Rino.Forthic;
using Rino.ForthicTests;

namespace Rino.ForthicTests.TokenDriven
{
    [TestClass]
    public class ModuleManagementTest
    {

        [TestMethod]
        public void SimulateUseModules()
        {
            Interpreter interp = new Interpreter();

            // Register modules
            interp.RegisterModule("test.A", new ModuleA());
            interp.RegisterModule("test.B", new ModuleB());

            // Test USING-MODULES
            // [ test.A ] USE-MODULES
            interp.HandleToken(new StartArrayToken());
            interp.HandleToken(new WordToken("test.A"));
            interp.HandleToken(new EndArrayToken());
            interp.HandleToken(new WordToken("USE-MODULES"));
        

            // Verify that we can find "A" but not "B"
            Word word_A, word_B;
            Assert.IsTrue(interp.TryFindWord("A", out word_A));
            Assert.IsFalse(interp.TryFindWord("B", out word_B));
        }

        // [TestMethod]
        public void TestModuleStack()
        {
            Interpreter interp = new Interpreter();

            // Register modules
            interp.RegisterModule("test.A", new ModuleA());
            interp.RegisterModule("test.B", new ModuleB());

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
}
