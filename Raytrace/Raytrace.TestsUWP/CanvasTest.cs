using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Rino.Forthic;
using System.Numerics;
using RaytraceUWP;

namespace Raytrace.TestsUWP
{
    [TestClass]
    public class CanvasTest
    {
        Interpreter interp;

        [TestInitialize]
        public void Initialize()
        {
            interp = RaytraceInterpreter.MakeInterp();
            interp.Run("[ Raytrace.canvas ] USE-MODULES");
        }

        [TestMethod]
        public void TestColor()
        {
            interp.Run("-0.5 0.4 1.7 COLOR");
            Assert.AreEqual(1, interp.stack.Count);
            Vector4Item v = (Vector4Item)interp.stack.Peek();
            TupleTest.AssertVector4Equal(-0.5f, 0.4f, 1.7f, 1.0f, v.Vector4Value);
            TestUtils.AssertStackTrue(interp, "DUP R  -0.5  ==");
            TestUtils.AssertStackTrue(interp, "DUP G   0.4  ==");
            TestUtils.AssertStackTrue(interp, "    B   1.7  ==");
        }
    }
}
