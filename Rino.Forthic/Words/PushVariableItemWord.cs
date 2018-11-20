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

        public PushVariableItemWord(ref VariableItem value) : base()
        {
            this.variableItem = value;
        }

        public override void Execute(Interpreter interp)
        {
            interp.StackPush(this.variableItem);
        }
    }
}
