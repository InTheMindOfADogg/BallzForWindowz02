using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.GamePhysicsParts
{
    class SizeD
    {
        double width;
        double height;

        public double Width { get { return width; } set { width = value; } }
        public double Height { get { return height; } set { height = value; } }

        public float fWidth { get { return (float)width; } }
        public float fHeight { get { return (float)height; } }

        public int iWidth { get { return (int)width; } }
        public int iHeight { get { return (int)height; } }

        public SizeD() { Set(0, 0); }
        public SizeD(double width, double height) { Set(width, height); }        
        public void Set(double width, double height) { this.width = width; this.height = height; }

        
        public override string ToString() { return $"{{Width={width}, Height={height}}}"; }
        public Size ToSize() { return new Size((int)width, (int)height); }
        public SizeF ToSizeF() { return new SizeF((float)width, (float)height); }

    }
}
