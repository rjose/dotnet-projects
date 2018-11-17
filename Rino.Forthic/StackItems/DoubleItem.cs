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

        /// <summary>
        /// Gets value
        /// </summary>
        public double DoubleValue { get; }

        /// <summary>
        /// Gets value as an int
        /// </summary>
        public int IntValue
        {
            get { return (int)this.DoubleValue; }
        }
    }
}
