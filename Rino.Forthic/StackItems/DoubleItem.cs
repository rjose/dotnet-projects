using System;
using System.Dynamic;

namespace Rino.Forthic
{
    /// <summary>
    /// Represents a double that's pushed onto the forthic stack.
    /// </summary>
    public class DoubleItem : StackItem
    {
        public DoubleItem(double value)
        {
            this.DoubleValue = value;
        }

        public double DoubleValue { get; }

        public int IntValue
        {
            get { return (int)this.DoubleValue; }
        }

        public float FloatValue
        {
            get { return (float)this.DoubleValue; }
        }
    }
}
