using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Rino.Forthic;

namespace Raytrace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Interpreter interp = RaytraceInterpreter.MakeInterp();
            ch1ProjectileSimulation();
            InitializeComponent();
        }

        void ch1ProjectileSimulation()
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
            System.Console.WriteLine("distance: {0}, numIterations: {1}", x, numIterations);

        }
    }
}
