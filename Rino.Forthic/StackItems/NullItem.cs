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
    }
}
