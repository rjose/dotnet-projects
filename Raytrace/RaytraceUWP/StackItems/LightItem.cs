using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Rino.Forthic;

namespace RaytraceUWP
{
    public class LightItem : StackItem
    {
        public Vector4 Position { get; set; }
        public Vector4 Intensity { get; set; }

        public LightItem(Vector4 position, Vector4 intensity)
        {
            Position = position;
            Intensity = intensity;
        }
    }
}
