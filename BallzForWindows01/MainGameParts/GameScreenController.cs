using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.MainGameParts
{

    using Structs;
    using GameScreensFolder;

    class GameScreenController
    {
        string clsName = "GameScreenController";
        List<BaseGameScreen01> gscreens;
        int startingActScrnIdx = -1;    // starting active screen index for Reset (class reset, aka reset everything)
        int actScrnIdx = -1;    // active screen index
        int newActScrnIdx = -1; // new active screen index (for changing active screen)
        Size size;
        Bitmap backbuffer;
        bool newScreenFirstFrame = false;


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
            gscreens.Add(new TestGameScreen02(size.Width, size.Height, false));

        }
        public void AddGameScreen(BaseGameScreen01 gs) { gscreens.Add(gs); }


        public void Load()
        {
            SetStartingActiveScreen();
            // Load all screens to have them ready.
            for (int i = 0; i < gscreens.Count; i++) { gscreens[i].Load(); }
        }



        public void Update(MouseControls mcontrols, KeyboardControls01 kcontrols)
        {
            string fnId = AssistFunctions.FnId(clsName, "Update");
            DbgFuncs.AddStr(fnId, $"starting active screen index: [startingActScrnIdx: {startingActScrnIdx}]");
            //DbgFuncs.AddStr(fnId, $"current active screen index: [actScrnIdx: {actScrnIdx}]");
            DbgFuncs.AddStr(fnId, $"current active screen: [{gscreens[actScrnIdx].ClsName}] [actScrnIdx: {actScrnIdx}]");
            DbgFuncs.AddStr(fnId, $"changeScreenRequest: [{gscreens[actScrnIdx].ChangeScreenRequest}]");
            DbgFuncs.AddStr(fnId, $"requestedScreen: [{gscreens[actScrnIdx].RequestedScreen}]");
            if (newScreenFirstFrame) 
            { 
                gscreens[actScrnIdx].Reset();
                mcontrols.Reset();
                kcontrols.Reset();
                newScreenFirstFrame = false; 
            }
            gscreens[actScrnIdx].Update(mcontrols, kcontrols);

        }
        public void PrepareBackbuffer()
        {
            backbuffer = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(backbuffer);
            gscreens[actScrnIdx].PrepareDrawingTools();
            gscreens[actScrnIdx].Draw(g);
            gscreens[actScrnIdx].DisposeOfDrawingTools();
            DbgFuncs.DrawDbgStrList(g);
            g.Dispose();
        }
        public void Draw(Graphics g)
        {
            g.DrawImage(backbuffer, new Point(0, 0));
            backbuffer.Dispose();

            //if (newActScrnIdx > 0) { ChangeActiveScreen(); }
            if (gscreens[actScrnIdx].ChangeScreenRequest)
            {
                ChangeActiveScreen(gscreens[actScrnIdx].RequestedScreen);
            }

        }
        public void Reset()
        {
            for (int i = 0; i < gscreens.Count; i++) { gscreens[i].Reset(); }

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

        /// <summary>
        /// Sets the active screen index to the first screen that has active flag set to true. Sets
        /// all other screens active flag to false.
        /// </summary>
        private void SetStartingActiveScreen()
        {
            // First screen with active flag set as active screen
            for (int i = 0; i < gscreens.Count; i++) { if (gscreens[i].Active) { actScrnIdx = i; break; } }
            // Set all screens except active screen index to false (in case 2 screens got set as active)
            for (int i = 0; i < gscreens.Count; i++) { if (i == actScrnIdx) { continue; } gscreens[i].Deactivate(false); }
            if (startingActScrnIdx < 0) startingActScrnIdx = actScrnIdx;
        }

        private void ResetActiveScreen() { gscreens[actScrnIdx].Reset(); }
        
        private int GetScreenIndex(string screenName)
        {
            for (int i = 0; i < gscreens.Count; i++) { if (streql(gscreens[i].ClsName, screenName)) { return i; } }
            return -1;
        }
        private void ChangeActiveScreen(string screenName)
        {
            newActScrnIdx = GetScreenIndex(screenName);
            gscreens[actScrnIdx].ClearChangeScreenRequest();
            if (newActScrnIdx < 0) { return; } // Did not find screen.
            gscreens[actScrnIdx].Deactivate();
            actScrnIdx = newActScrnIdx;
            gscreens[actScrnIdx].Activate();
            newScreenFirstFrame = true;
            newActScrnIdx = -1;
        }

        private bool streql(string str1, string str2) { return AssistFunctions.streql(str1, str2); }



        //public void SetNewActiveScreen(string newScreenName)
        //{
        //    for (int i = 0; i < gscreens.Count; i++) { if (streql(newScreenName, gscreens[i].ClsName)) { newActScrnIdx = (i == actScrnIdx) ? -1 : i; return; } }
        //}

    }



}
