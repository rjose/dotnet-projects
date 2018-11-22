using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// </summary>
    public class PushDoubleItemWord : Word
    {
        protected DoubleItem doubleItem;

        public PushDoubleItemWord(string text, double value) : base(text)
        {
            this.doubleItem = new DoubleItem(value);
        }

        public override void Execute(Interpreter interp)
        {
            interp.StackPush(this.doubleItem);
        }
    }
}
