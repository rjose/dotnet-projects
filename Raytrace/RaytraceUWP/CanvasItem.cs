using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Rino.Forthic;

namespace RaytraceUWP
{
    public class CanvasItem : StackItem
    {
        Vector4[,] pixels;

        public int Width
        {
            get { return pixels.GetLength(0);  }
        }

        public int Height
        {
            get { return pixels.GetLength(1); }
        }

        public CanvasItem(int width, int height)
        {
            pixels = new Vector4[width, height];
        }

        public Vector4 PixelAt(int x, int y)
        {
            return pixels[x, y];
        }

        public void WritePixel(int x, int y, Vector4 color)
        {
            pixels[x, y] = color;
        }

    }
}
