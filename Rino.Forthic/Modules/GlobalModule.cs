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
        public GlobalModule() : base()
        {
            AddLiteralHandler(TryHandleIntLiteral);
            AddLiteralHandler(TryHandleFloatLiteral);
            AddWord(new UseModulesWord("USE-MODULES"));
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
}
