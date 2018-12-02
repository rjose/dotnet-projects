using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Rino.Forthic;
using System.Diagnostics;
using System;

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

        [TestMethod]
        public void TestRecords()
        {
            Interpreter interp = new Interpreter();
            interp.Run("[ 2 'tacos' ] [ 'count' 'type' ] REC");
            Assert.AreEqual(1, interp.stack.Count);

            RecordItem rec = (RecordItem)interp.StackPop();

            // Get value
            interp.StackPush(rec);
            interp.Run("'type' REC@");
            Assert.AreEqual(1, interp.stack.Count);
            dynamic val = interp.StackPop();
            Assert.AreEqual("tacos", val.StringValue);

            // Set value
            interp.StackPush(rec);
            interp.Run("'burritos' 'type' REC!");
            StringItem typeValue = (StringItem)rec.GetValue("type");
            Assert.AreEqual("burritos", typeValue.StringValue);

            // Set value
            interp.StackPush(rec);
            interp.Run("'tostadas' 'type' <REC!");
            typeValue = (StringItem)rec.GetValue("type");
            Assert.AreEqual("tostadas", typeValue.StringValue);
            Assert.AreEqual(1, interp.stack.Count);
        }

        [TestMethod]
        public void TestForeach()
        {
            Interpreter interp = new Interpreter();
            useTestSupportModule(interp);
            interp.Run("[ 1 2 'hamburger' ] 'PRINT' FOREACH");
        }

        [TestMethod]
        public void TestMap()
        {
            Interpreter interp = new Interpreter();
            useTestSupportModule(interp);
            interp.Run("[ 1 2 3 ] 'DOUBLE' MAP");
            Assert.AreEqual(1, interp.stack.Count);
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

        void defineTestSupportModule(Interpreter interp)
        {
            interp.Run("{Rino.ForthicTests.Support } Rino.ForthicTests.Support");
            Module mod = (Module)interp.StackPop();
            mod.AddWord(new PrintWord("PRINT"));
            mod.AddWord(new DoubleWord("DOUBLE"));
        }

        void useTestSupportModule(Interpreter interp)
        {
            defineTestSupportModule(interp);
            interp.Run("[ Rino.ForthicTests.Support ] USE-MODULES");
        }
    }

    class PrintWord : Word
    {
        public PrintWord(string name) : base(name) { }

        // ( item -- )
        public override void Execute(Interpreter interp)
        {
            StackItem item = interp.StackPop();
            Trace.WriteLine(String.Format("{0}", item));
        }
    }

    class DoubleWord : Word
    {
        public DoubleWord(string name) : base(name) { }

        // ( item -- 2*item )
        public override void Execute(Interpreter interp)
        {
            IntItem item = (IntItem)interp.StackPop();
            IntItem result = new IntItem(2*item.IntValue);
            interp.StackPush(result);
        }
    }
}
