using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rino.Forthic;
using System.Numerics;
using RaytraceUWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            : p             0 1 0 Point ;
            : 1/2-quarter   PI 4.0 /  ROTATION-X ;
            : full-quarter  PI 2.0 /  ROTATION-X ;
            : ans1          0 ( 2 SQRT 2 / ) DUP Point ;
            : ans2          0 0 1 Point ;
            ");
            TestUtils.AssertStackTrue(interp, "1/2-quarter  p *  ans1 ~=");
            TestUtils.AssertStackTrue(interp, "full-quarter p *  ans2 ~=");

            // Inverse reverses rotation
            interp.Run(@"
            : ans3          0 ( 2 SQRT 2 / ) ( 2 SQRT -2 / ) Point ;
            ");
            TestUtils.AssertStackTrue(interp, "1/2-quarter INVERSE  p *  ans3 ~=");
        }


        [TestMethod]
        public void TestRotateAroundYAxis()
        {
            interp.Run(@"
            : p              0 0 1 Point ;
            : 1/2-quarter    PI 4.0 /  ROTATION-Y ;
            : full-quarter   PI 2.0 /  ROTATION-Y ;
            : root-2-over-2  2 SQRT 2 / ;
            : ans1           root-2-over-2 0 root-2-over-2 Point ; 
            : ans2           1 0 0 Point ;
            ");
            interp.Run("1/2-quarter  p *");
            TestUtils.AssertStackTrue(interp, "1/2-quarter  p *  ans1 ~=");
            TestUtils.AssertStackTrue(interp, "full-quarter p *  ans2 ~=");
        }


        [TestMethod]
        public void TestRotateAroundZAxis()
        {
            interp.Run(@"
            : p              0 1 0 Point ;
            : 1/2-quarter    PI 4.0 /  ROTATION-Z ;
            : full-quarter   PI 2.0 /  ROTATION-Z ;
            : root-2-over-2  2 SQRT 2 / ;
            : ans1           root-2-over-2 NEGATE  root-2-over-2 0 Point ; 
            : ans2           -1 0 0 Point ;
            ");
            interp.Run("1/2-quarter  p *");
            TestUtils.AssertStackTrue(interp, "1/2-quarter  p *  ans1 ~=");
            TestUtils.AssertStackTrue(interp, "full-quarter p *  ans2 ~=");
        }


    }
}
