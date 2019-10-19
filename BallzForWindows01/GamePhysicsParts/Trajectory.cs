using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.GamePhysicsParts
{
    class Trajectory
    {
        public string NameTag { get { return nameTag; } set { nameTag = value; } }
        public bool EndPointSet { get { return endPointSet; } set { endPointSet = value; } }
        public double RotAngle { get { return rot; } }

        string nameTag = "";
        double x, y;
        double endx, endy;
        double rot;
        double distance;
        bool endPointSet = false;

        double oppLen, hypLen, adjLen, anglePreRotation = 0;
        PointF originPos, rightPos, aimPos;

        public Trajectory(string nameTag = "")
        {
            this.nameTag = nameTag;
            x = 0;
            y = 0;
            endx = 0;
            endy = 0;
            rot = 0;
            distance = 100;
        }
        public void Load(float startx, float starty) { x = startx; y = starty; }
        public void SetEndPoint(double ex, double ey) { _SetEndPoint(ex, ey); }
        public void Update()
        {
        }

        public void Draw(Graphics g, float dbgTextPosX = 0, float dbgTextPosY = 0)
        {
            //Font f = new Font("Arial", 12, FontStyle.Regular);

            DebugDraw(g, dbgTextPosX, dbgTextPosY);
            DrawPointMarkers(g);
            if (endPointSet)
            {
                g.DrawLine(Pens.Red, (float)x, (float)y, (float)endx, (float)endy);
            }
            //f.Dispose();
        }
        public void Reset()
        {
            endx = 0;
            endy = 0;
            endPointSet = false;
        }

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
                DrawString(g, f, $"{nameTag}", ref fpos);
                DrawString(g, f, $"distance: {distance:N2}", ref fpos);
                DrawString(g, f, $"o: {{ {oppLen:N2} }} h: {{ {hypLen:N2} }} a: {{ {adjLen:N2} }} ", ref fpos);
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
            if(bsize.Width > dbgBlockSize.Width)
            {
                dbgBlockSize.Width = bsize.Width;
            }

        }
        void DrawPointMarkers(Graphics g)
        {
            float width = 20;
            g.DrawRectangle(Pens.Black, originPos.X - (width / 2), originPos.Y - (width / 2), width, width);
            g.DrawRectangle(Pens.Red, aimPos.X - (width / 2), aimPos.Y - (width / 2), width, width);
            g.DrawRectangle(Pens.Orange, rightPos.X - (width / 2), rightPos.Y - (width / 2), width, width);

            g.DrawRectangle(Pens.Purple, (float)endx - (width / 2), (float)endy - (width / 2), width, width); // endPos

            g.DrawLine(Pens.Green, originPos, aimPos); // hyp
            g.DrawLine(Pens.Blue, originPos, rightPos); // adj
            g.DrawLine(Pens.Orange, aimPos, rightPos); // opp

            Pen p = new Pen(Brushes.Black, 3);
            g.DrawLine(p, originPos, new PointF((float)endx, (float)endy));
            p.Dispose();
        }
        

        double CalcDistance(double sx, double sy, double ex, double ey)
        {
            double xdiff = ex - sx;
            double ydiff = ey - sy;
            double sumSqrs = (xdiff * xdiff) + (ydiff * ydiff);
            return Math.Sqrt(sumSqrs);
        }
        void SetRotation()
        {
            if (aimPos.X == originPos.X && aimPos.Y < originPos.Y)
            {
                rot = (3 * Math.PI) / 2;
                return;
            }
            anglePreRotation = Math.Asin(oppLen / hypLen);
            if (aimPos.X > originPos.X && aimPos.Y < originPos.Y)
            {
                rot = Math.PI * 2 - anglePreRotation;
                return;
            }
            if (aimPos.X < originPos.X && aimPos.Y < originPos.Y)
            {
                rot = Math.PI + anglePreRotation;
                return;
            }
        }
        void _SetEndPoint(double ex, double ey)
        {
            originPos = new PointF((float)(x), (float)(y));
            rightPos = new PointF((float)(ex), (float)(y));
            aimPos = new PointF((float)(ex), (float)(ey));
            oppLen = CalcDistance(aimPos.X, aimPos.Y, rightPos.X, rightPos.Y);
            hypLen = CalcDistance(originPos.X, originPos.Y, aimPos.X, aimPos.Y);
            adjLen = CalcDistance(originPos.X, originPos.Y, rightPos.X, rightPos.Y);
            distance = hypLen;
            SetRotation();
            endx = x + distance * Math.Cos(rot);
            endy = y + distance * Math.Sin(rot);
            endPointSet = true;
        }
        

        #region pending removal

        #region SetDistance - replaced by CalcDistance 2019-10-19
        //void SetDistance(double ex, double ey)
        //{
        //    endx = ex;
        //    endy = ey;
        //    double xdiff = endx - x;
        //    double ydiff = endy - y;
        //    double sumSqrs = (xdiff * xdiff) + (ydiff * ydiff);
        //    distance = Math.Sqrt(sumSqrs);
        //}
        #endregion SetDistance - replaced by CalcDistance

        #endregion pending removal


    }
}

#region DrawTrigStats - moved all these into Debug draw fuction
//void DrawTrigStats(Graphics g, Font f, ref PointF fpos)
//{
//    DrawString(g, f, $"TrigStats:", ref fpos);
//    DrawString(g, f, $"oppLen: {oppLen}", ref fpos);
//    DrawString(g, f, $"hypLen: {hypLen}", ref fpos);
//    DrawString(g, f, $"anglePreRotation(degrees): {anglePreRotation * 180 / Math.PI:N2}", ref fpos);
//    DrawString(g, f, $"rot(radians): {rot}", ref fpos);
//    DrawString(g, f, $"rot(degrees): {rot * 180 / Math.PI:N2}", ref fpos);
//    DrawString(g, f, $"originPos: {originPos}", ref fpos);
//    DrawString(g, f, $"aimPos: {aimPos}", ref fpos);
//    DrawString(g, f, $"rghtPos: {rightPos}", ref fpos);
//    double hypCheck = Math.Sqrt((oppLen * oppLen) + (adjLen * adjLen));
//    DrawString(g, f, $"hypCheck: {hypCheck}", ref fpos);
//    //DrawString(g, f, $"{((3 * Math.PI) / 2) * 180 / Math.PI}", ref fpos);
//}
#endregion DrawTrigStats - moved all these into Debug draw fuction

#region _SetEndPoint with old logic commented out 2019-10-19
//void _SetEndPoint(double ex, double ey)
//{
//    //TODO
//    #region attempt 1
//    //endx = ex;
//    //endy = ey;
//    //oppLen = CalcDistance(ex, y, ex, ey);
//    //hypLen = CalcDistance(x, y, ex, ey);
//    //adjLen = CalcDistance(x, y, ex, y);
//    //originPos = new PointF((float)(x), (float)(y));
//    //hypOppPos = new PointF((float)(x + adjLen), (float)(y - oppLen));
//    //rightPos = new PointF((float)(x + adjLen), (float)(y));
//    //distance = hypLen;
//    //SetRotation();
//    //endx = x + distance * Math.Cos(rot) * 180 / Math.PI;
//    //endy = y + distance * Math.Sin(rot) * 180 / Math.PI;
//    //endPointSet = true;
//    #endregion attempt 1

//    #region attempt 2
//    //originPos = new PointF((float)(x), (float)(y));
//    //rightPos = new PointF((float)(originPos.X + ex), (float)(originPos.Y));
//    //aimPos = new PointF((float)( rightPos.X), (float)(rightPos.Y + ey));
//    //oppLen = CalcDistance(aimPos.X, aimPos.Y, rightPos.X, rightPos.Y);
//    //hypLen = CalcDistance(originPos.X, originPos.Y, aimPos.X, aimPos.Y);
//    //adjLen = CalcDistance(originPos.X, originPos.Y, rightPos.X, rightPos.Y);
//    //distance = hypLen;
//    //SetRotation();
//    //endx = x + distance * Math.Cos(rot);
//    //endy = y + distance * Math.Sin(rot);
//    //endPointSet = true;
//    #endregion attempt 2

//    #region attempt 3
//    //originPos = new PointF((float)(0), (float)(0));
//    //rightPos = new PointF((float)(ex), (float)(0));
//    //aimPos = new PointF((float)(ex), (float)(ey));
//    //oppLen = CalcDistance(aimPos.X, aimPos.Y, rightPos.X, rightPos.Y);
//    //hypLen = CalcDistance(originPos.X, originPos.Y, aimPos.X, aimPos.Y);
//    //adjLen = CalcDistance(originPos.X, originPos.Y, rightPos.X, rightPos.Y);
//    //distance = hypLen;
//    //SetRotation();
//    //endx = x + distance * Math.Cos(rot);
//    //endy = y + distance * Math.Sin(rot);
//    //endPointSet = true;
//    #endregion attempt 3

//    originPos = new PointF((float)(x), (float)(y));
//    rightPos = new PointF((float)(ex), (float)(y));
//    aimPos = new PointF((float)(ex), (float)(ey));
//    oppLen = CalcDistance(aimPos.X, aimPos.Y, rightPos.X, rightPos.Y);
//    hypLen = CalcDistance(originPos.X, originPos.Y, aimPos.X, aimPos.Y);
//    adjLen = CalcDistance(originPos.X, originPos.Y, rightPos.X, rightPos.Y);
//    distance = hypLen;
//    SetRotation();
//    endx = x + distance * Math.Cos(rot);
//    endy = y + distance * Math.Sin(rot);
//    endPointSet = true;
//}
#endregion _SetEndPoint with old logic commented out 2019-10-19