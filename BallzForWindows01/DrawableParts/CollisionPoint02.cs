using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.DrawableParts
{
    using GamePhysicsParts;

    // Going to build cp that uses detection range instead of using box (aka hit circle)
    class CollisionPoint02 : DrawableObject
    {
        public PointD Pos { get { return pos; } }
        public double DetectionRadius { get { return cCircle.Radius; } }
        public double DistanceFromOrigin { get { return distanceFromOrigin; } set { distanceFromOrigin = value; } }

        public bool PointHit { get { return pointHit; } set { pointHit = value; } }
        public bool Collision { get { return collision; } set { collision = value; } }

        PointD pos;
        CircleD cCircle;

        double distanceFromOrigin = 0;

        bool collision = false;
        bool pointHit = false;

        public CollisionPoint02()
        {
            clsName = "CollisionPoint02";
            pos = new PointD(10, 10);
            cCircle = new CircleD(pos.X, pos.Y, 10);
        }

        public void Load(double x, double y, double detectionRadius)
        {
            //pos = new PointD(x, y);
            pos.Set(x, y);
            cCircle.Load(pos.X, pos.Y, detectionRadius);
        }

        public void Set(PointD p) { pos.Set(p); }
        public void Set(double x, double y) { pos.Set(x, y); }



        public void Update()
        {

        }

        public void Draw(Graphics g, Pen p, SolidBrush sb)
        {
            if (!visible) { return; }

            p.Color = color;
            p.Width = 1;
            //g.DrawRectangle(p, pos.fX - 1, pos.fY - 1, 2, 2); // center point for testing

            cCircle.DrawOutline(g, p);

            // pointHit is trigger for any collision point on the ball
            if (pointHit)
            {
                sb.Color = Color.Green;
                cCircle.FillCircle(g, sb);
            }

            // collision is trigger for a non player object (aka, anything but the ball at this time 2020-01-25)
            if (collision)
            {
                sb.Color = Color.FromArgb(105, 200, 10, 10);
                cCircle.FillCircle(g, sb);
            }
        }

        public void Reset()
        {
            pointHit = false;
            collision = false;
        }


        //public bool CheckForCollision(CollisionPoint02 cp)
        //{
        //    double distance = pos.DistanceTo(cp.Pos);
        //    double detectionRadius = this.DetectionRadius + cp.DetectionRadius;
        //    return (distance <= detectionRadius) ? true : false;
        //}
        public bool CheckForCollision(CollisionPoint02 cp) { return (collision = (pos.DistanceTo(cp.Pos) <= (DetectionRadius + cp.DetectionRadius)) ? true : false); }
        public bool CheckForCollision(PointD p) { return (collision = cCircle.InCircle(p)); }




        public static void ListCollisionPoints(string callingFnId, List<CollisionPoint> cplist)
        {
            string cpstr = "";
            for (int i = 0; i < cplist.Count; i++)
            {
                cpstr += cplist[i].Pos.ToString() + ", ";
            }
            DbgFuncs.AddStr(callingFnId, $"cpstr: {cpstr}");
        }
    }
}
