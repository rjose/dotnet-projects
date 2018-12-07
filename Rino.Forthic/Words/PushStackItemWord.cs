using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// </summary>
    public class PushStackItemWord : Word
    {
        protected StackItem item;

        public PushStackItemWord(string text, StackItem value) : base(text)
        {
            this.item = value;
        }

        public override void Execute(Interpreter interp)
        {
            interp.StackPush(this.item);
        }
    }
}
