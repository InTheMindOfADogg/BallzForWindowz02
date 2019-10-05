using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using System.Timers;

using BallzForWindows01.DrawableParts;

namespace BallzForWindows01.MainGameParts
{
    class MainGame01 : GameParts01
    {
        List<BasicBlock01> blockList;// = new List<BasicBlock01>();
        GameBall ball;
        public MainGame01(int gameWindowWidth, int gameWindowHeight)
        {
            InitializeGameParts(gameWindowWidth, gameWindowHeight);
            blockList = new List<BasicBlock01>();
            ball = new GameBall();
            ball.GameScreenSize = new Size(gameWindowWidth, gameWindowHeight);

        }
        public void Load()
        {
            LoadBlockList();

            // Load game ball starting position
            int ballStartX, ballStartY;
            ballStartX = (width / 2) - ball.Width / 2;
            //ballStartY = (height - ball.Height * 4);
            ballStartY = (height - 100);
            ball.Load(ballStartX, ballStartY);
        }
        public void Update()
        {
            //debug.writeLine("Updating");
            ball.Update();
            
            DrawToBuffer();
        }
        
        int mStartX, mStartY, deltaX, deltaY;
        public void UpdateMouseControls(MouseControls mc)
        {

            #region Adjust spin rectangle
            if (mc.leftState == UpDownState.Down && mc.lastLeftState == UpDownState.Up && ball.SettingSpin)
            {
                if (ball.IsInSpinRect(mc.x, mc.y))
                {
                    mStartX = mc.x;
                    mStartY = mc.y;
                    deltaX = 0;
                    deltaY = 0;
                    ball.PlacingSpinRect = true;
                }
            }
            if (mc.leftState == UpDownState.Down && mc.lastLeftState == UpDownState.Down && ball.PlacingSpinRect)
            {
                deltaX = mc.x - mStartX;
                deltaY = mc.y - mStartY;
                ball.AdjustSpinMarker(mc.x, mc.y);
            }
            if (mc.leftState == UpDownState.Up && mc.lastLeftState == UpDownState.Down && ball.PlacingSpinRect)
            {
                ball.PlacingSpinRect = false;
            }
            #endregion


            if (mc.leftState == UpDownState.Down && mc.lastLeftState == UpDownState.Up && ball.BallLaunched == false)
            {
                if (ball.IsInLaunchButtonRect(mc.x, mc.y) && ball.ReadyForLaunch)
                {
                    ball.LaunchBall();
                    debug.writeLine("Button clicked");
                    debug.writeLine("ball.ReadyForLaunch: " + ball.ReadyForLaunch);

                }
                else if (mc.y < ball.Y - ball.Height * 2)
                {
                    ball.SetFlightPath(mc.x, mc.y);
                }
                // launch ball(s)
                
            }

            if (mc.rightState == UpDownState.Down && mc.lastRightState == UpDownState.Up)
            {
                ball.Reset();
            }

            //mc.lastLeftState = mc.leftState;
            //mc.lastRightState = mc.rightState;
        }
        private void DrawToBuffer()
        {
            SolidBrush sb = new SolidBrush(Color.Aqua);
            using (Graphics g = Graphics.FromImage(bufferMap))
            {
                g.FillRectangle(sb, 0, 0, width, height);
                //DrawBlockList(g);
                for (int i = 0; i < blockList.Count; i++)
                {
                    blockList[i].Draw(g);
                }
                ball.Draw(g);


            }
        }
        public void Draw(Graphics gRef)
        {
            //DrawToBuffer();
            gRef.DrawImage(bufferMap, new PointF(0, 0));

        }

        #region BlockList Functions
        private void LoadBlockList()
        {
            int a, r, g, b;
            int x = 0;
            int y = 0;
            a = 255;
            r = 255;
            g = 0;
            b = 0;
            Color c = Color.FromArgb(a, r, g, b);
            for (int i = 0; i < 10; i++)
            {
                AddBlock(x, y, c);
                if (x < width)
                {
                    x = blockList[blockList.Count - 1].Width * i;
                }
                else
                {
                    x = 0;
                    y += blockList[blockList.Count - 1].Height;
                }
                if (g + i * 10 < 255)
                {
                    g += i * 10;
                }
                else
                {
                    g = 255;
                }
                c = Color.FromArgb(a, r, g, b);
            }
        }
        private void AddBlock(int x, int y)
        {
            BasicBlock01 b = new BasicBlock01();
            b.Load(x, y);
            blockList.Add(b);
        }
        private void AddBlock(int x, int y, Color c)
        {
            BasicBlock01 b = new BasicBlock01();
            b.Load(x, y);
            b.DrawColor = c;
            b.SetNumberOfHits(1, 20);
            blockList.Add(b);
        }
        private void UpdateBlockList()
        {
            for (int i = 0; i < blockList.Count; i++)
            {
                blockList[i].Update();
            }
        }
        private void DrawBlockList(Graphics g)
        {
            for (int i = 0; i < blockList.Count; i++)
            {
                blockList[i].Draw(g);
            }
        }
        #endregion BlockList Functions


    }
}
