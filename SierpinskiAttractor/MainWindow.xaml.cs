/**
 * Comp 585
 * Sierpinski Attractor
 * 
 * Kyle Dumo kyle.dumo.789@my.csun.edu
 * Joseph Pena joseph.pena.943@my.csun.edu
 * 
 */

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
        private double mx, my;   //mouse x and y in canvas
        private int shapeIndex = 0;
        private int cpCounter = 0;
        private double sx, sy;
        private ControlPoints[] cp = new ControlPoints[6];

        private Shape selectedPoint = null;
        private bool captured = false, loadDone = false, 
            run = false, pointMatch = false;
        private SolidColorBrush color;
        private int size = 10;
        Random seed = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Usage_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult usage = MessageBox.Show("Usage\n" + "Add points\tSpecify color and size\n\t\tRight Click on Canvas\n\n" 
                + "Edit Points\tLeft Click on Point\n\t\tChange Color and Size\n\n" + "Move Points\tLeft Click on Point, Hold, Drag and Release\n\n" 
                + "Run\t\tAdd atleast 3 points on Canvas\n\t\tClick Run\n\n" + "Clear\t\tClick Clear to remove all points\n");
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult about = MessageBox.Show("Sierpinski Attractor\nKyle Dumo\tkyle.dumo.789@my.csun.edu\nJoseph Pena\tjoseph.pena.943@my.csun.edu");
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            if (cpCounter < 3)
            {
                StatusLabel.Content = "Not Enough Control Points";
                StatusLabel.Visibility = System.Windows.Visibility.Visible;
            }
            else if (!run)
            {
                run = true;
                StatusLabel.Visibility = System.Windows.Visibility.Hidden;
                sierpinskinate();
            }
            else if (run)
            {
                DaCanvas.Children.Clear();
                for (int i = 0; i < cpCounter; i++)
                {
                    Canvas.SetLeft(cp[i].getShape(), cp[i].x());
                    Canvas.SetTop(cp[i].getShape(), cp[i].y());
                    DaCanvas.Children.Add(cp[i].getShape());
                }
                if (run)
                    sierpinskinate();
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            cpCounter = 0;
            Indexshape.Content = "Control Points: " + cpCounter;
            sx = 0;
            sy = 0;
            cp = new ControlPoints[6];
            DaCanvas.Children.Clear();
            run = false;
            StatusLabel.Content = "Add some Points";
            pointLabel.Content = "Add New Point";

        }

        private void DaCanvas_MouseMove_1(object sender, MouseEventArgs e)
        {
            double x = e.GetPosition(DaCanvas).X;
            double y = e.GetPosition(DaCanvas).Y;
            if (captured && shapeIndex < cpCounter)
            {
                sx = cp[shapeIndex].x();
                sx += x - mx;
                Canvas.SetLeft(selectedPoint, sx);
                mx = x;
                sy = cp[shapeIndex].y();
                sy += y - my;
                Canvas.SetTop(selectedPoint,sy);
                my = y;
                cp[shapeIndex].setX(sx);
                cp[shapeIndex].setY(sy);
            }
            //else...nothing right??
            //do nothing if not control point
            mousePos.Content = String.Format("Mouse at ({0},{1})", x, y);
            
        }

        private void setControlPoint(object sender, MouseButtonEventArgs e)
        {
            Shape newShape;
            //forr size debug
            if (cpCounter < 6 && !run)
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
                cp[cpCounter] = new ControlPoints(newShape, Canvas.GetLeft(newShape),
                    Canvas.GetTop(newShape));
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
                pointMatch = false;
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
                for (shapeIndex = 0; shapeIndex < cpCounter && 
                    selectedPoint.Name != cp[shapeIndex].getShape().Name; shapeIndex++) ;
                if (shapeIndex != cpCounter)  //we got a match!!!
                {
                    pointLabel.Content = "Edit Point";
                    enableOptions();
                    Mouse.Capture(selectedPoint);  //take it
                    captured = true;
                    mx = e.GetPosition(DaCanvas).X;
                    my = e.GetPosition(DaCanvas).Y;
                }
                else
                    pointMatch = false;
                loadDone = false;
                selectedValues();
            }
        }
        public void clearIt()  //clears and redraws control points
        {
            DaCanvas.Children.Clear();
            for (int i = 0; i < cpCounter; i++)
            {
                Canvas.SetLeft(cp[i].getShape(), cp[i].x());
                Canvas.SetTop(cp[i].getShape(), cp[i].y());
                DaCanvas.Children.Add(cp[i].getShape());
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
                clearIt();
                pointMatch = true;
                if (run)
                    sierpinskinate();
            }
            //clear canvas, redraw control points and rerun
        }

        public void sierpinskinate()
        {
            clearIt();
            SAShape[] shape = new SAShape[2000];
            ControlPoints last = cp[seed.Next(0, cpCounter)];
            ControlPoints next;
            for (int i = 0; i < 2000; i++)
            {
                next = cp[seed.Next(0, cpCounter)];
                last = new ControlPoints(last.getShape(), (next.x() + last.x()) / 2,
                    (next.y() + last.y()) / 2);
                shape[i] = new SAShape(next.getShape(), last.x(), last.y());
                Canvas.SetLeft(shape[i].getShape(), shape[i].x());
                Canvas.SetTop(shape[i].getShape(), shape[i].y());
                DaCanvas.Children.Add(shape[i].getShape());
            }
        }
       
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

            if (selectedPoint != null && loadDone && pointMatch)
            {
                selectedPoint.Fill = color;
                if(run)
                    sierpinskinate();
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

            if (selectedPoint != null && loadDone && pointMatch)
            {
                selectedPoint.Width = size;
                selectedPoint.Height = size;
                if(run)
                    sierpinskinate();
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
