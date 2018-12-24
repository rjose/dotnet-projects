using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Rino.Forthic;
using System.Numerics;
using RaytraceUWP;

namespace Raytrace.TestsUWP
{
    [TestClass]
    public class RayTest
    {
        Interpreter interp;

        [TestInitialize]
        public void Initialize()
        {
            interp = RaytraceInterpreter.MakeInterp();
            interp.Run("[ linear-algebra intersection ] USE-MODULES");
        }

        [TestMethod]
        public void TestCreateRay()
        {
            interp.Run(@"
            : origin     1 2 3 Point ;
            : direction  4 5 6 Vector ;
            : ray        origin direction Ray ;
            ");
            TestUtils.AssertStackTrue(interp, "ray ORIGIN  origin ==");
            TestUtils.AssertStackTrue(interp, "ray DIRECTION  direction ==");
        }

        [TestMethod]
        public void TestPosition()
        {
            interp.Run(@"
            : ray   2 3 4 Point  1 0 0 Vector Ray ;
            ");
            TestUtils.AssertStackTrue(interp, "ray 0 POSITION  2 3 4 Point  ==");
            TestUtils.AssertStackTrue(interp, "ray 1 POSITION  3 3 4 Point  ==");
            TestUtils.AssertStackTrue(interp, "ray -1 POSITION  1 3 4 Point  ==");
            TestUtils.AssertStackTrue(interp, "ray 2.5 POSITION  4.5 3 4 Point  ==");

        }
    }
}
