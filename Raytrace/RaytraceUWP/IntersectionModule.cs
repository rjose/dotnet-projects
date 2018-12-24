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

            this.Code = @"
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
            interp.StackPush(intersections(interp, obj, ray));
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
                double t1 = ray.Origin.Z + -b.FloatValue - Math.Sqrt(discriminant) / 2.0f / a.FloatValue;
                double t2 = ray.Origin.Z + -b.FloatValue + Math.Sqrt(discriminant) / 2.0f / a.FloatValue;
                result.Add(new DoubleItem(t1));
                result.Add(new DoubleItem(t2));
                return result;
            }
        }
    }
}