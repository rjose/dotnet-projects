using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raytrace;
using Rino.Forthic;
using System.Numerics;

namespace Raytrace.Tests
{
    [TestClass]
    public class TupleTest
    {
        Interpreter interp;

        [TestInitialize]
        public void Initialize()
        {
            interp = RaytraceInterpreter.MakeInterp();
            interp.Run("[ Raytrace.linear-algebra ] USE-MODULES");
        }

        [TestMethod]
        public void TestVector()
        {
            interp.Run("2 3 4 VECTOR");
            Assert.AreEqual(1, interp.stack.Count);
            Vector4Item v = (Vector4Item)interp.stack.Peek();
            Vector4 value = v.Vector4Value;
            Assert.AreEqual(2.0f, value.X);
            Assert.AreEqual(3.0f, value.Y);
            Assert.AreEqual(4.0f, value.Z);
            Assert.AreEqual(0.0f, value.W);

            interp.Run("POP  4 6 8 POINT");
            Assert.AreEqual(1, interp.stack.Count);
            v = (Vector4Item)interp.stack.Peek();
            value = v.Vector4Value;
            Assert.AreEqual(4.0f, value.X);
            Assert.AreEqual(6.0f, value.Y);
            Assert.AreEqual(8.0f, value.Z);
            Assert.AreEqual(1.0f, value.W);
        }
    }
}
