using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Rino.Forthic;

namespace Rino.ForthicModuleTests
{
    [TestClass]
    public class GlobalModuleTest
    {
        [TestMethod]
        public void TestArray()
        {
            Interpreter interp = new Interpreter();
            interp.Run("[ 1 2 3 ]");
            Assert.AreEqual(1, interp.stack.Count);
        }

        [TestMethod]
        public void TestDefinition()
        {
            Interpreter interp = new Interpreter();
            interp.Run(@"
            : TACO    'Here is a taco!' ;
            : 2TACO   TACO TACO ;
            2TACO
            ");
            Assert.AreEqual(2, interp.stack.Count);
        }

        [TestMethod]
        public void TestUSE_MODULES()
        {
            Interpreter interp = new Interpreter();
            defineModuleA(interp);
            interp.Run(@"
            [ module-A ] USE-MODULES
            2TACO            
            ");
            Assert.AreEqual(2, interp.stack.Count);
        }

        [TestMethod]
        public void TestIMPORT_WORDS()
        {
            Interpreter interp = new Interpreter();
            defineModuleA(interp);
            defineModuleB(interp);
            interp.Run(@"
            module-B [ '2TACO' 'TACO' ] IMPORT-WORDS
            2TACO            
            TACO            
            ");
            Assert.AreEqual(3, interp.stack.Count);

            interp.Run(@"
            module-A [ '2TACO' '2TACO-A' AS ] IMPORT-WORDS
            2TACO-A
            ");
            Assert.AreEqual(5, interp.stack.Count);
        }

        // ---------------------------------------------------------------------
        // Support

        void defineModuleA(Interpreter interp)
        {
            interp.Run(@"
            {module-A
                : TACO    'Here is A taco!' ;
                : 2TACO   TACO TACO ;
            }
            ");
        }

        void defineModuleB(Interpreter interp)
        {
            interp.Run(@"
            {module-B
                : TACO    'Here is B taco!' ;
                : 2TACO   TACO TACO ;
            }
            ");
        }
    }
}
