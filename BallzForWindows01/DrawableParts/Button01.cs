using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.DrawableParts
{
    class Button01 : DrawableObject
    {

        Rectangle clickRectangle;
        public Rectangle ClickRectangle { get { return clickRectangle; } }
        Point center;
        public Point Center { get { return center; } }

        Color color;
        public Color DrawColor { get { return color; } set { color = value; } }

        #region font properties
        Font font;
        Color fontColor;// = Color.Black;
        public Color FontColor { get { return fontColor; } set { fontColor = value; } }
        int fontSize = 20;
        string fontFamily = "Arial";
        #endregion font properties

        string text;

        bool placed = false;
        public bool IsPlaced { get { return placed; } }

        bool showClickRect = false;
        public bool ShowClickRectangle { get { return showClickRect; } set { showClickRect = value; } }

        public Button01()
        {
            x = 300;
            y = 300;
            width = 100;
            height = 40;
            alpha = 255;
            red = 255;
            green = 0;
            blue = 0;
            color = Color.FromArgb(alpha, red, green, blue);
            font = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            fontColor = Color.Black;
            center = new Point(x + width / 2, y + height / 2);
            clickRectangle = new Rectangle(x, y, width, height);
            visible = true;
            text = "Button";
        }


        public void Load(int x, int y)
        {
            this.x = x;
            this.y = y;
            center = new Point(x + width / 2, y + height / 2);
            clickRectangle = new Rectangle(x, y, width, height);
        }
        public void Load(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            center = new Point(x + width / 2, y + height / 2);
            clickRectangle = new Rectangle(x, y, width, height);
        }
        public void Load(int x, int y, int width, int height, string buttonText)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            center = new Point(x + width / 2, y + height / 2);
            clickRectangle = new Rectangle(x, y, width, height);
            text = buttonText;
        }


        public void Update()
        {

        }
        public void Draw(Graphics g)
        {
            using (SolidBrush sb = new SolidBrush(fontColor))
            {
                Pen p = new Pen(color, 5);
                
                PointF txtPos = new PointF();
                txtPos.X = center.X - g.MeasureString(text, font).Width / 2;
                txtPos.Y = center.Y - g.MeasureString(text, font).Height / 2;
                if (visible)
                {
                    g.DrawRectangle(p, x, y, width, height);
                    g.DrawString(text, font, sb, txtPos);
                }
            }
        }

        private void SetClickRectangle(int x, int y)
        {
            clickRectangle = new Rectangle();
            clickRectangle.X = x - width / 2;
            clickRectangle.Y = y - height / 2;
            clickRectangle.Width = width;
            clickRectangle.Height = height;
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
    }
}
