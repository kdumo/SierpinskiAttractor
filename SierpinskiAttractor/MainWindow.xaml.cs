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

namespace SierpinskiAttractor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private SOMETHING currColor;
        //private String currShape = "Circle"; 
        //default to circle
        private enum selectableShapes { Circle, Rectangle }
        private selectableShapes currShape = selectableShapes.Circle;//add more later maybe...or not
        private double mx, my;   //mouse x and y in canvas
        private double[] sx, sy;   //captured shape x and y
        private Shape[6] controlPoints;
        private int shapeIndex=0;
        private Shape selectedPoint;
        private bool captured = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Usage_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult usage = MessageBox.Show("");
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult about = MessageBox.Show("");
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DaCanvas_MouseMove_1(object sender, MouseEventArgs e)
        {
            mx = e.GetPosition(DaCanvas).X;
            my = e.GetPosition(DaCanvas).Y;
            
        }

        private void setControlPoint(object sender, MouseButtonEventArgs e)
        {
            Shape shape = null;
            //forr debug
            int size = 10;
            switch (currShape)
            {
                case selectableShapes.Circle:
                    shape = new Ellipse()
                    {
                        Height = size,
                        Width = size,
                        Fill = Brushes.Blue
                        //we will get values from buttons/boxes
                        //I fixed values for now
                    };break;
                case selectableShapes.Rectangle:
                    shape = new Rectangle(){
                        Height = size,
                        Width = size,
                        Fill = Brushes.Pink
                    };
                    break;
                default:
                    return;
            }
            Canvas.SetLeft(shape, e.GetPosition(DaCanvas).X);
            Canvas.SetTop(shape, e.GetPosition(DaCanvas).Y);
            DaCanvas.Children.Add(shape);
        }

        private void circle_Checked(object sender, RoutedEventArgs e)
        {   //how to keep people from choosing both more than one radio button at a time..??
            /*var button = sender as RadioButton;
            currShape = button.Content.ToString();*/
            currShape = selectableShapes.Circle;
        }

        private void square_Checked(object sender, RoutedEventArgs e)
        {
            /*var button = sender as RadioButton;
            currShape = button.Content.ToString();*/
            currShape = selectableShapes.Rectangle;
        }

        private void dragPoint(object sender, MouseButtonEventArgs e)
        {
            //drag point on click down
            try { selectedPoint = (Shape)e.Source; }
            catch (System.InvalidCastException ice)
            {
                return;
            }
            if (selectedPoint != null)
            {
                Mouse.Capture(selectedPoint);
                captured = true;

            }
        }

        private void releasePoint(object sender, MouseButtonEventArgs e)
        {
            //place point on click release
            //get mouse pos, place point on mouse pos (in canvas)

        }
    }
}
