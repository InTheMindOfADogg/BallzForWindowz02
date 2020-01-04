using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.DrawableParts
{
    using GamePhysicsParts;

    class CollisionPoint : DrawableObject
    {

        PointD pos;
        SizeD boxSize;
        RectangleD cbox;    // collision box
        bool collision = false;

        bool pointHit = false;
        
        public bool PointHit { get { return pointHit; } set { pointHit = value; } }
        public PointD Pos { get { return pos; } }
        public RectangleD Rect { get { return cbox; } }
        //public bool Collision { get { return collision; } }


        public CollisionPoint() { _Init(0, 0, 0, 0, Color.FromArgb(255, 255, 0, 200)); }
        public CollisionPoint(double x, double y, double cbSideLength) { _Init(x, y, cbSideLength, cbSideLength, Color.FromArgb(255, 175, 30, 50)); }
        public CollisionPoint(double x, double y, double cbSideLength, Color c) { _Init(x, y, cbSideLength, cbSideLength, c); }
        public CollisionPoint(double x, double y, double cboxWidth, double cboxHeight) { _Init(x, y, cboxWidth, cboxHeight, Color.FromArgb(255, 175, 30, 50)); }
        public CollisionPoint(double x, double y, double cboxWidth, double cboxHeight, Color c) { _Init(x, y, cboxWidth, cboxHeight, c); }
        void _Init(double x, double y, double cboxWidth, double cboxHeight, Color c)
        {
            clsName = "CollisionPoint";
            pos = new PointD(x, y);
            boxSize = new SizeD(cboxWidth, cboxHeight);
            cbox = new RectangleD();
            this.color = c;
            SetCollisionBox();
        }
        public void Load(double x, double y) { _Load(x, y, 25, 25); } // just using 25 as default for now, might change later
        public void Load(double x, double y, double cboxSideLength) { _Load(x, y, cboxSideLength, cboxSideLength); }
        public void Load(double x, double y, double cboxWidth, double cboxHeight) { _Load(x, y, cboxWidth, cboxHeight); }
        void _Load(double x, double y, double cboxWidth, double cboxHeight)
        {
            pos.Set(x, y);
            boxSize.Set(cboxWidth, cboxHeight);
            SetCollisionBox();
        }

        public void Set(double x, double y, double cboxWidth, double cboxHeight) 
        {
            pos.Set(x, y);
            boxSize.Set(cboxWidth, cboxHeight);
            SetCollisionBox();
        }
        
        public void Update(double x, double y) { }
        public void Draw(Graphics g)
        {
            
            Pen p = new Pen(color, 1);
            
            g.DrawRectangle(p, pos.fX - 1, pos.fY - 1, 2, 2); // center point for testing
            g.DrawRectangle(p, cbox.fX, cbox.fY, cbox.fWidth, cbox.fHeight);
            //if (pointHit) { g.FillRectangle(Brushes.Green, cbox.fX, cbox.fY, cbox.fWidth * 10, cbox.fHeight * 10); }  // makes points larger for viewing
            if (pointHit) { g.FillRectangle(Brushes.Green, cbox.fX, cbox.fY, cbox.fWidth, cbox.fHeight); }
            if (collision)
            {
                SolidBrush fillBrush = new SolidBrush(Color.FromArgb(25, 200, 10, 10));
                g.FillRectangle(fillBrush, cbox.fX, cbox.fY, cbox.fWidth, cbox.fHeight);
                fillBrush.Dispose();
            }
            
            p.Dispose();

        }

        public bool CheckForCollision(PointD pos) { return (collision = cbox.InBox(pos.X, pos.Y)); }
        public bool CheckForCollision(double px, double py) { return (collision = cbox.InBox(px, py)); }

        //public bool CheckForCollision(PointD pos) { return (collision = cbox.InBox(pos.X, pos.Y)); }
        //public bool CheckForCollision(double px, double py) { return (collision = cbox.InBox(px, py)); }

        void SetCollisionBox()
        {
            cbox.Set(pos.X - (boxSize.Width / 2), pos.Y - (boxSize.Height / 2), boxSize.Width, boxSize.Height);
        }
    }
}

#region 2020-01-01 Moved _Set into Set since there is only one version of Set at this time
//void _Set(double x, double y, double cboxWidth, double cboxHeight)
//{
//    pos.Set(x, y);
//    boxSize.Set(cboxWidth, cboxHeight);
//    SetCollisionBox();
//}
#endregion 2020-01-01 Moved _Set into Set since there is only one version of Set at this time
