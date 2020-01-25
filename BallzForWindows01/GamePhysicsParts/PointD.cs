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
        public PointD(PointD pos) { Set(pos.X, pos.Y); }
        public PointD(double x, double y) { Set(x, y); }


        public void Set(PointD p) { x = p.x; y = p.y; }
        public void Set(double x, double y) { this.x = x; this.y = y; }
        public void Set(PointD startPoint, double rotation, double distance)
        {
            x = startPoint.x + distance * Math.Cos(rotation);
            y = startPoint.y + distance * Math.Sin(rotation);
        }


        public void Move(double distance, double rotation)
        {
            x = x + distance * Math.Cos(rotation);
            y = y + distance * Math.Sin(rotation);
        }
        public void Zero() { x = y = 0; }

        public double DistanceTo(double toPointX, double toPointY) { return _DistanceTo(toPointX, toPointY); }
        public double DistanceTo(PointD toPoint) { return _DistanceTo(toPoint.X, toPoint.Y); }
        public PointD HalfWayTo(double endx, double endy) { return _HalfWayTo(endx, endy); }
        public PointD HalfWayTo(PointD endp) { return _HalfWayTo(endp.X, endp.Y); }

        public PointD PointFrom(double rotation, double distance)
        {
            PointD tmp = new PointD();
            tmp.X = x + distance * Math.Cos(rotation);
            tmp.Y = y + distance * Math.Sin(rotation);
            return tmp;
        }
        
        public double AngleTo(PointD p)
        {
            bool north, south;
            double anglePreRotation = 0;
            //double rot = 0;
            double oppLen = 0, hypLen = 0;
            PointD startPoint = new PointD(x, y);
            PointD endPoint = new PointD(p.X, p.Y);
            PointD rightPoint = new PointD(endPoint.X, startPoint.Y);
            oppLen = rightPoint.DistanceTo(endPoint);
            hypLen = startPoint.DistanceTo(endPoint);
            north = south = false;
            if (endPoint.Y < startPoint.Y) { north = true; }
            if (endPoint.Y > startPoint.Y) { south = true; }
            anglePreRotation = Math.Asin(oppLen / hypLen);
            // north
            if (endPoint.X == startPoint.X && north) { return (3 * Math.PI) / 2; }
            // south
            if (endPoint.X == startPoint.X && south) { return (Math.PI) / 2; }
            // east
            if (endPoint.X > startPoint.X && endPoint.Y == startPoint.Y) { return 0; }
            // west
            if (endPoint.X < startPoint.X && endPoint.Y == startPoint.Y) { return Math.PI; }
            // northwest
            if (endPoint.X > startPoint.X && north) { return Math.PI * 2 - anglePreRotation; }
            //northeast
            if (endPoint.X < startPoint.X && north) { return Math.PI + anglePreRotation; }
            // southwest
            if (endPoint.X > startPoint.X && south) { return anglePreRotation; }
            // southeast
            if (endPoint.X < startPoint.X && south) { return Math.PI - anglePreRotation; }
            return 0;
        }

        public override string ToString() { return $"{{X={x}, Y={y}}}"; }
        public Point ToPoint() { return new Point((int)x, (int)y); }
        public PointF ToPointF() { return new PointF((float)x, (float)y); }

        private PointD _HalfWayTo(double endx, double endy)
        {
            PointD midp = new PointD();
            midp.X = x + ((endx - x) / 2);
            midp.Y = y + ((endy - y) / 2);
            return midp;
        }
        double _DistanceTo(double toPointX, double toPointY)
        {
            double xdiff = x - toPointX;
            double ydiff = y - toPointY;
            double sumSqrs = (xdiff * xdiff) + (ydiff * ydiff);
            return Math.Sqrt(sumSqrs);
        }
    }
}


#region original DistanceTo
//public double DistanceTo(PointD toPoint)
//{
//    double xdiff = x - toPoint.x;
//    double ydiff = y - toPoint.y;
//    double sumSqrs = (xdiff * xdiff) + (ydiff * ydiff);
//    return Math.Sqrt(sumSqrs);
//}
#endregion original DistanceTo

