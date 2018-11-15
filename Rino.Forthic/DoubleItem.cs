using System;
using System.Dynamic;

namespace Rino.Forthic
{
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
    }
}
