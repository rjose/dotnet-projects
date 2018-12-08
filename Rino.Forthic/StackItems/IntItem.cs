using System;
using System.Dynamic;

namespace Rino.Forthic
{
    /// <summary>
    /// Represents an int that's pushed onto the forthic stack.
    /// </summary>
    public class IntItem : StackItem
    {
        public IntItem(int value)
        {
            this.IntValue = value;
        }

        public int IntValue { get; }

        public double DoubleValue
        {
            get { return (double)this.IntValue; }
        }

        public float FloatValue
        {
            get { return (float)this.IntValue; }
        }
    }
}
