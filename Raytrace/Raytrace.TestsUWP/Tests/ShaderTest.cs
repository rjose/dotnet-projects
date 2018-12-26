using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rino.Forthic;
using RaytraceUWP;

namespace Raytrace.TestsUWP
{
    [TestClass]
    public class ShaderTest
    {
        Interpreter interp;

        [TestInitialize]
        public void Initialize()
        {
            interp = RaytraceInterpreter.MakeInterp();
            interp.Run("[ linear-algebra intersection shader ] USE-MODULES");
        }

        [TestMethod]
        public void TestSphereNormal()
        {
            // x-axis
            TestUtils.AssertStackTrue(interp, "Sphere 1 0 0 Point NORMAL-AT  1 0 0 Vector ==");

            // y-axis
            TestUtils.AssertStackTrue(interp, "Sphere 0 1 0 Point NORMAL-AT  0 1 0 Vector ==");

            // z-axis
            TestUtils.AssertStackTrue(interp, "Sphere 0 0 1 Point NORMAL-AT  0 0 1 Vector ==");

            // Non-axial point
            interp.Run(": a   3.0 SQRT 3.0 / ;");
            TestUtils.AssertStackTrue(interp, "Sphere a a a Point NORMAL-AT  a a a Vector ~=");

            // Normal is normalized
            interp.Run(": n   Sphere a a a Point NORMAL-AT ;");
            TestUtils.AssertStackTrue(interp, "n n NORMALIZE ~=");
        }

        [TestMethod]
        public void TestTranslatedSphereNormal()
        {
            interp.Run(@"
            [ 's' 'n' ] VARIABLES
            Sphere s !
            s @  0 1 0 TRANSLATION TRANSFORM!
            s @  0 1.70711 -0.70711 Point NORMAL-AT n !
            ");
            TestUtils.AssertStackTrue(interp, "n @  0 0.70711 -0.70711 Vector ~=");
        }

        [TestMethod]
        public void TestTransformedSphereNormal()
        {
            interp.Run(@"
            [ 's' 'n' ] VARIABLES
            Sphere s !
            s @  [ PI 5 / ROTATION-Z  1 0.5 1 SCALING ] CHAIN  TRANSFORM!
            s @  0 ( 2 SQRT 2 / ) ( 2 SQRT -2 / )  Point NORMAL-AT n !
            ");
            TestUtils.AssertStackTrue(interp, "n @  0 0.97014 -0.24254 Vector ~=");
        }

    }
}
