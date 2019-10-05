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
        static Random r = new Random();
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


        Point center;

        int hits;
        public int Hits { get { return hits; } set { hits = value; } }

        Color color;// = Color.Green;
        public Color DrawColor { get { return color; } set { color = value; } }
        
        #region font properties
        Font font;
        Color fontColor = Color.Black;
        int fontSize = 20;
        string fontFamily = "Arial";
        #endregion font properties

        public BasicBlock01()
        {
            
            //argbColor = new ARGBColor();
            x = 0;
            y = 0;
            width = 50;
            height = 50;
            alpha = 255;
            red = 255;
            green = 0;
            blue = 0;
            //argbColor.a = alpha;
            //argbColor.r = red;
            //argbColor.g = green;
            //argbColor.b = blue;
            color = Color.FromArgb(alpha, red, green, blue);
            font = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            hits = 5;
            center = new Point(x + width / 2, y + height / 2);
        }
        public void Load(int x, int y)
        {
            this.x = x;
            this.y = y;
            center = new Point(x + width / 2, y + height / 2);
        }
        public void Load(int x, int y, int width, int height)
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
            SizeF strSize = g.MeasureString(hits.ToString(), font);
            Point strPos = new Point();
            strPos.X = (int)center.X - (int)strSize.Width / 2;
            strPos.Y = (int)center.Y - (int)strSize.Height / 2;
            using (SolidBrush sb = new SolidBrush(color))
            {
                g.FillRectangle(sb, x, y, width, height);
                
            }
            using (SolidBrush sb = new SolidBrush(fontColor))
            {
                g.DrawString(hits.ToString(), font, sb, strPos);
            }
        }
        
        private void ConfigureFont()
        {

        }
    }
}
