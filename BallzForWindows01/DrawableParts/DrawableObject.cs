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
        public int X { get { return x; } /*set { x = value; }*/ }
        public int Y { get { return y; } /*set { y = value; }*/ }
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public bool Visible { get { return visible; } set { visible = value; } }

        public int Alpha { get { return alpha; } }
        public int Red { get { return red; } }
        public int Green { get { return green; } }
        public int Blue { get { return blue; } }
        public Color Color { get { return color; } }

        protected int x;
        protected int y;
        protected int width;
        protected int height;

        protected int alpha;
        protected int red;
        protected int green;
        protected int blue;
        protected Color color;

        protected bool visible = true;

        protected void SetPosition(int x, int y) { this.x = x; this.y = y; }
        protected void SetSize(int width, int height) { this.width = width; this.height = height; }
        protected void SetColor(int a, int r, int g, int b)
        {
            this.alpha = a;
            this.red = r;
            this.green = g;
            this.blue = b;
            this.color = Color.FromArgb(alpha, red, green, blue);
        }




    }
}
