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
            interp.RegisterModule(new ModuleA("test.A"));
            interp.RegisterModule(new ModuleB("test.B"));

            // "[ test.A ] USE-MODULES"
            interp.HandleToken(new StartArrayToken());
            interp.HandleToken(new WordToken("test.A"));
            interp.HandleToken(new EndArrayToken());
            interp.HandleToken(new WordToken("USE-MODULES"));

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

            // "{test.A "
            interp.HandleToken(new StartModuleToken("test.A"));

            // Verify that we can find "A" but not "B"
            Word word_A, word_B;
            Assert.IsTrue(interp.TryFindWord("A", out word_A));
            Assert.IsFalse(interp.TryFindWord("B", out word_B));

            // Push module_B and verify that we can find both A and B
            // "{test.B "
            interp.HandleToken(new StartModuleToken("test.B"));

            // Verify that we can find "A" and "B"
            Assert.IsTrue(interp.TryFindWord("A", out word_A));
            Assert.IsTrue(interp.TryFindWord("B", out word_B));

            // Pop module_B and verify that we can find A but not B
            interp.HandleToken(new EndModuleToken());
            Assert.IsTrue(interp.TryFindWord("A", out word_A));
            Assert.IsFalse(interp.TryFindWord("B", out word_B));
        }

        [TestMethod]
        public void TestImportWords()
        {
            Interpreter interp = new Interpreter();

            // Register modules
            interp.RegisterModule(new ModuleA("test.A"));
            interp.RegisterModule(new ModuleB("test.B"));

            // Import A from test.A
            // "test.A [ "A" ] IMPORT-WORDS
            interp.HandleToken(new WordToken("test.A"));
            interp.HandleToken(new StartArrayToken());
            interp.HandleToken(new StringToken("A"));
            interp.HandleToken(new EndArrayToken());
            interp.HandleToken(new WordToken("IMPORT-WORDS"));

            // Import B from test.B
            // "test.B [ [ "B" "B2" ] ] IMPORT-WORDS
            interp.HandleToken(new WordToken("test.B"));
            interp.HandleToken(new StartArrayToken());
            interp.HandleToken(new StartArrayToken());
            interp.HandleToken(new StringToken("B"));
            interp.HandleToken(new StringToken("B2"));
            interp.HandleToken(new EndArrayToken());
            interp.HandleToken(new EndArrayToken());
            interp.HandleToken(new WordToken("IMPORT-WORDS"));

            Word word_A, word_B;
            Assert.IsTrue(interp.TryFindWord("A", out word_A));
            Assert.IsTrue(interp.TryFindWord("B2", out word_B));
        }

        [TestMethod]
        public void TestImportAs()
        {
            Interpreter interp = new Interpreter();

            // Register modules
            interp.RegisterModule(new ModuleB("test.B"));

            // Import B from test.B
            // "test.B [ "B" "B2" AS ] IMPORT-WORDS
            interp.HandleToken(new WordToken("test.B"));
            interp.HandleToken(new StartArrayToken());
            interp.HandleToken(new StringToken("B"));
            interp.HandleToken(new StringToken("B2"));
            interp.HandleToken(new WordToken("AS"));
            interp.HandleToken(new EndArrayToken());
            interp.HandleToken(new WordToken("IMPORT-WORDS"));

            Word word_B;
            Assert.IsTrue(interp.TryFindWord("B2", out word_B));
        }

    }
}
