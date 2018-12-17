using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Rino.Forthic;
using System.Dynamic;
using System.Linq.Expressions;

namespace RaytraceUWP
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

        public bool IsEqual(dynamic rhs)
        {
            Vector4Item r = (Vector4Item)rhs;
            return this.Vector4Value == r.Vector4Value;
        }
        
        public bool ApproxEqual(dynamic rhs, double tolerance)
        {
            Vector4Item r = (Vector4Item)rhs;
            Vector4 l_val = this.Vector4Value;
            Vector4 r_val = r.Vector4Value;
            bool result = Math.Abs(l_val.X - r_val.X) < tolerance &&
                          Math.Abs(l_val.Y - r_val.Y) < tolerance &&
                          Math.Abs(l_val.Z - r_val.Z) < tolerance &&
                          Math.Abs(l_val.W - r_val.W) < tolerance;
            return result;
        }
    }
}
