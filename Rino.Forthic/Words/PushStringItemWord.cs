using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// </summary>
    public class PushStringItemWord : Word
    {
        protected StringItem stringItem;

        public PushStringItemWord(string text) : base("STRING")
        {
            stringItem = new StringItem(text);
        }

        public override void Execute(Interpreter interp)
        {
            interp.StackPush(stringItem);
        }
    }
}
