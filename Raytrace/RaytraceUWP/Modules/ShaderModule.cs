using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rino.Forthic;
using System.Numerics;
using System.Diagnostics;

namespace RaytraceUWP
{
    public class ShaderModule : Module
    {
        public ShaderModule(string name) : base(name)
        {
            AddWord(new NormalAtWord("NORMAL-AT"));

            this.Code = @"
            ";

        }
    }

    class NormalAtWord : Word
    {
        public NormalAtWord(string name) : base(name) { }

        // ( shape point -- vector )
        public override void Execute(Interpreter interp)
        {
            dynamic point = interp.StackPop();
            dynamic shape = interp.StackPop();
            normal_at(interp, shape, point);
        }

        void normal_at(Interpreter interp, SphereItem s, Vector4Item p)
        {
            // ( -- object_point )
            interp.StackPush(s);
            interp.Run("TRANSFORM@ INVERSE");
            interp.StackPush(p);
            interp.Run("*");

            // ( object_point -- object_normal )
            interp.Run("0 0 0 Point -");

            // ( object_normal -- world_normal )
            interp.StackPush(s);
            interp.Run("TRANSFORM@ INVERSE TRANSPOSE SWAP *  0 <W! NORMALIZE");
        }
    }

}
