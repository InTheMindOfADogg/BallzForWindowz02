using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.DrawableParts
{
    class DrawableObject
    {
        public string ClsName { get { return clsName; } }
        public bool DrawDbgTxt { get { return drawDbgTxt; } set { drawDbgTxt = value; } }
        public bool Visible { get { return visible; } set { visible = value; } }

        //public int Alpha { get { return alpha; } }
        //public int Red { get { return red; } }
        //public int Green { get { return green; } }
        //public int Blue { get { return blue; } }
        //public Color Color { get { return color; } }        

        protected string clsName = "DrawableObject";
        bool drawDbgTxt = true;
        protected bool visible = true;

        protected Color color = Color.Black;
        protected int alpha = 255;
        protected int red = 0;
        protected int green = 0;
        protected int blue = 0;




        protected void SetColor(Color c) { _SetColor(c.A, c.R, c.G, c.B); }
        protected void SetColor(int alpha, int red, int green, int blue){_SetColor(alpha, red, green, blue);}
        private void _SetColor(int alpha, int red, int green, int blue)
        {
            this.alpha = alpha;
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.color = Color.FromArgb(alpha, red, green, blue);
        }

    }
}


#region 2019-10-26 removed the position and size storage from here so that I can use different data type for position and size depending on function
//public int X { get { return x; } /*set { x = value; }*/ }
//public int Y { get { return y; } /*set { y = value; }*/ }
//public int Width { get { return width; } }
//public int Height { get { return height; } }
//protected int x;
//protected int y;
//protected int width;
//protected int height;
//protected void SetPosition(int x, int y) { this.x = x; this.y = y; }
//protected void SetSize(int width, int height) { this.width = width; this.height = height; }
#endregion 2019-10-26 removed the position and size storage from here so that I can use different data type for position and size depending on function