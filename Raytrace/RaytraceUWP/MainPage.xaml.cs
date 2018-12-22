using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Rino.Forthic;
using Windows.UI;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RaytraceUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            //ch1Example();
            ch2Example();
            Debug.WriteLine("Done!");
        }

        void ch2Example()
        {
            Interpreter interp = RaytraceInterpreter.MakeInterp();
            double pos(string axis)
            {
                interp.Run(String.Format("{0}-POS", axis));
                DoubleItem item = (DoubleItem)interp.StackPop();
                return item.DoubleValue;
            }

            interp.RegisterModule(new Ch1Module());
            interp.Run(@"
            [ Raytrace.Ch1 Raytrace.canvas ] USE-MODULES
            [ '_canvas' '_color' ] VARIABLES
             900 550 CANVAS _canvas !
             0 0 1 COLOR _color !
            : CHART   _canvas @ ;
            ");

            interp.Run("0 1 0 POINT POSITION!");
            interp.Run("1 1.8 0 VECTOR NORMALIZE 11.25 *  VELOCITY!");
            double y = pos("Y");
            int numIterations = 0;
            while (y > 0)
            {
                interp.Run("TICK");
                interp.Run("CHART ( X-POS 900 >CLAMPED-INT ) ( Y-POS 550 >CLAMPED-INT 550 FLIP-Y ) _color @ WRITE-PIXEL");
                y = pos("Y");
                numIterations++;
            }
            double x = pos("X");
            interp.Run("CHART >PPM");
            StringItem ppm = (StringItem)interp.StackPop();
            System.IO.File.WriteAllText(@"C:\Users\Public\Test\ch2.ppm", ppm.StringValue);
            Debug.WriteLine("distance: {0}, numIterations: {1}", x, numIterations);
        }

        void ch1Example()
        {
            Interpreter interp = RaytraceInterpreter.MakeInterp();
            interp.RegisterModule(new Ch1Module());
            interp.Run("[ Raytrace.Ch1 ] USE-MODULES");

            double pos(string axis)
            {
                interp.Run(String.Format("{0}-POS", axis));
                DoubleItem item = (DoubleItem)interp.StackPop();
                return item.DoubleValue;
            }

            interp.Run("0 2 0 POINT POSITION!");
            double y = pos("Y");
            int numIterations = 0;
            while (y > 0)
            {
                interp.Run("TICK");
                y = pos("Y");
                numIterations++;
            }
            double x = pos("X");
            Debug.WriteLine("distance: {0}, numIterations: {1}", x, numIterations);

        }

        private void Canvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            args.DrawingSession.DrawText("Hello, World!", 100, 100, Colors.Black);
            //Color[] colors = { ColorHelper.FromArgb(0, 0, 0, 0) };
            //CanvasBitmap.CreateFromColors(sender, colors, 10, 20);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            this.canvas.RemoveFromVisualTree();
            this.canvas = null;
        }
    }
}
