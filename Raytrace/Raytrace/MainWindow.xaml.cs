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
            interp.Run(@"
            [ Raytrace.linear-algebra ] USE-MODULES
            2 4 6 POINT
            3 6 9 VECTOR
            ");
            InitializeComponent();
        }
    }
}
