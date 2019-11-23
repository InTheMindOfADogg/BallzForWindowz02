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
        GameBall2 ball2;
        
        //bool pause = false;

        
        List<CollisionPoint> cplist = new List<CollisionPoint>();
        public MainGame01(int gameWindowWidth, int gameWindowHeight)
        {
            width = gameWindowWidth;
            height = gameWindowHeight;
            blockList = new List<BasicBlock01>();

            ball = new GameBall(new Size(width, height));
            ball2 = new GameBall2(new Size(width, height));

            ball.DrawDbgTxt = false;
            ball2.DrawDbgTxt = true;

        }
        public void Load()
        {
            LoadBlockList();
            // Load game ball starting position
            int ballStartX, ballStartY;

            //ballStartX = (width / 2) - (ball.Width / 2);ballStartY = (height - 100);    // original starting position
            ballStartX = 300;
            ballStartY = height / 2 + 50;

            ball.Load(ballStartX, ballStartY);
            ball2.Load(ballStartX, ballStartY);

            AddCollisionPoint(300, 570, 25);
            AddCollisionPoint(300, 370, 25);
            AddCollisionPoint(400, 500, 25);
            AddCollisionPoint(450, 550, 25);
            AddCollisionPoint(333, 500, 25);
        }
        void AddCollisionPoint(double x, double y, double sideLength)
        {
            CollisionPoint tempcp = new CollisionPoint();
            tempcp.Load(x, y, sideLength);
            cplist.Add(tempcp);
        }
        
        public void Update(MouseControls mc)
        {
            UpdateMouseControls(mc);

            ball.Update();
            ball2.Update();

            UpdateCollisionPointList();
            UpdateCollisionPointList2(ref ball2);
            DrawToBuffer();
        }
        
        void UpdateCollisionPointList()
        {
            CollisionPoint tempcp;
            for (int i = 0; i < cplist.Count; i++)
            {
                for(int j = 0; j < ball.CollisionPointList.Count; j++)
                {
                    tempcp = ball.CollisionPointList[j];
                    if(cplist[i].CheckForCollision(tempcp.Pos.X, tempcp.Pos.Y))
                    {
                        tempcp.PointHit = true;
                        ball.Collide = true;
                        return;                        
                    }
                    tempcp.PointHit = false;
                }
            }
        }

        void UpdateCollisionPointList2(ref GameBall2 b)
        {
            CollisionPoint tempcp;
            for (int i = 0; i < cplist.Count; i++)
            {
                for (int j = 0; j < b.CollisionPointList.Count; j++)
                {
                    tempcp = b.CollisionPointList[j];
                    if (cplist[i].CheckForCollision(tempcp.Pos.X, tempcp.Pos.Y))
                    {
                        tempcp.PointHit = true;
                        b.Collide = true;
                        //return;
                    }
                    tempcp.PointHit = false;
                }
            }
        }

        int mStartX, mStartY, deltaX, deltaY;
        #region UpdateMouseControls versions
        public void UpdateMouseControls(MouseControls mc)
        {
            #region ball version 1
            #region Setting up the shot
            if(!ball.BallLaunched)
            {
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

                // launch / set flight path
                if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up)
                {
                    if (ball.IsInLaunchButtonRect(mc.X, mc.Y) && ball.ReadyForLaunch)
                    {
                        ball.LaunchBall();
                    }
                    else //if (mc.Y < ball.Y - ball.Height * 2) // uncomment if here to restrict aim to only north side of the ball
                    {
                        ball.SetFlightPath(mc.X, mc.Y);
                    }
                }
            }
            #endregion Setting up the shot
            // unpause
            if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up && ball.BallLaunched == true)
            {
                ball.Pause = false;
                
            }

            // Reset
            if (mc.RightButtonState == UpDownState.Down && mc.LastRightButtonState == UpDownState.Up)
            {
                ball.Reset();
            }

            #endregion ball version 1

            #region ball2
            #region Setting up the shot
            if (!ball2.BallLaunched)
            {
                if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up && ball2.SettingSpin)
                {
                    if (ball2.IsInSpinRect(mc.X, mc.Y))
                    {
                        mStartX = mc.X;
                        mStartY = mc.Y;
                        deltaX = 0;
                        deltaY = 0;
                        ball2.PlacingSpinRect = true;
                    }
                }
                if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Down && ball2.PlacingSpinRect)
                {
                    deltaX = mc.X - mStartX;
                    deltaY = mc.Y - mStartY;
                    ball2.AdjustSpinMarker(mc.X, mc.Y);
                }
                if (mc.LeftButtonState == UpDownState.Up && mc.LastLeftButtonState == UpDownState.Down && ball2.PlacingSpinRect)
                {
                    ball2.PlacingSpinRect = false;
                }

                // launch / set flight path
                if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up)
                {
                    if (ball2.IsInLaunchButtonRect(mc.X, mc.Y) && ball2.ReadyForLaunch)
                    {
                        ball2.LaunchBall();
                    }
                    else //if (mc.Y < ball2.Y - ball2.Height * 2) // uncomment if here to restrict aim to only north side of the ball2
                    {
                        ball2.SetFlightPath(mc.X, mc.Y);
                    }
                }
            }
            #endregion Setting up the shot
            // unpause
            if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up && ball2.BallLaunched == true)
            {
                ball2.Pause = false;

            }

            // Reset
            if (mc.RightButtonState == UpDownState.Down && mc.LastRightButtonState == UpDownState.Up)
            {
                ball2.Reset();
            }
            #endregion ball2
        }

        //public void UpdateMouseControls(MouseControls mc)
        //{
        //    #region Setting up the shot

        //    if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up && ball.SettingSpin)
        //    {
        //        if (ball.IsInSpinRect(mc.X, mc.Y))
        //        {
        //            mStartX = mc.X;
        //            mStartY = mc.Y;
        //            deltaX = 0;
        //            deltaY = 0;
        //            ball.PlacingSpinRect = true;
        //        }
        //    }
        //    if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Down && ball.PlacingSpinRect)
        //    {
        //        deltaX = mc.X - mStartX;
        //        deltaY = mc.Y - mStartY;
        //        ball.AdjustSpinMarker(mc.X, mc.Y);
        //    }
        //    if (mc.LeftButtonState == UpDownState.Up && mc.LastLeftButtonState == UpDownState.Down && ball.PlacingSpinRect)
        //    {
        //        ball.PlacingSpinRect = false;
        //    }
        //    #endregion Setting up the shot

        //    #region launch the ball


        //    if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up && ball.BallLaunched == false)
        //    {
        //        if (ball.IsInLaunchButtonRect(mc.X, mc.Y) && ball.ReadyForLaunch)
        //        {
        //            ball.LaunchBall();
        //        }
        //        else //if (mc.Y < ball.Y - ball.Height * 2) // uncomment if here to restrict aim to only north side of the ball
        //        {
        //            ball.SetFlightPath(mc.X, mc.Y);
        //        }
        //    }
        //    #endregion launch the ball

        //    // Reset
        //    if (mc.RightButtonState == UpDownState.Down && mc.LastRightButtonState == UpDownState.Up)
        //    {
        //        ball.Reset();
        //    }
        //}
        #endregion UpdateMouseControls versions
        private void DrawToBuffer()
        {
            backbuffer = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(backbuffer);
            SolidBrush sb = new SolidBrush(Color.CornflowerBlue);
            g.FillRectangle(sb, 0, 0, width, height);
            DrawBlockList(g);

            ball.Draw(g);
            ball2.Draw(g);

            DrawCollisionPointList(g);
            DbgFuncs.DrawDbgStrList(g);
            sb.Dispose();
            g.Dispose();
        }
        public void Draw(Graphics g)
        {
            g.DrawImage(backbuffer, new PointF(0, 0));
            backbuffer.Dispose();
        }

        void DrawCollisionPointList(Graphics g)
        {
            for(int i = 0; i < cplist.Count; i++)
            {
                cplist[i].Draw(g);
            }
        }

        #region BlockList Functions
        private void LoadBlockList()
        {
            int a, r, g, b;
            int x = 0;
            int y = 0;
            a = 255;
            r = 150;
            g = 0;
            b = 0;
            Color c = Color.FromArgb(a, r, g, b);
            for (int i = 0; i < 10; i++)
            {
                AddBlock(x, y, c);
                if (x < width) { x = blockList[blockList.Count - 1].Width * i; }
                else
                {
                    x = 0;
                    y += blockList[blockList.Count - 1].Height;
                }
                if (g + i * 10 < 255) { g += i * 10; }
                else { g = 255; }
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
        private void UpdateBlockList() { for (int i = 0; i < blockList.Count; i++) { blockList[i].Update(); } }
        private void DrawBlockList(Graphics g) { for (int i = 0; i < blockList.Count; i++) { blockList[i].Draw(g); } }
        private void CleanUpBlockList()
        {
            for (int i = 0; i < blockList.Count; i++) { blockList[i].CleanUp(); }
            blockList.RemoveRange(0, blockList.Count);
        }
        #endregion BlockList Functions

        public void CleanUp()
        {
            ball.CleanUp();
            ball2.CleanUp();
            CleanUpBlockList();

        }


    }
}
