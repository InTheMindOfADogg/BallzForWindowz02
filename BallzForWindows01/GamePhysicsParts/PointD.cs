using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.GamePhysicsParts
{
    class PointD
    {
        double x, y;

        public double X { get { return x; } set { x = value; } }
        public double Y { get { return y; } set { y = value; } }

        public float fX { get { return (float)x; } }
        public float fY { get { return (float)y; } }

        public float iX { get { return (int)x; } }
        public float iY { get { return (int)y; } }

        public PointD() { Set(0, 0); }
        public PointD(double x, double y) { Set(x, y); }
        
        public void Set(double x, double y) { this.x = x; this.y = y; }
        public double DistanceTo(PointD toPoint)
        {
            double xdiff = x - toPoint.x;
            double ydiff = y - toPoint.y;
            double sumSqrs = (xdiff * xdiff) + (ydiff * ydiff);
            return Math.Sqrt(sumSqrs);
        }
        public override string ToString() { return $"{{X={x}, Y={y}}}"; }        
        public Point ToPoint() { return new Point((int)x, (int)y); }
        public PointF ToPointF() { return new PointF((float)x, (float)y); }
    }
}
