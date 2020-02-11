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
        int startingActScrnIdx = -1;    // starting active screen index for Reset (class reset, aka reset everything)
        int actScrnIdx = -1;    // active screen index
        int newActScrnIdx = -1; // new active screen index (for changing active screen)
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
            SetStartingActiveScreen();
            // Load all screens to have them ready.
            for (int i = 0; i < gscreens.Count; i++) { gscreens[i].Load(); }
        }

        public void SetNewActiveScreen(string newScreenName)
        {
            for (int i = 0; i < gscreens.Count; i++)
            {
                if (streql(newScreenName, gscreens[i].ClsName)) { newActScrnIdx = (i == actScrnIdx) ? -1 : i; return; }
            }
        }

        public void Update(MouseControls mcontrols, KeyboardControls01 kcontrols)
        {
            string fnId = AssistFunctions.FnId(clsName, "Update");
            DbgFuncs.AddStr(fnId, $"starting active screen index: [startingActScrnIdx: {startingActScrnIdx}]");
            //DbgFuncs.AddStr(fnId, $"current active screen index: [actScrnIdx: {actScrnIdx}]");
            DbgFuncs.AddStr(fnId, $"current active screen: [{gscreens[actScrnIdx].ClsName}] [actScrnIdx: {actScrnIdx}]");

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

            if (newActScrnIdx > 0) { ChangeActiveScreen(); }

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

        private void ResetActiveScreen()
        {
            gscreens[actScrnIdx].Reset();
        }
        private void ChangeActiveScreen()
        {
            gscreens[actScrnIdx].Deactivate();
            actScrnIdx = newActScrnIdx;
            gscreens[actScrnIdx].Activate();
            newActScrnIdx = -1;
        }

        /// <summary>
        /// Compares lowercase version of both strings and returns true if match.<br/>
        /// Actual call is to AssistFunctions.streql
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        private bool streql(string str1, string str2) { return AssistFunctions.streql(str1, str2); }

    }



}
