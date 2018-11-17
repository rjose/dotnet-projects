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

        /// <summary>
        /// Gets value
        /// </summary>
        public int IntValue { get; }

        /// <summary>
        /// Gets value as double
        /// </summary>
        public double DoubleValue
        {
            get { return (double)this.IntValue; }
        }
    }
}
