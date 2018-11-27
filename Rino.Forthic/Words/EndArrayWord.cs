using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// </summary>
    public class EndArrayWord : Word
    {
        public EndArrayWord() : base("]")
        {
        }

        public override void Execute(Interpreter interp)
        {
            List<StackItem> result = new List<StackItem>();

            var item = interp.StackPop();
            while (!(item is StartArrayItem))
            {
                result.Add(item);
                item = interp.StackPop();
            }

            result.Reverse();
            interp.StackPush(new ArrayItem(result));
        }
    }
}
