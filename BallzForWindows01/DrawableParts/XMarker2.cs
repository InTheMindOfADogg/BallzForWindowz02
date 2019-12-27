using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace BallzForWindows01.DrawableParts
{
    using GamePhysicsParts;

    class XMarker2 : DrawableObject
    {

        public PointD Position { get { return position; } }
        public PointD Center { get { return box.Center(); } }
        public Color DrawColor { get { return color; } set { color = value; } }
        public bool Placed { get { return placed; } }
        public bool ShowClickRectangle { get { return showClickRect; } set { showClickRect = value; } }
        public bool ShowXMarker { get { return showXMarker; } set { showXMarker = value; } }


        PointD position;
        RectangleD box;
        bool placed;
        bool showClickRect; // = false;
        bool showXMarker; // = true;

        int penWidth = 2;


        public XMarker2() { _Init(); }
        public void Load() { _Load(); }
        public void Update() { _Update(); }
        public void Draw(Graphics g) { _Draw(g); }
        public void Reset() { _Reset(); }
        public void CleanUp() { _CleanUp(); }


        public void SetSize(double width, double height)
        {
            SetClickRectangle(position.X, position.Y, width, height);
        }
        public void Place(double x, double y)
        {
            position.Set(x, y);
            SetClickRectangle(position.X, position.Y, box.Width, box.Height, true);
            placed = true;
            visible = true;

        }

        public bool InClickRect(PointD p) { return box.InBox(p.X, p.Y); }
        public bool InClickRect(double x, double y) { return box.InBox(x, y); }


        private void _Init()
        {
            clsName = "XMarker2";
            position = new PointD(0, 0);
            box = new RectangleD(position.X, position.Y, 20, 20);
            box.CenterOnXY(position.X, position.Y);
            visible = false;
            placed = false;
            showClickRect = false;
            showXMarker = true;
        }
        private void _Load() { }
        private void _Update() { }
        private void _Draw(Graphics g)
        {
            if (visible)
            {
                Pen p = new Pen(color, penWidth);
                if (showXMarker) { DrawX(g, p); }
                if (showClickRect) { DrawClickRectangle(g, p); }
                p.Dispose();
            }
        }
        private void _Reset()
        {
            position.Set(0, 0);
            visible = false;
            placed = false;
        }
        private void _CleanUp() { }


        private void DrawX(Graphics g, Pen p)
        {
            g.DrawLine(p, box.TopLeft().ToPointF(), box.BottomRight().ToPointF());
            g.DrawLine(p, box.BottomLeft().ToPoint(), box.TopRight().ToPointF());
        }
        private void DrawClickRectangle(Graphics g, Pen p)
        {
            g.DrawRectangle(p, box.fX, box.fY, box.fWidth, box.fHeight);
        }
        private void SetClickRectangle(double x, double y, double width, double height, bool centerOnXY = true)
        {
            box.Set(x, y, width, height);
            if (centerOnXY)
            {
                box.X -= box.Width / 2;
                box.Y -= box.Height / 2;
            }
        }

    }
}
