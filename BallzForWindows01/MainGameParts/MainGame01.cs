using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using System.Timers;

using BallzForWindows01.DrawableParts;
using BallzForWindows01.Structs;

namespace BallzForWindows01.MainGameParts
{
    using static AssistFunctions;

    class MainGame01 
    {
        int width, height;
        Bitmap backbuffer = null;

        List<BasicBlock01> blockList;
        GameBall ball;
        public MainGame01(int gameWindowWidth, int gameWindowHeight)
        {
            width = gameWindowWidth;
            height = gameWindowHeight;

            blockList = new List<BasicBlock01>();
            ball = new GameBall(new Size(width, height));

        }
        public void Load()
        {
            LoadBlockList();

            // Load game ball starting position
            int ballStartX, ballStartY;
            ballStartX = (width / 2) - ball.Width / 2;
            ballStartY = (height - 100);
            ball.Load(ballStartX, ballStartY);
        }
        
        public void Update(MouseControls mc)
        {
            UpdateMouseControls(mc);
            ball.Update();
            DrawToBuffer();
        }

        int mStartX, mStartY, deltaX, deltaY;
        public void UpdateMouseControls(MouseControls mc)
        {
            #region Setting up the shot
            if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up && ball.SettingSpin)
            {
                if (ball.IsInSpinRect(mc.X, mc.Y))
                {
                    mStartX = mc.X;
                    mStartY = mc.Y;
                    deltaX = 0;
                    deltaY = 0;
                    ball.PlacingSpinRect = true;
                }
            }
            if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Down && ball.PlacingSpinRect)
            {
                deltaX = mc.X - mStartX;
                deltaY = mc.Y - mStartY;
                ball.AdjustSpinMarker(mc.X, mc.Y);
            }
            if (mc.LeftButtonState == UpDownState.Up && mc.LastLeftButtonState == UpDownState.Down && ball.PlacingSpinRect)
            {
                ball.PlacingSpinRect = false;
            }
            #endregion Setting up the shot

            #region launch the ball
            if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up && ball.BallLaunched == false)
            {
                if (ball.IsInLaunchButtonRect(mc.X, mc.Y) && ball.ReadyForLaunch)
                {
                    ball.LaunchBall();
                    cwl("Button clicked");
                    cwl("ball.ReadyForLaunch: " + ball.ReadyForLaunch);
                }
                else if (mc.Y < ball.Y - ball.Height * 2)
                {
                    ball.SetFlightPath(mc.X, mc.Y);
                }
                // launch ball(s)

            }
            #endregion launch the ball

            if (mc.RightButtonState == UpDownState.Down && mc.LastRightButtonState == UpDownState.Up)
            {
                ball.Reset();
            }
        }
        private void DrawToBuffer()
        {
            backbuffer = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(backbuffer);
            SolidBrush sb = new SolidBrush(Color.Aqua);
            g.FillRectangle(sb, 0, 0, width, height);
            //DrawBlockList(g);
            for (int i = 0; i < blockList.Count; i++)
            {
                blockList[i].Draw(g);
            }
            ball.Draw(g);

            sb.Dispose();
            g.Dispose();
            
        }
        public void Draw(Graphics g)
        {
            //DrawToBuffer();
            g.DrawImage(backbuffer, new PointF(0, 0));
            backbuffer.Dispose();

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
