using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rino.Forthic;
using System.Numerics;
using RaytraceUWP;
using System;

namespace Raytrace.TestsUWP
{
    [TestClass]
    public class ChainedTransformationTest
    {
        Interpreter interp;

        [TestInitialize]
        public void Initialize()
        {
            interp = RaytraceInterpreter.MakeInterp();
            interp.Run("[ Raytrace.linear-algebra ] USE-MODULES");
        }

        [TestMethod]
        public void TestChain()
        {
            interp.Run(@"
            : p     1 0 1 POINT ;
            : A     PI 2 / ROTATION-X ;
            : B     5 5 5 SCALING ;
            : C     10 5 7 TRANSLATION ;
            : T     C B * A * ;
            : T2    [ A B C ] CHAIN ;
            : p2    A p * ;
            : p3    B p2 * ;
            : p4    C p3 * ;
            ");
            TestUtils.AssertStackTrue(interp, "p2      1 -1 0 POINT ~=");
            TestUtils.AssertStackTrue(interp, "p3      5 -5 0 POINT ~=");
            TestUtils.AssertStackTrue(interp, "p4      15 0 7 POINT ~=");
            TestUtils.AssertStackTrue(interp, "T  p *  15 0 7 POINT ~=");
            TestUtils.AssertStackTrue(interp, "T2 p *  15 0 7 POINT ~=");
        }
    }
}
