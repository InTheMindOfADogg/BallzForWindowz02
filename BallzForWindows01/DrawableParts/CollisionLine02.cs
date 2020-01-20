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

        /// <summary>
        /// Negative space between (spaceBtCp) makes line extend in oppisite direction.
        /// </summary>
        /// <param name="startx"></param>
        /// <param name="starty"></param>
        /// <param name="length"></param>
        /// <param name="rotation"></param>
        /// <param name="thickness"></param>
        /// <param name="spaceBtCp"></param>
        public void Load(double startx, double starty, double length, double rotation, int thickness, double spaceBtCp)
        {
            // make sure that the line is atleast 1 unit long
            if (length < 1) { length = 1; }
            if (spaceBtCp == 0) { spaceBtCp = length; }
            if (spaceBtCp < 0) { spaceBtCp *= -1; rotation += Math.PI; }
            if (spaceBtCp > length / 2) { spaceBtCp = length / 2; }

            startPoint.Set(startx, starty);
            this.length = length;
            this.rot.Set(rotation);
            this.endPoint = rot.PointDFrom(startPoint, this.length);
            this.thickness = thickness;
            this.spaceBtCp = spaceBtCp;
            AddCollisionPoints(this.length, this.spaceBtCp);

        }

        void AddCollisionPoints(double distance, double space)
        {
            int numOfCp = 0;
            double remainingLength = 0;     // Remaining length not covered by cp (if first cp is placed at start of line and rest are spaced using spaceBtCp)
            double distributeSpacePerCp = 0;
            double actualSpaceBt = 0;
            numOfCp = (int)(distance / space);
            remainingLength = length - (numOfCp * space);
            if (remainingLength > 0){distributeSpacePerCp = (remainingLength / numOfCp);}
            actualSpaceBt = space + distributeSpacePerCp;
            PointD tmp;
            double tmpdist = 0;

            // + 1 b.c first cp is at the start of the line (0 distance)
            for (int i = 0; i < numOfCp + 1; i++)
            {
                tmp = rot.PointDFrom(startPoint, tmpdist);
                //cplist.Add(new CollisionPoint(tmp.X, tmp.Y, thickness, Color.Red));
                cplist.Add(new CollisionPoint(tmp.X, tmp.Y, thickness * 2, Color.Red));
                tmpdist += actualSpaceBt;
            }

        }


        public void Update()
        {
            //string fnId = AssistFunctions.FnId(clsName, "Update");
            //DbgFuncs.AddStr(fnId, $"length: {length}");
            //DbgFuncs.AddStr(fnId, $"startPoint: {startPoint.ToString()}");
            //DbgFuncs.AddStr(fnId, $"endPoint: {endPoint.ToString()}");
            //DbgFuncs.AddStr(fnId, $"cplist.Count: {cplist.Count}");
            //CollisionPoint.ListCollisionPoints(fnId, cplist);
        }


        public void Draw(Graphics g, bool showCp = false)
        {
            Pen pen = new Pen(color, thickness);
            g.DrawLine(pen, startPoint.fX, startPoint.fY, endPoint.fX, endPoint.fY);
            if (showCp) { DrawCollisionPoints(g); }
            pen.Dispose();

        }

        void DrawCollisionPoints(Graphics g)
        {
            Pen p = new Pen(Color.Black);
            SolidBrush sb = new SolidBrush(Color.Black);
            for (int i = 0; i < cplist.Count; i++) { cplist[i].Draw(g, p, sb); }
            p.Dispose();
            sb.Dispose();
        }
    }
}

#region DrawCollisionPoints Version 2
//void DrawCollisionPoints(Graphics g, Pen p)
//{
//    Color originalPenColor = p.Color;
//    p.Color = Color.Red;
//    SolidBrush collisionFillBrush = new SolidBrush(Color.FromArgb(25, Color.Red));
//    SolidBrush pointHitFillBrush = new SolidBrush(Color.FromArgb(25, Color.Green));
//    for (int i = 0; i < cplist.Count; i++)
//    {
//        cplist[i].Draw(g, p, collisionFillBrush, pointHitFillBrush);
//    }
//    collisionFillBrush.Dispose();
//    pointHitFillBrush.Dispose();
//    p.Color = originalPenColor;
//}
#endregion DrawCollisionPoints Version 2

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