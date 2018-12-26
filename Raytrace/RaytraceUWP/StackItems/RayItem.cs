using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Rino.Forthic;

namespace RaytraceUWP
{
    public class RayItem : StackItem
    {
        public Vector4 Origin { get; set;  }
        public Vector4 Direction { get; set; }

        public RayItem(Vector4 origin, Vector4 direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Vector4 Position(float t)
        {
            return Origin + Direction * t;
        }

        override public void SetValue(string key, StackItem value)
        {
            if (key == "origin") Origin = ((Vector4Item)(value)).Vector4Value;
            else if (key == "direction") Direction = ((Vector4Item)(value)).Vector4Value;
            else throw new InvalidOperationException(String.Format("Unknown key: {0}", key));
        }

        override public StackItem GetValue(string key)
        {
            if (key == "origin") return new Vector4Item(Origin);
            else if (key == "direction") return new Vector4Item(Direction);
            else throw new InvalidOperationException(String.Format("Unknown key: {0}", key));
        }

    }
}
