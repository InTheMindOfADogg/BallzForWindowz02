using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.GamePhysicsParts
{
    class Trajectory2
    {

        public double Rotation { get { return rotation; } }
        public bool ShowDebugLine { get { return showDebugLines; } set { showDebugLines = value; } }

        public PointD EndPos { get { return aimPos; } }
        public PointD StartPos { get { return originPos; } }


        string clsName = "Trajectory2";
        string nameTag = "";

        PointD originPos;
        PointD rightPos;
        PointD aimPos;

        double rotation;
        double oppLen, hypLen, adjLen, anglePreRotation;

        bool endPointSet;
        bool north = false, south = false;
        bool showDebugLines = false;


        public Trajectory2(string nameTag) { this.nameTag = nameTag; _Init(); }
        //public void Load(double startX, double startY) { _Load(startX, startY); }
        public void Update() { }
        public void Draw(Graphics g) { _Draw(g); }
        public void Reset() { _Reset(); }
        public void SetStartPoint(PointD p) { _SetStartPoint(p.X, p.Y); }
        public void SetStartPoint(double sx, double sy) { _SetStartPoint(sx, sy); }
        public void SetEndPoint(PointD p) { _SetEndPoint(p.X, p.Y); }
        public void SetEndPoint(double ex, double ey) { _SetEndPoint(ex, ey); }

        void _Init()
        {
            originPos = new PointD();
            rightPos = new PointD();
            aimPos = new PointD();

            oppLen = hypLen = adjLen = anglePreRotation = rotation = 0;
            endPointSet = north = south = false;
        }
        void _Draw(Graphics g)
        {
            if (/*showDebugLines*/ true) { DrawPointMarkers(g); }
            if (/*endPointSet*/ true) { g.DrawLine(Pens.Red, originPos.fX, originPos.fY, aimPos.fX, aimPos.fY); }
        }
        void _Reset()
        {
            originPos.Zero();
            rightPos.Zero();
            aimPos.Zero();            
            endPointSet = false;
        }

        void SetRotation()
        {
            north = south = false;
            if (aimPos.Y < originPos.Y) { north = true; }
            if (aimPos.Y > originPos.Y) { south = true; }
            anglePreRotation = Math.Asin(oppLen / hypLen);
            // north
            if (aimPos.X == originPos.X && north) { rotation = (3 * Math.PI) / 2; return; }
            // south
            if (aimPos.X == originPos.X && south) { rotation = (Math.PI) / 2; return; }
            // east
            if (aimPos.X > originPos.X && aimPos.Y == originPos.Y) { rotation = 0; return; }
            // west
            if (aimPos.X < originPos.X && aimPos.Y == originPos.Y) { rotation = Math.PI; return; }
            // northwest
            if (aimPos.X > originPos.X && north) { rotation = Math.PI * 2 - anglePreRotation; return; }
            //northeast
            if (aimPos.X < originPos.X && north) { rotation = Math.PI + anglePreRotation; return; }
            // southwest
            if (aimPos.X > originPos.X && south) { rotation = anglePreRotation; return; }
            // southeast
            if (aimPos.X < originPos.X && south) { rotation = Math.PI - anglePreRotation; return; }
        }

        void DrawPointMarkers(Graphics g)
        {
            float width = 20;
            // drawing origin position box
            g.DrawRectangle(Pens.Black, originPos.fX - (width / 2), originPos.fY - (width / 2), width, width);

            // drawing aim position box
            g.DrawRectangle(Pens.Red, aimPos.fX - (width / 2), aimPos.fY - (width / 2), width, width);

            // drawing right position box
            g.DrawRectangle(Pens.Orange, rightPos.fX - (width / 2), rightPos.fY - (width / 2), width, width);

            // drawing endPoint box
            //g.DrawRectangle(Pens.Purple, endPoint.fX - (width / 2), endPoint.fY - (width / 2), width, width); // endPos

            g.DrawLine(Pens.Green, originPos.ToPointF(), aimPos.ToPointF()); // hyp
            g.DrawLine(Pens.Blue, originPos.ToPointF(), rightPos.ToPointF()); // adj
            g.DrawLine(Pens.Orange, aimPos.ToPointF(), rightPos.ToPointF()); // opp

            //Pen p = new Pen(Brushes.Black, 3);
            //g.DrawLine(p, originPos.ToPointF(), endPoint.ToPointF());
            //p.Dispose();
        }

        void _SetStartPoint(double sx, double sy)
        {
            originPos.Set(sx, sy);
        }
        
        void _SetEndPoint(double ex, double ey)
        {
            aimPos.Set(ex, ey);
            rightPos.Set(aimPos.X, originPos.Y);
            oppLen = rightPos.DistanceTo(aimPos);
            hypLen = originPos.DistanceTo(aimPos);
            adjLen = originPos.DistanceTo(rightPos);
            SetRotation();
            // endPoint is for testing. endPoint should match aimPos if calculations are correct.
            //endPoint.X = originPos.X + hypLen * Math.Cos(rotation);
            //endPoint.Y = originPos.Y + hypLen * Math.Sin(rotation);
            endPointSet = true;
        }

        #region Drawing debug info logic
        //public RectangleF DbgTextBlock { get { return new RectangleF(dbgBlockPos, dbgBlockSize); } }
        //public PointF DbgBlockPos { get { return dbgBlockPos; } }
        //public SizeF DbgBlockSize { get { return dbgBlockSize; } }
        PointF dbgBlockPos = new PointF();
        SizeF dbgBlockSize = new SizeF();
        public void DebugDraw(Graphics g, float dbgTextPosX = 0, float dbgTextPosY = 0)
        {

            if (dbgTextPosX != 0 || dbgTextPosY != 0)
            {
                Font f = new Font("Arial", 12, FontStyle.Regular);
                PointF fpos = new PointF(dbgTextPosX, dbgTextPosY);
                dbgBlockPos = new PointF(dbgTextPosX, dbgTextPosY);
                DrawString(g, f, $"{clsName}", ref fpos);
                DrawString(g, f, $"hypLen: {hypLen:N2}", ref fpos);
                DrawString(g, f, $"opp: {{ {oppLen:N2} }} hyp: {{ {hypLen:N2} }} adj: {{ {adjLen:N2} }} ", ref fpos);
                DrawString(g, f, $"north: {north}", ref fpos);
                DrawString(g, f, $"south: {south}", ref fpos);
                DrawString(g, f, $"anglePreRotation(degrees): {anglePreRotation * 180 / Math.PI:N2}", ref fpos);
                DrawString(g, f, $"rot(radians): {rotation:N2}", ref fpos);
                DrawString(g, f, $"rot(degrees): {rotation * 180 / Math.PI:N2}", ref fpos);
                DrawString(g, f, $"originPos: {originPos}", ref fpos);
                DrawString(g, f, $"aimPos: {aimPos}", ref fpos);
                //DrawString(g, f, $"endPoint: {endPoint}", ref fpos);
                DrawString(g, f, $"rightPos: {rightPos}", ref fpos);
                f.Dispose();
            }
        }
        void DrawString(Graphics g, Font f, string str, ref PointF fontPos)
        {
            g.DrawString(str, f, Brushes.Black, fontPos);
            SizeF bsize = g.MeasureString(str, f);
            fontPos.Y += bsize.Height;
            dbgBlockSize.Height += bsize.Height;
            if (bsize.Width > dbgBlockSize.Width)
            {
                dbgBlockSize.Width = bsize.Width;
            }
        }
        #endregion Drawing debug info logic
    }
}
