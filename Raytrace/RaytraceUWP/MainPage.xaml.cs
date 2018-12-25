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
using Windows.Storage;
using Windows.Storage.Pickers;

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
            //ch2Example();
            //ch3Example();
            ch4Example();
            Debug.WriteLine("Done!");
        }

        async void writeFile(string filename, string text)
        {
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(file, text);
        }

        void ch4Example()
        {
            Interpreter interp = RaytraceInterpreter.MakeInterp();
            interp.CurModule().AddWord(new WriteHitWord("WRITE-HIT"));
            interp.CurModule().AddWord(new ForWord("FOR"));

            interp.Run(@"
            [ canvas linear-algebra intersection ] USE-MODULES
            [ 'ray_origin' 'wall_z' 'wall_size' 'canvas_pixels' 'pixel_size' 'half' ] VARIABLES

            0 0 -5 Point   ray_origin !
            10             wall_z !
            7              wall_size !
            100            canvas_pixels !
            wall_size @  canvas_pixels @ /   pixel_size !
            wall_size @ 2.0 /   half !

            [ 'canvas' 'color' 'shape' ] VARIABLES
            canvas_pixels @ DUP Canvas   canvas !
            1 0 0 Color                  color !
            Sphere                       shape !

            # shape @  [ 0.1 0.5 1 SCALING  0.1 -0.2 -2 TRANSLATION ] CHAIN TRANSFORM!

            [ '_x' '_y' ] VARIABLES
            : WORLD-Y    half @  pixel_size @ _y @ * - ;
            : WORLD-X    pixel_size @ _x @ *  half @ - ;
            : POS        WORLD-X WORLD-Y wall_z @ Point ;
            : RAY        ray_origin @  DUP POS SWAP - NORMALIZE  Ray ;
            : XS         shape @ RAY INTERSECTS ;
            : HIT?       XS HIT  NULL == NOT ;
            
            [ canvas_pixels @ DUP ] '_y ! _x ! WRITE-HIT' FOR
            canvas @ >PPM
            ");
            dynamic ppmData = interp.StackPop();
            writeFile("ch4.ppm", ppmData.StringValue);
        }

        void ch3Example()
        {
            Interpreter interp = RaytraceInterpreter.MakeInterp();

            interp.Run(@"
            [ canvas linear-algebra ] USE-MODULES

            : OFFSETS         [ 0 1 2 3 4 5 6 7 8 9 10 11 ] ;
            : DELTA-ANGLE     2 PI * 12 / ;
            : p0              1 0 0 Point ;
            : Canvas-SIZE     500 ;
            : PointS          OFFSETS  'DELTA-ANGLE * ROTATION-Z  p0 *' MAP ;
            : scale           200 200 0 SCALING ;
            : translate       250 250 0 TRANSLATION ;
            : transform       [ scale translate ] CHAIN ;
            : Canvas-PointS   PointS 'transform SWAP *' MAP ;
            : white           1 1 1 Color ;

            'canvas' VARIABLE
            : CHART   canvas @ ;
            : XY      DUP X SWAP Y ;  # ( point -- x y )

            Canvas-SIZE DUP Canvas  canvas !
            Canvas-PointS 'CHART SWAP XY white WRITE-PIXEL' FOREACH
            CHART >PPM
            ");
            dynamic ppmData = interp.StackPop();
            writeFile("ch3.ppm", ppmData.StringValue);
            Debug.WriteLine("Howdy");
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
            [ Raytrace.Ch1 canvas ] USE-MODULES
            [ '_canvas' '_color' ] VARIABLES
             900 550 Canvas _canvas !
             0 0 1 Color _color !
            : CHART   _canvas @ ;
            ");

            interp.Run("0 1 0 Point POSITION!");
            interp.Run("1 1.8 0 Vector NORMALIZE 11.25 *  VELOCITY!");
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

            interp.Run("0 2 0 Point POSITION!");
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

    class WriteHitWord : Word
    {
        public WriteHitWord(string name) : base(name) { }

        // ( -- )
        public override void Execute(Interpreter interp)
        {
            interp.Run("HIT?");
            dynamic isHit = interp.StackPop();
            if (isHit.BoolValue)
            {
                interp.Run("canvas @ _x @ _y @ color @ WRITE-PIXEL");
            }
        }
    }

    class ForWord : Word
    {
        public ForWord(string name) : base(name) { }

        // ( array forthic -- ? )
        public override void Execute(Interpreter interp)
        {
            StringItem forthic = (StringItem)interp.StackPop();
            ArrayItem indices = (ArrayItem)interp.StackPop();

            if (indices.ArrayValue.Count == 2)
            {
                IntItem i_max = (IntItem)indices.ArrayValue[0];
                IntItem j_max = (IntItem)indices.ArrayValue[1];
                for (var i=0; i < i_max.IntValue; i++)
                {
                    for (var j=0; j < j_max.IntValue; j++)
                    {
                        interp.StackPush(new IntItem(i));
                        interp.StackPush(new IntItem(j));
                        interp.Run(forthic.StringValue);
                    }
                }
            }
        }
    }
}
