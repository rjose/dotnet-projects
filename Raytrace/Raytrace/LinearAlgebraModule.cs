using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rino.Forthic;

namespace Raytrace
{
    class LinearAlgebraModule : Module
    {
        public LinearAlgebraModule() : base("Raytrace.linear-algebra")
        {
            AddWord(new Vector4Word("VECTOR4"));
            this.Code = @"
            : POINT    1 VECTOR4 ;   # ( x y z -- Vector4 )
            : VECTOR   0 VECTOR4 ;   # ( x y z -- Vector4 )
            ";
        }
    }


    // -------------------------------------------------------------------------
    // Words
 
    class Vector4Word : Word
    {
        public Vector4Word(string name) : base(name) { }

        // ( x y z w -- Vector4Item )
        public override void Execute(Interpreter interp)
        {
            dynamic wItem = interp.StackPop();
            dynamic zItem = interp.StackPop();
            dynamic yItem = interp.StackPop();
            dynamic xItem = interp.StackPop();
            interp.StackPush(new Vector4Item(xItem.FloatValue, yItem.FloatValue, zItem.FloatValue, wItem.FloatValue));
        }
    }
}
