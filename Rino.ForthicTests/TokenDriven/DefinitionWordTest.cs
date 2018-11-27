using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Rino.Forthic;
using Rino.ForthicTests;

namespace Rino.ForthicTests.TokenDriven
{
    [TestClass]
    public class DefinitionWordTest
    {

        [TestMethod]
        public void CreateSimpleDefinition()
        {
            // Use test.A and test.B modules
            Interpreter interp = new Interpreter();
            interp.UseModule(new ModuleA("test.A"));
            interp.UseModule(new ModuleB("test.B"));

            // "[ test.A ] USE-MODULES"
            interp.HandleToken(new StartDefinitionToken("TACO"));
            interp.HandleToken(new WordToken("A"));
            interp.HandleToken(new WordToken("B"));
            interp.HandleToken(new EndDefinitionToken());

            // Verify that we can find "TACO"
            Word tacoWord;
            Assert.IsTrue(interp.TryFindWord("TACO", out tacoWord));

            // Execute TACO
            interp.HandleToken(new WordToken("TACO"));
            // TODO: Test that stack has 1 and 2 on it
        }
    }
}
