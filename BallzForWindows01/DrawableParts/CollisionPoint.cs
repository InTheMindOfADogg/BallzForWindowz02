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


        public PointD Pos { get { return pos; } }
        public bool Collision { get { return collision; } }

        public CollisionPoint() { _Init(0,0,0,0,Color.FromArgb(255,255,0,200));  }
        public CollisionPoint(double x, double y, double cboxWidth, double cboxHeight)
        {
            _Init(x, y, cboxWidth, cboxHeight, Color.FromArgb(255, 175, 30, 50));
        }
        public CollisionPoint(double x, double y, double cboxWidth, double cboxHeight, Color c)
        {
            _Init(x, y, cboxWidth, cboxHeight, c);
        }
        void _Init(double x, double y, double cboxWidth, double cboxHeight, Color c)
        {
            
            pos = new PointD(x,y);
            boxSize = new SizeD(cboxWidth, cboxHeight);
            cbox = new RectangleD();
            this.color = c;
            SetCollisionBox();
        }
        public void Load(double x, double y) { _Load(x, y, 25, 25); } // just using 25 as default for now, might change later
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
        public void Update(double x, double y)
        {

        }

        public void Draw(Graphics g)
        {
            Pen p = new Pen(color, 1);
            g.DrawRectangle(p, pos.fX - 1, pos.fY - 1, 2, 2); // center point for testing

            g.DrawRectangle(p, cbox.fX, cbox.fY, cbox.fWidth, cbox.fHeight);

            if (collision) { g.FillRectangle(Brushes.Red, cbox.fX, cbox.fY, cbox.fWidth, cbox.fHeight); }
            p.Dispose();
        }

        public bool CheckForCollision(double px, double py) { return (collision = cbox.InBox(px, py)); }
        void SetCollisionBox()
        {
            cbox.Set(pos.X - (boxSize.Width / 2), pos.Y - (boxSize.Height / 2), boxSize.Width, boxSize.Height);
            //if (cbox == null) { cbox = new RectangleD(pos.X - (boxSize.Width / 2), pos.Y - (boxSize.Height / 2), boxSize.Width, boxSize.Height); }
            //else { cbox.Set(pos.X - (boxSize.Width / 2), pos.Y - (boxSize.Height / 2), boxSize.Width, boxSize.Height); }
        }
    }
}
