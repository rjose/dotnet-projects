using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Rino.Forthic;

namespace RaytraceUWP
{
    public class SphereItem : StackItem
    {
        public bool IsEqual(dynamic rhs)
        {
            return this == rhs;
        }

    }
}
