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
        public CanvasModule(string name) : base(name)
        {
            AddWord(new CanvasWord("Canvas"));
            AddWord(new PixelAtWord("PIXEL-AT"));
            AddWord(new WritePixelWord("WRITE-PIXEL"));
            AddWord(new ToPPMWord(">PPM"));
            AddWord(new ClearPixelsWord("CLEAR-PIXELS"));

            this.Code = @"
            [ linear-algebra ] USE-MODULES

            : Color   Vector ; # ( r, g, b -- Vector4 )
            ";
        }
    }


    // -------------------------------------------------------------------------
    // Words


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
            dynamic y = interp.StackPop();
            dynamic x = interp.StackPop();
            CanvasItem canvas = (CanvasItem)interp.StackPop();
            canvas.WritePixel(x.IntValue, y.IntValue, color.Vector4Value);
        }
    }

    class ToPPMWord : Word
    {
        public ToPPMWord(string name) : base(name) { }

        // ( Canvas -- PPM )
        public override void Execute(Interpreter interp)
        {
            CanvasItem canvas = (CanvasItem)interp.StackPop();
            interp.StackPush(new StringItem(canvas.ToPPM()));
        }
    }

    class ClearPixelsWord : Word
    {
        public ClearPixelsWord(string name) : base(name) { }

        // ( Canvas color -- )
        public override void Execute(Interpreter interp)
        {
            Vector4Item color = (Vector4Item)interp.StackPop();
            CanvasItem canvas = (CanvasItem)interp.StackPop();
            canvas.ClearPixels(color.Vector4Value);
        }
    }
}
