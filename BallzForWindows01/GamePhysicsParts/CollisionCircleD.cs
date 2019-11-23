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
        string clsName = "CollisionCircleD";

        List<CollisionPoint> cplist;
        int cpCount = 1;
        protected double cpBoxSize = 2;

        double middleCpidx = 0;
        bool[] cpHitArr = null;

        public List<CollisionPoint> CollisionPointList { get { return cplist; } }
        public double MiddleCPIdx { get { return middleCpidx; } }

        public CollisionCircleD() : base() { _Init(); }
        public CollisionCircleD(double x, double y, double radius, double rotation = 0) : base(x, y, radius, rotation) { _Init(); }
        void _Init()
        {
            cplist = new List<CollisionPoint>();
        }

        public void Load(double x, double y, double radius, double rotation, int collisionPoints)
        {
            base.Load(x, y, radius, rotation);
            //_Load(collisionPoints);
            _Load2(collisionPoints);
        }
        void _Load(int collisionPoints)
        {
            cpCount = collisionPoints;
            middleCpidx = (int)(((double)cpCount / 2.0) + 0.5) - 1;
            cpHitArr = new bool[cpCount];
            double tempRot = rot;
            tempRot = 0;    // for testing

            for (int i = 0; i < cpCount; i++)
            {
                cplist.Add(CreateCollisionPoint(center.X, center.Y, radius, tempRot));
                tempRot += Math.PI / 4;
            }
        }

        
        void _Load2(int collisionPoints)
        {
            cpCount = collisionPoints;
            middleCpidx = (int)(((double)cpCount / 2.0) + 0.5) - 1;
            //middleCpidx = (((double)cpCount / 2.0) + 0.5) - 1;
            cpHitArr = new bool[cpCount];
            double tempRot = rot;
            tempRot = 0;    // for testing            
            spaceBetween = (Math.PI / 2) / ((double)cpCount / 2.0);
            tempRot -= (spaceBetween * middleCpidx);    // adjusting first point so that the points line up on front of ball
            //firstCpAngle = tempRot - (spaceBetween * middleCpidx);
            
            for (int i = 0; i < cpCount; i++)
            {
                //calculatedCpAngle = firstCpAngle + (spaceBetween * i);
                cplist.Add(CreateCollisionPoint(center.X, center.Y, radius, tempRot));
                tempRot += spaceBetween;
            }
        }
        protected override void _Update(double x, double y, double radius, double rotation)
        {
            base._Update(x, y, radius, rotation);
            //UpdateCollisionPointList(x, y, radius, rotation); // commented for testing
            UpdateCollisionPointList2(x, y, radius, rotation); 
            DbgFuncs.AddStr($"[{clsName}._Update]: middleCpidx: {middleCpidx}");
            DbgFuncs.AddStr($"[{clsName}._Update]: spaceBetween: {spaceBetween * 180 / Math.PI}");
            DbgFuncs.AddStr($"[{clsName}._Update]: firstCpAngle: {firstCpAngle * 180 / Math.PI}");
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

        double spaceBetween = 0;
        double firstCpAngle = 0;
        void UpdateCollisionPointList2(double x, double y, double radius, double rotation)
        {
            PointD cppos = new PointD();
            double tempRot = rotation;
            double rotAdjust = (Math.PI * cplist.Count) / 2;
            spaceBetween = (Math.PI / 2) / ((double)cpCount / 2.0);
            //firstCpAngle = tempRot - (spaceBetween * middleCpidx);
            tempRot-= (spaceBetween * middleCpidx);

            for (int i = 0; i < cplist.Count; i++)
            {
                cppos.X = x + radius * Math.Cos(tempRot);
                cppos.Y = y + radius * Math.Sin(tempRot);
                cplist[i].Set(cppos.X, cppos.Y, cpBoxSize, cpBoxSize);
                tempRot += spaceBetween;
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
            for (int i = 0; i < cplist.Count; i++) { if (cplist[i].PointHit && i != middleCpidx) { return i; } }
            return -1;
        }



        List<int> TriggeredHitPointIdxList()
        {
            List<int> triggeredHitIdx = new List<int>();

            for (int i = 0; i < cplist.Count; i++)
            {
                cpHitArr[i] = false;
                if (cplist[i].PointHit)
                {
                    cpHitArr[i] = true;
                    triggeredHitIdx.Add(i);
                }

            }
            return triggeredHitIdx;
        }
        CollisionPoint CreateCollisionPoint(double x, double y, double radius, double rotation)
        {
            PointD cppos = new PointD();
            cppos.X = (x + radius * Math.Cos(rotation));
            cppos.Y = (y + radius * Math.Sin(rotation));
            CollisionPoint cp = new CollisionPoint(x, y, cpBoxSize, cpBoxSize);
            return cp;
        }


    }
}



#region old CollisionPointHit logic
//public int CollisionPointHit()
//{
//    List<int> pointsHit = new List<int>();
//    //int numberHit = 0;
//    for (int i = 0; i < cplist.Count; i++)
//    {
//        cpHitArr[i] = false;
//        if (cplist[i].PointHit)
//        {
//            cpHitArr[i] = true;
//            //numberHit++;
//            pointsHit.Add(i);
//        }
//    }
//    if (pointsHit.Count== 1)
//    {
//        return pointsHit[0];
//    }
//    if (pointsHit.Count > 1)
//    {
//        double total = 0;
//        for (int i = 0; i < pointsHit.Count; i++)
//        {
//            total += pointsHit[i] + 1;
//        }
//        double avg = total / pointsHit.Count + 0.5;
//        return (int)avg;
//    }
//    return -1;
//}
#endregion old CollisionPointHit logic