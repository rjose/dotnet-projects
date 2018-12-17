using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rino.Forthic;

namespace Raytrace.TestsUWP
{
    public class TestUtils
    {
        public static void AssertStackTrue(Interpreter interp, string forth)
        {
            interp.Run(forth);
            dynamic res = interp.StackPop();
            Assert.IsTrue(res.BoolValue);
        }

    }
}
