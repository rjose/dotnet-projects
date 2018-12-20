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

        public void ClearPixels(Vector4 color)
        {
            for (var i=0; i < Width; i++)
            {
                for (var j=0; j < Height; j++)
                {
                    pixels[i, j] = color;
                }
            }
        }

        int clampValue(float value)
        {
            int result = 0;
            if (value < 0.0)
            {
                result = 0;
            }
            else if (value >= 1.0)
            {
                result = 255;
            }
            else
            {
                result = (int)Math.Round(value * 255);
            }
            return result;
        }

        public string ToPPM()
        {
            StringBuilder builder = new StringBuilder();

            // Add header
            builder.Append("P3\n");
            builder.AppendFormat("{0} {1}\n", Width, Height);
            builder.Append("255\n");

            // Add pixels
            for (var j=0; j < Height; j++)
            {
                StringBuilder rowString = new StringBuilder(70);
                for (var i = 0; i < Width; i++)
                {
                    Vector4 color = pixels[i, j];
                    builder.AppendFormat("{0} ", clampValue(color.X));
                    builder.AppendFormat("{0} ", clampValue(color.Y));
                    builder.AppendFormat("{0} ", clampValue(color.Z));
                }
                builder.Length--;
                builder.Append("\n");
            }
            return builder.ToString();
        }
    }
}
