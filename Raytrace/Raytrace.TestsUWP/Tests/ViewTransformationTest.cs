using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rino.Forthic;
using RaytraceUWP;

namespace Raytrace.TestsUWP
{
    [TestClass]
    public class ViewTransformationTest
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
        public void TestDefaultOrientation()
        {
            interp.Run(@"
            [ 'from' 'to' 'up' 't' ]   VARIABLES
            0 0  0 Point   from !
            0 0 -1 Point   to !
            0 1  0 Vector  up !
            from @  to @  up @  VIEW-TRANSFORM   t !
            ");
            TestUtils.AssertStackTrue(interp, "t @  IDENTITY   ==");
        }

        [TestMethod]
        public void TestLookInPositiveZDirection()
        {
            interp.Run(@"
            [ 'from' 'to' 'up' 't' ]   VARIABLES
            0 0 0 Point   from !
            0 0 1 Point   to !
            0 1 0 Vector  up !
            from @  to @  up @  VIEW-TRANSFORM   t !
            ");
            TestUtils.AssertStackTrue(interp, "t @  -1 1 -1 SCALING   ==");
        }

        [TestMethod]
        public void TestViewTransformMovesWorld()
        {
            interp.Run(@"
            [ 'from' 'to' 'up' 't' ]   VARIABLES
            0 0 8 Point   from !
            0 0 0 Point   to !
            0 1 0 Vector  up !
            from @  to @  up @  VIEW-TRANSFORM   t !
            ");
            TestUtils.AssertStackTrue(interp, "t @  0 0 -8 TRANSLATION   ==");
        }

        [TestMethod]
        public void TestArbitraryViewTransformation()
        {
            interp.Run(@"
            [ 'from' 'to' 'up' 't' 'res' ]   VARIABLES
            1  3 2 Point   from !
            4 -2 8 Point   to !
            1  1 0 Vector  up !
            from @  to @  up @  VIEW-TRANSFORM   t !

            [ -0.50709 0.50709  0.67612 -2.36643
               0.76772 0.60609  0.12122 -2.82843
              -0.35857 0.59761 -0.71714  0.00000
               0.00000 0.00000  0.00000  1.00000 ]  Matrix   res !
            ");
            interp.Run("t @");
            TestUtils.AssertStackTrue(interp, "t @   res @  ~=");
        }
    }
}
