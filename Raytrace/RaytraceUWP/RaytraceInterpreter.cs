using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rino.Forthic;

namespace RaytraceUWP
{
    public class RaytraceInterpreter
    {
        public static Interpreter MakeInterp()
        {
            Interpreter result = new Interpreter();
            result.RegisterModule(new LinearAlgebraModule("Raytrace.linear-algebra"));
            result.RegisterModule(new CanvasModule("Raytrace.canvas"));
            result.RegisterModule(new IntersectionModule("Raytrace.intersection"));
            return result;
        }
    }
}
