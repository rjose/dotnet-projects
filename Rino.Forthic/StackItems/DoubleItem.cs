using System;
using System.Dynamic;

namespace Rino.Forthic
{
    /// <summary>
    /// Represents a double that's pushed onto the forthic stack.
    /// </summary>
    public class DoubleItem : ScalarItem
    {
        const double tolerance = 1E-6;
        public DoubleItem(double value)
        {
            this.DoubleValue = value;
        }

        public override double DoubleValue { get; }

        public override int IntValue
        {
            get { return (int)this.DoubleValue; }
        }

        public override float FloatValue
        {
            get { return (float)this.DoubleValue; }
        }

        public bool IsEqual(DoubleItem rhs)
        {
            return ApproxEqual(rhs, tolerance);
        }

        public bool ApproxEqual(DoubleItem rhs, double tol)
        {
            double delta = this.DoubleValue - rhs.DoubleValue;
            return Math.Abs(delta) < tol;
        }

        public bool IsEqual(IntItem rhs)
        {
            double delta = this.DoubleValue - rhs.DoubleValue;
            return Math.Abs(delta) < tolerance;
        }

    }
}
