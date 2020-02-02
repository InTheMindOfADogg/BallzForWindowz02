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
        List<BaseGameScreen01> gscreens;
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
            gscreens.Add(new TestGameScreen01(size.Width, size.Height));
        }

        public void Load()
        {
            for(int i = 0; i < gscreens.Count; i++)
            {
                gscreens[i].Load();
            }
        }

        public void Update(MouseControls mcontrols, KeyboardControls01 kcontrols) 
        {
            for (int i = 0; i < gscreens.Count; i++)
            {
                if (gscreens[i].Active) 
                {
                    gscreens[i].Update(mcontrols, kcontrols);
                }
            }
        }
        public void PrepareBackbuffer()
        {
            backbuffer = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(backbuffer);
            for (int i = 0; i < gscreens.Count; i++)
            {
                if(gscreens[i].Active)
                {
                    gscreens[i].Draw(g);
                }
                
            }

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
            for (int i = 0; i < gscreens.Count; i++)
            {
                if (gscreens[i].Active) 
                { 
                    gscreens[i].Reset(); 
                }
            }
        }

        public void CleanUp() 
        {
            for (int i = 0; i < gscreens.Count; i++)
            {
                if(gscreens[i].Active)
                {
                    gscreens[i].CleanUp();
                }
            }
            if(backbuffer != null)
            {
                backbuffer.Dispose();
                backbuffer = null;
            }
        }

    }
}
