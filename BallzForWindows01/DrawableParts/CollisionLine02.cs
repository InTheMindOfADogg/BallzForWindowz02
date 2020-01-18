using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.DrawableParts
{
    using GamePhysicsParts;
    class CollisionLine02 : DrawableObject
    {
        public List<CollisionPoint> CpList { get { return cplist; } }

        PointD startPoint, endPoint;
        RotationD rot;
        int thickness = 5;
        double length = 0;
        double spaceBtCp = 10;

        List<CollisionPoint> cplist;// = new List<CollisionPoint>();

        public CollisionLine02()
        {
            clsName = "CollisionLine02";
            startPoint = new PointD();
            endPoint = new PointD();
            rot = new RotationD();

            SetColor(140, 150, 225, 50);    // set line color. green color

            cplist = new List<CollisionPoint>();

        }

        bool negativeSpaceBt = false;
        public void Load(double startx, double starty, double length, double rotation, int thickness, double spaceBtCp)
        {
            

            // make sure that the line is atleast 1 unit long
            if (length < 1) { length = 1; }
            if (spaceBtCp == 0) { spaceBtCp = length; }            
            if (spaceBtCp < 0)
            {
                negativeSpaceBt = true;
                spaceBtCp *= -1;
                rotation += Math.PI;
            }
            if (spaceBtCp > length / 2) { spaceBtCp = length / 2; }


            this.startPoint.X = startx;
            this.startPoint.Y = starty;
            this.length = length;
            this.rot.Set(rotation);
            this.endPoint = rot.PointDFrom(startPoint, this.length);
            this.thickness = thickness;
            this.spaceBtCp = spaceBtCp;
            AddCollisionPoints(this.length, this.spaceBtCp);

        }

        int numOfCp = 0;
        double remainingLength = 0;     // Remaining length not covered by cp (if first cp is placed at start of line and rest are spaced using spaceBtCp)
        double distributeSpacePerCp = 0;
        double actualSpaceBt = 0;

        void AddCollisionPoints(double distance, double space)
        {
            numOfCp = (int)(distance / space);
            remainingLength = length - (numOfCp * space);
            if (remainingLength > 0)
            {
                distributeSpacePerCp = (remainingLength / numOfCp);
            }
            actualSpaceBt = space + distributeSpacePerCp;
            PointD tmp;
            double tmpdist = 0;

            // + 1 b.c first cp is at the start of the line (0 distance)
            for (int i = 0; i < numOfCp + 1; i++)
            {
                tmp = rot.PointDFrom(startPoint, tmpdist);
                cplist.Add(new CollisionPoint(tmp.X, tmp.Y, thickness, Color.Red));
                tmpdist += actualSpaceBt;
            }

        }


        public void Update()
        {
            string fnId = AssistFunctions.FnId(clsName, "Update");
            string cpstr = "";
            DbgFuncs.AddStr(fnId, $"length: {length}");
            DbgFuncs.AddStr(fnId, $"startPoint: {startPoint.ToString()}");
            DbgFuncs.AddStr(fnId, $"endPoint: {endPoint.ToString()}");
            DbgFuncs.AddStr(fnId, $"numOfCp: {numOfCp}");
            //DbgFuncs.AddStr(fnId, $"additionalNumOfCp: {additionalNumOfCp}");
            DbgFuncs.AddStr(fnId, $"spaceBtCp: {spaceBtCp}");
            DbgFuncs.AddStr(fnId, $"remainingLength: {remainingLength}");
            DbgFuncs.AddStr(fnId, $"distributeSpacePerCp: {distributeSpacePerCp}");
            DbgFuncs.AddStr(fnId, $"actualSpaceBt: {actualSpaceBt}");
            DbgFuncs.AddStr(fnId, $"cplist.Count: {cplist.Count}");
            for (int i = 0; i < cplist.Count; i++)
            {
                cpstr += cplist[i].Pos.ToString() + ", ";
            }
            DbgFuncs.AddStr(fnId, $"cpstr: {cpstr}");
        }


        public void Draw(Graphics g)
        {
            Pen pen = new Pen(color, thickness);
            g.DrawLine(pen, startPoint.fX, startPoint.fY, endPoint.fX, endPoint.fY);
            DrawCollisionPoints(g);
            pen.Dispose();
        }


        void DrawCollisionPoints(Graphics g)
        {
            for (int i = 0; i < cplist.Count; i++)
            {
                cplist[i].Draw(g);
            }
        }

    }
}


#region AddCollisionPoints previous versions
////adding 1 to number of cp when first calculating numOfCp
//void AddCollisionPoints(double distance, double space)
//{
//    numOfCp = (int)(distance / space) + 1;      // + 1 b.c first cp is at the start of the line (0 distance)
//    remainingLength = length - (numOfCp * space);
//    // If there is any remaining length, I am thinking about distributing it evenly between the points so that the first and last points are on the ends of the line.
//    if (remainingLength > 0)
//    {
//        distributeSpacePerCp = (remainingLength / space) / numOfCp;
//    }

//    actualSpaceBt = space + distributeSpacePerCp;
//    PointD tmp;
//    double tmpdist = 0;
//    for (int i = 0; i < numOfCp; i++)
//    {
//        tmp = rot.PointDFrom(startPoint, tmpdist);
//        cplist.Add(new CollisionPoint(tmp.X, tmp.Y, thickness, Color.Red));
//        tmpdist += actualSpaceBt;
//    }

//}

////add one to number of cp in for loop
//void AddCollisionPoints2(double distance, double space)
//{
//    numOfCp = (int)(distance / space);
//    remainingLength = length - (numOfCp * space);
//    // If there is any remaining length, I am thinking about distributing it evenly between the points so that the first and last points are on the ends of the line.
//    if (remainingLength > 0)
//    {
//        distributeSpacePerCp = (remainingLength / space) / numOfCp;
//    }
//    actualSpaceBt = space + distributeSpacePerCp;
//    PointD tmp;
//    double tmpdist = 0;

//    // + 1 b.c first cp is at the start of the line (0 distance)
//    for (int i = 0; i < numOfCp + 1; i++)
//    {
//        tmp = rot.PointDFrom(startPoint, tmpdist);
//        cplist.Add(new CollisionPoint(tmp.X, tmp.Y, thickness, Color.Red));
//        tmpdist += actualSpaceBt;
//    }

//}
#endregion AddCollisionPoints previous versions