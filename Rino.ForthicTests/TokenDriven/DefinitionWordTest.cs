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

        [TestMethod]
        public void CreateDefWithString()
        {
            Interpreter interp = new Interpreter();
            interp.HandleToken(new StartDefinitionToken("TACO"));
            interp.HandleToken(new StringToken("Now is the time"));
            interp.HandleToken(new EndDefinitionToken());

            // Verify that we can find "TACO"
            Word tacoWord;
            Assert.IsTrue(interp.TryFindWord("TACO", out tacoWord));

            // Execute TACO
            interp.HandleToken(new WordToken("TACO"));
        }

        [TestMethod]
        public void CreateDefWithArray()
        {
            Interpreter interp = new Interpreter();
            interp.HandleToken(new StartDefinitionToken("TACO"));
            interp.HandleToken(new StartArrayToken());
            interp.HandleToken(new StringToken("Now is the time"));
            interp.HandleToken(new WordToken("1"));
            interp.HandleToken(new WordToken("12.3"));
            interp.HandleToken(new EndArrayToken());
            interp.HandleToken(new EndDefinitionToken());

            // Verify that we can find "TACO"
            Word tacoWord;
            Assert.IsTrue(interp.TryFindWord("TACO", out tacoWord));

            // Execute TACO
            interp.HandleToken(new WordToken("TACO"));
        }

        [TestMethod]
        public void CreateDefUsingModuleStack()
        {
            Interpreter interp = new Interpreter();
            interp.RegisterModule(new ModuleA("test.A"));

            interp.HandleToken(new StartDefinitionToken("TACO"));
            interp.HandleToken(new StartModuleToken("test.A"));
            interp.HandleToken(new WordToken("A"));
            interp.HandleToken(new EndModuleToken());
            interp.HandleToken(new EndDefinitionToken());

            // Verify that we can find "TACO"
            Word tacoWord;
            Assert.IsTrue(interp.TryFindWord("TACO", out tacoWord));

            // Execute TACO
            interp.HandleToken(new WordToken("TACO"));
        }

        [TestMethod]
        public void CreateDefWithMissingWord()
        {
            Interpreter interp = new Interpreter();
            bool exceptionThrown = false;
            try
            {
                interp.HandleToken(new StartDefinitionToken("TACO"));
                interp.HandleToken(new WordToken("GARBAGE"));
                interp.HandleToken(new EndDefinitionToken());
            }
            catch(ArgumentException e)
            {
                exceptionThrown = true;
            }
            Assert.IsTrue(exceptionThrown);
        }
    }
}
