using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rino.Forthic;
using RaytraceUWP;

namespace Raytrace.TestsUWP
{
    [TestClass]
    public class HitTest
    {
        Interpreter interp;

        [TestInitialize]
        public void Initialize()
        {
            interp = RaytraceInterpreter.MakeInterp();
            interp.Run("[ linear-algebra intersection ] USE-MODULES");
        }

        [TestMethod]
        public void TestAllPositive()
        {
            interp.Run(@"
            [ 's' 'i1' 'i2' ] VARIABLES
            Sphere s !
            1 s Intersection i1 !
            2 s Intersection i2 !
            ");
            TestUtils.AssertStackTrue(interp, "[ i1 @  i2 @ ] HIT  i1 @ ==");
        }

        [TestMethod]
        public void TestSomeNegative()
        {
            interp.Run(@"
            [ 's' 'i1' 'i2' ] VARIABLES
            Sphere s !
            -1 s Intersection i1 !
            2 s Intersection i2 !
            ");
            TestUtils.AssertStackTrue(interp, "[ i1 @  i2 @ ] HIT  i2 @ ==");
        }

        [TestMethod]
        public void TestAllNegative()
        {
            interp.Run(@"
            [ 's' 'i1' 'i2' ] VARIABLES
            Sphere s !
            -1 s Intersection i1 !
            -2 s Intersection i2 !
            ");
            TestUtils.AssertStackTrue(interp, "[ i1 @  i2 @ ] HIT  NULL ==");
        }

        [TestMethod]
        public void TestUnordered()
        {
            interp.Run(@"
            [ 's' 'i1' 'i2' 'i3' 'i4' ] VARIABLES
            Sphere s !
            5 s Intersection i1 !
            7 s Intersection i2 !
            -3 s Intersection i3 !
            2 s Intersection i4 !
            ");
            TestUtils.AssertStackTrue(interp, "[ i1 @  i2 @ i3 @ i4 @ ] HIT  i4 @ ==");
        }

    }
}
