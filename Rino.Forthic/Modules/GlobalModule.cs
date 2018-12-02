using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// Modules store words and variables and handle literals. 
    /// </summary>
    public class GlobalModule : Module
    {
        public GlobalModule() : base("Forthic.GLOBAL")
        {
            AddLiteralHandler(TryHandleIntLiteral);
            AddLiteralHandler(TryHandleFloatLiteral);
            AddWord(new UseModulesWord("USE-MODULES"));
            AddWord(new ImportWordsWord("IMPORT-WORDS"));
            AddWord(new AsWord("AS"));
        }

        protected bool TryHandleIntLiteral(string text, out Word result)
        {
            bool found = false;
            try {
                int val = Int32.Parse(text);
                result = new PushIntItemWord(text, val);
                found = true;
            }
            catch {
                result = null;
                found = false;
            }

            return found;
        }

        protected bool TryHandleFloatLiteral(string text, out Word result)
        {
            bool found = false;
            try {
                double val = Double.Parse(text);
                result = new PushDoubleItemWord(text, val);
                found = true;
            }
            catch {
                result = null;
                found = false;
            }

            return found;
        }
    }


    // -------------------------------------------------------------------------
    // Words

    class UseModulesWord : Word
    {
        public UseModulesWord(string name) : base(name)
        {
        }

        // ( modules -- )
        public override void Execute(Interpreter interp)
        {
            ArrayItem modules = (ArrayItem)interp.StackPop();
            foreach(Module m in modules.ArrayValue)
            {
                interp.UseModule(m);
            }
        }
    }

    class ImportWordsWord : Word
    {
        public ImportWordsWord(string name) : base(name) { }

        // ( module names -- )
        public override void Execute(Interpreter interp)
        {
            ArrayItem names = (ArrayItem)interp.StackPop();
            Module module = (Module)interp.StackPop();

            foreach(StackItem item in names.ArrayValue)
            {
                string name, newName;
                if (item is ArrayItem)
                {
                    ArrayItem arrayItem = (ArrayItem)item;
                    name = ((StringItem)arrayItem.ArrayValue[0]).StringValue;
                    newName = ((StringItem)arrayItem.ArrayValue[1]).StringValue;
                }
                else
                {
                    name = ((StringItem)item).StringValue;
                    newName = name;
                }

                Word word;
                if(!module.TryFindWord(name, out word))
                {
                    string msg = String.Format("Can't find word {0} in module '{1}'", name, module.Name);
                    throw new InvalidOperationException(msg);
                }

                word.Text = newName;
                interp.CurModule().AddWord(word);
            }
        }
    }

    class AsWord : Word
    {
        public AsWord(string name) : base(name) { }

        // ( a b -- [ a b ] )
        public override void Execute(Interpreter interp)
        {
            StackItem b = interp.StackPop();
            StackItem a = interp.StackPop();
            ArrayItem result = new ArrayItem();
            result.Add(a);
            result.Add(b);
            interp.StackPush(result);
        }
    }
}
