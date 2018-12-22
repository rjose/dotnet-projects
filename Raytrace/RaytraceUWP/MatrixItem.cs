using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Rino.Forthic;

namespace RaytraceUWP
{
    public class MatrixItem : StackItem
    {
        public Matrix4x4 MatrixValue { get; }

        public MatrixItem(float m11, float m12, float m13, float m14,
            float m21, float m22, float m23, float m24,
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44)
        {
            MatrixValue = new Matrix4x4(m11, m12, m13, m14,
                m21, m22, m23, m24,
                m31, m32, m33, m34,
                m41, m42, m43, m44);
        }

        public MatrixItem(Matrix4x4 matrix)
        {
            MatrixValue = matrix;
        }

        public Vector4 GetRow(int index)
        {
            switch (index)
            {
                case 0: return new Vector4(MatrixValue.M11, MatrixValue.M12, MatrixValue.M13, MatrixValue.M14);
                case 1: return new Vector4(MatrixValue.M21, MatrixValue.M22, MatrixValue.M23, MatrixValue.M24);
                case 2: return new Vector4(MatrixValue.M31, MatrixValue.M32, MatrixValue.M33, MatrixValue.M34);
                case 3: return new Vector4(MatrixValue.M41, MatrixValue.M42, MatrixValue.M43, MatrixValue.M44);
                default: throw new InvalidOperationException(String.Format("Invalid row: {0}", index));
            }
        }

        public double GetElement(int i, int j)
        {
            int i_1 = i + 1;
            int j_1 = j + 1;
            switch (i_1)
            {
                case 1:
                    switch (j_1)
                    {
                        case 1: return MatrixValue.M11;
                        case 2: return MatrixValue.M12;
                        case 3: return MatrixValue.M13;
                        case 4: return MatrixValue.M14;
                    }
                    break;
                case 2:
                    switch (j_1)
                    {
                        case 1: return MatrixValue.M21;
                        case 2: return MatrixValue.M22;
                        case 3: return MatrixValue.M23;
                        case 4: return MatrixValue.M24;
                    }
                    break;
                case 3:
                    switch (j_1)
                    {
                        case 1: return MatrixValue.M31;
                        case 2: return MatrixValue.M32;
                        case 3: return MatrixValue.M33;
                        case 4: return MatrixValue.M34;
                    }
                    break;
                case 4:
                    switch (j_1)
                    {
                        case 1: return MatrixValue.M41;
                        case 2: return MatrixValue.M42;
                        case 3: return MatrixValue.M43;
                        case 4: return MatrixValue.M44;
                    }
                    break;
            }
            throw new InvalidOperationException(String.Format("Invalid (i, j): ({0}, {1})", i, j));
        }

        public bool IsEqual(MatrixItem rhs)
        {
            return this.MatrixValue == rhs.MatrixValue;
        }

    }
}
