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

        [TestMethod]
        public void TestPrecomputation()
        {
            interp.Run(@"
            [ 'comps' 'intersection' 'ray' ] VARIABLES
            0 0 -5 Point  0 0 1 Vector Ray   ray !
            4 Sphere Intersection   intersection !
            intersection @  ray @  PREPARE-COMPUTATIONS   comps !
            ");
            TestUtils.AssertStackTrue(interp, "comps @ 't' REC@  4 ==");
            TestUtils.AssertStackTrue(interp, "comps @ 'obj' REC@   intersection @ 'obj' REC@ ==");
            TestUtils.AssertStackTrue(interp, "comps @ 'point' REC@  0 0 -1 Point ~=");
            TestUtils.AssertStackTrue(interp, "comps @ 'eyev' REC@  0 0 -1 Vector ~=");
            TestUtils.AssertStackTrue(interp, "comps @ 'normalv' REC@  0 0 -1 Vector ~=");
        }

        [TestMethod]
        public void TestPrecomputationOutsideIntersection()
        {
            interp.Run(@"
            [ 'comps' 'intersection' 'ray' ] VARIABLES
            0 0 -5 Point  0 0 1 Vector Ray   ray !
            4 Sphere Intersection   intersection !
            intersection @  ray @  PREPARE-COMPUTATIONS   comps !
            ");
            TestUtils.AssertStackTrue(interp, "comps @ 'inside' REC@  false ==");
        }

        [TestMethod]
        public void TestPrecomputationInsideIntersection()
        {
            interp.Run(@"
            [ 'comps' 'intersection' 'ray' ] VARIABLES
            0 0 0 Point  0 0 1 Vector Ray   ray !
            1 Sphere Intersection   intersection !
            intersection @  ray @  PREPARE-COMPUTATIONS   comps !
            ");
            interp.Run("comps @");
            TestUtils.AssertStackTrue(interp, "comps @ 'inside' REC@  true ==");
            TestUtils.AssertStackTrue(interp, "comps @ 'point' REC@  0 0 1 Point ~=");
            TestUtils.AssertStackTrue(interp, "comps @ 'eyev' REC@  0 0 -1 Vector ~=");
            TestUtils.AssertStackTrue(interp, "comps @ 'normalv' REC@  0 0 -1 Vector ~=");
        }

        [TestMethod]
        public void TestShadeIntersection()
        {
            interp.Run(@"
            [ 'comps' 'intersection' 'ray' 'shape' 'c' ] VARIABLES
            0 0 -5 Point  0 0 1 Vector Ray                 ray !
            default_world @ 'objects' REC@ 0 NTH          shape !
            4  shape @  Intersection                      intersection !
            intersection @  ray @  PREPARE-COMPUTATIONS   comps !
            default_world @  comps @ SHADE-HIT            c !
            ");
            TestUtils.AssertStackTrue(interp, "c @  0.38066 0.47583 0.2855 Color   ~=");
        }

        [TestMethod]
        public void TestShadeIntersectionFromInside()
        {
            interp.Run(@"
            [ 'comps' 'intersection' 'ray' 'shape' 'c' ] VARIABLES
            default_world @  0 0.25 0 Point  1 1 1 Color  PointLight 'light' REC!
            0 0 0 Point  0 0 1 Vector Ray                 ray !
            default_world @ 'objects' REC@ 1 NTH          shape !
            0.5  shape @  Intersection                    intersection !
            intersection @  ray @  PREPARE-COMPUTATIONS   comps !
            default_world @  comps @ SHADE-HIT            c !
            ");
            TestUtils.AssertStackTrue(interp, "c @  0.90498 DUP DUP Color  ~=");
        }

        [TestMethod]
        public void TestColorAtMiss()
        {
            interp.Run(@"
            [ 'ray' ] VARIABLES
            0 0 -5 Point  0 1 0 Vector Ray   ray !
            ");
            TestUtils.AssertStackTrue(interp, "default_world @  ray @  COLOR-AT   0 0 0 Color  ~=");
        }

        [TestMethod]
        public void TestColorAtHit()
        {
            interp.Run(@"
            [ 'ray' ] VARIABLES
            0 0 -5 Point  0 0 1 Vector Ray   ray !
            ");
            TestUtils.AssertStackTrue(interp, "default_world @  ray @  COLOR-AT   0.38066 0.47583 0.2855 Color  ~=");
        }

        [TestMethod]
        public void TestColorWithIntersectionBehindRay()
        {
            interp.Run(@"
            [ 'ray' 'outer' 'inner' 'w' ] VARIABLES
            default_world @   w !
            w @ 'objects' REC@ 0 NTH   outer !
            w @ 'objects' REC@ 1 NTH   inner !

            outer @ 'material' REC@  1 'ambient' REC!
            inner @ 'material' REC@  1 'ambient' REC!
            0 0 0.75 Point  0 0 -1 Vector Ray   ray !
            ");
            TestUtils.AssertStackTrue(interp, "default_world @  ray @  COLOR-AT   inner @ 'material' REC@ 'color' REC@  ~=");
        }

        [TestMethod]
        public void TestRenderWithCamera()
        {
            interp.Run(@"
            [ 'w' 'c' 'from' 'to' 'up' 'image' ] VARIABLES
            default_world @       w !
            11 11 PI 2 / Camera   c !
            0 0 -5 Point          from !
            0 0 0  Point          to !
            0 1 0  Vector         up !
            c @  ( from @ to @ up @ VIEW-TRANSFORM ) 'transform' REC!
            c @  w @  RENDER   image !
            ");
            TestUtils.AssertStackTrue(interp, "image @ 5 5 PIXEL-AT  0.38066 0.47583 0.2855 Color ~=");
        }
    }
}
