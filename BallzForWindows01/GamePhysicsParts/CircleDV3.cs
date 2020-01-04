using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.GamePhysicsParts
{
    using DrawableParts;

    class CircleDV3 : DrawableObject
    {

        protected PointD position;
        protected double radius = 15;
        protected double rotation = 0;

        public CircleDV3()
        {
            clsName = "CircleDV3";
            position = new PointD(30, 30);
            radius = 15;
            rotation = 0;
        }

        protected void Load(double x, double y, double radius, double rotation)
        {
            position.Set(x, y);
            this.radius = radius;
            this.rotation = rotation;
        }

        protected void DrawCircle(Graphics g)
        {
            float len = 4;      // length of sides for center marker
            Pen p = new Pen(color);
            g.FillRectangle(Brushes.Red, position.fX - (len / 2), position.fY - (len / 2), len, len);   // draw center marker for testing            
            //g.DrawEllipse(Pens.Green, (float)center.fX - ((float)radius), center.fY - ((float)radius), (float)radius * 2, (float)radius * 2);   // draw ball outline
            g.DrawEllipse(p, (float)position.fX - ((float)radius), position.fY - ((float)radius), (float)radius * 2, (float)radius * 2);   // draw ball outline

            //g.DrawLine(p, position.fX - 10, position.fY, position.fX + 10, position.fY);  // test to verify where position is
            p.Dispose();
        }

        protected bool InCircle(double x, double y)
        {
            if (position.DistanceTo(x, y) < radius) { return true; }
            return false;
        }

        public void SetCircleColor(Color c) { SetColor(c); }



    }
}

#region removed _Init and steps into constructor 2020-01-04
//void _Init(double x, double y, double radius, double rotation)
//{
//    clsName = "CircleDV3";
//    position = new PointD(x, y);
//    this.radius = radius;
//    this.rotation = rotation;
//}
#endregion removed _Init and steps into constructor 2020-01-04

#region removed _Load and moved into public Load 2020-01-04
//void _Load(double x, double y, double radius, double rotation)
//{
//    position.Set(x, y);
//    this.radius = radius;
//    this.rotation = rotation;
//}
#endregion removed _Load and moved into public Load 2020-01-04

#region removed 2019-12-07
//public void Draw(Graphics g) { DrawCircle(g); }

//public virtual void Load(double x, double y, double radius, double rotation)
//{
//    _Load(x, y, radius, rotation);
//}
#endregion removed 2019-12-07
