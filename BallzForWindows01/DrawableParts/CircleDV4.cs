using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace BallzForWindows01.DrawableParts
{
    
    using GamePhysicsParts;
    using Structs;

    class CircleDV4 : DrawableObject
    {



        PointD position;
        double radius;
        RotationD rotation;

        Color fillColor = Color.Green;
        Color outlineColor = Color.Green;


        public CircleDV4()
        {
            clsName = "CircleDV4";
            position = new PointD(30, 30);
            radius = 15;
            rotation = new RotationD();
        }
        public void Load(double x, double y, double radius, double rotation)
        {
            position.Set(x, y);
            this.radius = radius;
            this.rotation.Set(rotation);
            position.SetStartingPosition(x, y);
            this.rotation.SetStartingRotation(rotation);
        }
        public void Update(MouseControls mc, KeyboardControls01 kc)
        {

        }
        public void Draw(Graphics g, Pen p, SolidBrush sb)
        {
            FillCircle(g, sb);
            DrawOutline(g, p);
        }
        
        public void Reset()
        {
            position.Reset();
            rotation.Reset();
        }
        public void CleanUp()
        {

        }

        public void FillCircle(Graphics g, SolidBrush sb)
        {
            sb.Color = fillColor;
            g.FillEllipse(sb, (float)position.fX - ((float)radius), position.fY - ((float)radius), (float)radius * 2, (float)radius * 2);   // draw ball outline

        }
        public void DrawOutline(Graphics g, Pen p, float width = 1)
        {
            p.Color = outlineColor;
            p.Width = 1;
            g.DrawEllipse(p, (float)position.fX - ((float)radius), position.fY - ((float)radius), (float)radius * 2, (float)radius * 2);   // draw ball outline
        }
        public void DrawCenterMarker(Graphics g, SolidBrush sb)
        {
            float len = 4;      // length of sides for center marker
            sb.Color = Color.Red;
            g.FillRectangle(sb, position.fX - (len / 2), position.fY - (len / 2), len, len);   // draw center marker for testing
        }

        public bool InCircle(PointD p) { return position.DistanceTo(p.X, p.Y) < radius ? true : false; }
        public bool InCircle(double x, double y) { return position.DistanceTo(x, y) < radius ? true : false; }

        /// <summary>
        /// Sets fill color and outline color to color c.
        /// </summary>
        /// <param name="c"></param>
        public void SetCircleColor(Color c) { fillColor = outlineColor = c; }
        public void SetFillColor(Color c) { fillColor = c; }
        public void SetOutlineColor(Color c) { outlineColor = c; }

    }
}
