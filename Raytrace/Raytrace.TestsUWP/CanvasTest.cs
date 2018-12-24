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
            interp.Run("-0.5 0.4 1.7 Color");
            Assert.AreEqual(1, interp.stack.Count);
            Vector4Item v = (Vector4Item)interp.stack.Peek();
            TupleTest.AssertVector4Equal(-0.5f, 0.4f, 1.7f, 0.0f, v.Vector4Value);
            TestUtils.AssertStackTrue(interp, "DUP R  -0.5  ==");
            TestUtils.AssertStackTrue(interp, "DUP G   0.4  ==");
            TestUtils.AssertStackTrue(interp, "    B   1.7  ==");
        }

        [TestMethod]
        public void TestColorOperations()
        {
            interp.Run(@"
            : c1   0.9 0.6 0.75 Color ;
            : c2   0.7 0.1 0.25 Color ;
            c1 c2 +
            ");
            TestUtils.AssertStackTrue(interp, "1.6 0.7 1.0 Color  ~=");

            interp.Run(@"
            c1 c2 -
            ");
            TestUtils.AssertStackTrue(interp, "0.2 0.5 0.5 Color  ~=");

            interp.Run(@"
            : c   0.2 0.3 0.4 Color ;
            c 2.0 *
            ");
            TestUtils.AssertStackTrue(interp, "0.4 0.6 0.8 Color  ~=");
        }

        [TestMethod]
        public void TestHadamardMultiplication()
        {
            interp.Run(@"
            : c1   1.0 0.2 0.4 Color ;
            : c2   0.9 1.0 0.1 Color ;
            c1 c2 *
            ");
            TestUtils.AssertStackTrue(interp, "0.9 0.2 0.04 Color  ~=");
        }

        [TestMethod]
        public void TestCanvas()
        {
            interp.Run(@"
            [ 'canvas' 'red' ] VARIABLES
            10 20 Canvas canvas !
            1 0 0 Color red !
            ");
            TestUtils.AssertStackTrue(interp, "canvas @  WIDTH 10  ==");
            TestUtils.AssertStackTrue(interp, "canvas @  HEIGHT 20  ==");
            TestUtils.AssertStackTrue(interp, "canvas @  2 3 PIXEL-AT  0 0 0 Color ==");

            interp.Run("canvas @ 2 3 red @ WRITE-PIXEL");
            TestUtils.AssertStackTrue(interp, "canvas @  2 3 PIXEL-AT  1 0 0 Color ==");
        }

        [TestMethod]
        public void TestToPPMHeader()
        {
            interp.Run("5 3 Canvas >PPM");
            StringItem ppm = (StringItem)interp.StackPop();
            String[] lines = ppm.StringValue.Split("\n");
            Assert.AreEqual("P3", lines[0]);
            Assert.AreEqual("5 3", lines[1]);
            Assert.AreEqual("255", lines[2]);
        }

        [TestMethod]
        public void TestToPPM()
        {
            interp.Run(@"
            : c1   1.5 0 0 Color ;
            : c2   0 0.5 0 Color ;
            : c3   -0.5 0 1 Color ;

            'canvas' VARIABLE
            5 3 Canvas canvas !

            canvas @ 0 0 c1 WRITE-PIXEL
            canvas @ 2 1 c2 WRITE-PIXEL
            canvas @ 4 2 c3 WRITE-PIXEL

            canvas @ >PPM
            ");
            StringItem ppm = (StringItem)interp.StackPop();
            String[] lines = ppm.StringValue.Split("\n");
            Assert.AreEqual("255 0 0 0 0 0 0 0 0 0 0 0 0 0 0", lines[3]);
            Assert.AreEqual("0 0 0 0 0 0 0 128 0 0 0 0 0 0 0", lines[4]);
            Assert.AreEqual("0 0 0 0 0 0 0 0 0 0 0 0 0 0 255", lines[5]);
        }

        [TestMethod]
        public void TestToPPMLong()
        {
            interp.Run(@"
            : c1   1 0.8 0.6 Color ;
            'canvas' VARIABLE
            10 2 Canvas canvas !

            canvas @ c1 CLEAR-PIXELS
            canvas @ >PPM
            ");
            StringItem ppm = (StringItem)interp.StackPop();
            String[] lines = ppm.StringValue.Split("\n");
            Assert.AreEqual("255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204", lines[3]);
            Assert.AreEqual("153 255 204 153 255 204 153 255 204 153 255 204 153", lines[4]);
            Assert.AreEqual("255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204", lines[5]);
            Assert.AreEqual("153 255 204 153 255 204 153 255 204 153 255 204 153", lines[6]);
            Assert.AreEqual('\n', ppm.StringValue[ppm.StringValue.Length - 1]);
        }
    }
}