using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Rino.Forthic;

namespace Raytrace
{
    public class Vector4Item : StackItem
    {
        public Vector4 Vector4Value { get; }

        public Vector4Item(float x, float y, float z, float w)
        {
            this.Vector4Value = new Vector4(x, y, z, w);
        }

        public Vector4Item(Vector4 value)
        {
            this.Vector4Value = value;
        }
    }
}
