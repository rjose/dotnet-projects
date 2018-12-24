using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raytrace;
using Rino.Forthic;
using System.Numerics;
using RaytraceUWP;

namespace Raytrace.TestsUWP
{
    [TestClass]
    public class TupleTest
    {
        Interpreter interp;

        public static void AssertVector4Equal(float x, float y, float z, float w, Vector4 vec)
        {
            Assert.AreEqual(x, vec.X, 0.001);
            Assert.AreEqual(y, vec.Y, 0.001);
            Assert.AreEqual(z, vec.Z, 0.001);
            Assert.AreEqual(w, vec.W, 0.001);
        }

        [TestInitialize]
        public void Initialize()
        {
            interp = RaytraceInterpreter.MakeInterp();
            interp.Run("[ linear-algebra ] USE-MODULES");
        }

        [TestMethod]
        public void TestVector()
        {
            interp.Run("2 3 4 Vector");
            Assert.AreEqual(1, interp.stack.Count);
            Vector4Item v = (Vector4Item)interp.stack.Peek();
            TupleTest.AssertVector4Equal(2.0f, 3.0f, 4.0f, 0.0f, v.Vector4Value);

            interp.Run("POP  4 6 8 Point");
            Assert.AreEqual(1, interp.stack.Count);
            v = (Vector4Item)interp.stack.Peek();
            TupleTest.AssertVector4Equal(4.0f, 6.0f, 8.0f, 1.0f, v.Vector4Value);
        }

        [TestMethod]
        public void TestAddVectorToPoint()
        {
            interp.Run("3 -2 5 Point  -2 3 1 Vector  +");
            TestUtils.AssertStackTrue(interp, "1 1 6 Point  ==");
        }

        [TestMethod]
        public void TestSubtractPoints()
        {
            interp.Run("3 2 1 Point  5 6 7 Point  -");
            TestUtils.AssertStackTrue(interp, "-2 -4 -6 Vector  ==");
        }

        [TestMethod]
        public void TestNegate()
        {
            interp.Run("1 -2 3 -4 Vector4  NEGATE");
            TestUtils.AssertStackTrue(interp, "-1 2 -3 4 Vector4  ==");
        }

        [TestMethod]
        public void TestMultiply()
        {
            interp.Run("1 -2 3 -4 Vector4 3.5 *");
            TestUtils.AssertStackTrue(interp, "3.5 -7 10.5 -14 Vector4  ==");

            interp.Run("1 -2 3 -4 Vector4 0.5 *");
            TestUtils.AssertStackTrue(interp, "0.5 -1 1.5 -2 Vector4  ==");
        }

        [TestMethod]
        public void TestDivide()
        {
            interp.Run("1 -2 3 -4 Vector4 2 /");
            TestUtils.AssertStackTrue(interp, "0.5 -1 1.5 -2 Vector4  ==");
        }

        [TestMethod]
        public void TestMagnitude()
        {
            interp.Run("1 0 0 Vector  MAGNITUDE");
            TestUtils.AssertStackTrue(interp, "1.0  ==");

            interp.Run("0 1 0 Vector  MAGNITUDE");
            TestUtils.AssertStackTrue(interp, "1.0  ==");

            interp.Run("0 0 1 Vector  MAGNITUDE");
            TestUtils.AssertStackTrue(interp, "1.0  ==");

            interp.Run("1 2 3 Vector  MAGNITUDE");
            TestUtils.AssertStackTrue(interp, "14.0 SQRT  ==");

            interp.Run("-1 -2 -3 Vector  MAGNITUDE");
            TestUtils.AssertStackTrue(interp, "14.0 SQRT  ==");
        }

        [TestMethod]
        public void TestNormalize()
        {
            interp.Run("4 0 0 Vector  NORMALIZE");
            TestUtils.AssertStackTrue(interp, "1 0 0 Vector  ==");

            interp.Run("1 2 3 Vector  NORMALIZE");
            TestUtils.AssertStackTrue(interp, "0.26726 0.53452 0.80178 Vector  ~=");
        }

        [TestMethod]
        public void TestDotProduct()
        {
            interp.Run("1 2 3 Vector  2 3 4 Vector DOT");
            TestUtils.AssertStackTrue(interp, "20.0 ==");
        }

        [TestMethod]
        public void TestCrossProduct()
        {
            interp.Run("1 2 3 Vector  2 3 4 Vector CROSS");
            TestUtils.AssertStackTrue(interp, "-1 2 -1 Vector ==");

            interp.Run("2 3 4 Vector  1 2 3 Vector CROSS");
            TestUtils.AssertStackTrue(interp, "1 -2 1 Vector ==");
        }
    }
}
