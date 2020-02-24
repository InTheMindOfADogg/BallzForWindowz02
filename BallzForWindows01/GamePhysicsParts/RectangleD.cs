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

        public PointD Position { get { return new PointD(x, y); } }
        public SizeD Size { get { return new SizeD(width, height); } }
        public PointD Center { get { return new PointD(x + width / 2, y + height / 2); } }

        public PointD TopLeft { get { return new PointD(x, y); } }
        public PointD TopRight { get { return new PointD(x + width, y); } }
        public PointD BottomLeft { get { return new PointD(x, y + height); } }
        public PointD BottomRight { get { return new PointD(x + width, y + height); } }


        public double X { get { return x; } set { x = value; } }
        public double Y { get { return y; } set { y = value; } }
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
        public double Left { get { return x; } }
        public double Right { get { return x + width; } }
        public double Top { get { return y; } }
        public double Bottom { get { return y + height; } }


        double x, y, width, height;
        double startX = 0, startY = 0, startWidth = 0, startHeight = 0;


        public RectangleD() { Set(0, 0, 0, 0); }
        public RectangleD(double x, double y, double width, double height) { Set(x, y, width, height); }
        public void Set(double x, double y, double width, double height, bool centerOnpoint = false)
        {
            
            _Set(x, y, width, height, centerOnpoint);
        }
        public void Set(PointD p, SizeD s, bool centerOnPoint = false)
        {
            _Set(p.X, p.Y, s.Width, s.Height, centerOnPoint);
        }
        private void _Set(double x, double y, double width, double height, bool centerOnpoint = false)
        {
            this.x = x; this.y = y; this.width = width; this.height = height;
            if (centerOnpoint) { CenterOnXY(x, y); }
        }
        public void SetStartingRect(double x, double y, double width, double height)
        {
            startX = x;
            startY = y;
            startWidth = width;
            startHeight = height;
        }

        public void SetPosition(double x, double y) { this.x = x; this.y = y; }
        public void SetSize(double width, double height) { this.width = width; this.height = height; }


        public void CenterOnXY(double x, double y)
        {
            this.x = x - (width / 2);
            this.y = y - (height / 2);
        }

        public bool InBox(double dx, double dy) { return (dx > x && dx < x + width && dy > y && dy < y + height); }

        public override string ToString() { return $"{{X={x}, Y={y}, Width={width}, Height={height}}}"; }
        public Rectangle ToRectangle() { return new Rectangle((int)x, (int)y, (int)width, (int)height); }
        public RectangleF ToRectangleF() { return new RectangleF((float)x, (float)y, (float)width, (float)height); }


        public void Reset() { x = startX; y = startY; width = startWidth; height = startHeight; }

    }
}
