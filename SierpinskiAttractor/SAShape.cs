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
using System.Windows.Shapes;
using System.Windows.Media;

namespace SierpinskiAttractor
{

    class SAShape
    {
        private double sx;
        private double sy;//shape x and y
        private Shape shape;

        public SAShape(Shape point)
        {
            shape = identifyShape(point);
            shape.Fill = point.Fill;
            shape.Height = 0.2 * point.Height;
            shape.Width = 0.2 * point.Width;
        }
        public SAShape(Shape point, double x, double y)
        {
            shape = identifyShape(point);
            sx = x;
            sy = y;
            shape.Fill = point.Fill;
            shape.Height = 0.2 * point.Height;
            shape.Width = 0.2 * point.Width;
        }
        public void position(double x, double y)
        {

        }

        public Shape getShape() { return shape; }

        public double x() { return sx; }

        public double y() { return sy; }

        public Shape identifyShape(Shape s)
        {
            Shape temp = s;
            string output = temp.Name.Substring(temp.Name.Length - 1, 1);
            if (output == "1")
            {
                //shape is ellipse
                temp = new Ellipse()
                {
                    Height = 0.3* temp.Height,
                    Width = 0.3 * temp.Width,
                    Fill = temp.Fill  //parents color
                };
            }
            else
            {
                //shape is rectangle
                temp = new Rectangle()
                {
                    Height = 0.3 * temp.Height,
                    Width = 0.3 * temp.Width,
                    Fill = temp.Fill   //parents color
                };
            }
            return temp;
        }
    }
}
