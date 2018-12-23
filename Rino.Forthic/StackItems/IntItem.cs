using System;
using System.Dynamic;

namespace Rino.Forthic
{
    /// <summary>
    /// Represents an int that's pushed onto the forthic stack.
    /// </summary>
    public class IntItem : ScalarItem
    {
        public IntItem(int value)
        {
            this.IntValue = value;
        }

        public override int IntValue { get; }

        public override double DoubleValue
        {
            get { return (double)this.IntValue; }
        }

        public override float FloatValue
        {
            get { return (float)this.IntValue; }
        }

        public bool IsEqual(IntItem rhs)
        {
            return this.IntValue == rhs.IntValue;
        }
    }
}
