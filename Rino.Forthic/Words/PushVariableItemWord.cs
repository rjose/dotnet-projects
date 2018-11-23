using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// Pushes VariableItem onto stack
    /// </summary>
    public class PushVariableItemWord : Word
    {
        protected VariableItem variableItem;

        public PushVariableItemWord(string text, VariableItem value) : base(text)
        {
            this.variableItem = value;
        }

        public override void Execute(Interpreter interp)
        {
            interp.StackPush(this.variableItem);
        }
    }
}
