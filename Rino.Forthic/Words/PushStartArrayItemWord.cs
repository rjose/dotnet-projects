using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// </summary>
    public class PushStartArrayItemWord : Word
    {
        public PushStartArrayItemWord() : base("[")
        {
        }

        public override void Execute(Interpreter interp)
        {
            interp.StackPush(new StartArrayItem());
        }
    }
}
