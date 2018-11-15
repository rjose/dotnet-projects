using System;
using System.Dynamic;

namespace Rino.Forthic
{
    public class IntItem : StackItem
    {
        public IntItem(int value)
        {
            this.IntValue = value;
        }

        public int IntValue { get; }
    }
}
