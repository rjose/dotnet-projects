using System;
using System.Dynamic;

namespace Rino.Forthic
{
    public class StackItem : DynamicObject
    {
        virtual public void SetValue(string key, StackItem value)
        {
            throw new InvalidOperationException(String.Format("{0} must override SetValue", this));
        }

        virtual public StackItem GetValue(string key)
        {
            throw new InvalidOperationException(String.Format("{0} must override GetValue", this));
        }

        virtual public int CompareTo(StackItem r_val)
        {
            throw new NotImplementedException();
        }
    }
}
