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
    public class MatrixTransformationTest
    {
        Interpreter interp;

        [TestInitialize]
        public void Initialize()
        {
            interp = RaytraceInterpreter.MakeInterp();
            interp.Run("[ Raytrace.linear-algebra ] USE-MODULES");
        }

        [TestMethod]
        public void TestTranslation()
        {
            interp.Run(@"
            : p   -3 4 5 POINT ;
            : t    5 -3 2 TRANSLATION ;
            : ans  2 1 7 POINT ;
            ");
            TestUtils.AssertStackTrue(interp, "t p *  ans ==");

            // Invert translation
            interp.Run(@"
            : t_inv   t INVERSE ;
            : ans  -8 7 3 POINT ;
            ");
            TestUtils.AssertStackTrue(interp, "t_inv p *  ans ==");

            // Translation doesn't affect vectors
            interp.Run(@"
            : v   -3 4 5 VECTOR ;
            ");
            TestUtils.AssertStackTrue(interp, "t v *  v ==");
        }

        [TestMethod]
        public void TestScaling()
        {
            interp.Run(@"
            : p   -4 6 8 POINT ;
            : t    2 3 4 SCALING ;
            : ans  -8 18 32 POINT ;
            ");
            TestUtils.AssertStackTrue(interp, "t p *  ans ==");

            // Scaling affects vectors
            interp.Run(@"
            : v   -4 6 8 VECTOR ;
            : ans  -8 18 32 VECTOR ;
            ");
            TestUtils.AssertStackTrue(interp, "t v *  ans ==");

            // Inverse scaling
            interp.Run(@"
            : v   -4 6 8 VECTOR ;
            : t_inv   t INVERSE ;
            : ans  -2 2 2 VECTOR ;
            ");
            TestUtils.AssertStackTrue(interp, "t_inv v *  ans ==");

            // Reflection
            interp.Run(@"
            : p   2 3 4 POINT ;
            : t   -1 1 1 SCALING ;
            : ans  -2 3 4 POINT ;
            ");
            TestUtils.AssertStackTrue(interp, "t p *  ans ==");
        }

    }
}
