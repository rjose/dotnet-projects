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
            AddWord(new MatrixWord("MATRIX"));
            AddWord(new MWord("M"));
            AddWord(new MatrixMulWord("MATRIX-MUL"));
            AddWord(new IdentityWord("IDENTITY"));
            AddWord(new TransposeWord("TRANSPOSE"));
            AddWord(new DeterminantWord("DETERMINANT"));
            AddWord(new InverseWord("INVERSE"));

            this.Code = @"
            : TUPLE    VECTOR4 ;
            : POINT    1 VECTOR4 ;   # ( x y z -- Vector4 )
            : VECTOR   0 VECTOR4 ;   # ( x y z -- Vector4 )
            : !=       == NOT ;
            : INVERTIBLE?   DETERMINANT 0 != ;
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
            interp.StackPush(new BoolItem(v1.ApproxEqual(v2, 1E-4)));
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
                a.X * b.Y - a.Y * b.X,
                0);
            interp.StackPush(result);
        }
    }

    class MatrixWord : Word
    {
        public MatrixWord(string name) : base(name) { }

        // ( 16-values -- Matrix )
        public override void Execute(Interpreter interp)
        {
            ArrayItem values = (ArrayItem)interp.StackPop();
            if (values.ArrayValue.Count != 16) throw new InvalidOperationException("MatrixItem requires 16 values");
            float getValue(dynamic item)
            {
                return item.FloatValue;
            }
            float m11 = getValue(values.ArrayValue[0]);
            float m12 = getValue(values.ArrayValue[1]);
            float m13 = getValue(values.ArrayValue[2]);
            float m14 = getValue(values.ArrayValue[3]);
            float m21 = getValue(values.ArrayValue[4]);
            float m22 = getValue(values.ArrayValue[5]);
            float m23 = getValue(values.ArrayValue[6]);
            float m24 = getValue(values.ArrayValue[7]);
            float m31 = getValue(values.ArrayValue[8]);
            float m32 = getValue(values.ArrayValue[9]);
            float m33 = getValue(values.ArrayValue[10]);
            float m34 = getValue(values.ArrayValue[11]);
            float m41 = getValue(values.ArrayValue[12]);
            float m42 = getValue(values.ArrayValue[13]);
            float m43 = getValue(values.ArrayValue[14]);
            float m44 = getValue(values.ArrayValue[15]);
            interp.StackPush(new MatrixItem(m11, m12, m13, m14,
                m21, m22, m23, m24,
                m31, m32, m33, m34,
                m41, m42, m43, m44));
        }
    }

    class MWord : Word
    {
        public MWord(string name) : base(name) { }

        // ( matrix i j -- float )
        public override void Execute(Interpreter interp)
        {
            IntItem j = (IntItem)interp.StackPop();
            IntItem i = (IntItem)interp.StackPop();
            MatrixItem matrix = (MatrixItem)interp.StackPop();
            interp.StackPush(new DoubleItem(matrix.GetElement(i.IntValue, j.IntValue)));
        }
    }

    class MatrixMulWord : Word
    {
        public MatrixMulWord(string name) : base(name) { }

        // ( A B -- A*B )
        public override void Execute(Interpreter interp)
        {
            dynamic B = interp.StackPop();
            dynamic A = interp.StackPop();
            if (B.GetType() == typeof(MatrixItem) )
            {
                Matrix4x4 result = Matrix4x4.Multiply(A.MatrixValue, B.MatrixValue);
                interp.StackPush(new MatrixItem(result));
            }
            else if (B.GetType() == typeof(Vector4Item))
            {
                Vector4Item result = new Vector4Item(
                    Vector4.Dot(A.GetRow(0), B.Vector4Value),
                    Vector4.Dot(A.GetRow(1), B.Vector4Value),
                    Vector4.Dot(A.GetRow(2), B.Vector4Value),
                    Vector4.Dot(A.GetRow(3), B.Vector4Value));
                interp.StackPush(result);
            }
            else
            {
                throw new InvalidOperationException(String.Format("Can't matrix multiply with {0}", B.GetType()));
            }
        }
    }

    class IdentityWord : Word
    {
        public IdentityWord(string name) : base(name) { }

        // ( -- identity_matrix )
        public override void Execute(Interpreter interp)
        {
            interp.StackPush(new MatrixItem(Matrix4x4.Identity));
        }
    }

    class TransposeWord : Word
    {
        public TransposeWord(string name) : base(name) { }

        // ( A -- A_t )
        public override void Execute(Interpreter interp)
        {
            dynamic A = interp.StackPop();
            interp.StackPush(new MatrixItem(Matrix4x4.Transpose(A.MatrixValue)));
        }
    }

    class DeterminantWord : Word
    {
        public DeterminantWord(string name) : base(name) { }

        // ( A -- determinant )
        public override void Execute(Interpreter interp)
        {
            dynamic A = interp.StackPop();
            interp.StackPush(new DoubleItem(A.MatrixValue.GetDeterminant()));
        }
    }

    class InverseWord : Word
    {
        public InverseWord(string name) : base(name) { }

        // ( A -- A_inv )
        public override void Execute(Interpreter interp)
        {
            MatrixItem A = (MatrixItem)interp.StackPop();
            Matrix4x4 result;
            if (Matrix4x4.Invert(A.MatrixValue, out result))
            {
                interp.StackPush(new MatrixItem(result));
            }
            else
            {
                throw new InvalidOperationException(String.Format("Could not invert: {0}", A.MatrixValue));
            }            
        }
    }
}
