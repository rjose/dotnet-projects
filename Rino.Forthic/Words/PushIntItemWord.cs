using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// Base class for Forthic words 
    /// </summary>
    public class PushIntItemWord : Word
    {
        protected IntItem intItem;

        public PushIntItemWord(string text, int value) : base(text)
        {
            this.intItem = new IntItem(value);
        }

        public override void Execute(Interpreter interp)
        {
            interp.StackPush(this.intItem);
        }
    }
}
