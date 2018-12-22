using System;
using System.Dynamic;

namespace Rino.Forthic
{
    public class BoolItem : StackItem
    {
        public BoolItem(bool value)
        {
            this.BoolValue = value;
        }

        public bool BoolValue { get; }

        public bool IsEqual(BoolItem rhs)
        {
            return this.BoolValue == rhs.BoolValue;
        }

    }
}
