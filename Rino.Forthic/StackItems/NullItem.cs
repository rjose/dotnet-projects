using System;
using System.Collections.Generic;
using System.Text;

namespace Rino.Forthic
{
    public class NullItem : StackItem
    {
        public bool IsEqual(StackItem rhs)
        {
            if (rhs.GetType() == typeof(NullItem)) return true;
            else return false;
        }

        override public void SetValue(string key, StackItem value)
        {
            throw new InvalidOperationException(String.Format("{0} Can't set value for key: {1}", this, key));
        }

        override public StackItem GetValue(string key)
        {
            return this;
        }

    }
}
