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

        PointD startPoint;
        PointD endPoint;
        PointD rightPos;

        XMarker2 startMarker;
        XMarker2 endMarker;

        double rotation;
        double oppLen, hypLen, adjLen, anglePreRotation;

        bool north, south, endPointSet;

        public Trajectory03()
        {
            startPoint = new PointD();
            endPoint = new PointD();
            rightPos = new PointD();
            startMarker = new XMarker2();
            endMarker = new XMarker2();
            north = south = endPointSet = false;

            startMarker.DrawColor = Color.Black;
            endMarker.DrawColor = Color.Black;

        }
        public void Load()
        {

        }
        public void Update()
        {
            string fnId = FnId(clsName, "Update");
            if (!inows(nameTag)) { fnId += $"[\"{nameTag}\"]"; }

            dbgPrintAngle(fnId, "rotation", rotation);


        }
        public void Draw(Graphics g)
        {
            startMarker.Draw(g);
            endMarker.Draw(g);
            //DrawPointMarkers(g);
            //if (/*showDebugLines*/ true) { DrawPointMarkers(g); }
            //if (/*endPointSet*/ true) { g.DrawLine(Pens.Red, startPoint.fX, startPoint.fY, endPoint.fX, endPoint.fY); }
        }
        public void Reset()
        {
            startPoint.Zero();
            rightPos.Zero();
            endPoint.Zero();
            endPointSet = false;
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

        /// ----- SetRotation -----

        void SetRotation(PointD startPoint, PointD endPoint)
        {
            //PointD endPoint = new PointD();
            //PointD startPoint = new PointD();
            //startPoint.Set(startMarker.Position);
            //endPoint.Set(endMarker.Position);

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
        /// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //void SetRotation()
        //{
        //    PointD endPoint = new PointD();
        //    PointD startPoint = new PointD();
        //    startPoint.Set(startMarker.Position);
        //    endPoint.Set(endMarker.Position);

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
        /// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //void SetRotation()
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
        /// ----- SetRotation -----
        
        void _SetStartPoint(double sx, double sy)
        {
            startPoint.Set(sx, sy);
            startMarker.Place(sx, sy);
        }

        void _SetEndPoint(double ex, double ey)
        {
            endMarker.Place(ex, ey);
            endPoint.Set(ex, ey);
            rightPos.Set(endPoint.X, startPoint.Y);
            oppLen = rightPos.DistanceTo(endPoint);
            hypLen = startPoint.DistanceTo(endPoint);
            adjLen = startPoint.DistanceTo(rightPos);
            SetRotation();
            endPointSet = true;
        }

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

        private bool _InEndRect(double px, double py) { return endMarker.InClickRect(px, py); }

        void DrawPointMarkers(Graphics g)
        {

            float width = 20;
            // drawing origin position box
            g.DrawRectangle(Pens.Black, startPoint.fX - (width / 2), startPoint.fY - (width / 2), width, width);

            // drawing aim position box
            g.DrawRectangle(Pens.Red, endPoint.fX - (width / 2), endPoint.fY - (width / 2), width, width);

            // drawing right position box
            g.DrawRectangle(Pens.Orange, rightPos.fX - (width / 2), rightPos.fY - (width / 2), width, width);

            // drawing endPoint box
            //g.DrawRectangle(Pens.Purple, endPoint.fX - (width / 2), endPoint.fY - (width / 2), width, width); // endPos

            g.DrawLine(Pens.Green, startPoint.ToPointF(), endPoint.ToPointF()); // hyp
            g.DrawLine(Pens.Blue, startPoint.ToPointF(), rightPos.ToPointF()); // adj
            g.DrawLine(Pens.Orange, endPoint.ToPointF(), rightPos.ToPointF()); // opp

            //Pen p = new Pen(Brushes.Black, 3);
            //g.DrawLine(p, originPos.ToPointF(), endPoint.ToPointF());
            //p.Dispose();
        }
    }
}
