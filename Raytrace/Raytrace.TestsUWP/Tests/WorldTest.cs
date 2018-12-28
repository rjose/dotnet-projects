using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rino.Forthic;
using RaytraceUWP;

namespace Raytrace.TestsUWP
{
    [TestClass]
    public class WorldTest
    {
        Interpreter interp;

        [TestInitialize]
        public void Initialize()
        {
            interp = RaytraceInterpreter.MakeInterp();
            interp.Run(@"
            [ canvas linear-algebra intersection shader scene ] USE-MODULES
            [ 's1' 's2' 'light' 'default_world' ] VARIABLES

            -10 10 -10 Point  1 1 1 Color  PointLight   light !
            Sphere   s1 !
            s1 @ 'material' REC@  0.8 1.0 0.6 Color  'color'   <REC!
                                  0.7                'diffuse' <REC!
                                  0.2                'specular' REC!
            Sphere   s2 !
            s2 @  0.5 0.5 0.5 SCALING 'transform' REC!

            World   default_world !
            default_world @  light @  'light' REC!
            default_world @  s1 @ ADD-OBJECT
            default_world @  s2 @ ADD-OBJECT
            ");
        }

        [TestMethod]
        public void TestWorld()
        {
            // Non-axial point
            interp.Run(@"
            [ 'w' ]   VARIABLES
            World   w !
            ");
            TestUtils.AssertStackTrue(interp, "w @ 'objects' REC@ LENGTH  0 ==");
            TestUtils.AssertStackTrue(interp, "w @ 'light' REC@  NULL ==");
        }

        [TestMethod]
        public void TestDefaultWorld()
        {
            // Non-axial point
            interp.Run(@"
            ");
            TestUtils.AssertStackTrue(interp, "default_world @ 'light' REC@  light @ ==");
            TestUtils.AssertStackTrue(interp, "default_world @  s1 @  CONTAINS");
            TestUtils.AssertStackTrue(interp, "default_world @  s2 @  CONTAINS");
        }

        [TestMethod]
        public void TestIntersectWorld()
        {
            interp.Run(@"
            [ 'xs' ] VARIABLES
            : RAY   0 0 -5 Point  0 0 1 Vector Ray ;
            default_world @  RAY  INTERSECT-WORLD  xs !
            ");
            TestUtils.AssertStackTrue(interp, "xs @ LENGTH  4 ==");
            TestUtils.AssertStackTrue(interp, "xs @ 0 NTH 't' REC@  4.0 ~=");
            TestUtils.AssertStackTrue(interp, "xs @ 1 NTH 't' REC@  4.5 ~=");
            TestUtils.AssertStackTrue(interp, "xs @ 2 NTH 't' REC@  5.5 ~=");
            TestUtils.AssertStackTrue(interp, "xs @ 3 NTH 't' REC@  6.0 ~=");
        }
    }
}
