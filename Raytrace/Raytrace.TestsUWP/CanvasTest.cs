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
    public class CanvasTest
    {
        Interpreter interp;

        [TestInitialize]
        public void Initialize()
        {
            interp = RaytraceInterpreter.MakeInterp();
            interp.Run("[ Raytrace.canvas ] USE-MODULES");
        }

        [TestMethod]
        public void TestColor()
        {
            interp.Run("-0.5 0.4 1.7 COLOR");
            Assert.AreEqual(1, interp.stack.Count);
            Vector4Item v = (Vector4Item)interp.stack.Peek();
            TupleTest.AssertVector4Equal(-0.5f, 0.4f, 1.7f, 1.0f, v.Vector4Value);
            TestUtils.AssertStackTrue(interp, "DUP R  -0.5  ==");
            TestUtils.AssertStackTrue(interp, "DUP G   0.4  ==");
            TestUtils.AssertStackTrue(interp, "    B   1.7  ==");
        }

        [TestMethod]
        public void TestColorOperations()
        {
            interp.Run(@"
            : c1   0.9 0.6 0.75 COLOR ;
            : c2   0.7 0.1 0.25 COLOR ;
            c1 c2 +
            ");
            TestUtils.AssertStackTrue(interp, "1.6 0.7 1.0 COLOR  ~=");

            interp.Run(@"
            c1 c2 -
            ");
            TestUtils.AssertStackTrue(interp, "0.2 0.5 0.5 COLOR  ~=");

            interp.Run(@"
            : c   0.2 0.3 0.4 COLOR ;
            c 2.0 *
            ");
            TestUtils.AssertStackTrue(interp, "0.4 0.6 0.8 COLOR  ~=");
        }

        [TestMethod]
        public void TestHadamardMultiplication()
        {
            interp.Run(@"
            : c1   1.0 0.2 0.4 COLOR ;
            : c2   0.9 1.0 0.1 COLOR ;
            c1 c2 H*
            ");
            TestUtils.AssertStackTrue(interp, "0.9 0.2 0.04 COLOR  ~=");
        }

        [TestMethod]
        public void TestCanvas()
        {
            interp.Run(@"
            [ 'canvas' 'red' ] VARIABLES
            10 20 CANVAS canvas !
            1 0 0 COLOR red !
            ");
            TestUtils.AssertStackTrue(interp, "canvas @  WIDTH 10  ==");
            TestUtils.AssertStackTrue(interp, "canvas @  HEIGHT 20  ==");
            TestUtils.AssertStackTrue(interp, "canvas @  2 3 PIXEL-AT  0 0 0 COLOR ==");

            interp.Run("canvas @ 2 3 red @ WRITE-PIXEL");
            TestUtils.AssertStackTrue(interp, "canvas @  2 3 PIXEL-AT  1 0 0 COLOR ==");
        }

    }
}