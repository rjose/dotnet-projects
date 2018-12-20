﻿using System;
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
            AddWord(new HadamardMultiplyWord("H*"));

            this.Code = @"
            [ Raytrace.linear-algebra ] USE-MODULES

            : COLOR   VECTOR ; # ( r, g, b -- Vector4 )
            ";
        }
    }


    // -------------------------------------------------------------------------
    // Words

    class HadamardMultiplyWord : Word
    {
        public HadamardMultiplyWord(string name) : base(name) { }

        // ( v1 v2 -- v )
        public override void Execute(Interpreter interp)
        {
            Vector4Item v2 = (Vector4Item)interp.StackPop();
            Vector4Item v1 = (Vector4Item)interp.StackPop();
            Vector4Item result = new Vector4Item(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z, v1.W * v2.W);
            interp.StackPush(result);
        }
    }

}