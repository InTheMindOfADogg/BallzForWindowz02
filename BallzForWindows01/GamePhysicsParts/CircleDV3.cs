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
        protected double radius = 0;
        protected double rotation = 0;

        public CircleDV3()
        {
            _Init(0, 0, 20, 0);
        }
        public CircleDV3(double x, double y, double radius, double rotation = 0)
        {
            _Init(x, y, radius, rotation);
        }
        
        protected void Load(double x, double y, double radius, double rotation)
        {
            _Load(x, y, radius, rotation);
        }
        

        void _Init(double x, double y, double radius, double rotation)
        {
            clsName = "CircleDV3";
            position = new PointD(x, y);
            this.radius = radius;
            this.rotation = rotation;
        }
        void _Load(double x, double y, double radius, double rotation)
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
            p.Dispose();
        }

        public void SetCircleColor(Color c) { SetColor(c); }

    }
}

#region removed 2019-12-07
//public void Draw(Graphics g) { DrawCircle(g); }

//public virtual void Load(double x, double y, double radius, double rotation)
//{
//    _Load(x, y, radius, rotation);
//}
#endregion removed 2019-12-07
