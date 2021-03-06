﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rino.Forthic;
using System.Numerics;
using RaytraceUWP;
using System;

namespace Raytrace.TestsUWP
{
    [TestClass]
    public class ShearingTest
    {
        Interpreter interp;

        [TestInitialize]
        public void Initialize()
        {
            interp = RaytraceInterpreter.MakeInterp();
            interp.Run("[ linear-algebra ] USE-MODULES");
        }

        [TestMethod]
        public void TestShearXWithY()
        {
            interp.Run(@"
            : p     2 3 4 Point ;
            : t     1 0 0 0 0 0 SHEARING ;
            : ans   5 3 4 Point ;
            ");
            TestUtils.AssertStackTrue(interp, "t  p *  ans ~=");
        }

        [TestMethod]
        public void TestShearXWithZ()
        {
            interp.Run(@"
            : p     2 3 4 Point ;
            : t     0 1 0 0 0 0 SHEARING ;
            : ans   6 3 4 Point ;
            ");
            TestUtils.AssertStackTrue(interp, "t  p *  ans ~=");
        }

        [TestMethod]
        public void TestShearYWithX()
        {
            interp.Run(@"
            : p     2 3 4 Point ;
            : t     0 0 1 0 0 0 SHEARING ;
            : ans   2 5 4 Point ;
            ");
            TestUtils.AssertStackTrue(interp, "t  p *  ans ~=");
        }

        [TestMethod]
        public void TestShearYWithZ()
        {
            interp.Run(@"
            : p     2 3 4 Point ;
            : t     0 0 0 1 0 0 SHEARING ;
            : ans   2 7 4 Point ;
            ");
            TestUtils.AssertStackTrue(interp, "t  p *  ans ~=");
        }

        [TestMethod]
        public void TestShearZWithX()
        {
            interp.Run(@"
            : p     2 3 4 Point ;
            : t     0 0 0 0 1 0 SHEARING ;
            : ans   2 3 6 Point ;
            ");
            TestUtils.AssertStackTrue(interp, "t  p *  ans ~=");
        }

        [TestMethod]
        public void TestShearZWithY()
        {
            interp.Run(@"
            : p     2 3 4 Point ;
            : t     0 0 0 0 0 1 SHEARING ;
            : ans   2 3 7 Point ;
            ");
            TestUtils.AssertStackTrue(interp, "t  p *  ans ~=");
        }

    }
}
