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

        Rectangle clickRectangle;
        public Rectangle ClickRectangle { get { return clickRectangle; } }
        Point center;
        public Point Center { get { return center; } }

        Color color;
        public Color DrawColor { get { return color; } set { color = value; } }

        #region font properties
        Font font;
        Color fontColor = Color.Black;
        int fontSize = 20;
        string fontFamily = "Arial";
        #endregion font properties

        bool placed = false;
        public bool IsPlaced { get { return placed; } }

        bool showClickRect = false;
        public bool ShowClickRectangle { get { return showClickRect; } set { showClickRect = value; } }

        //bool isClicked = false;
        //public bool IsClicked { get { return isClicked; } set { isClicked = value; } }

        public XMarker()
        {
            x = 0;
            y = 0;
            width = 20;
            height = 20;
            alpha = 255;
            red = 255;
            green = 0;
            blue = 0;
            color = Color.FromArgb(alpha, red, green, blue);
            font = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            center = new Point(x + width / 2, y + height / 2);
            clickRectangle = new Rectangle(x, y, width, height);
            visible = false;
        }

        public void Load(int x, int y)
        {
            this.x = x;
            this.y = y;
            center = new Point(x + width / 2, y + height / 2);
            //clickRectangle = new Rectangle(x, y, width, height);
            SetClickRectangle(x, y);
        }
        public void Load(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            center = new Point(x + width / 2, y + height / 2);
            //clickRectangle = new Rectangle(x, y, width, height);
            SetClickRectangle(x, y);
        }
        public void Place(int x, int y)
        {
            this.x = x;
            this.y = y;
            center = new Point(x + width / 2, y + height / 2);
            SetClickRectangle(x, y);
            visible = true;
            placed = true;
        }
        public void AdjustPosition(int setx, int sety)
        {
            x = setx;
            y = sety;
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
                return false;
        }
        private void SetClickRectangle(int x, int y)
        {
            clickRectangle = new Rectangle();
            clickRectangle.X = x - width / 2;
            clickRectangle.Y = y - height / 2;
            clickRectangle.Width = width;
            clickRectangle.Height = height;
        }
        public void Remove()
        {
            this.x = 0;
            this.y = 0;
            visible = false;
            placed = false;
        }
        public void Update()
        {

        }
        public void Draw(Graphics g)
        {
            using (SolidBrush sb = new SolidBrush(color))
            {
                Pen p = new Pen(color, 5);
                if(visible)
                {
                    DrawX(g, p);
                    if (showClickRect)
                        DrawClickRectangle(g);
                }
            }
        }
        
        private void DrawX(Graphics g, Pen p)
        {
            Point tl = new Point();
            Point br = new Point();
            Point tr = new Point();
            Point bl = new Point();
            tl.X = x;
            tl.Y = y;
            br.X = x + width;
            br.Y = y + height;

            tr.X = x + width;
            tr.Y = y;
            bl.X = x;
            bl.Y = y + height;

            // center marker
            tl.X -= width / 2;
            tl.Y -= height / 2;
            br.X -= width / 2;
            br.Y -= height / 2;
            tr.X -= width / 2;
            tr.Y -= height / 2;
            bl.X -= width / 2;
            bl.Y -= height / 2;
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

        public void Reset()
        {
            x = 0;
            y = 0;
            visible = false;
            placed = false;
        }
    }
}
