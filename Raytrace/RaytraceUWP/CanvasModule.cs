using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rino.Forthic;
using System.Numerics;

namespace RaytraceUWP
{
    public class CanvasModule : Module
    {
        public CanvasModule() : base("Raytrace.canvas")
        {
            AddWord(new XWord("R"));
            AddWord(new YWord("G"));
            AddWord(new ZWord("B"));

            this.Code = @"
            [ Raytrace.linear-algebra ] USE-MODULES

            : COLOR   POINT ; # ( r, g, b -- Vector4 )
            ";
        }
    }


    // -------------------------------------------------------------------------
    // Words

}
