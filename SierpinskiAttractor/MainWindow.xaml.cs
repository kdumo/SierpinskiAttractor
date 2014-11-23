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
using System.Diagnostics;

namespace SierpinskiAttractor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private SOMETHING currColor;
        //private String shapeType = "Circle"; 
        //default to circle
        private enum selectableShapes { Circle, Rectangle }
        private selectableShapes shapeType = selectableShapes.Circle;//add more later maybe...or not
        private double mx, my;   //mouse x and y in canvas
        private double[] sx = new double[6]; 
        private double[] sy = new double[6];   //shape x and y
        private Shape[] controlPoints = new Shape[6];
        private int shapeIndex = 0;
        private int cpCounter = 0;
        private Shape selectedPoint = null;
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
            //sierpinskinate();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            cpCounter = 0;
            Indexshape.Content = "Control Points: " + cpCounter;
            sx = new double[6];
            sy = new double[6];
            controlPoints = new Shape[6];
            DaCanvas.Children.Clear();

        }

        private void DaCanvas_MouseMove_1(object sender, MouseEventArgs e)
        {
            double x = e.GetPosition(DaCanvas).X;
            double y = e.GetPosition(DaCanvas).Y;
            if (captured)
            {
                sx[shapeIndex] += x - mx;
                Canvas.SetLeft(selectedPoint, sx[shapeIndex]);
                mx = x;
                sy[shapeIndex] += y - my;
                Canvas.SetTop(selectedPoint,sy[shapeIndex]);
                my = y;
            }
            //else...nothing right??
            mousePos.Content = String.Format("Mouse at ({0},{1})", x, y);
            
        }

        private void setControlPoint(object sender, MouseButtonEventArgs e)
        {
            Shape newShape;
            //forr size debug, can add method for size and color
            int size = 10;
            if(cpCounter < 6){  //out of bounds stuff
                String index = "Shape"+cpCounter;
                switch (shapeType)
                {
                    case selectableShapes.Circle:
                        newShape = new Ellipse()
                        {
                            Name = index,
                            Height = size,
                            Width = size,
                            Fill = Brushes.Blue
                            //we will get values from buttons/boxes
                            //I fixed values for now
                        };break;
                    case selectableShapes.Rectangle:
                        newShape = new Rectangle(){
                            Name = index,
                            Height = size,
                            Width = size,
                            Fill = Brushes.Pink
                        };
                        break;
                    default:
                        return;
                }

                Canvas.SetLeft(newShape, e.GetPosition(DaCanvas).X);
                Canvas.SetTop(newShape, e.GetPosition(DaCanvas).Y);
                DaCanvas.Children.Add(newShape);

                //save for reference
                controlPoints[cpCounter] = newShape;
                sx[cpCounter] = Canvas.GetLeft(newShape);
                sy[cpCounter] = Canvas.GetTop(newShape);

                cpCounter++;
                Indexshape.Content = "Control Points: " + cpCounter;
            }
            else
                StatusLabel.Content = "Only 6 points Allowed!";
        }

        private void circle_Checked(object sender, RoutedEventArgs e)
        {   //how to keep people from choosing both more than one radio button at a time..??
            /*var button = sender as RadioButton;
            shapeType = button.Content.ToString();*/
            shapeType = selectableShapes.Circle;
        }

        private void square_Checked(object sender, RoutedEventArgs e)
        {
            /*var button = sender as RadioButton;
            shapeType = button.Content.ToString();*/
            shapeType = selectableShapes.Rectangle;
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
                Mouse.Capture(selectedPoint);  //take it
                captured = true;
                mx = e.GetPosition(DaCanvas).X;
                my = e.GetPosition(DaCanvas).Y;
                for (shapeIndex = 0; shapeIndex < cpCounter && selectedPoint.Name != controlPoints[shapeIndex].Name; shapeIndex++) ;
            }
        }

        private void releasePoint(object sender, MouseButtonEventArgs e)
        {
            //place point on click release
            //get mouse pos, place point on mouse pos (in canvas)
            if (captured)
            {
                Mouse.Capture(null); //take nothing (release)
                captured = false;
            }
            //clear canvas, redraw control points and rerun
            DaCanvas.Children.Clear();
            for (int i = 0; i < cpCounter; i++)
            {
                Canvas.SetLeft(controlPoints[i], sx[i]);
                Canvas.SetTop(controlPoints[i], sy[i]);
                DaCanvas.Children.Add(controlPoints[i]);
            }

        }

        public void sierpinskinate()
        {

        }
    }
}
