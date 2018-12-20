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
            AddWord(new HadamardMultiplyWord("H*"));
            AddWord(new CanvasWord("CANVAS"));
            AddWord(new WidthWord("WIDTH"));
            AddWord(new HeightWord("HEIGHT"));
            AddWord(new PixelAtWord("PIXEL-AT"));
            AddWord(new WritePixelWord("WRITE-PIXEL"));

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

    class CanvasWord : Word
    {
        public CanvasWord(string name) : base(name) { }

        // ( w h -- Canvas )
        public override void Execute(Interpreter interp)
        {
            IntItem h = (IntItem)interp.StackPop();
            IntItem w = (IntItem)interp.StackPop();
            interp.StackPush(new CanvasItem(w.IntValue, h.IntValue));
        }
    }

    class WidthWord : Word
    {
        public WidthWord(string name) : base(name) { }

        // ( Canvas -- width )
        public override void Execute(Interpreter interp)
        {
            CanvasItem canvas = (CanvasItem)interp.StackPop();
            interp.StackPush(new IntItem(canvas.Width));
        }
    }

    class HeightWord : Word
    {
        public HeightWord(string name) : base(name) { }

        // ( Canvas -- height )
        public override void Execute(Interpreter interp)
        {
            CanvasItem canvas = (CanvasItem)interp.StackPop();
            interp.StackPush(new IntItem(canvas.Height));
        }
    }

    class PixelAtWord : Word
    {
        public PixelAtWord(string name) : base(name) { }

        // ( Canvas x y -- color )
        public override void Execute(Interpreter interp)
        {
            IntItem y = (IntItem)interp.StackPop();
            IntItem x = (IntItem)interp.StackPop();
            CanvasItem canvas = (CanvasItem)interp.StackPop();

            interp.StackPush(new Vector4Item(canvas.PixelAt(x.IntValue, y.IntValue)));
        }
    }

    class WritePixelWord : Word
    {
        public WritePixelWord(string name) : base(name) { }

        // ( Canvas x y color -- )
        public override void Execute(Interpreter interp)
        {
            Vector4Item color = (Vector4Item)interp.StackPop();
            IntItem y = (IntItem)interp.StackPop();
            IntItem x = (IntItem)interp.StackPop();
            CanvasItem canvas = (CanvasItem)interp.StackPop();
            canvas.WritePixel(x.IntValue, y.IntValue, color.Vector4Value);
        }
    }
}
