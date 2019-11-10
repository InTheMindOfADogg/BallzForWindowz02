using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.GamePhysicsParts
{
    class RectangleD
    {
        double x, y, width, height;

        public double X { get { return x; } set { x = value; } }
        public double Y { get { return Y; } set { y = value; } }
        public double Width { get { return width; } set { width = value; } }
        public double Height { get { return height; } set { height = value; } }

        public float fX { get { return (float)x; } }
        public float fY { get { return (float)y; } }
        public float fWidth { get { return (float)width; } }
        public float fHeight { get { return (float)height; } }

        public float iX { get { return (int)x; } }
        public float iY { get { return (int)y; } }
        public float iWidth { get { return (int)width; } }
        public float iHeight { get { return (int)height; } }

        public double CenterX { get { return (x + width / 2); } }
        public double CenterY { get { return (y + height / 2); } }

        public RectangleD() { Set(0, 0, 0, 0); }
        public RectangleD(double x, double y, double width, double height) { Set(x, y, width, height); }
        public void Set(double x, double y, double width, double height)
        {
            this.x = x; this.y = y; this.width = width; this.height = height;
        }
        public bool InBox(double dx, double dy) { return (dx > x && dx < x + width && dy > y && dy < y + height) ? true : false; }
        //public bool HitBox(double dx, double dy) { return (dx >= x && dx <= x + width && dy >= y && dy <= y + height) ? true : false; }

        public override string ToString() { return $"{{X={x}, Y={y}, Width={width}, Height={height}}}"; }
        public Rectangle ToRectangle() { return new Rectangle((int)x, (int)y, (int)width, (int)height); }
        public RectangleF ToRectangleF() { return new RectangleF((float)x, (float)y, (float)width, (float)height); }


    }
}
