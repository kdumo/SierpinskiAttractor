/**
 * Comp 585
 * Sierpinski Attractor
 * 
 * Kyle Dumo kyle.dumo.789@my.csun.edu
 * Joseph Peña joseph.pena.943@my.csun.edu
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
    class ControlPoints
    {
        private double sx;
        private double sy;//shape x and y
        private Shape shape;

        public ControlPoints(Shape point)
        {
            shape = point;
        }
        public ControlPoints(Shape point, double x, double y)
        {
            shape = point;
            sx = x;  //left
            sy = y;  //top
        }
        public void position(double x, double y)
        {

        }
        public Shape getShape() { return shape; }
        public double x() { return sx; }
        public double y() { return sy; }
        public void setY(double y) { sy = y; }
        public void setX(double x) { sx = x; }
    }
}
