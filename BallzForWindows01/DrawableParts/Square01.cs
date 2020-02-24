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
        PointD position;
        SizeD size;

        public Square01()
        {
            position = new PointD(30,30);
            size = new SizeD(20, 20);
            color = Color.Beige;
        }
        public void Load(double x, double y, double width, double height)
        {
            position.Set(x, y);
            size.Set(width, height);
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
