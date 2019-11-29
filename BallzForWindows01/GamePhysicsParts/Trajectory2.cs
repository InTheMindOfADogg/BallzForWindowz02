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
        string clsName = "Trajectory2";
        string nameTag = "";

        PointD originPos;
        PointD rightPos;
        PointD aimPos;
        // endPoint is just for testing. If the calculations are correct, endpoint should be the same as aimPos
        PointD endPoint;    
        
        double rot;
        double oppLen, hypLen, adjLen, anglePreRotation;

        bool endPointSet;
        bool north = false, south = false;

        public Trajectory2(string nameTag)
        {
            this.nameTag = nameTag;
            _Init();
        }
        public void Load(double startX, double startY)
        {
            _Load(startX, startY);
        }
        public void Update() { }
        public void Draw(Graphics g)
        {

        }
        public void Reset()
        {
            _Reset();
        }

        public void SetEndPoint(double ex, double ey)
        {
            aimPos.Set(ex, ey);
            rightPos.Set(aimPos.X, originPos.Y);
            oppLen = rightPos.DistanceTo(aimPos);
            hypLen = originPos.DistanceTo(aimPos);
            adjLen = originPos.DistanceTo(rightPos);
            SetRotation();
            endPoint.X = originPos.X + hypLen * Math.Cos(rot);
            endPoint.Y = originPos.Y + hypLen * Math.Sin(rot);
            endPointSet = true;
        }

        void _Init()
        {
            originPos = new PointD();
            rightPos = new PointD();
            aimPos = new PointD();
            endPoint = new PointD();
            
            oppLen = hypLen = adjLen = anglePreRotation = rot = 0d;
            endPointSet = north = south = false;
        }

        void _Load(double startX, double startY)
        {
            originPos.Set(startX, startY);
        }

        void _Draw(Graphics g)
        {

        }

        #region Drawing debug info logic
        public RectangleF DbgTextBlock { get { return new RectangleF(dbgBlockPos, dbgBlockSize); } }
        public PointF DbgBlockPos { get { return dbgBlockPos; } }
        public SizeF DbgBlockSize { get { return dbgBlockSize; } }
        PointF dbgBlockPos = new PointF();
        SizeF dbgBlockSize = new SizeF();
        void DebugDraw(Graphics g, float dbgTextPosX = 0, float dbgTextPosY = 0)
        {
            if (dbgTextPosX != 0 || dbgTextPosY != 0)
            {
                Font f = new Font("Arial", 12, FontStyle.Regular);
                PointF fpos = new PointF(dbgTextPosX, dbgTextPosY);
                dbgBlockPos = new PointF(dbgTextPosX, dbgTextPosY);
                DrawString(g, f, $"{clsName}", ref fpos);
                DrawString(g, f, $"hypLen: {hypLen:N2}", ref fpos);
                DrawString(g, f, $"o: {{ {oppLen:N2} }} h: {{ {hypLen:N2} }} a: {{ {adjLen:N2} }} ", ref fpos);
                DrawString(g, f, $"north: {north}", ref fpos);
                DrawString(g, f, $"south: {south}", ref fpos);
                DrawString(g, f, $"anglePreRotation(degrees): {anglePreRotation * 180 / Math.PI:N2}", ref fpos);
                DrawString(g, f, $"rot(radians): {rot:N2}", ref fpos);
                DrawString(g, f, $"rot(degrees): {rot * 180 / Math.PI:N2}", ref fpos);
                DrawString(g, f, $"originPos: {originPos}", ref fpos);
                DrawString(g, f, $"aimPos: {aimPos}", ref fpos);
                DrawString(g, f, $"rghtPos: {rightPos}", ref fpos);
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
        void DrawPointMarkers(Graphics g)
        {
            float width = 20;
            g.DrawRectangle(Pens.Black, originPos.fX - (width / 2), originPos.fY - (width / 2), width, width);
            g.DrawRectangle(Pens.Red, aimPos.fX - (width / 2), aimPos.fY - (width / 2), width, width);
            g.DrawRectangle(Pens.Orange, rightPos.fX - (width / 2), rightPos.fY - (width / 2), width, width);

            g.DrawRectangle(Pens.Purple, endPoint.fX - (width / 2), endPoint.fY - (width / 2), width, width); // endPos

            g.DrawLine(Pens.Green, originPos.ToPointF(), aimPos.ToPointF()); // hyp
            g.DrawLine(Pens.Blue, originPos.ToPointF(), rightPos.ToPointF()); // adj
            g.DrawLine(Pens.Orange, aimPos.ToPointF(), rightPos.ToPointF()); // opp

            Pen p = new Pen(Brushes.Black, 3);
            g.DrawLine(p, originPos.ToPointF(), endPoint.ToPointF());
            p.Dispose();
        }
        #endregion Drawing debug info logic

        void _Reset()
        {
            endPoint.Zero();
            endPointSet = false;
        }

        void SetRotation()
        {
            north = south = false;
            if (aimPos.Y < originPos.Y) { north = true; }
            if (aimPos.Y > originPos.Y) { south = true; }
            anglePreRotation = Math.Asin(oppLen / hypLen);
            // north
            if (aimPos.X == originPos.X && north) { rot = (3 * Math.PI) / 2; return; }
            // south
            if (aimPos.X == originPos.X && south) { rot = (Math.PI) / 2; return; }
            // east
            if (aimPos.X > originPos.X && aimPos.Y == originPos.Y) { rot = 0; return; }
            // west
            if (aimPos.X < originPos.X && aimPos.Y == originPos.Y) { rot = Math.PI; return; }
            // northwest
            if (aimPos.X > originPos.X && north) { rot = Math.PI * 2 - anglePreRotation; return; }
            //northeast
            if (aimPos.X < originPos.X && north) { rot = Math.PI + anglePreRotation; return; }
            // southwest
            if (aimPos.X > originPos.X && south) { rot = anglePreRotation; return; }
            // southeast
            if (aimPos.X < originPos.X && south) { rot = Math.PI - anglePreRotation; return; }
        }
    }
}
