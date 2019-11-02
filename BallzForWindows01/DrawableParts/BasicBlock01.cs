using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.DrawableParts
{
    class BasicBlock01 : DrawableObject
    {

        #region font settings
        Font font;
        Color fontColor = Color.Black;
        int fontSize = 20;
        string fontFamily = "Arial";
        #endregion font settings

        static Random r = new Random();

        Point center;
        int hits;
        int x, y, width, height;

        public int X { get { return x; } /*set { x = value; }*/ }
        public int Y { get { return y; } /*set { y = value; }*/ }
        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public int Hits { get { return hits; } set { hits = value; } }
        public Color DrawColor { get { return color; } set { color = value; } }


        public BasicBlock01()
        {
            x = 0;
            y = 0;
            width = 50;
            height = 50;
            alpha = 255;
            red = 255;
            green = 0;
            blue = 0;
            //color = Color.FromArgb(alpha, red, green, blue);            
            SetColor(alpha, red, green, blue);
            hits = 5;
            center = new Point(x + width / 2, y + height / 2);
            ConfigureFont();
        }
        public void Load(int x, int y) { _Load(x, y, width, height); }
        public void Load(int x, int y, int width, int height) { _Load(x, y, width, height); }
        void _Load(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            center = new Point(x + width / 2, y + height / 2);
            this.height = height;
        }
        public void SetNumberOfHits(int min = 1, int max = 1)
        {
            hits = r.Next(min, max);
        }
        public void Update()
        {

        }

        public void Draw(Graphics g)
        {
            SolidBrush sb = new SolidBrush(color);

            SizeF strSize = g.MeasureString(hits.ToString(), font);
            Point strPos = new Point();
            strPos.X = (int)center.X - (int)strSize.Width / 2;
            strPos.Y = (int)center.Y - (int)strSize.Height / 2;

            g.FillRectangle(sb, x, y, width, height);
            sb.Color = fontColor;
            g.DrawString(hits.ToString(), font, sb, strPos);
            sb.Dispose();

        }
        public void CleanUp()
        {
            if (font != null) { font.Dispose(); }
        }

        private void ConfigureFont()
        {
            font = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            fontColor = Color.Black;
        }

        protected void SetPosition(int x, int y) { this.x = x; this.y = y; }
        protected void SetSize(int width, int height) { this.width = width; this.height = height; }
    }
}

#region 2019-10-26 old draw block list with usings
//public void Draw(Graphics g)
//{
//    SizeF strSize = g.MeasureString(hits.ToString(), font);
//    Point strPos = new Point();
//    strPos.X = (int)center.X - (int)strSize.Width / 2;
//    strPos.Y = (int)center.Y - (int)strSize.Height / 2;

//    using (SolidBrush sb = new SolidBrush(color))
//    {
//        g.FillRectangle(sb, x, y, width, height);

//    }
//    using (SolidBrush sb = new SolidBrush(fontColor))
//    {
//        g.DrawString(hits.ToString(), font, sb, strPos);
//    }
//}
#endregion 2019-10-26 old draw block list with usings

#region old load 2019-10-26
//public void Load(int x, int y)
//{
//    this.x = x;
//    this.y = y;
//    center = new Point(x + width / 2, y + height / 2);
//}
//public void Load(int x, int y, int width, int height)
//{
//    this.x = x;
//    this.y = y;
//    this.width = width;
//    center = new Point(x + width / 2, y + height / 2);
//    this.height = height;
//}
#endregion old load 2019-10-26

#region Moved to DrawableObject 5/13/2017
// Moved to DrawableObject 5/13/2017
//int x;
//int y;
//int width;
//int height;
//public int Width { get { return width; } }
//public int Height { get { return height; } }
//int alpha;
//int red;
//int green;
//int blue;
#endregion