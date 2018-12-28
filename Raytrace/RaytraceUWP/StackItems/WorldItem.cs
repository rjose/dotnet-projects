using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Rino.Forthic;

namespace RaytraceUWP
{
    public class WorldItem : StackItem
    {
        public List<StackItem> Objects { get; }
        public StackItem PointLight { get; set; }        

        public WorldItem()
        {
            Objects = new List<StackItem>();
            PointLight = new NullItem();
        }

        public void AddObject(StackItem item)
        {
            Objects.Add(item);
        }

        public bool Contains(StackItem item)
        {
            return Objects.Contains(item);
        }

        override public void SetValue(string key, StackItem value)
        {
            if (key == "light")   PointLight = value;
            else throw new InvalidOperationException(String.Format("Unknown key: {0}", key));
        }

        override public StackItem GetValue(string key)
        {
            if (key == "objects")       return new ArrayItem(Objects);
            else if (key == "light")    return PointLight;
            else throw new InvalidOperationException(String.Format("Unknown key: {0}", key));
        }
    }
}
