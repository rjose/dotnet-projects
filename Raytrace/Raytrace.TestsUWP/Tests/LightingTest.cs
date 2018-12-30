using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rino.Forthic;
using RaytraceUWP;

namespace Raytrace.TestsUWP
{
    [TestClass]
    public class LightingTest
    {
        Interpreter interp;

        [TestInitialize]
        public void Initialize()
        {
            interp = RaytraceInterpreter.MakeInterp();
            interp.Run(@"
            [ canvas linear-algebra intersection shader ] USE-MODULES
            [ 'm' 'position' 'eyev' 'normalv' 'light' 'in_shadow' ] VARIABLES
            Material      m !
            0 0 0 Point   position !
            false in_shadow !
            : M   m @ ;
            : L   light @ ;
            : P   position @ ;
            : E   eyev @ ;
            : N   normalv @ ;
            : S   in_shadow @ ;
            ");
        }

        [TestMethod]
        public void TestEyeBetweenLightAndSurface()
        {
            // Normal is normalized
            interp.Run(@"
            0 0 -1 Vector   eyev !
            0 0 -1 Vector   normalv !
            0 0 -10 Point  1 1 1 Color  PointLight   light !
            ");
            TestUtils.AssertStackTrue(interp, "M L P E N S  LIGHTING  1.9 1.9 1.9 Color ~=");
        }

        [TestMethod]
        public void TestLightWithSurfaceInShadow()
        {
            // Normal is normalized
            interp.Run(@"
            0 0 -1 Vector   eyev !
            0 0 -1 Vector   normalv !
            0 0 -10 Point  1 1 1 Color  PointLight   light !
            true   in_shadow !
            ");
            interp.Run("M L P E N S  LIGHTING");
            TestUtils.AssertStackTrue(interp, "M L P E N S  LIGHTING  0.1 0.1 0.1 Color ~=");
        }

        [TestMethod]
        public void TestEyeBetweenLightAndSurfaceAt45deg()
        {
            // Normal is normalized
            interp.Run(@"
            : a   2 SQRT 2 / ;
            : -a   a NEGATE ;
            0 a -a Vector   eyev !
            0 0 -1 Vector   normalv !
            0 0 -10 Point  1 1 1 Color  PointLight   light !
            ");
            TestUtils.AssertStackTrue(interp, "M L P E N S  LIGHTING  1.0 1.0 1.0 Color ~=");
        }

        [TestMethod]
        public void TestEyeOppositeSurfaceLightAt45deg()
        {
            // Normal is normalized
            interp.Run(@"
            0 0 -1 Vector   eyev !
            0 0 -1 Vector   normalv !
            0 10 -10 Point  1 1 1 Color  PointLight   light !
            ");
            TestUtils.AssertStackTrue(interp, "M L P E N S  LIGHTING  0.7364 DUP DUP  Color ~=");
        }

        [TestMethod]
        public void TestEyeInReflectionVectorPath()
        {
            // Normal is normalized
            interp.Run(@"
            : a   2 SQRT 2 / ;
            : -a   a NEGATE ;
            0 -a -a Vector   eyev !
            0 0 -1 Vector   normalv !
            0 10 -10 Point  1 1 1 Color  PointLight   light !
            ");
            TestUtils.AssertStackTrue(interp, "M L P E N S  LIGHTING  1.6364 DUP DUP  Color ~=");
        }

        [TestMethod]
        public void TestEyeWithLightBehindSurface()
        {
            // Normal is normalized
            interp.Run(@"
            0 0 -1 Vector   eyev !
            0 0 -1 Vector   normalv !
            0 0 10 Point  1 1 1 Color  PointLight   light !
            ");
            TestUtils.AssertStackTrue(interp, "M L P E N S  LIGHTING  0.1 DUP DUP  Color ~=");
        }

    }
}
