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
            result.RegisterModule(new LinearAlgebraModule("linear-algebra"));
            result.RegisterModule(new CanvasModule("canvas"));
            result.RegisterModule(new IntersectionModule("intersection"));
            result.RegisterModule(new ShaderModule("shader"));
            result.RegisterModule(new SceneModule("scene"));
            return result;
        }
    }
}
