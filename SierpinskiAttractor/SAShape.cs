using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;

namespace SierpinskiAttractor
{

    class SAShape
    {
        private double sx;
        private double sy;//shape x and y
        private Shape parent;
        private Shape shape;
        private int parentIndex;
        //private Shape[] parents;
        //private int cpCounter = 0;


        public SAShape(Shape point, int index)
        {
            parent = point;
            parentIndex = index;
            shape = identifyShape();
        }
        public SAShape(Shape point, double x, double y, int index)
        {
            parent = point;
            sx = x;
            sy = y;
            parentIndex = index;
            shape = identifyShape();
            //cpCounter = parents.Length; 
        }
        public void position(double x, double y)
        {

        }

        public Shape getParent() { return parent; }

        public Shape getShape() { return shape; }

        public int getIndex() { return parentIndex; }

        public double x() { return sx; }

        public double y() { return sy; }

        public Shape identifyShape()
        {
            Shape temp;
            if (parent.Name == "Point" + parentIndex + "1")
            {
                //shape is ellipse
                temp = new Ellipse(){
                    Height = 0.5*parent.Height,
                    Width = 0.5*parent.Width,
                    Fill = Brushes.Blue  //parents color
                };
            }
            else
            {
                //shape is rectangle
                temp = new Rectangle()
                {
                    Height = 0.5 * parent.Height,
                    Width = 0.5 * parent.Width,
                    Fill = Brushes.Pink  //parents color
                };
            }
            return temp;
        }
    }
}
