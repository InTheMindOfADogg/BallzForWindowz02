using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.GamePhysicsParts
{
    using DrawableParts;

    class CircleD : DrawableObject
    {
        public double Radius { get { return radius; } }

        protected PointD position;
        protected double radius = 15;

        public CircleD()
        {
            clsName = "CircleD";
            position = new PointD(30, 30);
            radius = 15;
            color = Color.Black;
        }
        public CircleD(double x, double y, double radius)
        {
            clsName = "CircleD";
            position = new PointD(30, 30);
            this.radius = radius;
            color = Color.Black;
        }

        public void Load(double x, double y, double radius)
        {
            position.Set(x, y);
            this.radius = radius;
        }
        public void Draw(Graphics g, Pen p, SolidBrush sb)
        {
            //string fnId = AssistFunctions.FnId(clsName, "Draw");
            //DbgFuncs.AddStr(fnId, $"CircleD.color: {color}");
            DrawOutline(g, p);
            FillCircle(g, sb);
        }
        public void DrawOutline(Graphics g, Pen p) { g.DrawEllipse(p, position.fX - ((float)radius), position.fY - ((float)radius), (float)radius * 2, (float)radius * 2); }
        public void FillCircle(Graphics g, SolidBrush sb) { g.FillEllipse(sb, position.fX - ((float)radius), position.fY - ((float)radius), (float)radius * 2, (float)radius * 2); }

        public void Set(PointD p) { position.Set(p); }
        public void Set(double x, double y) { position.Set(x, y); }


        public bool InCircle(PointD p) { return position.DistanceTo(p.X, p.Y) < radius ? true : false; }
        public bool InCircle(double x, double y) { return position.DistanceTo(x, y) < radius ? true : false; }

        public void SetCircleColor(Color c) { SetColor(c); }



    }
}
