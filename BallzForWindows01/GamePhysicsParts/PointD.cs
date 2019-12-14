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


        public void Set(PointD p) { x = p.x; y = p.y; }
        public void Set(double x, double y) { this.x = x; this.y = y; }
        public void Zero() { x = y = 0; }

        public double DistanceTo(double toPointX, double toPointY)
        {
            return _DistanceTo(toPointX, toPointY);
        }
        public double DistanceTo(PointD toPoint)
        {
            return _DistanceTo(toPoint.X, toPoint.Y);
        }

        //bool north = false, south = false;
        public double RotationTo(PointD p2)
        {
            PointD originPos = this;
            PointD aimPos = p2;
            PointD rightPos = new PointD(aimPos.x, originPos.y);
            double oppLen = aimPos.DistanceTo(rightPos);
            double hypLen = aimPos.DistanceTo(p2);
            double rot = 0;

            bool north = false, south = false;
            if (aimPos.Y < originPos.Y) { north = true; }
            if (aimPos.Y > originPos.Y) { south = true; }
            double anglePreRotation = Math.Asin(oppLen / hypLen);
            // north
            if (aimPos.X == originPos.X && north) { rot = (3 * Math.PI) / 2; return rot; }
            // south
            if (aimPos.X == originPos.X && south) { rot = (Math.PI) / 2; return rot; }
            // east
            if (aimPos.X > originPos.X && aimPos.Y == originPos.Y) { rot = 0; return rot; }
            // west
            if (aimPos.X < originPos.X && aimPos.Y == originPos.Y) { rot = Math.PI; return rot; }
            // northwest
            if (aimPos.X > originPos.X && north) { rot = Math.PI * 2 - anglePreRotation; return rot; }
            //northeast
            if (aimPos.X < originPos.X && north) { rot = Math.PI + anglePreRotation; return rot; }
            // southwest
            if (aimPos.X > originPos.X && south) { rot = anglePreRotation; return rot; }
            // southeast
            if (aimPos.X < originPos.X && south) { rot = Math.PI - anglePreRotation; return rot; }
            return rot;
        }

        public override string ToString() { return $"{{X={x}, Y={y}}}"; }
        public Point ToPoint() { return new Point((int)x, (int)y); }
        public PointF ToPointF() { return new PointF((float)x, (float)y); }


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