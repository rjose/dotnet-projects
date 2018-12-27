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
        public MatrixItem  Transform { get; set; }
        public MaterialItem  Material { get; set; }

        public SphereItem()
        {
            Transform = IdentityWord.Identity;
            Material = new MaterialItem();
        }

        public bool IsEqual(dynamic rhs)
        {
            return this == rhs;
        }

        override public void SetValue(string key, StackItem value)
        {
            if (key == "transform") Transform = (MatrixItem)value;
            else if (key == "material") Material = (MaterialItem)value;
            else throw new InvalidOperationException(String.Format("Unknown key: {0}", key));
        }

        override public StackItem GetValue(string key)
        {
            if (key == "transform") return Transform;
            else if (key == "material") return Material;
            else throw new InvalidOperationException(String.Format("Unknown key: {0}", key));
        }
    }
}
