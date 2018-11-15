using System;
using System.Dynamic;

namespace Rino.Forthic
{
    public class IntLiteral : StackItem
    {
        public IntLiteral(int value)
        {
            this.IntValue = value;
        }

        public int IntValue { get; }
    }
}
