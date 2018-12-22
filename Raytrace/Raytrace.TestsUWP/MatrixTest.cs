using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rino.Forthic;
using System.Numerics;
using RaytraceUWP;


namespace Raytrace.TestsUWP
{
    [TestClass]
    public class MatrixTest
    {
        Interpreter interp;

        [TestInitialize]
        public void Initialize()
        {
            interp = RaytraceInterpreter.MakeInterp();
            interp.Run("[ Raytrace.linear-algebra ] USE-MODULES");
        }

        [TestMethod]
        public void TestMatrixCreation()
        {
            interp.Run(@"
            [ 1    2    3    4
            5.5  6.5  7.5  8.5
            9    10   11   12
            13.5 14.5 15.5 16.5 ] MATRIX");
            Assert.AreEqual(1, interp.stack.Count);
            TestUtils.AssertStackTrue(interp, "DUP 0 0 M  1.0  ==");
            TestUtils.AssertStackTrue(interp, "DUP 0 3 M  4.0  ==");
            TestUtils.AssertStackTrue(interp, "DUP 1 0 M  5.5  ==");
            TestUtils.AssertStackTrue(interp, "DUP 1 2 M  7.5  ==");
            TestUtils.AssertStackTrue(interp, "DUP 2 2 M  11.0  ==");
            TestUtils.AssertStackTrue(interp, "DUP 3 0 M  13.5  ==");
            TestUtils.AssertStackTrue(interp, "    3 2 M  15.5  ==");
        }

        [TestMethod]
        public void TestEquality()
        {
            interp.Run(@"
            [ 'A' 'B1' 'B2' ] VARIABLES

            [  1  2  3  4
               5  6  7  8
               9  8  7  6
               5  4  3  2 ] MATRIX A !

            [  1  2  3  4
               5  6  7  8
               9  8  7  6
               5  4  3  2 ] MATRIX B1 !

            [  2  3  4  5
               6  7  8  9
               8  7  6  5
               4  3  2  1 ] MATRIX B2 !
            ");
            TestUtils.AssertStackTrue(interp, "A @  B1 @ ==");
            TestUtils.AssertStackTrue(interp, "A @  B2 @ ==  false ==");
        }

        [TestMethod]
        public void TestMultiplyByTuple()
        {
            interp.Run(@"
            : A   [  1  2  3  4
                     2  4  4  2
                     8  6  4  1
                     0  0  0  1 ] MATRIX ;
            : b   1 2 3 1 TUPLE ;
            : RES 18 24 33 1 TUPLE ;
            ");
            TestUtils.AssertStackTrue(interp, "A b MATRIX-MUL  RES ==");
        }

        [TestMethod]
        public void TestMultiplication()
        {
            interp.Run(@"
            : A   [  1  2  3  4
                     5  6  7  8
                     9  8  7  6
                     5  4  3  2 ] MATRIX ;
            : B   [ -2  1  2  3
                     3  2  1 -1
                     4  3  6  5
                     1  2  7  8 ] MATRIX ;
            : RES [ 20 22 50 48
                    44 54 114 108
                    40 58 110 102
                    16 26 46  42 ] MATRIX ;
            ");
            TestUtils.AssertStackTrue(interp, "A B MATRIX-MUL  RES ==");
        }

        [TestMethod]
        public void TestIdentity()
        {
            interp.Run(@"
            : A   [  0  1  2  4
                     1  2  4  8
                     2  4  8 16
                     4  8 16 32 ] MATRIX ;
            ");
            TestUtils.AssertStackTrue(interp, "A IDENTITY MATRIX-MUL  A ==");
            TestUtils.AssertStackTrue(interp, "IDENTITY A MATRIX-MUL  A ==");

            interp.Run(": a   1 2 3 4 TUPLE ;");
            TestUtils.AssertStackTrue(interp, "IDENTITY a MATRIX-MUL  a ==");
        }

        [TestMethod]
        public void TestTranspose()
        {
            interp.Run(@"
            : A   [  0  9  3  0
                     9  8  0  8
                     1  8  5  3
                     0  0  5  8 ] MATRIX ;
            : A_t [  0  9  1  0
                     9  8  8  0
                     3  0  5  5
                     0  8  3  8 ] MATRIX ;
            ");
            TestUtils.AssertStackTrue(interp, "A TRANSPOSE A_t ==");
            TestUtils.AssertStackTrue(interp, "IDENTITY TRANSPOSE  IDENTITY ==");
        }
    }
}
