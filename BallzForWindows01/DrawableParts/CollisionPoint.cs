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
        SizeF boxSize;
        bool collision = false;

        public bool Collision { get { return collision; } /*set { collision = value; }*/ }

        public CollisionPoint()
        {
            pos = new PointD();
            boxSize = new SizeF(25, 25);
            SetColor(255, 255, 0, 200);
        }
        public void Load(double x, double y)
        {
            pos.X = x;
            pos.Y = y;
        }
        public void Update(/*double ballx, double bally*/)
        {
            //IsInBoundingRect(ballx, bally);
        }
        public bool CheckForCollision(double px, double py)
        {
            if (px > pos.X && px < pos.X + boxSize.Width && py > pos.Y && py < pos.Y + boxSize.Height) { collision = true; return collision; }
            else { collision = false; return collision; }

            // wonder if I can do this?
            if (px > pos.X && px < pos.X + boxSize.Width && py > pos.Y && py < pos.Y + boxSize.Height) { return (collision = true); }
            else { return (collision = false); }
        }
        public void Draw(Graphics g)
        {
            Pen p = new Pen(color, 1);
            g.DrawRectangle(p, pos.fX - boxSize.Width / 2, pos.fY - boxSize.Height / 2, boxSize.Width, boxSize.Height);
            if(collision)
            {
                //SolidBrush sb = new SolidBrush(Color.Red);
                g.FillRectangle(Brushes.Red, pos.fX - boxSize.Width / 2, pos.fY - boxSize.Height / 2, boxSize.Width, boxSize.Height);
                //sb.Dispose();
            }
            p.Dispose();
        }
    }
}
