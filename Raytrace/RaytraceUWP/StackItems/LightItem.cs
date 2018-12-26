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

        override public void SetValue(string key, StackItem value)
        {
            if (key == "position")       Position = ((Vector4Item)(value)).Vector4Value;
            else if (key == "intensity") Intensity = ((Vector4Item)(value)).Vector4Value;
            else throw new InvalidOperationException(String.Format("Unknown key: {0}", key));
        }

        override public StackItem GetValue(string key)
        {
            if (key == "position")       return new Vector4Item(Position);
            else if (key == "intensity") return new Vector4Item(Intensity);
            else throw new InvalidOperationException(String.Format("Unknown key: {0}", key));
        }
    }
}
