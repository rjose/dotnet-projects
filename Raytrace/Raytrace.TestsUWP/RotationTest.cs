using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rino.Forthic;
using System.Numerics;
using RaytraceUWP;

namespace Raytrace.TestsUWP
{
    [TestClass]
    public class RotationTest
    {
        Interpreter interp;

        [TestInitialize]
        public void Initialize()
        {
            interp = RaytraceInterpreter.MakeInterp();
            interp.Run("[ Raytrace.linear-algebra ] USE-MODULES");
        }

        [TestMethod]
        public void TestRotateAroundXAxis()
        {
            interp.Run(@"
            : p             0 1 0 POINT ;
            : 1/2-quarter   PI 4.0 /  ROTATION-X ;
            : full-quarter  PI 2.0 /  ROTATION-X ;
            : ans1          0 ( 2 SQRT 2 / ) DUP POINT ;
            : ans2          0 0 1 POINT ;
            ");
            interp.Run("1/2-quarter  p MATRIX-MUL");
            TestUtils.AssertStackTrue(interp, "1/2-quarter  p MATRIX-MUL  ans1 ~=");
            TestUtils.AssertStackTrue(interp, "full-quarter p MATRIX-MUL  ans2 ~=");

            // Inverse reverses rotation
            interp.Run(@"
            : ans3          0 ( 2 SQRT 2 / ) ( 2 SQRT -2 / ) POINT ;
            ");
            TestUtils.AssertStackTrue(interp, "1/2-quarter INVERSE  p MATRIX-MUL  ans3 ~=");
        }


    }
}
