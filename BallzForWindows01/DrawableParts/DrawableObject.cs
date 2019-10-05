using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BallzForWindows01.DrawableParts
{
    class DrawableObject
    {

        protected int x;
        protected int y;
        protected int width;
        protected int height;

        protected int alpha;
        protected int red;
        protected int green;
        protected int blue;

        protected bool visible = true;

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public bool Visible { get { return visible; } set { visible = value; } }

        public int Alpha { get { return alpha; } }
        public int Red { get { return red; } }
        public int Green { get { return green; } }
        public int Blue { get { return blue; } }
        
        


    }
}
