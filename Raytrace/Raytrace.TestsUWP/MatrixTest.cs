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
            TestUtils.AssertStackTrue(interp, "A b *  RES ==");
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
            TestUtils.AssertStackTrue(interp, "A B *  RES ==");
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
            TestUtils.AssertStackTrue(interp, "A IDENTITY *  A ==");
            TestUtils.AssertStackTrue(interp, "IDENTITY A *  A ==");

            interp.Run(": a   1 2 3 4 TUPLE ;");
            TestUtils.AssertStackTrue(interp, "IDENTITY a *  a ==");
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

        [TestMethod]
        public void TestDeterminant()
        {
            interp.Run(@"
            : A   [  -2  -8   3   5
                     -3   1   7   3
                      1   2  -9   6
                     -6   7   7  -9 ] MATRIX ;
            ");
            TestUtils.AssertStackTrue(interp, "A DETERMINANT  -4071  ==");
        }

        [TestMethod]
        public void TestInvertible()
        {
            interp.Run(@"
            : A   [   6   4   4   4
                      5   5   7   6
                      4  -9   3  -7
                      9   1   7  -6 ] MATRIX ;
            ");
            TestUtils.AssertStackTrue(interp, "A INVERTIBLE?");

            interp.Run(@"
            : B   [  -4   2  -2  -3
                      9   6   2   6
                      0  -5   1  -5
                      0   0   0   0 ] MATRIX ;
            ");
            TestUtils.AssertStackTrue(interp, "B INVERTIBLE? NOT");
        }

        [TestMethod]
        public void TestInverse()
        {
            interp.Run(@"
            : A   [  -5   2   6  -8
                      1  -5   1   8
                      7   7  -6  -7
                      1  -3   7   4 ] MATRIX ;
            : B   [  0.21805  0.45113  0.24060  -0.04511
                    -0.80827 -1.45677 -0.44361   0.52068
                    -0.07895 -0.22368 -0.05263   0.19737
                    -0.52256 -0.81391 -0.30075   0.30639 ] MATRIX ;
            ");
            TestUtils.AssertStackTrue(interp, "A INVERSE B ~=");

            interp.Run(@"
            : A   [ 8 -5  9  2
                    7  5  6  1
                   -6  0  9  6
                   -3  0 -9 -4 ] MATRIX ;
            : B   [  -0.15385 -0.15385 -0.28205 -0.53846 
                     -0.07692  0.12308  0.02564  0.03077 
                      0.35897  0.35897  0.43590  0.92308 
                     -0.69231 -0.69231 -0.76923 -1.92308 ] MATRIX ;
            ");
            TestUtils.AssertStackTrue(interp, "A INVERSE B ~=");

            interp.Run(@"
            : A   [ 9  3  0  9 
                   -5 -2 -6 -3 
                   -4  9  6  4 
                   -7  6  6  2 ] MATRIX ;
            : B   [  -0.04074 -0.07778  0.14444 -0.22222 
                     -0.07778  0.03333  0.36667 -0.33333 
                     -0.02901 -0.14630 -0.10926  0.12963 
                      0.17778  0.06667 -0.26667  0.33333 ] MATRIX ;
            ");
            TestUtils.AssertStackTrue(interp, "A INVERSE B ~=");
        }

        [TestMethod]
        public void TestInverse2()
        {
            interp.Run(@"
            : A   [ 3 -9  7  3 
                    3 -8  2 -9 
                   -4  4  4  1 
                   -6  5 -1  1  ] MATRIX ;

            : B   [  8  2  2  2 
                     3 -1  7  0 
                     7  0  5  4 
                     6 -2  0  5  ] MATRIX ;
            : C   A B * ;
            ");
            TestUtils.AssertStackTrue(interp, "C B INVERSE *  A ~=");
        }
    }
}
