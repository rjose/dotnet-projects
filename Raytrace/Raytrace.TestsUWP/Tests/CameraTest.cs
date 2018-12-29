using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rino.Forthic;
using RaytraceUWP;

namespace Raytrace.TestsUWP
{
    [TestClass]
    public class CameraTest
    {
        Interpreter interp;

        [TestInitialize]
        public void Initialize()
        {
            interp = RaytraceInterpreter.MakeInterp();
            interp.Run(@"
            [ linear-algebra scene ] USE-MODULES
            ");
        }

        [TestMethod]
        public void TestCamera()
        {
            interp.Run(@"
            [ 'hsize' 'vsize' 'field_of_view' 'camera' ] VARIABLES
            160   hsize !
            120   vsize !
            PI 2 /   field_of_view !
            hsize @ vsize @ field_of_view @  Camera   camera !
            ");
            TestUtils.AssertStackTrue(interp, "camera @ 'hsize' REC@  160 ==");
            TestUtils.AssertStackTrue(interp, "camera @ 'vsize' REC@  120 ==");
            TestUtils.AssertStackTrue(interp, "camera @ 'field_of_view' REC@  PI 2 / ~=");
            TestUtils.AssertStackTrue(interp, "camera @ 'transform' REC@  IDENTITY ==");
        }

        [TestMethod]
        public void TestPixelSizeForHorizontalCanvas()
        {
            interp.Run(@"
            [ 'camera' ] VARIABLES
            200 125 PI 2 /  Camera   camera !
            ");
            TestUtils.AssertStackTrue(interp, "camera @ 'pixel_size' REC@  0.01 ~=");
        }

        [TestMethod]
        public void TestPixelSizeForVerticalCanvas()
        {
            interp.Run(@"
            [ 'camera' ] VARIABLES
            125 200 PI 2 /  Camera   camera !
            ");
            TestUtils.AssertStackTrue(interp, "camera @ 'pixel_size' REC@  0.01 ~=");
        }

        [TestMethod]
        public void TestRayThroughCenterOfCanvas()
        {
            interp.Run(@"
            [ 'c' 'r' ] VARIABLES
            201 101 PI 2 /  Camera     c !
            c @ 100 50 RAY-FOR-PIXEL   r !
            ");
            interp.Run("r @");
            TestUtils.AssertStackTrue(interp, "r @ 'origin'    REC@  0 0  0 Point ~=");
            TestUtils.AssertStackTrue(interp, "r @ 'direction' REC@  0 0 -1 Vector ~=");
        }

        [TestMethod]
        public void TestRayThroughCornerOfCanvas()
        {
            interp.Run(@"
            [ 'c' 'r' 'dir' ] VARIABLES
            201 101 PI 2 /  Camera     c !
            c @ 0 0 RAY-FOR-PIXEL      r !
            0.66519 0.33259 -0.66851 Vector   dir !
            ");
            TestUtils.AssertStackTrue(interp, "r @ 'origin'    REC@  0 0  0 Point ~=");
            TestUtils.AssertStackTrue(interp, "r @ 'direction' REC@   dir @ ~=");
        }

        [TestMethod]
        public void TestRayWhenCameraIsTransformed()
        {
            interp.Run(@"
            [ 'c' 'r' 'dir' ] VARIABLES
            201 101 PI 2 /  Camera     c !
            c @  ( PI 4 / ROTATION-Y  0 -2 5 TRANSLATION * )  'transform' REC!
            c @ 100 50 RAY-FOR-PIXEL   r !
            ( 2 SQRT 2 / )  0  ( 2 SQRT -2 / ) Vector   dir !
            ");
            TestUtils.AssertStackTrue(interp, "r @ 'origin'    REC@  0 2 -5 Point ~=");
            TestUtils.AssertStackTrue(interp, "r @ 'direction' REC@   dir @ ~=");
        }


    }
}
