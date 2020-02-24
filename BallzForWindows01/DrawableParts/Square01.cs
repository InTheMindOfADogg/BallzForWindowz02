using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.DrawableParts
{
    using GamePhysicsParts;

    class Square01 : DrawableObject
    {
        RectangleD rect;

        public Square01()
        {
            rect = new RectangleD(30, 30, 20, 20);
            color = Color.Beige;
        }
        public void Load(double x, double y, double width, double height, bool centerOnpoint = true)
        {
            rect.Set(x, y, width, height);
            if (centerOnpoint) { rect.CenterOnXY(x,y); }
        }
        public void Update()
        {

        }
        public void Draw(Graphics g, Pen p, SolidBrush sb)
        {

        }
        public void Reset()
        {

        }
        public void CleanUp()
        {

        }
    }
}
