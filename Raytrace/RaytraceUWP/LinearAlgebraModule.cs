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
            AddWord(new PlusWord("+"));
            AddWord(new MinusWord("-"));
            AddWord(new AreEqualWord("=="));
            AddWord(new ApproxEqualWord("~="));
            AddWord(new NegateWord("NEGATE"));
            AddWord(new MultiplyWord("*"));
            AddWord(new DivideWord("/"));
            AddWord(new MagnitudeVector4Word("MAGNITUDE"));
            AddWord(new SqrtWord("SQRT"));
            AddWord(new NormalizeWord("NORMALIZE"));
            AddWord(new DotWord("DOT"));
            AddWord(new CrossWord("CROSS"));
            AddWord(new MatrixWord("MATRIX"));
            AddWord(new MWord("M"));
            AddWord(new IdentityWord("IDENTITY"));
            AddWord(new TransposeWord("TRANSPOSE"));
            AddWord(new DeterminantWord("DETERMINANT"));
            AddWord(new InverseWord("INVERSE"));
            AddWord(new TranslationWord("TRANSLATION"));
            AddWord(new ScalingWord("SCALING"));
            AddWord(new PiWord("PI"));
            AddWord(new RotationXWord("ROTATION-X"));
            AddWord(new RotationYWord("ROTATION-Y"));
            AddWord(new RotationZWord("ROTATION-Z"));
            AddWord(new ShearingWord("SHEARING"));
            AddWord(new ChainWord("CHAIN"));

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
            dynamic v = interp.StackPop();
            interp.StackPush(new DoubleItem(v.X));
        }
    }

    class ZWord : Word
    {
        public ZWord(string name) : base(name) { }

        // ( v -- v.z )
        public override void Execute(Interpreter interp)
        {
            dynamic v = interp.StackPop();
            interp.StackPush(new DoubleItem(v.Z));
        }
    }

    class WWord : Word
    {
        public WWord(string name) : base(name) { }

        // ( v -- v.w )
        public override void Execute(Interpreter interp)
        {
            dynamic v = interp.StackPop();
            interp.StackPush(new DoubleItem(v.W));
        }
    }

    class YWord : Word
    {
        public YWord(string name) : base(name) { }

        // ( v -- v.y )
        public override void Execute(Interpreter interp)
        {
            dynamic v = interp.StackPop();
            interp.StackPush(new DoubleItem(v.Y));
        }
    }


    class PlusWord : Word
    {
        public PlusWord(string name) : base(name) { }

        // ( l r -- sum )
        public override void Execute(Interpreter interp)
        {
            dynamic r = interp.StackPop();
            dynamic l = interp.StackPop();
            interp.StackPush(plus(l, r));
        }

        StackItem plus(Vector4Item l, Vector4Item r)
        {
            return new Vector4Item(l.Vector4Value + r.Vector4Value);
        }

        StackItem plus(IntItem l, IntItem r)
        {
            return new IntItem(l.IntValue + r.IntValue);
        }

        StackItem plus(ScalarItem l, ScalarItem r)
        {
            return new DoubleItem(l.DoubleValue + r.DoubleValue);
        }

        StackItem plus(MatrixItem l, MatrixItem r)
        {
            return new MatrixItem(l.MatrixValue + r.MatrixValue);
        }
    }


    class MinusWord : Word
    {
        public MinusWord(string name) : base(name) { }


        // ( l r -- difference )
        public override void Execute(Interpreter interp)
        {
            dynamic r = interp.StackPop();
            dynamic l = interp.StackPop();
            interp.StackPush(minus(l, r));
        }

        StackItem minus(Vector4Item l, Vector4Item r)
        {
            return new Vector4Item(l.Vector4Value - r.Vector4Value);
        }

        StackItem minus(IntItem l, IntItem r)
        {
            return new IntItem(l.IntValue - r.IntValue);
        }

        StackItem minus(ScalarItem l, ScalarItem r)
        {
            return new DoubleItem(l.DoubleValue - r.DoubleValue);
        }

        StackItem minus(MatrixItem l, MatrixItem r)
        {
            return new MatrixItem(l.MatrixValue - r.MatrixValue);
        }
    }

    class AreEqualWord : Word
    {
        public AreEqualWord(string name) : base(name) { }

        // ( l r -- bool )
        public override void Execute(Interpreter interp)
        {
            dynamic r = interp.StackPop();
            dynamic l = interp.StackPop();
            bool isEqual = l.IsEqual(r);
            interp.StackPush(new BoolItem(isEqual));
        }
    }

    class ApproxEqualWord : Word
    {
        public ApproxEqualWord(string name) : base(name) { }

        // ( l r -- bool )
        public override void Execute(Interpreter interp)
        {
            dynamic r = interp.StackPop();
            dynamic l = interp.StackPop();
            interp.StackPush(new BoolItem(l.ApproxEqual(r, 1E-4)));
        }
    }

    class NegateWord : Word
    {
        public NegateWord(string name) : base(name) { }

        // ( v -- -v )
        public override void Execute(Interpreter interp)
        {
            dynamic item = interp.StackPop();
            interp.StackPush(negate(item));
        }

        StackItem negate(Vector4Item item)
        {
            return new Vector4Item(Vector4.Negate(item.Vector4Value));
        }

        StackItem negate(IntItem item)
        {
            return new IntItem(-item.IntValue);
        }

        StackItem negate(DoubleItem item)
        {
            return new DoubleItem(-item.DoubleValue);
        }

        StackItem negate(MatrixItem item)
        {
            return new MatrixItem(-item.MatrixValue);
        }
    }

    class MultiplyWord : Word
    {
        public MultiplyWord(string name) : base(name) { }

        // ( l r -- l*r )
        public override void Execute(Interpreter interp)
        {
            dynamic r = interp.StackPop();
            dynamic l = interp.StackPop();
            interp.StackPush(multiply(l, r));
        }

        // Multiply options
        StackItem multiply(Vector4Item l, ScalarItem r)
        {
            return new Vector4Item(Vector4.Multiply(r.FloatValue, l.Vector4Value));
        }

        StackItem multiply(ScalarItem l, Vector4Item r)
        {
            return multiply(r, l);
        }

        StackItem multiply(IntItem l, IntItem r)
        {
            return new IntItem(l.IntValue * r.IntValue);
        }

        StackItem multiply(ScalarItem l, ScalarItem r)
        {
            return new DoubleItem(l.DoubleValue * r.DoubleValue);
        }

        // Hadamard multiplication
        StackItem multiply(Vector4Item l, Vector4Item r)
        {
            return new Vector4Item(l.X * r.X, l.Y * r.Y, l.Z * r.Z, l.W * r.W);
        }

        StackItem multiply(MatrixItem l, MatrixItem r)
        {
            return new MatrixItem(Matrix4x4.Multiply(l.MatrixValue, r.MatrixValue));
        }

        StackItem multiply(MatrixItem l, Vector4Item r)
        {
            return new Vector4Item(
                Vector4.Dot(l.GetRow(0), r.Vector4Value),
                Vector4.Dot(l.GetRow(1), r.Vector4Value),
                Vector4.Dot(l.GetRow(2), r.Vector4Value),
                Vector4.Dot(l.GetRow(3), r.Vector4Value));
        }

    }

    class DivideWord : Word
    {
        public DivideWord(string name) : base(name) { }

        // ( l r -- l*r )
        public override void Execute(Interpreter interp)
        {
            dynamic r = interp.StackPop();
            dynamic l = interp.StackPop();
            interp.StackPush(divide(l, r));
        }

        StackItem divide(Vector4Item l, ScalarItem r)
        {
            return new Vector4Item(Vector4.Divide(l.Vector4Value, r.FloatValue));
        }

        StackItem divide(ScalarItem l, ScalarItem r)
        {
            return new DoubleItem(l.DoubleValue / r.DoubleValue);
        }

        StackItem divide(MatrixItem l, ScalarItem r)
        {
            return new MatrixItem(Matrix4x4.Multiply(l.MatrixValue, 1.0f/r.FloatValue));
        }
    }

    class MagnitudeVector4Word : Word
    {
        public MagnitudeVector4Word(string name) : base(name) { }

        // ( v -- ||v|| )
        public override void Execute(Interpreter interp)
        {
            dynamic v = interp.StackPop();
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
            dynamic a = interp.StackPop();
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
            dynamic v = interp.StackPop();
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
            dynamic v2 = interp.StackPop();
            dynamic v1 = interp.StackPop();
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
            dynamic v2 = interp.StackPop();
            dynamic v1 = interp.StackPop();
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
            dynamic values = interp.StackPop();
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
            dynamic j = interp.StackPop();
            dynamic i = interp.StackPop();
            dynamic matrix = interp.StackPop();
            interp.StackPush(new DoubleItem(matrix.GetElement(i.IntValue, j.IntValue)));
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
            dynamic A = interp.StackPop();
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

    class TranslationWord : Word
    {
        public TranslationWord(string name) : base(name) { }

        // ( dx dy dz -- matrix )
        public override void Execute(Interpreter interp)
        {
            dynamic dz = interp.StackPop();
            dynamic dy = interp.StackPop();
            dynamic dx = interp.StackPop();

            Matrix4x4 result = Matrix4x4.Identity;
            result.M14 = dx.FloatValue;
            result.M24 = dy.FloatValue;
            result.M34 = dz.FloatValue;
            result.M44 = 1.0f;
            interp.StackPush(new MatrixItem(result));
        }
    }

    class ScalingWord : Word
    {
        public ScalingWord(string name) : base(name) { }

        // ( sx sy sz -- matrix )
        public override void Execute(Interpreter interp)
        {
            dynamic sz = interp.StackPop();
            dynamic sy = interp.StackPop();
            dynamic sx = interp.StackPop();

            Matrix4x4 result = Matrix4x4.Identity;
            result.M11 = sx.FloatValue;
            result.M22 = sy.FloatValue;
            result.M33 = sz.FloatValue;
            interp.StackPush(new MatrixItem(result));
        }
    }

    class PiWord : Word
    {
        public PiWord(string name) : base(name) { }

        // ( -- pi )
        public override void Execute(Interpreter interp)
        {
            interp.StackPush(new DoubleItem(Math.PI));
        }
    }

    class RotationXWord : Word
    {
        public RotationXWord(string name) : base(name) { }

        // ( radians -- matrix )
        public override void Execute(Interpreter interp)
        {
            dynamic rads_item = interp.StackPop();
            double rads = rads_item.DoubleValue;
            Matrix4x4 result = Matrix4x4.Identity;
            result.M22 = (float)Math.Cos(rads);
            result.M23 = (float)-Math.Sin(rads);
            result.M32 = (float)Math.Sin(rads);
            result.M33 = (float)Math.Cos(rads);
            interp.StackPush(new MatrixItem(result));
        }
    }

    class RotationYWord : Word
    {
        public RotationYWord(string name) : base(name) { }

        // ( radians -- matrix )
        public override void Execute(Interpreter interp)
        {
            dynamic rads_item = interp.StackPop();
            double rads = rads_item.DoubleValue;
            Matrix4x4 result = Matrix4x4.Identity;
            result.M11 = (float)Math.Cos(rads);
            result.M13 = (float)Math.Sin(rads);
            result.M31 = (float)-Math.Sin(rads);
            result.M33 = (float)Math.Cos(rads);
            interp.StackPush(new MatrixItem(result));
        }
    }

    class RotationZWord : Word
    {
        public RotationZWord(string name) : base(name) { }

        // ( radians -- matrix )
        public override void Execute(Interpreter interp)
        {
            dynamic rads_item = interp.StackPop();
            double rads = rads_item.DoubleValue;
            Matrix4x4 result = Matrix4x4.Identity;
            result.M11 = (float)Math.Cos(rads);
            result.M12 = (float)-Math.Sin(rads);
            result.M21 = (float)Math.Sin(rads);
            result.M22 = (float)Math.Cos(rads);
            interp.StackPush(new MatrixItem(result));
        }
    }

    class ShearingWord : Word
    {
        public ShearingWord(string name) : base(name) { }

        // ( xy xz yx yz zx zy -- matrix )
        public override void Execute(Interpreter interp)
        {
            dynamic zy = interp.StackPop();
            dynamic zx = interp.StackPop();
            dynamic yz = interp.StackPop();
            dynamic yx = interp.StackPop();
            dynamic xz = interp.StackPop();
            dynamic xy = interp.StackPop();

            Matrix4x4 result = Matrix4x4.Identity;
            result.M12 = xy.FloatValue;
            result.M13 = xz.FloatValue;
            result.M21 = yx.FloatValue;
            result.M23 = yz.FloatValue;
            result.M31 = zx.FloatValue;
            result.M32 = zy.FloatValue;
            interp.StackPush(new MatrixItem(result));
        }
    }

    class ChainWord : Word
    {
        public ChainWord(string name) : base(name) { }

        // ( matrices -- matrix )
        public override void Execute(Interpreter interp)
        {
            dynamic matrices = interp.StackPop();
            List<StackItem> matrix_items = matrices.ArrayValue;
            matrix_items.Reverse();

            interp.Run("IDENTITY");
            foreach (MatrixItem item in matrix_items)
            {
                interp.StackPush(item);
                interp.Run("*");
            }
        }
    }


}
