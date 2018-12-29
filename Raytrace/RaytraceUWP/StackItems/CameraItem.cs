using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Rino.Forthic;

namespace RaytraceUWP
{
    public class CameraItem : StackItem
    {
        public ScalarItem Hsize { get; set; }
        public ScalarItem Vsize { get; set; }
        public ScalarItem FieldOfView { get; set; }
        public MatrixItem Transform { get; set; }
        public DoubleItem HalfWidth { get; set; }
        public DoubleItem HalfHeight { get; set; }
        public DoubleItem PixelSize { get; set; }

        public CameraItem(ScalarItem hsize, ScalarItem vsize, ScalarItem field_of_view)
        {
            Hsize = hsize;
            Vsize = vsize;
            FieldOfView = field_of_view;
            Transform = new MatrixItem();
            compute_parameters();
        }

        void compute_parameters()
        {
            double half_view = Math.Tan(FieldOfView.DoubleValue / 2.0f);
            double aspect = Hsize.DoubleValue / Vsize.DoubleValue;
            double half_width, half_height;
            if (aspect >= 1)
            {
                half_width = half_view;
                half_height = half_view / aspect;
            }
            else
            {
                half_height = half_view;
                half_width = half_view * aspect;
            }

            // Set params
            HalfWidth = new DoubleItem(half_width);
            HalfHeight = new DoubleItem(half_height);
            PixelSize = new DoubleItem(half_width*2.0f / Hsize.DoubleValue);
        }

        override public void SetValue(string key, StackItem value)
        {
            if      (key == "hsize")         Hsize = (ScalarItem)value;
            else if (key == "vsize")         Vsize = (ScalarItem)value;
            else if (key == "field_of_view") FieldOfView = (ScalarItem)value;
            else if (key == "transform")     Transform = (MatrixItem)value;
            else throw new InvalidOperationException(String.Format("{0}::SetValue Unknown key: {1}", this, key));
        }

        override public StackItem GetValue(string key)
        {
            if      (key == "hsize")         return Hsize;
            else if (key == "vsize")         return Vsize;
            else if (key == "field_of_view") return FieldOfView;
            else if (key == "transform")     return Transform;
            else if (key == "half_width")    return HalfWidth;
            else if (key == "half_height")   return HalfHeight;
            else if (key == "pixel_size")    return PixelSize;
            else throw new InvalidOperationException(String.Format("{0}::GetValue Unknown key: {1}", this, key));
        }
    }
}
