using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// </summary>
    public class PushModuleWord : Word
    {
        protected Module module;

        public PushModuleWord(string text, Module module) : base(text)
        {
            this.module = module;
        }

        public override void Execute(Interpreter interp)
        {
            interp.StackPush(this.module);
        }
    }
}
