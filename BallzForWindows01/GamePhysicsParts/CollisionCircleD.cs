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

        


        List<CollisionPoint> cplist;
        int cpCount = 1;
        double cpBoxSize = 2;
        Color dfltColorCollisionPoints = Color.FromArgb(255, 175, 30, 50);

        double middleCpidx = 0;
        bool[] cpHitArr = null;

        public List<CollisionPoint> CollisionPointList { get { return cplist; } }
        public double MiddleCPIdx { get { return middleCpidx; } }

        public CollisionCircleD() : base() { _Init(); }
        public CollisionCircleD(double x, double y, double radius, double rotation = 0) : base(x, y, radius, rotation) { _Init(); }
        void _Init()
        {
            clsName = "CollisionCircleD";
            cplist = new List<CollisionPoint>();
        }
        public void Load(double x, double y, double radius, double rotation, int collisionPoints)
        {
            base.Load(x, y, radius, rotation);
            _Load(collisionPoints, cpBoxSize);
        }
        public void Load(double x, double y, double hitBoxSideLength, double radius, double rotation, int collisionPoints)
        {            
            base.Load(x, y, radius, rotation);            
            _Load(collisionPoints, hitBoxSideLength);
        }
        
        void _Load(int collisionPoints, double hitBoxSideLength)
        {
            cpCount = collisionPoints;
            cpBoxSize = hitBoxSideLength;
            if(cpCount %2 == 0) { cpCount++; }      // adds 1 collision point if there is an even number
            middleCpidx = (int)(((double)cpCount / 2.0) + 0.5) - 1;
            cpHitArr = new bool[cpCount];
            double tempRot = rot;
            tempRot = 0;    // for testing            
            spaceBetween = (Math.PI / 2) / ((double)cpCount / 2.0);
            tempRot -= (spaceBetween * middleCpidx);    // adjusting first point so that the points line up on front of ball
            Color cpColor = Color.Blue;
            for (int i = 0; i < cpCount; i++)
            {
                
                cplist.Add(CreateCollisionPoint(center.X, center.Y, cpBoxSize, radius, tempRot, cpColor));
                tempRot += spaceBetween;
                if (i == 0) { cpColor = dfltColorCollisionPoints; }
            }
        }
        CollisionPoint CreateCollisionPoint(double x, double y, double radius, double rotation)
        {
            return _CreateCollisionPoint(x, y, radius, cpBoxSize, rotation, dfltColorCollisionPoints);
        }
        CollisionPoint CreateCollisionPoint(double x, double y, double radius, double rotation, Color c)
        {
            return _CreateCollisionPoint(x, y, cpBoxSize, radius, rotation, c);
        }
        CollisionPoint CreateCollisionPoint(double x, double y, double boxSize, double radius, double rotation, Color c)
        {
            return _CreateCollisionPoint(x, y, boxSize, radius, rotation, c);
        }
        CollisionPoint _CreateCollisionPoint(double x, double y, double boxSize, double radius, double rotation, Color c)
        {
            PointD cppos = new PointD();
            cppos.X = (x + radius * Math.Cos(rotation));
            cppos.Y = (y + radius * Math.Sin(rotation));
            CollisionPoint cp = new CollisionPoint(x, y, boxSize, boxSize, c);
            return cp;
        }


        protected override void _Update(double x, double y, double radius, double rotation)
        {
            base._Update(x, y, radius, rotation);
            UpdateCollisionPointList(x, y, radius, rotation);
            if(DrawDbgTxt)
            {
                DbgFuncs.AddStr($"[{clsName}._Update]: middleCpidx: {middleCpidx}");
                DbgFuncs.AddStr($"[{clsName}._Update]: spaceBetween: {spaceBetween * 180 / Math.PI}");
                DbgFuncs.AddStr($"[{clsName}._Update]: firstCpAngle: {firstCpAngle * 180 / Math.PI}");
                DbgFuncs.AddStr($"[{clsName}._Update]: cpBoxSize: {cpBoxSize}");
            }
            

        }

        
        double spaceBetween = 0;
        double firstCpAngle = 0;
        void UpdateCollisionPointList(double x, double y, double radius, double rotation)
        {
            PointD cppos = new PointD();
            double tempRot = rotation;
            double rotAdjust = (Math.PI * cplist.Count) / 2;
            spaceBetween = (Math.PI / 2) / ((double)cpCount / 2.0);
            tempRot -= (spaceBetween * middleCpidx);

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
        public int CollisionPointHit2()
        {
            for (int i = 0; i < cplist.Count; i++)
            {
                if (cplist[i].PointHit){return i;}
            }
            return -1;
        }
        
        public List<int> TriggeredHitPointIdxList()
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



        public void SetCircleColor(Color c) { SetColor(c); }


    }
}


#region old UpdateCollisionPointList
//void UpdateCollisionPointList(double x, double y, double radius, double rotation)
//{
//    PointD cppos = new PointD();
//    double tempRot = rotation;
//    double rotAdjust = (Math.PI * cplist.Count) / 2;
//    for (int i = 0; i < cplist.Count; i++)
//    {
//        cppos.X = x + radius * Math.Cos(tempRot - rotAdjust);
//        cppos.Y = y + radius * Math.Sin(tempRot - rotAdjust);
//        cplist[i].Set(cppos.X, cppos.Y, cpBoxSize, cpBoxSize);
//        tempRot += Math.PI / 4;
//    }
//}
#endregion old UpdateCollisionPointList

#region old _Load
//void _Load(int collisionPoints)
//{
//    cpCount = collisionPoints;
//    middleCpidx = (int)(((double)cpCount / 2.0) + 0.5) - 1;
//    cpHitArr = new bool[cpCount];
//    double tempRot = rot;
//    tempRot = 0;    // for testing
//    Color cpColor = Color.Green;
//    for (int i = 0; i < cpCount; i++)
//    {
//        cplist.Add(CreateCollisionPoint(center.X, center.Y, radius, tempRot, cpColor));
//        tempRot += Math.PI / 4;
//    }
//}
#endregion old _Load

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