﻿using System;
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

        public bool IsEqual(IntersectionItem rhs)
        {
            return this == rhs;
        }

    }
}
