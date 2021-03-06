﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rino.Forthic;
using RaytraceUWP;

namespace Raytrace.TestsUWP
{
    [TestClass]
    public class TransformRayTest
    {
        Interpreter interp;

        [TestInitialize]
        public void Initialize()
        {
            interp = RaytraceInterpreter.MakeInterp();
            interp.Run("[ linear-algebra intersection ] USE-MODULES");
        }

        [TestMethod]
        public void TestTranslateRay()
        {
            interp.Run(@"
            [ 'r' 'm' 'r2' ] VARIABLES
            1 2 3 Point  0 1 0 Vector  Ray r !
            3 4 5 TRANSLATION m !
            r @  m @  TRANSFORM  r2 !
            ");
            TestUtils.AssertStackTrue(interp, "r2 @ 'origin' REC@     4 6 8 Point ~=");
            TestUtils.AssertStackTrue(interp, "r2 @ 'direction' REC@  0 1 0 Vector ~=");
        }

        [TestMethod]
        public void TestScaleRay()
        {
            interp.Run(@"
            [ 'r' 'm' 'r2' ] VARIABLES
            1 2 3 Point  0 1 0 Vector  Ray r !
            2 3 4 SCALING m !
            r @  m @  TRANSFORM  r2 !
            ");
            TestUtils.AssertStackTrue(interp, "r2 @ 'origin' REC@     2 6 12 Point ~=");
            TestUtils.AssertStackTrue(interp, "r2 @ 'direction' REC@  0 3 0 Vector ~=");
        }

        [TestMethod]
        public void TestSphereTransform()
        {
            interp.Run(@"
            [ 's' ] VARIABLES
            Sphere s !
            ");
            TestUtils.AssertStackTrue(interp, "s @ 'transform' REC@  IDENTITY ==");
        }

        [TestMethod]
        public void TestChangeSphereTransform()
        {
            interp.Run(@"
            [ 's' 't' ] VARIABLES
            Sphere s !
            2 3 4 TRANSLATION t !
            s @  t @ 'transform' REC!
            ");
            TestUtils.AssertStackTrue(interp, "s @ 'transform' REC@  t @ ==");
        }

        [TestMethod]
        public void TestIntersectWithScaledSphere()
        {
            interp.Run(@"
            [ 'r' 's' ] VARIABLES
            0 0 -5 Point  0 0 1 Vector  Ray r !
            Sphere s !
            s @  2 2 2 SCALING  'transform' REC!
            : xs   s @  r @  INTERSECTS ;
            ");
            interp.Run("xs");
            TestUtils.AssertStackTrue(interp, "xs LENGTH  2 ==");
            TestUtils.AssertStackTrue(interp, "xs 0 NTH 't' REC@  3.0 ~=");
            TestUtils.AssertStackTrue(interp, "xs 1 NTH 't' REC@  7.0 ~=");
        }

        [TestMethod]
        public void TestIntersectWithTranslatedSphere()
        {
            interp.Run(@"
            [ 'r' 's' ] VARIABLES
            0 0 -5 Point  0 0 1 Vector  Ray r !
            Sphere s !
            s @  5 0 0 TRANSLATION  'transform' REC!
            : xs   s @  r @  INTERSECTS ;
            ");
            interp.Run("xs");
            TestUtils.AssertStackTrue(interp, "xs LENGTH  0 ==");
        }
    }
}
