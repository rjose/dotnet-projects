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
        public Vector4 Origin { get; }
        public Vector4 Direction { get; }

        public RayItem(Vector4 origin, Vector4 direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Vector4 Position(float t)
        {
            return Origin + Direction * t;
        }
    }
}
