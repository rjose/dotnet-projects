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
            AddWord(new PointLightWord("PointLight"));
            AddWord(new MaterialWord("Material"));
            AddWord(new LightingWord("LIGHTING"));

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
            interp.Run("'transform' REC@ INVERSE");
            interp.StackPush(p);
            interp.Run("*");

            // ( object_point -- object_normal )
            interp.Run("0 0 0 Point -");

            // ( object_normal -- world_normal )
            interp.StackPush(s);
            interp.Run("'transform' REC@ INVERSE TRANSPOSE SWAP *  0 'W' <REC! NORMALIZE");
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

    class PointLightWord : Word
    {
        public PointLightWord(string name) : base(name) { }

        // ( pos intensity -- PointLight )
        public override void Execute(Interpreter interp)
        {
            dynamic intensity = interp.StackPop();
            dynamic pos = interp.StackPop();
            interp.StackPush(new PointLightItem(pos.Vector4Value, intensity.Vector4Value));
        }
    }

    class MaterialWord : Word
    {
        public MaterialWord(string name) : base(name) { }

        // ( -- Material )
        public override void Execute(Interpreter interp)
        {
            interp.StackPush(new MaterialItem());
        }
    }

    class LightingWord : Word
    {
        public LightingWord(string name) : base(name) { }

        // ( material light point eyev normalv -- lighting )
        public override void Execute(Interpreter interp)
        {
            dynamic normalv = interp.StackPop();
            dynamic eyev = interp.StackPop();
            dynamic point = interp.StackPop();
            dynamic light = interp.StackPop();
            dynamic material = interp.StackPop();

            // Effective color
            interp.StackPush(material);
            interp.StackPush(light);
            interp.Run("'intensity' REC@  SWAP 'color' REC@  *");
            dynamic effective_color = interp.StackPop();

            // lightv
            interp.StackPush(point);
            interp.StackPush(light);
            interp.Run("'position' REC@ SWAP  - NORMALIZE");
            dynamic lightv = interp.StackPop();

            // Ambient contribution
            interp.StackPush(effective_color);
            interp.StackPush(material);
            interp.Run("'ambient' REC@ *");
            dynamic ambient = interp.StackPop();

            // light_dot_normal
            interp.StackPush(lightv);
            interp.StackPush(normalv);
            interp.Run("DOT");
            dynamic light_dot_normal = interp.StackPop();

            // black
            interp.Run("0 0 0 Color");
            Vector4Item black = (Vector4Item)interp.StackPop();

            Vector4Item diffuse = black;
            Vector4Item specular = black;

            if (light_dot_normal.FloatValue > 0)
            {
                // Compute diffuse
                interp.StackPush(effective_color);
                interp.StackPush(light_dot_normal);
                interp.StackPush(material);
                interp.Run("'diffuse' REC@ * *");
                diffuse = (Vector4Item)interp.StackPop();

                // reflect_dot_eye
                interp.StackPush(eyev);
                interp.StackPush(normalv);
                interp.StackPush(lightv);
                interp.Run("NEGATE SWAP REFLECT DOT");
                dynamic reflect_dot_eye = interp.StackPop();

                if (reflect_dot_eye.FloatValue <= 0)
                {
                    specular = black;
                }
                else
                {
                    // Compute specular contribution
                    interp.StackPush(reflect_dot_eye);
                    interp.StackPush(material);
                    interp.Run("'shininess' REC@ POW");  // factor
                    interp.StackPush(light);
                    interp.StackPush(material);
                    interp.Run("'specular' REC@  SWAP 'intensity' REC@ * *");
                    specular = (Vector4Item)interp.StackPop();
                }
            }

            // Compute result
            interp.StackPush(ambient);
            interp.StackPush(diffuse);
            interp.StackPush(specular);
            interp.Run("+ +");
        }
    }
}
