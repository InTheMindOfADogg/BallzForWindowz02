using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace BallzForWindows01.MainGameParts
{
    using GamePhysicsParts;
    using Structs;
    class BaseGameScreen01
    {
        public string ClsName { get { return clsName; } }
        public PointD Center { get { return gameWindowRect.Center; } }
        public SizeD ScreenSize { get { return size; } }
        public bool Active { get { return active; } set { active = value; } }


        protected string clsName = "BaseGameScreen01";
        protected SizeD size = new SizeD(100, 100);
        protected RectangleD gameWindowRect;
        protected bool active = false;

        protected SolidBrush sb;
        protected Pen p;

        protected Color backgroundColor = Color.CornflowerBlue;

        public BaseGameScreen01(int gameWindowWidth, int gameWindowHeight, bool active = false)
        {
            clsName = "BaseGameScreen01";
            this.active = active;
            size = new SizeD(gameWindowWidth, gameWindowHeight);
            gameWindowRect = new RectangleD(0, 0, size.Width, size.Height);

        }
        public virtual void Load() { }
        public virtual void Update(MouseControls mcontrols, KeyboardControls01 kcontrols) { }

        
        public virtual void Draw(Graphics g)
        {
            DrawBackground(g);
        }
        
        public virtual void Reset() { }
        public virtual void CleanUp() 
        {
            DisposeOfDrawingTools();
        }

        public virtual void PrepareDrawingTools()
        {
            if (sb == null) { sb = new SolidBrush(backgroundColor); }
            if (p == null) { p = new Pen(Color.Black); }
        }
        public virtual void DisposeOfDrawingTools()
        {
            if (sb != null) { sb.Dispose(); sb = null; }
            if (p != null) { p.Dispose(); p = null; }
        }
        

        protected void DrawBackground(Graphics g)
        {
            sb.Color = backgroundColor;
            g.FillRectangle(sb, 0, 0, size.iWidth, size.iHeight);
        }

        /// <summary>
        /// Draws a black square at center of game window rectangle
        /// </summary>
        /// <param name="g"></param>
        protected void DrawCenterMarker(Graphics g)
        {
            // Test drawing center point
            Point pnt = gameWindowRect.Center.ToPoint(); int pntWidth = 10;
            g.FillRectangle(Brushes.Black, pnt.X - (pntWidth / 2), pnt.Y - (pntWidth / 2), pntWidth, pntWidth);
        }
    }
}
