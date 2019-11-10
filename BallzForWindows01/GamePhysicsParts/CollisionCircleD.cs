using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BallzForWindows01.GamePhysicsParts
{
    using System.Drawing;
    using DrawableParts;

    class CollisionCircleD : CircleDV2
    {
        public List<CollisionPoint> CollisionPointList { get { return cplist; } }

        List<CollisionPoint> cplist;
        int cpCount = 1;
        protected double cpBoxSize = 2;

        public CollisionCircleD() : base() { _Init(); }
        public CollisionCircleD(double x, double y, double radius, double rotation = 0) : base(x, y, radius, rotation) { _Init(); }
        void _Init()
        {
            cplist = new List<CollisionPoint>();
        }

        public void Load(double x, double y, double radius, double rotation, int collisionPoints)
        {
            base.Load(x, y, radius, rotation);
            _Load(collisionPoints);
        }
        void _Load(int collisionPoints )
        {
            cpCount = collisionPoints;
            double tempRot = rot;
            tempRot = 0;    // for testing

            for(int i = 0; i < cpCount; i++)
            {
                cplist.Add(CreateCollisionPoint(center.X, center.Y, radius, tempRot));
                tempRot += Math.PI / 4;

            }
        }
        protected override void _Update(double x, double y, double radius, double rotation)
        {
            base._Update(x, y, radius, rotation);
            UpdateCollisionPointList(x, y, radius, rotation);
        }
        void UpdateCollisionPointList(double x, double y, double radius, double rotation)
        {
            PointD cppos = new PointD();
            double tempRot = rotation;
            double rotAdjust = (Math.PI * cplist.Count) / 2;
            for (int i = 0; i < cplist.Count; i++)
            {
                cppos.X = x + radius * Math.Cos(tempRot - rotAdjust);
                cppos.Y = y + radius * Math.Sin(tempRot - rotAdjust);
                cplist[i].Set(cppos.X, cppos.Y, cpBoxSize, cpBoxSize);
                tempRot += Math.PI / 4;
            }
        }

        protected override void _Draw(Graphics g)
        {
            base._Draw(g);
            DrawCollisionPointList(g);
        }
        
        void DrawCollisionPointList(Graphics g)
        {            
            for (int i = 0; i < cplist.Count; i++)
            {
                cplist[i].Draw(g);
            }
        }
        public int CollisionPointHit()
        {
            for (int i = 0; i < cplist.Count; i++) { if (cplist[i].PointHit) { return i; } }
            return -1;
        }
        CollisionPoint CreateCollisionPoint(double x, double y, double radius, double rotation)
        {
            PointD cppos = new PointD();
            cppos.X = (x + radius * Math.Cos(rotation));
            cppos.Y = (y + radius * Math.Sin(rotation));
            CollisionPoint cp = new CollisionPoint(x,y,cpBoxSize,cpBoxSize);
            return cp;
        }


    }
}
