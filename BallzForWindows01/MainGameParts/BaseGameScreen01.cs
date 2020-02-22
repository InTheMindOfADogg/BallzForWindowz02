using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace BallzForWindows01.MainGameParts
{
    using GamePhysicsParts;
    using DrawableParts;
    using Structs;
    class BaseGameScreen01
    {
        public string ClsName { get { return clsName; } }
        public PointD Center { get { return gameScreenRect.Center; } }
        public SizeD ScreenSize { get { return size; } }
        public bool Active { get { return active; } set { active = value; } }
        public bool ResetOnDeactivate { get { return resetOnDeactivate; } }
        public bool ChangeScreenRequest { get { return changeScreenRequest; } }
        public string RequestedScreen { get { return requestedScreen; } }




        protected string clsName = "BaseGameScreen01";
        protected SizeD size = new SizeD(100, 100);
        protected RectangleD gameScreenRect;
        protected bool active = false;
        protected bool resetOnDeactivate = false;
        protected bool changeScreenRequest = false;
        protected string requestedScreen = "";

        protected string testMsg = "";


        protected SolidBrush sb;
        protected Pen p;
        protected Color backgroundColor = Color.CornflowerBlue;

        protected List<Button03> btnList;

        protected Button03 btnMainMenu;

        public BaseGameScreen01(int gameWindowWidth, int gameWindowHeight, bool active = false)
        {
            clsName = "BaseGameScreen01";
            this.active = active;
            size = new SizeD(gameWindowWidth, gameWindowHeight);
            gameScreenRect = new RectangleD(0, 0, size.Width, size.Height);
            btnList = new List<Button03>();

            AddBackToMainMenuButton(10, 10);
        }
        public virtual void Load() { }
        public virtual void Update(MouseControls mcontrols, KeyboardControls01 kcontrols)
        {
            if (!active) { return; }

            DbgFuncs.AddStr(clsName, $"(From BaseGameScreen01.Update) btnList.Count: {btnList.Count}");
            for (int i = 0; i < btnList.Count; i++)
            {
                btnList[i].Update(mcontrols);
            }
        }
        public virtual void Draw(Graphics g) 
        {
            if (!active) { return; }
            DrawBackground(g);

            for (int i = 0; i < btnList.Count; i++)
            {
                btnList[i].Draw(g);
            }
        }
        public virtual void Reset() { }
        public virtual void CleanUp() { DisposeOfDrawingTools(); }

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

        /// <summary>
        /// Sets active flag to true, can override to do more.
        /// </summary>
        public virtual void Activate() { active = true; }

        /// <summary>
        /// Created 2020-02-08<br/>
        /// Sets active flag to false and calls Reset if resetOnDeactivate is true (set in class).<br/>
        /// Default value for resetOnDeactivate is false.<br/>
        /// Can pass in a true bool to force reset on deactivate.
        /// </summary>
        public virtual void Deactivate(bool forceReset = false)
        {
            active = false;
            if (forceReset || resetOnDeactivate) { Reset(); }
        }
        public void ClearChangeScreenRequest()
        {
            changeScreenRequest = false;
            requestedScreen = "";
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
            Point pnt = gameScreenRect.Center.ToPoint(); int pntWidth = 10;
            g.FillRectangle(Brushes.Black, pnt.X - (pntWidth / 2), pnt.Y - (pntWidth / 2), pntWidth, pntWidth);
        }
        protected Button03.delButtonEvent SetBtnEventChangeScreen(Button03 b)
        {
            Button03.delButtonEvent evnt = delegate ()
            {
                changeScreenRequest = true;
                requestedScreen = b.ClickValue;
                testMsg = $"Request to change screen to {requestedScreen}";
            };
            return evnt;
        }

        protected void AddBackToMainMenuButton(double x, double y, double width = 0, double height = 0)
        {
            btnMainMenu = new Button03();
            RectangleD r = new RectangleD();
            r.X = x;
            r.Y = y;
            r.Width = width;
            r.Height = height;
            btnMainMenu.Load("Back to Main Menu", r.X, r.Y, r.Width, r.Height);
            btnMainMenu.ClickValue = "MainMenu";
            btnMainMenu.BtnEvent += SetBtnEventChangeScreen(btnMainMenu);
            btnList.Add(btnMainMenu);
        }



    }
}
