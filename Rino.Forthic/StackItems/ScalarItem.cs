using System;
using System.Collections.Generic;
using System.Text;

namespace Rino.Forthic
{
    public class ScalarItem : StackItem
    {
        public virtual int IntValue { get { throw new InvalidOperationException("Subclass of ScalarItem must implement IntValue"); } }
        public virtual float FloatValue { get { throw new InvalidOperationException("Subclass of ScalarItem must implement FloatValue"); } }
        public virtual double DoubleValue { get { throw new InvalidOperationException("Subclass of ScalarItem must implement DoubleValue"); } }
    }
}
