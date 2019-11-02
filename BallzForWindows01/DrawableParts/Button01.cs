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

        #region Font settings
        Font font;
        Color fontColor;// = Color.Black;
        public Color FontColor { get { return fontColor; } set { fontColor = value; } }
        int fontSize = 20;
        string fontFamily = "Arial";
        void ConfigureFont()
        {
            font = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            fontColor = Color.Black;
        }
        #endregion Font settings

        int x, y, width, height;
        Rectangle clickRectangle;
        Point center;
        string text;
        bool placed = false;
        bool showClickRect = false;

        public int X { get { return x; } /*set { x = value; }*/ }
        public int Y { get { return y; } /*set { y = value; }*/ }
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public Rectangle ClickRectangle { get { return clickRectangle; } }
        public Point Center { get { return center; } }
        public Color DrawColor { get { return color; } set { color = value; } }
        public bool IsPlaced { get { return placed; } }
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
            SetColor(alpha, red, green, blue);
            center = new Point(x + width / 2, y + height / 2);
            clickRectangle = new Rectangle(x, y, width, height);
            visible = true;
            ConfigureFont();
        }
        public void Load(int x, int y) { _Load(x, y, width, height); }
        public void Load(int x, int y, string buttonText) { _Load(x, y, 0, 0, buttonText); }

        public void Load(int x, int y, int width, int height) { _Load(x, y, width, height); }
        public void Load(int x, int y, int width, int height, string buttonText) { _Load(x, y, width, height, buttonText); }
        void _Load(int x, int y, int width, int height, string buttonText = "")
        {
            this.x = x;
            this.y = y;
            if((width == 0 || height == 0) && (!string.IsNullOrWhiteSpace(buttonText)) )
            {
                SetSizeFromText(buttonText);
            }
            else
            {
                this.width = width;
                this.height = height;
            }
            
            center = new Point(x + width / 2, y + height / 2);
            clickRectangle = new Rectangle(x, y, width, height);
            text = buttonText;
        }

        void SetSizeFromText(string btnText)
        {
            Bitmap bmp = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(bmp);
            SizeF strSize = g.MeasureString(btnText, font);
            width = (int)(strSize.Width + 1.5);
            height = (int)(strSize.Height + 1.5);
            g.Dispose();            
            bmp.Dispose();
        }
        
        public void Update()
        {

        }
        public void Draw(Graphics g)
        {
            SolidBrush sb = new SolidBrush(fontColor);
            Pen p = new Pen(color, 5);
            PointF txtPos = new PointF();
            txtPos.X = center.X - g.MeasureString(text, font).Width / 2;
            txtPos.Y = center.Y - g.MeasureString(text, font).Height / 2;
            if (visible)
            {
                g.DrawRectangle(p, x, y, width, height);
                g.DrawString(text, font, sb, txtPos);
            }
            p.Dispose();
            sb.Dispose();
        }
        public void CleanUp() { if (font != null) { font.Dispose(); } }

        
        public bool IsInBoundingRect(int posx, int posy)
        {
            if (posx > clickRectangle.X && posx < clickRectangle.X + clickRectangle.Width
                && posy > clickRectangle.Y && posy < clickRectangle.Y + clickRectangle.Height)
            {
                return true;
            }
            else
                return false;
        }

    }
}

#region setting click rectangle, 0 refs as of 2019-10-26.
//private void SetClickRectangle(int x, int y) // 0 refs as of 2019-10-26
//{
//    //clickRectangle = new Rectangle();
//    clickRectangle.X = x - width / 2;
//    clickRectangle.Y = y - height / 2;
//    //clickRectangle.Width = width;
//    //clickRectangle.Height = height;
//} 
#endregion setting click rectangle, 0 refs as of 2019-10-26.

#region old Loads - reworked 2019-10-26
//public void Load(int x, int y, int width, int height)
//{
//    this.x = x;
//    this.y = y;
//    this.width = width;
//    this.height = height;
//    center = new Point(x + width / 2, y + height / 2);
//    clickRectangle = new Rectangle(x, y, width, height);
//}
//public void Load(int x, int y, int width, int height, string buttonText)
//{
//    this.x = x;
//    this.y = y;
//    this.width = width;
//    this.height = height;
//    center = new Point(x + width / 2, y + height / 2);
//    clickRectangle = new Rectangle(x, y, width, height);
//    text = buttonText;
//}
#endregion old Loads - reworked 2019-10-26

#region old draw - reworked 2019-10-26
//public void Draw(Graphics g)
//{
//    using (SolidBrush sb = new SolidBrush(fontColor))
//    {
//        Pen p = new Pen(color, 5);

//        PointF txtPos = new PointF();
//        txtPos.X = center.X - g.MeasureString(text, font).Width / 2;
//        txtPos.Y = center.Y - g.MeasureString(text, font).Height / 2;
//        if (visible)
//        {
//            g.DrawRectangle(p, x, y, width, height);
//            g.DrawString(text, font, sb, txtPos);
//        }
//        p.Dispose();
//    }
//}
#endregion old draw - reworked 2019-10-26