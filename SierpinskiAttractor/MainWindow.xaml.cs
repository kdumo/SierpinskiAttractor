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
        private double mx, my;   //mouse x and y in canvas

        private double[] sx = new double[6]; 
        private double[] sy = new double[6];   //shape x and y
        private Shape[] controlPoints = new Shape[6];
        private int shapeIndex = 0;
        private int cpCounter = 0;

        private Shape selectedPoint = null;
        private bool captured = false, loadDone = false;
        private SolidColorBrush color;
        private int size = 10; 
        
        Random seed = new Random();
        //SAShape[] pattern = new SAShape[2000];

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
            if (cpCounter < 3)
            {
                StatusLabel.Content = "Not Enough Control Points";
                StatusLabel.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                StatusLabel.Visibility = System.Windows.Visibility.Hidden;
                sierpinskinate();
            }
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
            //forr size debug
            if (cpCounter < 6)
            {
                String index = "Shape" + cpCounter;
                newShape = new Ellipse()
                {
                    Name = index,
                    Height = size,
                    Width = size,
                    Fill = color
                };

                Canvas.SetLeft(newShape, e.GetPosition(DaCanvas).X);
                Canvas.SetTop(newShape, e.GetPosition(DaCanvas).Y);
                DaCanvas.Children.Add(newShape);

                //save for reference
                controlPoints[cpCounter] = newShape;
                sx[cpCounter] = Canvas.GetLeft(newShape);
                sy[cpCounter] = Canvas.GetTop(newShape);

                cpCounter++;
                Indexshape.Content = "Control Points: " + cpCounter;

                defaultValues();
            }

            if (cpCounter >= 6)
            {
                Indexshape.Content = "Control Points: 6 (Max Reached)";
                pointLabel.Content = "Select Point to Edit";
                disableOptions();
            }
            else
            {
                enableOptions();
            }
        }

        private void dragPoint(object sender, MouseButtonEventArgs e)
        {
            //drag point on click down
            try { selectedPoint = (Shape)e.Source; }
            catch (System.InvalidCastException ice)
            {
                selectedPoint = null;
                defaultValues();
                if (cpCounter >= 6)
                {
                    pointLabel.Content = "Select Point to Edit";
                    disableOptions();
                }
                else
                {
                    pointLabel.Content = "Add New Point";
                    enableOptions();
                }
                return;
            }

            if (selectedPoint != null)
            {
                pointLabel.Content = "Edit Point";
                enableOptions();

                Mouse.Capture(selectedPoint);  //take it
                captured = true;
                mx = e.GetPosition(DaCanvas).X;
                my = e.GetPosition(DaCanvas).Y;
                for (shapeIndex = 0; shapeIndex < cpCounter && selectedPoint.Name != controlPoints[shapeIndex].Name; shapeIndex++) ;

                loadDone = false;
                selectedValues();
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
                enableOptions();
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
            SAShape[] pattern = new SAShape[2000];
            int lastSeed = seed.Next(0,cpCounter);
            //Shape currentPoint = controlPoints[initial];
            SAShape currentPoint = new SAShape(controlPoints[lastSeed], sx[lastSeed], sy[lastSeed], lastSeed);
            SAShape lastPoint;
            SAShape nextPoint;
            for(int i = 0;i < 2000;i++)
            {
                int random = seed.Next(0, cpCounter);
                while(random==lastSeed)
                    random = seed.Next(0, cpCounter);
                nextPoint = new SAShape(controlPoints[random], sx[random], sy[random], random);
                if (i == 0)
                    lastPoint = new SAShape(currentPoint.getParent(), (currentPoint.x() + nextPoint.x())/2, 
                        (currentPoint.y() + nextPoint.y())/2, random);
                else
                    lastPoint = new SAShape(currentPoint.getParent(), (currentPoint.x() + nextPoint.x()) / 2, 
                        (currentPoint.y() + nextPoint.y()) / 2, random);
                pattern[i] = lastPoint;
                Canvas.SetLeft(pattern[i].getShape(), pattern[i].x());
                Canvas.SetTop(pattern[i].getShape(), pattern[i].y());
                DaCanvas.Children.Add(pattern[i].getShape());
                currentPoint = lastPoint;
                lastSeed = random;
            }
        }
       // public void sierpinskinate()
       // {

       // }


        public void Color_Load(object sender, RoutedEventArgs e)
        {
            List<string> data = new List<string>();
            data.Add("0");
            data.Add("128");
            data.Add("255");

            var comboBox = sender as ComboBox;

            comboBox.ItemsSource = data;
        }

        private void Color_Changed(object sender, SelectionChangedEventArgs e)
        {
            int r = Convert.ToInt32(rVal.SelectedValue);
            int g = Convert.ToInt32(gVal.SelectedValue);
            int b = Convert.ToInt32(bVal.SelectedValue);
            color = new SolidColorBrush(Color.FromRgb((byte) r, (byte) g, (byte) b));
            colorPreview.Fill = color;

            if (selectedPoint != null && loadDone)
            {
                selectedPoint.Fill = color;
            }
            
        }

        private void Size_Checked(object sender, RoutedEventArgs e)
        {
            var radio = sender as RadioButton;
            if ((string)radio.Content == "Small")
            {
                size = 10;
            }
            else if ((string)radio.Content == "Medium")
            {
                size = 15;
            }
            else
            {
                size = 20;
            }

            if (selectedPoint != null && loadDone)
            {
                selectedPoint.Width = size;
                selectedPoint.Height = size;
            }
            
        }

        private void defaultValues(){
            size = 10;
            sizeS.IsChecked = true;

            rVal.SelectedIndex = 0;
            bVal.SelectedIndex = 0;
            gVal.SelectedIndex = 0;
            color = new SolidColorBrush(Colors.Black);
            colorPreview.Fill = color;
        }

        public void selectedValues()
        {
            rVal.SelectedValue = Convert.ToString(Convert.ToInt32(((SolidColorBrush)selectedPoint.Fill).Color.R));
            gVal.SelectedValue = Convert.ToString(Convert.ToInt32(((SolidColorBrush)selectedPoint.Fill).Color.G));
            bVal.SelectedValue = Convert.ToString(Convert.ToInt32(((SolidColorBrush)selectedPoint.Fill).Color.B));
            colorPreview.Fill = (SolidColorBrush) selectedPoint.Fill;
            color = (SolidColorBrush)selectedPoint.Fill;

            switch ((int)selectedPoint.Width)
            {
                case 10:
                    sizeS.IsChecked = true;
                    size = 10;
                    break;
                case 15:
                    sizeM.IsChecked = true;
                    size = 15;
                    break;
                case 20:
                    sizeL.IsChecked = true;
                    size = 20;
                    break;
            }

            loadDone = true;
        }

        public void disableOptions()
        {
            rVal.IsEnabled = false;
            bVal.IsEnabled = false;
            gVal.IsEnabled = false;
            sizeS.IsEnabled = false;
            sizeM.IsEnabled = false;
            sizeL.IsEnabled = false;
            colorPreview.IsEnabled = false;
        }

        public void enableOptions()
        {
            rVal.IsEnabled = true;
            bVal.IsEnabled = true;
            gVal.IsEnabled = true;
            sizeS.IsEnabled = true;
            sizeM.IsEnabled = true;
            sizeL.IsEnabled = true;
            colorPreview.IsEnabled = true;

        }

        public void CursorOver(object sender, MouseEventArgs e)
        {
            var btn = sender as Label;

            btn.Cursor = Cursors.Hand;
        }

        public void CursorOut(object sender, MouseEventArgs e)
        {
            var btn = sender as Label;

            btn.Cursor = Cursors.Arrow;
        }
    }
    
}
