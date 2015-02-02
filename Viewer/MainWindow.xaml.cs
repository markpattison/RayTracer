using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Viewer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DateTime start = DateTime.Now;

            Scene.Scene scene = Scene.ReferenceScene;
            BitmapSource rt = RayTrace.Render(scene, 1000, 1000);
            image1.Source = rt;

            DateTime end = DateTime.Now;
            TimeSpan duration = end - start;
            textBox1.Text = rt.Width.ToString() + "x" + rt.Height.ToString() + " image took " + (duration.TotalMilliseconds / 1000.0).ToString("F2") + "s";
        }
    }
}

