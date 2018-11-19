using System;
using System.Dynamic;
using System.Collections.Generic;

namespace Rino.Forthic
{
    /// <summary>
    /// 
    /// </summary>
    public class Interpreter
    {
        public Stack<StackItem> stack { get; }

        public Interpreter()
        {
            this.stack = new Stack<StackItem>();
        }

        public void StackPush(StackItem item)
        {
            this.stack.Push(item);
        }

        public StackItem StackPop()
        {
            return this.stack.Pop();
        }
    }
}
