using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rino.Forthic;
using System.Numerics;
using System.Diagnostics;

namespace RaytraceUWP
{
    public class IntersectionModule : Module
    {
        public IntersectionModule(string name) : base(name)
        {
            AddWord(new RayWord("Ray"));
            AddWord(new OriginWord("ORIGIN"));
            AddWord(new DirectionWord("DIRECTION"));
            AddWord(new PositionWord("POSITION"));
            AddWord(new SphereWord("Sphere"));
            AddWord(new IntersectsWord("INTERSECTS"));

            AddWord(new IntersectionWord("Intersection"));
            AddWord(new TWord("T"));
            AddWord(new ObjWord("OBJ"));

            AddWord(new HitWord("HIT"));
            AddWord(new TransformAtWord("TRANSFORM@"));
            AddWord(new TransformBangWord("TRANSFORM!"));

            this.Code = @"
            : TRANSFORM   SWAP * ;   # ( ray matrix -- ray )
            ";

        }
    }

    class RayWord : Word
    {
        public RayWord(string name) : base(name) { }

        // ( origin direction -- Ray )
        public override void Execute(Interpreter interp)
        {
            dynamic direction = interp.StackPop();
            dynamic origin = interp.StackPop();
            interp.StackPush(new RayItem(origin.Vector4Value, direction.Vector4Value));
        }
    }

    class OriginWord : Word
    {
        public OriginWord(string name) : base(name) { }

        // ( Ray -- origin )
        public override void Execute(Interpreter interp)
        {
            dynamic ray = interp.StackPop();
            interp.StackPush(new Vector4Item(ray.Origin));
        }
    }

    class DirectionWord: Word
    {
        public DirectionWord(string name) : base(name) { }

        // ( Ray -- Direction )
        public override void Execute(Interpreter interp)
        {
            dynamic ray = interp.StackPop();
            interp.StackPush(new Vector4Item(ray.Direction));
        }
    }

    class PositionWord : Word
    {
        public PositionWord(string name) : base(name) { }

        // ( Ray t -- Point )
        public override void Execute(Interpreter interp)
        {
            dynamic t = interp.StackPop();
            dynamic ray = interp.StackPop();
            interp.StackPush(new Vector4Item(ray.Position(t.FloatValue)));
        }
    }

    class SphereWord : Word
    {
        public SphereWord(string name) : base(name) { }

        // ( -- Sphere )
        public override void Execute(Interpreter interp)
        {
            interp.StackPush(new SphereItem());
        }
    }

    class IntersectsWord : Word
    {
        public IntersectsWord(string name) : base(name) { }

        // ( object ray -- intersections )
        public override void Execute(Interpreter interp)
        {
            RayItem ray = (RayItem)interp.StackPop();
            dynamic obj = interp.StackPop();
            interp.StackPush(intersections(interp, obj, transformRay(interp, obj, ray)));
        }

        RayItem transformRay(Interpreter interp, dynamic obj, RayItem ray)
        {
            interp.StackPush(obj);
            interp.Run("TRANSFORM@ INVERSE");
            interp.StackPush(ray);
            interp.Run("SWAP TRANSFORM");
            RayItem result = (RayItem)interp.StackPop();
            return result;
        }

        ArrayItem intersections(Interpreter interp, SphereItem sphere, RayItem ray)
        {
            // Compute sphere_to_ray
            interp.StackPush(ray);
            interp.Run("ORIGIN  0 0 0 Point -");
            Vector4Item sphere_to_ray = (Vector4Item)interp.StackPop();

            // Compute a
            interp.StackPush(ray);
            interp.Run("DIRECTION DUP DOT");
            DoubleItem a = (DoubleItem)interp.StackPop();

            // Compute b
            interp.StackPush(sphere_to_ray);
            interp.StackPush(ray);
            interp.Run("DIRECTION DOT 2 *");
            DoubleItem b = (DoubleItem)interp.StackPop();

            // Compute c
            interp.StackPush(sphere_to_ray);
            interp.Run("DUP DOT 1 -");
            DoubleItem c = (DoubleItem)interp.StackPop();

            float discriminant = b.FloatValue * b.FloatValue - 4.0f * a.FloatValue * c.FloatValue;

            ArrayItem result = new ArrayItem();
            if (discriminant < 0)
            {
                return result;
            }
            else
            {
                double t1 = (-b.FloatValue - Math.Sqrt(discriminant)) / 2.0f / a.FloatValue;
                double t2 = (-b.FloatValue + Math.Sqrt(discriminant)) / 2.0f / a.FloatValue;
                result.Add(new IntersectionItem(t1, sphere));
                result.Add(new IntersectionItem(t2, sphere));
                return result;
            }
        }
    }

    class TWord : Word
    {
        public TWord(string name) : base(name) { }

        // ( Intersection -- t)
        public override void Execute(Interpreter interp)
        {
            IntersectionItem item = (IntersectionItem)interp.StackPop();
            interp.StackPush(new DoubleItem(item.T));
        }
    }

    class ObjWord : Word
    {
        public ObjWord(string name) : base(name) { }

        // ( Intersection -- Obj )
        public override void Execute(Interpreter interp)
        {
            IntersectionItem item = (IntersectionItem)interp.StackPop();
            interp.StackPush(item.Obj);
        }
    }

    class IntersectionWord : Word
    {
        public IntersectionWord(string name) : base(name) { }

        // ( t obj -- Intersection )
        public override void Execute(Interpreter interp)
        {
            dynamic obj = interp.StackPop();
            dynamic t = interp.StackPop();
            interp.StackPush(new IntersectionItem(t.DoubleValue, obj));
        }
    }

    class HitWord : Word
    {
        public HitWord(string name) : base(name) { }

        // ( intersections -- Intersection )
        public override void Execute(Interpreter interp)
        {
            dynamic intersections = interp.StackPop();
            List<StackItem> items = (List<StackItem>)intersections.ArrayValue;
            sort(items);
            interp.StackPush(firstWithPositiveT(items));
        }

        StackItem firstWithPositiveT(List<StackItem> items)
        {
            StackItem hit = new NullItem();
            foreach (IntersectionItem item in items)
            {
                if (item.T < 0)
                {
                    continue;
                }
                else
                {
                    hit = item;
                    break;
                }
            }
            return hit;
        }

        void sort(List<StackItem> items)
        {
            int sortByTAsc(StackItem l, StackItem r)
            {
                IntersectionItem xs_l = (IntersectionItem)l;
                IntersectionItem xs_r = (IntersectionItem)r;

                return xs_l.T.CompareTo(xs_r.T);
            }
            items.Sort(sortByTAsc);
        }
    }

    class TransformAtWord : Word
    {
        public TransformAtWord(string name) : base(name) { }

        // ( Sphere -- matrix )
        public override void Execute(Interpreter interp)
        {
            dynamic obj = interp.StackPop();
            interp.StackPush(obj.Transform);
        }
    }

    class TransformBangWord : Word
    {
        public TransformBangWord(string name) : base(name) { }

        // ( Sphere matrix -- )
        public override void Execute(Interpreter interp)
        {
            MatrixItem matrix = (MatrixItem)interp.StackPop();
            dynamic obj = interp.StackPop();
            obj.Transform = matrix;
        }
    }

}