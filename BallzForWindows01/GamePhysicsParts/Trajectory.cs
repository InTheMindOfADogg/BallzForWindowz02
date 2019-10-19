using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.GamePhysicsParts
{
    class Trajectory
    {
        public bool EndPointSet { get { return endPointSet; } set { endPointSet = value; } }

        double x, y;
        double endx, endy;
        double rot;
        double distance;
        bool endPointSet = false;

        public Trajectory()
        {
            x = 0;
            y = 0;
            endx = 0;
            endy = 0;
            rot = 0;
            distance = 100;
        }
        public void Load(float startx, float starty) { x = startx; y = starty; }
        public void SetEndPoint(double ex, double ey) { _SetEndPoint(ex, ey); }
        public void Update()
        {
        }

        public void Draw(Graphics g)
        {
            DebugDraw(g);
            if (endPointSet)
            {
                g.DrawLine(Pens.Red, (float)x, (float)y, (float)endx, (float)endy);
            }
        }

        
        public void Reset()
        {
            endx = 0;
            endy = 0;
            endPointSet = false;
        }

        void DebugDraw(Graphics g)
        {
            PointF fpos = new PointF(500, 20);
            Font f = new Font("Arial", 12, FontStyle.Regular);

            DrawString(g, f, $"start pos: {{{x:N2}, {y:N2} }}", ref fpos);
            DrawString(g, f, $"end pos: {{{endx:N2}, {endy:N2} }}", ref fpos);
            DrawString(g, f, $"distance: {distance:N2}", ref fpos);
            DrawString(g, f, $"angle: {rot:N2}", ref fpos);

            f.Dispose();
        }
        void DrawString(Graphics g, Font f, string str, ref PointF fontPos)
        {
            g.DrawString(str, f, Brushes.Black, fontPos);
            fontPos.Y += g.MeasureString(str, f).Height;
        }

        void SetDistance(double ex, double ey)
        {
            endx = ex;
            endy = ey;
            double xdiff = endx - x;
            double ydiff = endy - y;            
            double sumSqrs = (xdiff * xdiff) + (ydiff * ydiff);            
            distance = Math.Sqrt(sumSqrs);

        }
        double CalcDistance(double sx, double sy, double ex, double ey)
        {
            double xdiff = ex - sx;
            double ydiff = ey - sy;
            double sumSqrs = (xdiff * xdiff) + (ydiff * ydiff);
            return Math.Sqrt(sumSqrs);
        }
        void SetRotation()
        {
            double oppLen = CalcDistance(endx, y, endx, endy);
            double hypLen = distance;
            double angle = Math.Cos(oppLen / hypLen);
            rot = angle * 180 / Math.PI;
        }
        void _SetEndPoint(double ex, double ey)
        {
            // Not working

            endx = ex;
            endy = ey;
            
            SetDistance(ex, ey);
            SetRotation();
            endx = x + distance * Math.Cos(rot) * 180 / Math.PI;
            endy = y + distance * Math.Sin(rot) * 180 / Math.PI;
            endPointSet = true;
        }


    }
}
