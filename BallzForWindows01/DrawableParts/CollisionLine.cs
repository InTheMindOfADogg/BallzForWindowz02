using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.DrawableParts
{
    using GamePhysicsParts;

    class CollisionLine : DrawableObject
    {

        public List<CollisionPoint> CpList { get { return cpList; } }

        PointD p1, p2;
        double rot = 0;
        double length = 0;
        double angleDegrees = 0;
        int thickness = 3;
        bool collision = false;

        CollisionPoint cp1;
        CollisionPoint cp2;
        List<CollisionPoint> cpList = new List<CollisionPoint>();

        public bool Collision { get { return collision; } }

        public CollisionLine()
        {
            cp1 = new CollisionPoint();
            cp2 = new CollisionPoint();
            p1 = new PointD();
            p2 = new PointD();
            SetColor(255, 230, 175, 50);    // set line color
        }
        public void Load(double startx, double starty, double length, double rotation, int thickness)
        {
            p1.X = startx;
            p1.Y = starty;
            this.rot = rotation;
            this.length = length;
            this.thickness = thickness;
            SetEndPoint();

            // loading cps
            cp1.Load(p1.X, p1.Y, this.thickness);
            cp2.Load(p2.X, p2.Y, this.thickness);
            cpList.Add(cp1);
            cpList.Add(cp2);
        }
        void SetEndPoint()
        {
            p2.X = p1.X + length * Math.Cos(rot);
            p2.Y = p1.Y + length * Math.Sin(rot);
        }

        
        public void Update(double adjustRotation = 0)
        {
            if (adjustRotation != 0)
            {
                rot += adjustRotation;
                rot = rot % (2 * Math.PI);
                if (rot < 0) { angleDegrees = (2 * Math.PI) + rot; }
                SetEndPoint();
            }
            DbgFuncs.AddStr($"CollisionLine.rot(deg): {(angleDegrees * 180 / Math.PI):N2}");

        }

        public void Draw(Graphics g)
        {
            Pen pen = new Pen(color, thickness);
            g.DrawLine(pen, (float)p1.X, (float)p1.Y, (float)p2.X, (float)p2.Y);
            pen.Dispose();

            //cp.Draw(g);
            DrawCollisionPoints(g);
        }

        public void CheckCollision(double px, double py)
        {
            for (int i = 0; i < cpList.Count; i++)
            {
                collision = cpList[i].CheckForCollision(px, py);
                //DbgFuncs.AddStr($"CollisionLine.CollisionPoint[{i}] collision: {cpList[i].Collision}");
            }
        }

        void DrawCollisionPoints(Graphics g)
        {
            for (int i = 0; i < cpList.Count; i++)
            {
                cpList[i].Draw(g);
            }
        }
    }


}
