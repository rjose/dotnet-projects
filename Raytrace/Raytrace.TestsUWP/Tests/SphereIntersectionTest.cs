using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rino.Forthic;
using RaytraceUWP;

namespace Raytrace.TestsUWP
{
    [TestClass]
    public class SphereIntersectionTest
    {
        Interpreter interp;

        [TestInitialize]
        public void Initialize()
        {
            interp = RaytraceInterpreter.MakeInterp();
            interp.Run("[ linear-algebra intersection ] USE-MODULES");
        }

        [TestMethod]
        public void TestIntersectionAtTwoPoints()
        {
            interp.Run(@"
            : ray      0 0 -5 Point  0 0 1 Vector  Ray ;
            : sphere   Sphere ;
            : xs       sphere ray INTERSECTS ;
            ");
            interp.Run("xs");
            TestUtils.AssertStackTrue(interp, "xs LENGTH  2 ==");
            TestUtils.AssertStackTrue(interp, "xs 0 NTH 't' REC@ 4.0 ~=");
            TestUtils.AssertStackTrue(interp, "xs 1 NTH 't' REC@ 6.0 ~=");
        }

        [TestMethod]
        public void TestIntersectionAtTangent()
        {
            interp.Run(@"
            : ray      0 1 -5 Point  0 0 1 Vector  Ray ;
            : sphere   Sphere ;
            : xs       sphere ray INTERSECTS ;
            ");
            TestUtils.AssertStackTrue(interp, "xs LENGTH  2 ==");
            TestUtils.AssertStackTrue(interp, "xs 0 NTH 't' REC@ 5.0 ~=");
            TestUtils.AssertStackTrue(interp, "xs 1 NTH 't' REC@ 5.0 ~=");
        }

        [TestMethod]
        public void TestIntersectionAtNoPoints()
        {
            // Unit sphere intersect with Ray at no points
            interp.Run(@"
            : ray      0 2 -5 Point  0 0 1 Vector  Ray ;
            : sphere   Sphere ;
            : xs       sphere ray INTERSECTS ;
            ");
            TestUtils.AssertStackTrue(interp, "xs LENGTH  0 ==");
        }

        [TestMethod]
        public void TestIntersectionInsideSphere()
        {
            interp.Run(@"
            : ray      0 0 0 Point  0 0 1 Vector  Ray ;
            : sphere   Sphere ;
            : xs       sphere ray INTERSECTS ;
            ");
            TestUtils.AssertStackTrue(interp, "xs LENGTH  2 ==");
            TestUtils.AssertStackTrue(interp, "xs 0 NTH 't' REC@ -1.0 ~=");
            TestUtils.AssertStackTrue(interp, "xs 1 NTH 't' REC@  1.0 ~=");
        }

        [TestMethod]
        public void TestIntersectionBehindSphere()
        {
            interp.Run(@"
            : ray      0 0 5 Point  0 0 1 Vector  Ray ;
            : sphere   Sphere ;
            : xs       sphere ray INTERSECTS ;
            ");
            TestUtils.AssertStackTrue(interp, "xs LENGTH  2 ==");
            TestUtils.AssertStackTrue(interp, "xs 0 NTH 't' REC@ -6.0 ~=");
            TestUtils.AssertStackTrue(interp, "xs 1 NTH 't' REC@ -4.0 ~=");
        }

        [TestMethod]
        public void TestIntersectionTracksSphere()
        {
            interp.Run(@"
            : ray      0 0 -5 Point  0 0 1 Vector  Ray ;
            's' VARIABLE
            Sphere s !
            : sphere   s @ ;
            : xs       sphere ray INTERSECTS ;
            ");
            TestUtils.AssertStackTrue(interp, "xs LENGTH  2 ==");
            TestUtils.AssertStackTrue(interp, "xs 0 NTH 'obj' REC@ sphere ==");
            TestUtils.AssertStackTrue(interp, "xs 1 NTH 'obj' REC@ sphere ==");
        }
    }
}
