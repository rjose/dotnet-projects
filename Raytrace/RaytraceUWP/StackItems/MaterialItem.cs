using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Rino.Forthic;

namespace RaytraceUWP
{
    public class MaterialItem : StackItem
    {
        public Vector4 Color { get; set; }
        public double Ambient { get; set; }
        public double Diffuse { get; set; }
        public double Specular { get; set; }
        public double Shininess { get; set; }

        public MaterialItem()
        {
            Color = new Vector4(1, 1, 1, 0);
            Ambient = 0.1;
            Diffuse = 0.9;
            Specular = 0.9;
            Shininess = 200.0;
        }

        override public void SetValue(string key, StackItem value)
        {
            if      (key == "color")      Color =     ((Vector4Item)(value)).Vector4Value;
            else if (key == "ambient")    Ambient =   ((ScalarItem)(value)).DoubleValue;
            else if (key == "diffuse")    Diffuse =   ((ScalarItem)(value)).DoubleValue;
            else if (key == "specular")   Specular =  ((ScalarItem)(value)).DoubleValue;
            else if (key == "shininess")  Shininess = ((ScalarItem)(value)).DoubleValue;
            else throw new InvalidOperationException(String.Format("Unknown key: {0}", key));
        }

        override public StackItem GetValue(string key)
        {
            if      (key == "color")     return new Vector4Item(Color);
            else if (key == "ambient")   return new DoubleItem(Ambient);
            else if (key == "diffuse")   return new DoubleItem(Diffuse);
            else if (key == "specular")  return new DoubleItem(Specular);
            else if (key == "shininess") return new DoubleItem(Shininess);
            else throw new InvalidOperationException(String.Format("Unknown key: {0}", key));
        }

        public bool IsEqual(dynamic rhs)
        {
            if (rhs.GetType() == typeof(MaterialItem))
            {
                MaterialItem rhs_m = (MaterialItem)rhs;
                return Color == rhs_m.Color &&
                       Ambient == rhs_m.Ambient &&
                       Diffuse == rhs_m.Diffuse &&
                       Specular == rhs_m.Specular &&
                       Shininess == rhs_m.Shininess;
            }
            else return false;
        }

    }
}
