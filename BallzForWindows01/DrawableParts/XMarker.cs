using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace BallzForWindows01.DrawableParts
{
    class XMarker : DrawableObject
    {

        #region font settings
        Font font;
        Color fontColor = Color.Black;
        int fontSize = 20;
        string fontFamily = "Arial";
        #endregion font settings

        public Rectangle ClickRectangle { get { return clickRectangle; } }
        public Point Center { get { return center; } }
        public Color DrawColor { get { return color; } set { color = value; } }
        public bool IsPlaced { get { return placed; } }
        public bool ShowClickRectangle { get { return showClickRect; } set { showClickRect = value; } }

        public int X { get { return x; } /*set { x = value; }*/ }
        public int Y { get { return y; } /*set { y = value; }*/ }
        public int Width { get { return width; } }
        public int Height { get { return height; } }


        Rectangle clickRectangle;
        Point center;
        bool placed = false;
        bool showClickRect = false;

        int x, y, width, height;

        public XMarker()
        {
            _Load(0, 0, 20, 20);
            SetColor(255, 255, 0, 0);
            font = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            visible = false;
        }

        public void Load(int x, int y) { _Load(x, y, width, height); }
        public void Load(int x, int y, int width, int height) { _Load(x, y, width, height); }
        private void _Load(int x, int y, int width, int height)
        {

            SetPosition(x, y);
            SetSize(width, height);
            center = new Point(x + width / 2, y + height / 2);
            SetClickRectangle(x, y, width, height);
        }
        private void SetClickRectangle(int x, int y, int width, int height)
        {
            clickRectangle = new Rectangle();
            clickRectangle.X = x - width / 2;
            clickRectangle.Y = y - height / 2;
            clickRectangle.Width = width;
            clickRectangle.Height = height;
        }
        public void Place(int x, int y)
        {
            _Load(x, y, width, height);
            visible = true;
            placed = true;
        }

        public void AdjustPosition(int x, int y)
        {
            SetPosition(x, y);
        }
        public bool IsInBoundingRect(int mPosX, int mPosY)
        {
            if (mPosX > clickRectangle.X &&
                mPosX < clickRectangle.X + clickRectangle.Width &&
                mPosY > clickRectangle.Y &&
                mPosY < clickRectangle.Y + clickRectangle.Height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public double AngleFromPoint(Point p)
        {
            double angle = 0;
            double xdiff = p.X - center.X;
            double ydiff = p.Y - center.Y;
            angle = Math.Atan2(xdiff, ydiff) /* * 180 / Math.PI */;
            return angle;
        }


        public void Update()
        {

        }
        public void Draw(Graphics g)
        {
            Pen p = new Pen(color, 5);
            if (visible)
            {
                DrawX(g, p);
                if (showClickRect)
                    DrawClickRectangle(g);
            }

        }

        private void DrawX(Graphics g, Pen p)
        {
            Point tl = new Point(x - (width / 2), y - (height / 2));
            Point br = new Point(x + width - (width / 2), y + height - (height / 2));
            Point tr = new Point(x + width - (width / 2), y - (height / 2));
            Point bl = new Point(x - (width / 2), y + height - (height / 2));

            center.X = tl.X + width / 2;
            center.Y = tl.Y + height / 2;
            g.DrawLine(p, tl, br);
            g.DrawLine(p, tr, bl);

            clickRectangle.X = tl.X;
            clickRectangle.Y = tl.Y;
        }

        private void DrawClickRectangle(Graphics g)
        {
            g.DrawRectangle(Pens.Black, clickRectangle);
        }
        public void Remove()
        {
            _Reset();
        }
        public void Reset()
        {
            _Reset();
        }
        void _Reset()
        {
            SetPosition(0, 0);
            visible = false;
            placed = false;
        }

        protected void SetPosition(int x, int y) { this.x = x; this.y = y; }
        protected void SetSize(int width, int height) { this.width = width; this.height = height; }
    }
}
