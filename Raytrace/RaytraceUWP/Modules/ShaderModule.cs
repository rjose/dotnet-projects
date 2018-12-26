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
            AddWord(new ReflectWord("REFLECT"));
            AddWord(new LightWord("Light"));
            AddWord(new IntensityAtWord("INTENSITY@"));
            AddWord(new PositionAtWord("POSITION@"));

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

    class ReflectWord : Word
    {
        public ReflectWord(string name) : base(name) { }

        // ( in_vec normal -- vector )
        public override void Execute(Interpreter interp)
        {
            dynamic normal = interp.StackPop();
            dynamic in_vec = interp.StackPop();
            interp.StackPush(in_vec);
            interp.StackPush(normal);
            interp.StackPush(in_vec);
            interp.StackPush(normal);
            interp.Run("DOT 2 * * -");
        }
    }

    class LightWord : Word
    {
        public LightWord(string name) : base(name) { }

        // ( pos intensity -- vector )
        public override void Execute(Interpreter interp)
        {
            dynamic intensity = interp.StackPop();
            dynamic pos = interp.StackPop();
            interp.StackPush(new LightItem(pos.Vector4Value, intensity.Vector4Value));
        }
    }

    class IntensityAtWord : Word
    {
        public IntensityAtWord(string name) : base(name) { }

        // ( light -- color )
        public override void Execute(Interpreter interp)
        {
            dynamic light = interp.StackPop();
            interp.StackPush(new Vector4Item(light.Intensity));
        }
    }

    class PositionAtWord : Word
    {
        public PositionAtWord(string name) : base(name) { }

        // ( light -- pos )
        public override void Execute(Interpreter interp)
        {
            dynamic light = interp.StackPop();
            interp.StackPush(new Vector4Item(light.Position));
        }
    }
}
