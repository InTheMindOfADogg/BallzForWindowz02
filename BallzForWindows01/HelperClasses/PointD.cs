using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.HelperClasses
{
    class PointD
    {
        double X { get { return x; } set { x = value; } }
        double Y { get { return y; } set { y = value; } }
        double x, y = 0;

        public PointD()
        {
            x = y = 0;
        }
        public PointD(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public PointF ToPointF() { return new PointF((float)x, (float)y); }
        public Point ToPoint() { return new Point((int)x, (int)y); }
        public double DistanceTo(PointD toPoint)
        {
            double xdiff = x - toPoint.x;
            double ydiff = y - toPoint.y;
            double sumSqrs = (xdiff * xdiff) + (ydiff * ydiff);
            return Math.Sqrt(sumSqrs);
        }
        public override string ToString() { return $"{{X={x}, Y={y}}}"; }
    }
}
