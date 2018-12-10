using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rino.Forthic;
using System.Numerics;

namespace Raytrace
{
    class LinearAlgebraModule : Module
    {
        public LinearAlgebraModule() : base("Raytrace.linear-algebra")
        {
            AddWord(new Vector4Word("VECTOR4"));
            AddWord(new XWord("X"));
            AddWord(new YWord("Y"));
            AddWord(new ZWord("Z"));
            AddWord(new WWord("W"));
            AddWord(new AddVector4Word("+"));
            AddWord(new SubtractVector4Word("-"));
            AddWord(new AreEqualWord("=="));
            AddWord(new ApproxEqualWord("~="));
            AddWord(new NegateVector4Word("NEGATE"));
            AddWord(new MultiplyVector4Word("*"));
            AddWord(new DivideVector4Word("/"));
            AddWord(new MagnitudeVector4Word("MAGNITUDE"));
            AddWord(new SqrtWord("SQRT"));
            AddWord(new NormalizeWord("NORMALIZE"));
            AddWord(new DotWord("DOT"));
            AddWord(new CrossWord("CROSS"));

            this.Code = @"
            : POINT    1 VECTOR4 ;   # ( x y z -- Vector4 )
            : VECTOR   0 VECTOR4 ;   # ( x y z -- Vector4 )
            ";
        }
    }


    // -------------------------------------------------------------------------
    // Words

    class Vector4Word : Word
    {
        public Vector4Word(string name) : base(name) { }

        // ( x y z w -- Vector4Item )
        public override void Execute(Interpreter interp)
        {
            dynamic wItem = interp.StackPop();
            dynamic zItem = interp.StackPop();
            dynamic yItem = interp.StackPop();
            dynamic xItem = interp.StackPop();
            interp.StackPush(new Vector4Item(xItem.FloatValue, yItem.FloatValue, zItem.FloatValue, wItem.FloatValue));
        }
    }

    class XWord : Word
    {
        public XWord(string name) : base(name) { }

        // ( v -- v.x )
        public override void Execute(Interpreter interp)
        {
            Vector4Item v = (Vector4Item)interp.StackPop();
            interp.StackPush(new DoubleItem(v.Vector4Value.X));
        }
    }

    class ZWord : Word
    {
        public ZWord(string name) : base(name) { }

        // ( v -- v.z )
        public override void Execute(Interpreter interp)
        {
            Vector4Item v = (Vector4Item)interp.StackPop();
            interp.StackPush(new DoubleItem(v.Vector4Value.Z));
        }
    }

    class WWord : Word
    {
        public WWord(string name) : base(name) { }

        // ( v -- v.w )
        public override void Execute(Interpreter interp)
        {
            Vector4Item v = (Vector4Item)interp.StackPop();
            interp.StackPush(new DoubleItem(v.Vector4Value.W));
        }
    }

    class YWord : Word
    {
        public YWord(string name) : base(name) { }

        // ( v -- v.y )
        public override void Execute(Interpreter interp)
        {
            Vector4Item v = (Vector4Item)interp.StackPop();
            interp.StackPush(new DoubleItem(v.Vector4Value.Y));
        }
    }


    class AddVector4Word : Word
    {
        public AddVector4Word(string name) : base(name) { }

        // ( v1 v2 -- v1+v2 )
        public override void Execute(Interpreter interp)
        {
            Vector4Item v2 = (Vector4Item)interp.StackPop();
            Vector4Item v1 = (Vector4Item)interp.StackPop();
            Vector4 sum = v1.Vector4Value + v2.Vector4Value;
            interp.StackPush(new Vector4Item(sum));
        }
    }


    class SubtractVector4Word : Word
    {
        public SubtractVector4Word(string name) : base(name) { }

        // ( v1 v2 -- v1-v2 )
        public override void Execute(Interpreter interp)
        {
            Vector4Item v2 = (Vector4Item)interp.StackPop();
            Vector4Item v1 = (Vector4Item)interp.StackPop();
            Vector4 diff = v1.Vector4Value - v2.Vector4Value;
            interp.StackPush(new Vector4Item(diff));
        }
    }

    class AreEqualWord : Word
    {
        public AreEqualWord(string name) : base(name) { }

        // ( v1 v2 -- bool )
        public override void Execute(Interpreter interp)
        {
            dynamic v2 = interp.StackPop();
            dynamic v1 = interp.StackPop();
            bool isEqual = v1.IsEqual(v2);
            interp.StackPush(new BoolItem(isEqual));
        }
    }

    class ApproxEqualWord : Word
    {
        public ApproxEqualWord(string name) : base(name) { }

        // ( v1 v2 -- bool )
        public override void Execute(Interpreter interp)
        {
            dynamic v2 = interp.StackPop();
            dynamic v1 = interp.StackPop();
            bool isEqual = v1.ApproxEqual(v2, 1E-4);
            interp.StackPush(new BoolItem(isEqual));
        }
    }

    class NegateVector4Word : Word
    {
        public NegateVector4Word(string name) : base(name) { }

        // ( v -- -v )
        public override void Execute(Interpreter interp)
        {
            Vector4Item v = (Vector4Item)interp.StackPop();
            Vector4Item result = new Vector4Item(Vector4.Negate(v.Vector4Value));
            interp.StackPush(result);
        }
    }

    class MultiplyVector4Word : Word
    {
        public MultiplyVector4Word(string name) : base(name) { }

        // ( v a -- a*v )
        public override void Execute(Interpreter interp)
        {
            dynamic a = interp.StackPop();
            Vector4Item v = (Vector4Item)interp.StackPop();
            Vector4Item result = new Vector4Item(Vector4.Multiply(a.FloatValue, v.Vector4Value));
            interp.StackPush(result);
        }
    }

    class DivideVector4Word : Word
    {
        public DivideVector4Word(string name) : base(name) { }

        // ( v a -- v/a )
        public override void Execute(Interpreter interp)
        {
            dynamic a = interp.StackPop();
            Vector4Item v = (Vector4Item)interp.StackPop();
            Vector4Item result = new Vector4Item(Vector4.Divide(v.Vector4Value, a.FloatValue));
            interp.StackPush(result);
        }
    }

    class MagnitudeVector4Word : Word
    {
        public MagnitudeVector4Word(string name) : base(name) { }

        // ( v -- ||v|| )
        public override void Execute(Interpreter interp)
        {
            Vector4Item v = (Vector4Item)interp.StackPop();
            DoubleItem result = new DoubleItem(v.Vector4Value.Length());
            interp.StackPush(result);
        }
    }

    class SqrtWord : Word
    {
        public SqrtWord(string name) : base(name) { }

        // ( a -- sqrt(a) )
        public override void Execute(Interpreter interp)
        {
            DoubleItem a = (DoubleItem)interp.StackPop();
            DoubleItem result = new DoubleItem(Math.Sqrt(a.DoubleValue));
            interp.StackPush(result);
        }
    }

    class NormalizeWord : Word
    {
        public NormalizeWord(string name) : base(name) { }

        // ( v -- v' )
        public override void Execute(Interpreter interp)
        {
            Vector4Item v = (Vector4Item)interp.StackPop();
            Vector4Item result = new Vector4Item(Vector4.Normalize(v.Vector4Value));
            interp.StackPush(result);
        }
    }

    class DotWord : Word
    {
        public DotWord(string name) : base(name) { }

        // ( v1 v2 -- s )
        public override void Execute(Interpreter interp)
        {
            Vector4Item v2 = (Vector4Item)interp.StackPop();
            Vector4Item v1 = (Vector4Item)interp.StackPop();
            DoubleItem result = new DoubleItem(Vector4.Dot(v1.Vector4Value, v2.Vector4Value));
            interp.StackPush(result);
        }
    }

    class CrossWord : Word
    {
        public CrossWord(string name) : base(name) { }

        // ( v1 v2 -- v )
        public override void Execute(Interpreter interp)
        {
            Vector4Item v2 = (Vector4Item)interp.StackPop();
            Vector4Item v1 = (Vector4Item)interp.StackPop();
            Vector4 a = v1.Vector4Value;
            Vector4 b = v2.Vector4Value;
            Vector4Item result = new Vector4Item(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y* b.X,
                0);
            interp.StackPush(result);
        }
    }
}
