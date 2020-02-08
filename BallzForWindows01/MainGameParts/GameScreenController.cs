using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.MainGameParts
{
    
    using Structs;

    class GameScreenController
    {
        string clsName = "GameScreenController";
        List<BaseGameScreen01> gscreens;
        int activeScreenIdx = -1;
        int newActiveScreenIdx = -1;
        Size size;
        Bitmap backbuffer;


        public GameScreenController(int width, int height)
        {
            size = new Size(width, height);
            gscreens = new List<BaseGameScreen01>();
            InitGameScreens();
        }

        void InitGameScreens()
        {
            gscreens.Add(new MainMenu(size.Width, size.Height, true));
            gscreens.Add(new TestGameScreen01(size.Width, size.Height, false));

        }
        public void AddGameScreen(BaseGameScreen01 gs) { gscreens.Add(gs); }


        public void Load()
        {
            SetActiveScreen();
            // Load all screens to have them ready. I have it set up now so that
            for (int i = 0; i < gscreens.Count; i++) { gscreens[i].Load(); }
        }

        /// <summary>
        /// Sets the active screen index to the first screen that has active flag set to true. Sets
        /// all other screens active flag to false.
        /// </summary>
        private void SetActiveScreen()
        {
            // First screen with active flag set as active screen
            for (int i = 0; i < gscreens.Count; i++) { if (gscreens[i].Active) { activeScreenIdx = i; break; } }
            // Set all screens except active screen index to false (in case 2 screens got set as active)
            for (int i = 0; i < gscreens.Count; i++) { if (i == activeScreenIdx) { continue; } gscreens[i].Active = false; }
        }


        public void SetNewActiveScreen(string screenClassName)
        {
            for (int i = 0; i < gscreens.Count; i++)
            {
                if (streql(screenClassName, gscreens[i].ClsName)) { newActiveScreenIdx = i; return; }
            }
        }
        public void Update(MouseControls mcontrols, KeyboardControls01 kcontrols)
        {
            gscreens[activeScreenIdx].Update(mcontrols, kcontrols);
        }
        public void PrepareBackbuffer()
        {
            backbuffer = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(backbuffer);
            gscreens[activeScreenIdx].PrepareDrawingTools();
            gscreens[activeScreenIdx].Draw(g);
            gscreens[activeScreenIdx].DisposeOfDrawingTools();
            DbgFuncs.DrawDbgStrList(g);
            g.Dispose();
        }
        public void Draw(Graphics g)
        {
            g.DrawImage(backbuffer, new Point(0, 0));
            backbuffer.Dispose();
        }
        public void Reset()
        {
            gscreens[activeScreenIdx].Reset();
        }

        public void CleanUp()
        {
            for (int i = 0; i < gscreens.Count; i++) { gscreens[i].CleanUp(); }

            if (backbuffer != null)
            {
                backbuffer.Dispose();
                backbuffer = null;
            }
        }



        private bool streql(string str1, string str2) { return AssistFunctions.streql(str1, str2); }

    }



}
