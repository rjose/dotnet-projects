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
    }
}
