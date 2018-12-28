using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rino.Forthic;

namespace RaytraceUWP
{
    public class IntersectionItem : StackItem
    {
        public StackItem Obj { get;  }
        public double T { get; }

        public IntersectionItem(double t, StackItem obj)
        {
            Obj = obj;
            T = t;
        }

        public bool IsEqual(dynamic rhs)
        {
            if (rhs.GetType() == typeof(IntersectionItem))
            {
                return this == rhs;
            }
            else
            {
                return false;
            }
        }

        override public void SetValue(string key, StackItem value)
        {
            throw new InvalidOperationException(String.Format("{0} attributes are read-only", this));
        }

        override public StackItem GetValue(string key)
        {
            if (key == "t") return new DoubleItem(T);
            else if (key == "obj") return Obj;
            else throw new InvalidOperationException(String.Format("Unknown key: {0}", key));
        }


    }
}
