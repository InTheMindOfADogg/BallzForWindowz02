using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.GamePhysicsParts
{
    using static AssistFunctions;
    using DrawableParts;

    class Trajectory03
    {

        public bool Placed { get { return endMarker.Placed; } }
        public string NameTag { get { return nameTag; } set { nameTag = value; } }

        string clsName = "Trajectory03";
        string nameTag = "";

        XMarker2 startMarker;
        XMarker2 endMarker;

        double rotation;
        double oppLen, hypLen, adjLen, anglePreRotation;

        bool north, south;

        public Trajectory03()
        {
            startMarker = new XMarker2();
            endMarker = new XMarker2();
            north = south = false;

            startMarker.DrawColor = Color.Black;
            endMarker.DrawColor = Color.Black;

        }
        public void Load()
        {

        }
        double testRotFromPointDs = 0;
        public void Update()
        {
            string fnId = FnId(clsName, "Update");
            if (!inows(nameTag)) { fnId += $"[\"{nameTag}\"]"; }

            dbgPrintAngle(fnId, "rotation", rotation);
            testRotFromPointDs = startMarker.Position.RotationTo(endMarker.Position);
            dbgPrintAngle(fnId, "testRotFromPointDs", testRotFromPointDs);


        }
        public void Draw(Graphics g)
        {
            startMarker.Draw(g);
            endMarker.Draw(g);
        }
        public void Reset()
        {
            startMarker.Reset();
            endMarker.Reset();
        }
        public void CleanUp()
        {

        }

        public void SetXColor(Color c)
        {
            startMarker.DrawColor = c;
            endMarker.DrawColor = c;
        }

        public bool InEndRect(PointD p) { return _InEndRect(p.X, p.Y); }
        public bool InEndRect(double px, double py) { return _InEndRect(px, py); }

        public void SetStartPoint(PointD p) { _SetStartPoint(p.X, p.Y); }
        public void SetStartPoint(double sx, double sy) { _SetStartPoint(sx, sy); }
        public void SetEndPoint(PointD p) { _SetEndPoint(p.X, p.Y); }
        public void SetEndPoint(double ex, double ey) { _SetEndPoint(ex, ey); }


        private bool _InEndRect(double px, double py) { return endMarker.InClickRect(px, py); }

        void _SetStartPoint(double sx, double sy)
        {
            startMarker.Place(sx, sy);
        }

        void _SetEndPoint(double ex, double ey)
        {
            endMarker.Place(ex, ey);

            SetSideLengths(startMarker.Position, endMarker.Position);
            SetRotation(startMarker.Position, endMarker.Position);
        }
        void SetSideLengths(PointD startPoint, PointD endPoint)
        {
            PointD rightPoint = new PointD(endPoint.X, startPoint.Y);
            oppLen = rightPoint.DistanceTo(endPoint);
            hypLen = startPoint.DistanceTo(endPoint);
            adjLen = startPoint.DistanceTo(rightPoint);
        }


        void SetRotation(PointD startPoint, PointD endPoint)
        {
            north = south = false;
            if (endPoint.Y < startPoint.Y) { north = true; }
            if (endPoint.Y > startPoint.Y) { south = true; }
            anglePreRotation = Math.Asin(oppLen / hypLen);
            // north
            if (endPoint.X == startPoint.X && north) { rotation = (3 * Math.PI) / 2; return; }
            // south
            if (endPoint.X == startPoint.X && south) { rotation = (Math.PI) / 2; return; }
            // east
            if (endPoint.X > startPoint.X && endPoint.Y == startPoint.Y) { rotation = 0; return; }
            // west
            if (endPoint.X < startPoint.X && endPoint.Y == startPoint.Y) { rotation = Math.PI; return; }
            // northwest
            if (endPoint.X > startPoint.X && north) { rotation = Math.PI * 2 - anglePreRotation; return; }
            //northeast
            if (endPoint.X < startPoint.X && north) { rotation = Math.PI + anglePreRotation; return; }
            // southwest
            if (endPoint.X > startPoint.X && south) { rotation = anglePreRotation; return; }
            // southeast
            if (endPoint.X < startPoint.X && south) { rotation = Math.PI - anglePreRotation; return; }
        }



        

        void DrawPointMarkers(Graphics g)
        {

            float width = 20;
            // Drawing origin position box
            g.DrawRectangle(Pens.Black, startMarker.Position.fX - (width / 2), startMarker.Position.fY - (width / 2), width, width);

            // Drawing aim position box
            g.DrawRectangle(Pens.Red, endMarker.Position.fX - (width / 2), endMarker.Position.fY - (width / 2), width, width);

            PointD rightPoint = new PointD(endMarker.Position.X, startMarker.Position.Y);
            // Drawing right position box
            g.DrawRectangle(Pens.Orange, rightPoint.fX - (width / 2), rightPoint.fY - (width / 2), width, width);

            // Drawing lines between boxes            
            g.DrawLine(Pens.Green, startMarker.Position.ToPointF(), endMarker.Position.ToPointF()); // hyp
            g.DrawLine(Pens.Blue, startMarker.Position.ToPointF(), rightPoint.ToPointF()); // adj
            g.DrawLine(Pens.Orange, endMarker.Position.ToPointF(), rightPoint.ToPointF()); // opp

        }
    }
}

#region rebuilding SetRotation process
///// ----- SetRotation -----
// This is the one I eneded up using at end of the cleaning/building process 2019-12-27
//void SetRotation(PointD startPoint, PointD endPoint)
//{
//    north = south = false;
//    if (endPoint.Y < startPoint.Y) { north = true; }
//    if (endPoint.Y > startPoint.Y) { south = true; }
//    anglePreRotation = Math.Asin(oppLen / hypLen);
//    // north
//    if (endPoint.X == startPoint.X && north) { rotation = (3 * Math.PI) / 2; return; }
//    // south
//    if (endPoint.X == startPoint.X && south) { rotation = (Math.PI) / 2; return; }
//    // east
//    if (endPoint.X > startPoint.X && endPoint.Y == startPoint.Y) { rotation = 0; return; }
//    // west
//    if (endPoint.X < startPoint.X && endPoint.Y == startPoint.Y) { rotation = Math.PI; return; }
//    // northwest
//    if (endPoint.X > startPoint.X && north) { rotation = Math.PI * 2 - anglePreRotation; return; }
//    //northeast
//    if (endPoint.X < startPoint.X && north) { rotation = Math.PI + anglePreRotation; return; }
//    // southwest
//    if (endPoint.X > startPoint.X && south) { rotation = anglePreRotation; return; }
//    // southeast
//    if (endPoint.X < startPoint.X && south) { rotation = Math.PI - anglePreRotation; return; }
//}

///// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

//#region SetRotation previous versions, rebuilt 2019-12-27    
////void SetRotation()
////{
////    PointD endPoint = new PointD();
////    PointD startPoint = new PointD();
////    startPoint.Set(startMarker.Position);
////    endPoint.Set(endMarker.Position);

////    north = south = false;
////    if (endPoint.Y < startPoint.Y) { north = true; }
////    if (endPoint.Y > startPoint.Y) { south = true; }
////    anglePreRotation = Math.Asin(oppLen / hypLen);
////    // north
////    if (endPoint.X == startPoint.X && north) { rotation = (3 * Math.PI) / 2; return; }
////    // south
////    if (endPoint.X == startPoint.X && south) { rotation = (Math.PI) / 2; return; }
////    // east
////    if (endPoint.X > startPoint.X && endPoint.Y == startPoint.Y) { rotation = 0; return; }
////    // west
////    if (endPoint.X < startPoint.X && endPoint.Y == startPoint.Y) { rotation = Math.PI; return; }
////    // northwest
////    if (endPoint.X > startPoint.X && north) { rotation = Math.PI * 2 - anglePreRotation; return; }
////    //northeast
////    if (endPoint.X < startPoint.X && north) { rotation = Math.PI + anglePreRotation; return; }
////    // southwest
////    if (endPoint.X > startPoint.X && south) { rotation = anglePreRotation; return; }
////    // southeast
////    if (endPoint.X < startPoint.X && south) { rotation = Math.PI - anglePreRotation; return; }
////}
///// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
////void SetRotation()
////{
////    north = south = false;
////    if (endPoint.Y < startPoint.Y) { north = true; }
////    if (endPoint.Y > startPoint.Y) { south = true; }
////    anglePreRotation = Math.Asin(oppLen / hypLen);
////    // north
////    if (endPoint.X == startPoint.X && north) { rotation = (3 * Math.PI) / 2; return; }
////    // south
////    if (endPoint.X == startPoint.X && south) { rotation = (Math.PI) / 2; return; }
////    // east
////    if (endPoint.X > startPoint.X && endPoint.Y == startPoint.Y) { rotation = 0; return; }
////    // west
////    if (endPoint.X < startPoint.X && endPoint.Y == startPoint.Y) { rotation = Math.PI; return; }
////    // northwest
////    if (endPoint.X > startPoint.X && north) { rotation = Math.PI * 2 - anglePreRotation; return; }
////    //northeast
////    if (endPoint.X < startPoint.X && north) { rotation = Math.PI + anglePreRotation; return; }
////    // southwest
////    if (endPoint.X > startPoint.X && south) { rotation = anglePreRotation; return; }
////    // southeast
////    if (endPoint.X < startPoint.X && south) { rotation = Math.PI - anglePreRotation; return; }
////}

//#endregion SetRotation previous versions, rebuilt 2019-12-27
///// ----- SetRotation -----
#endregion rebuilding SetRotation process


#region _SetEndPoint previous version, rebuilt 2019-12-27
//void _SetEndPoint(double ex, double ey)
//{
//    endMarker.Place(ex, ey);
//    endPoint.Set(ex, ey);
//    rightPos.Set(endPoint.X, startPoint.Y);
//    oppLen = rightPos.DistanceTo(endPoint);
//    hypLen = startPoint.DistanceTo(endPoint);
//    adjLen = startPoint.DistanceTo(rightPos);
//    SetRotation();
//    endPointSet = true;
//}
#endregion _SetEndPoint previous version, rebuilt 2019-12-27