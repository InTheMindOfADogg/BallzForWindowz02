using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using System.Timers;



namespace BallzForWindows01.MainGameParts
{
    using static AssistFunctions;
    using DrawableParts;
    using Structs;

    class MainGame01
    {
        int width, height;

        Bitmap backbuffer = null;

        List<BasicBlock01> blockList;

        GameBall04 ball4 = null;

        List<CollisionPoint> cplist = new List<CollisionPoint>();

        CollisionLine cline;
        CollisionLine02 cline2;
        CollisionLineMoveable01 clm;

        public MainGame01(int gameWindowWidth, int gameWindowHeight)
        {
            width = gameWindowWidth;
            height = gameWindowHeight;
            blockList = new List<BasicBlock01>();

            ball4 = new GameBall04(new Size(width, height));
            if (ball4 != null) { ball4.DrawDbgTxt = true; }

            cline = new CollisionLine();
            cline2 = new CollisionLine02();
            clm = new CollisionLineMoveable01();

        }
        public void Load()
        {
            LoadBlockList();
            // Load game ball starting position
            int ballStartX, ballStartY;

            //ballStartX = (width / 2) - (ball.Width / 2);ballStartY = (height - 100);    // original starting position

            ballStartX = 500;
            ballStartY = height / 2 + 50;

            if (ball4 != null)
            {
                ball4.Load(ballStartX, ballStartY, 2, 10, 0, 7);
                ball4.SetCircleColor(Color.FromArgb(255, 255, 50, 50));
            }


            //AddCollisionPoint(300, 570, 25);
            //AddCollisionPoint(300, 370, 25);
            //AddCollisionPoint(400, 500, 25);
            //AddCollisionPoint(450, 550, 25);
            //AddCollisionPoint(333, 500, 25);

            TESTLoadCollisionLine01();  // cline is yellowish color

            TESTLoadCollisionLine02();  // cline2 is greenish color

            TESTLoadCollisionMoveable01();  // clm is dark red color


        }
        void TESTLoadCollisionLine01()
        {
            double
                startx = 550,
                starty = 550,
                length = 50,
                rotation = 0;
            int
                thickness = 5;

            cline.Load(startx, starty, length, rotation, thickness);
            cplist.AddRange(cline.CpList);
        }
        void TESTLoadCollisionLine02()
        {
            double
                startx = 550, starty = 550,
                length = 50,
                rotation = 0,
                spaceBtCp = -10;    
            int
                thickness = 5;

            cline2.Load(startx, starty, length, rotation, thickness, spaceBtCp);
            cplist.AddRange(cline2.CpList);
        }
        void TESTLoadCollisionMoveable01()
        {
            double
                startx = 650,
                starty = 550,
                length = 50,
                rotation = 0,
                spaceBtCp = -10;
            int
                thickness = 5;

            clm.Load(startx, starty, length, rotation, thickness, spaceBtCp);
            
            cplist.AddRange(clm.CpList);
        }

        double lineRotationSpeed = 0.005;
        public void Update(MouseControls mc, KeyboardControls01 kc)
        {
            // GameBall04 ball4 has mouse controls and collision detection logic built into Update function
            if (ball4 != null) { ball4.Update(mc, kc, cplist); }

            cline.Update(lineRotationSpeed);
            //cline.Update();
            //cline2.Update();


            clm.Update(lineRotationSpeed);

            DrawToBuffer();
        }
        public void Draw(Graphics g)
        {
            // Add drawing in DrawToBuffer
            g.DrawImage(backbuffer, new PointF(0, 0));
            backbuffer.Dispose();
        }
        public void CleanUp()
        {
            if (ball4 != null) { ball4.CleanUp(); }
            CleanUpBlockList();
        }


        void AddCollisionPoint(double x, double y, double sideLength)
        {
            CollisionPoint tempcp = new CollisionPoint();
            tempcp.Load(x, y, sideLength);
            cplist.Add(tempcp);
        }
        private void DrawToBuffer()
        {
            backbuffer = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(backbuffer);
            SolidBrush sb = new SolidBrush(Color.CornflowerBlue);
            //Pen p = new Pen(Color.Black);
            g.FillRectangle(sb, 0, 0, width, height);
            DrawBlockList(g);

            if (ball4 != null) { ball4.Draw(g); }

            //DrawCollisionPointList(g);



            cline.Draw(g, false);
            cline2.Draw(g, false);
            clm.Draw(g, true);
            

            DbgFuncs.DrawDbgStrList(g);

            sb.Dispose();            
            g.Dispose();
        }

        //void DrawCollisionPointList(Graphics g)
        //{
        //    if(cplist.Count == 0) { return; }
        //    Pen p = new Pen(Color.Black);
        //    SolidBrush sb = new SolidBrush(Color.Black);
        //    for (int i = 0; i < cplist.Count; i++) { cplist[i].Draw(g, p, sb); }
        //    p.Dispose();
        //    sb.Dispose();
        //}

        
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




    }
}

#region previous versions of DrawCollisionPointList
// DrawCollisionPointList - creates all resources needed in function
//void DrawCollisionPointList(Graphics g)
//{
//    Pen p = new Pen(Color.Red);
//    SolidBrush collisionFillBrush = new SolidBrush(Color.FromArgb(25, Color.Red));
//    SolidBrush pointHitFillBrush = new SolidBrush(Color.FromArgb(25, Color.Green));
//    for (int i = 0; i < cplist.Count; i++)
//    {
//        //cplist[i].Draw(g);
//        cplist[i].Draw(g, p, collisionFillBrush, pointHitFillBrush);
//    }
//    p.Dispose();
//    collisionFillBrush.Dispose();
//    pointHitFillBrush.Dispose();
//}


//void DrawCollisionPointList(Graphics g, SolidBrush sb)
//{
//    Pen p = new Pen(Color.Red);
//    SolidBrush collisionFillBrush = new SolidBrush(Color.FromArgb(25, Color.Red));
//    SolidBrush pointHitFillBrush = new SolidBrush(Color.FromArgb(25, Color.Green));
//    for (int i = 0; i < cplist.Count; i++)
//    {
//        //cplist[i].Draw(g);
//        cplist[i].Draw(g, p, collisionFillBrush, pointHitFillBrush);
//    }
//    p.Dispose();
//    collisionFillBrush.Dispose();
//    pointHitFillBrush.Dispose();
//}
#endregion previous versions of DrawCollisionPointList

#region UpdateCollisionPointList functions (only used in GameBall1 and GameBall2)

//// UpdateCollisionPointList for ball 
//void UpdateCollisionPointList()
//{
//    CollisionPoint tempcp;
//    for (int i = 0; i < cplist.Count; i++)
//    {
//        for (int j = 0; j < ball.CollisionPointList.Count; j++)
//        {
//            tempcp = ball.CollisionPointList[j];
//            if (cplist[i].CheckForCollision(tempcp.Pos.X, tempcp.Pos.Y))
//            {
//                tempcp.PointHit = true;
//                ball.Collide = true;
//                return;
//            }
//            tempcp.PointHit = false;
//        }
//    }
//}

//// UpdateCollisionPointList2 for ball2
//void UpdateCollisionPointList2()
//{
//    CollisionPoint tempcp;
//    for (int i = 0; i < cplist.Count; i++)
//    {
//        for (int j = 0; j < ball2.CollisionPointList.Count; j++)
//        {
//            tempcp = ball2.CollisionPointList[j];
//            if (cplist[i].CheckForCollision(tempcp.Pos.X, tempcp.Pos.Y))
//            {
//                tempcp.PointHit = true;
//                ball2.Collide = true;
//                ball2.PublicHitIdxList.Add(i);
//                return;
//                //continue;
//            }
//            tempcp.PointHit = false;
//        }
//    }
//}


#endregion UpdateCollisionPointList functions (only used in GameBall1 and GameBall2)


#region UpdateMouseControls versions (only used in GameBall1 and GameBall2)
//int mStartX, mStartY, deltaX, deltaY;
//public void UpdateMouseControls(MouseControls mc)
//{
//    if (ball == null) { goto Ball2Controls; }
//    #region ball version 1
//    #region Setting up the shot
//    if (!ball.BallLaunched)
//    {
//        if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up && ball.SettingSpin)
//        {
//            if (ball.IsInSpinRect(mc.X, mc.Y))
//            {
//                mStartX = mc.X;
//                mStartY = mc.Y;
//                deltaX = 0;
//                deltaY = 0;
//                ball.PlacingSpinRect = true;
//            }
//        }
//        if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Down && ball.PlacingSpinRect)
//        {
//            deltaX = mc.X - mStartX;
//            deltaY = mc.Y - mStartY;
//            ball.AdjustSpinMarker(mc.X, mc.Y);
//        }
//        if (mc.LeftButtonState == UpDownState.Up && mc.LastLeftButtonState == UpDownState.Down && ball.PlacingSpinRect)
//        {
//            ball.PlacingSpinRect = false;
//        }

//        // launch / set flight path
//        if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up)
//        {
//            if (ball.IsInLaunchButtonRect(mc.X, mc.Y) && ball.ReadyForLaunch)
//            {
//                ball.LaunchBall();
//            }
//            else //if (mc.Y < ball.Y - ball.Height * 2) // uncomment if here to restrict aim to only north side of the ball
//            {
//                ball.SetFlightPath(mc.X, mc.Y);
//            }
//        }
//    }
//    #endregion Setting up the shot
//    // unpause
//    if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up && ball.BallLaunched == true)
//    {
//        ball.Pause = false;

//    }

//    // Reset
//    if (mc.RightButtonState == UpDownState.Down && mc.LastRightButtonState == UpDownState.Up)
//    {
//        ball.Reset();
//    }

//#endregion ball version 1

//Ball2Controls:
//    if (ball2 == null) { goto EndControls; }
//    #region ball2 only
//    #region Setting up the shot
//    if (!ball2.BallLaunched)
//    {
//        if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up && ball2.SettingSpin)
//        {
//            if (ball2.IsInSpinRect(mc.X, mc.Y))
//            {
//                mStartX = mc.X;
//                mStartY = mc.Y;
//                deltaX = 0;
//                deltaY = 0;
//                ball2.PlacingSpinRect = true;
//            }
//        }
//        if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Down && ball2.PlacingSpinRect)
//        {
//            deltaX = mc.X - mStartX;
//            deltaY = mc.Y - mStartY;
//            ball2.AdjustSpinMarker(mc.X, mc.Y);
//        }
//        if (mc.LeftButtonState == UpDownState.Up && mc.LastLeftButtonState == UpDownState.Down && ball2.PlacingSpinRect)
//        {
//            ball2.PlacingSpinRect = false;
//        }

//        // launch / set flight path
//        if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up)
//        {
//            if (ball2.IsInLaunchButtonRect(mc.X, mc.Y) && ball2.ReadyForLaunch)
//            {
//                ball2.LaunchBall();
//            }
//            else //if (mc.Y < ball2.Y - ball2.Height * 2) // uncomment if here to restrict aim to only north side of the ball2
//            {
//                ball2.SetFlightPath(mc.X, mc.Y);
//            }
//        }
//    }
//    #endregion Setting up the shot
//    // unpause
//    if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up && ball2.BallLaunched == true)
//    {
//        ball2.Pause = false;
//    }

//    // Reset
//    if (mc.RightButtonState == UpDownState.Down && mc.LastRightButtonState == UpDownState.Up)
//    {
//        ball2.Reset();
//    }
//#endregion ball2

//EndControls:
//    return;

//}
#endregion UpdateMouseControls versions

#region full back up before removing gameball1 and gameball2
//namespace BallzForWindows01.MainGameParts
//{
//    using static AssistFunctions;
//    using DrawableParts;
//    using Structs;

//    class MainGame01
//    {
//        int width, height;
//        Bitmap backbuffer = null;

//        List<BasicBlock01> blockList;
//        //GameBall ball = null;
//        //GameBall2 ball2 = null;

//        GameBall04 ball4 = null;

//        //bool pause = false;


//        List<CollisionPoint> cplist = new List<CollisionPoint>();
//        public MainGame01(int gameWindowWidth, int gameWindowHeight)
//        {
//            width = gameWindowWidth;
//            height = gameWindowHeight;
//            blockList = new List<BasicBlock01>();

//            //ball = new GameBall(new Size(width, height));
//            //ball2 = new GameBall2(new Size(width, height));
//            ball4 = new GameBall04(new Size(width, height));

//            //if (ball != null) { ball.DrawDbgTxt = false; }
//            //if (ball2 != null) { ball2.DrawDbgTxt = false; }
//            if (ball4 != null) { ball4.DrawDbgTxt = true; }

//        }
//        public void Load()
//        {
//            LoadBlockList();
//            // Load game ball starting position
//            int ballStartX, ballStartY;

//            //ballStartX = (width / 2) - (ball.Width / 2);ballStartY = (height - 100);    // original starting position

//            ballStartX = 500;
//            ballStartY = height / 2 + 50;

//            //if (ball != null) { ball.Load(ballStartX, ballStartY); }
//            //if (ball2 != null) { ball2.Load(ballStartX, ballStartY); }
//            if (ball4 != null)
//            {
//                ball4.Load(ballStartX, ballStartY, 2, 10, 0, 5);
//                ball4.SetCircleColor(Color.FromArgb(255, 255, 50, 50));
//            }


//            AddCollisionPoint(300, 570, 25);
//            AddCollisionPoint(300, 370, 25);
//            AddCollisionPoint(400, 500, 25);
//            AddCollisionPoint(450, 550, 25);
//            AddCollisionPoint(333, 500, 25);
//        }
//        void AddCollisionPoint(double x, double y, double sideLength)
//        {
//            CollisionPoint tempcp = new CollisionPoint();
//            tempcp.Load(x, y, sideLength);
//            cplist.Add(tempcp);
//        }

//        public void Update(MouseControls mc, KeyboardControls01 kc)
//        {
//            // GameBall and GameBall2 do not have mouse controls built into Update
//            //UpdateMouseControls(mc);
//            //if (ball != null) { ball.Update(); }
//            //if (ball2 != null) { ball2.Update(); }

//            //if (ball != null) { UpdateCollisionPointList(); }
//            //if (ball2 != null) { UpdateCollisionPointList2(); }


//            // GameBall04 ball4 has mouse controls and collision detection logic built into Update function
//            if (ball4 != null) { ball4.Update(mc, kc, cplist); }



//            DrawToBuffer();
//        }

//        #region UpdateCollisionPointList functions (only used in GameBall1 and GameBall2)

//        //// UpdateCollisionPointList for ball 
//        //void UpdateCollisionPointList()
//        //{
//        //    CollisionPoint tempcp;
//        //    for (int i = 0; i < cplist.Count; i++)
//        //    {
//        //        for (int j = 0; j < ball.CollisionPointList.Count; j++)
//        //        {
//        //            tempcp = ball.CollisionPointList[j];
//        //            if (cplist[i].CheckForCollision(tempcp.Pos.X, tempcp.Pos.Y))
//        //            {
//        //                tempcp.PointHit = true;
//        //                ball.Collide = true;
//        //                return;
//        //            }
//        //            tempcp.PointHit = false;
//        //        }
//        //    }
//        //}

//        //// UpdateCollisionPointList2 for ball2
//        //void UpdateCollisionPointList2()
//        //{
//        //    CollisionPoint tempcp;
//        //    for (int i = 0; i < cplist.Count; i++)
//        //    {
//        //        for (int j = 0; j < ball2.CollisionPointList.Count; j++)
//        //        {
//        //            tempcp = ball2.CollisionPointList[j];
//        //            if (cplist[i].CheckForCollision(tempcp.Pos.X, tempcp.Pos.Y))
//        //            {
//        //                tempcp.PointHit = true;
//        //                ball2.Collide = true;
//        //                ball2.PublicHitIdxList.Add(i);
//        //                return;
//        //                //continue;
//        //            }
//        //            tempcp.PointHit = false;
//        //        }
//        //    }
//        //}


//        #endregion UpdateCollisionPointList functions (only used in GameBall1 and GameBall2)

//        //int mStartX, mStartY, deltaX, deltaY;
//        #region UpdateMouseControls versions (only used in GameBall1 and GameBall2)
//        //public void UpdateMouseControls(MouseControls mc)
//        //{
//        //    if (ball == null) { goto Ball2Controls; }
//        //    #region ball version 1
//        //    #region Setting up the shot
//        //    if (!ball.BallLaunched)
//        //    {
//        //        if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up && ball.SettingSpin)
//        //        {
//        //            if (ball.IsInSpinRect(mc.X, mc.Y))
//        //            {
//        //                mStartX = mc.X;
//        //                mStartY = mc.Y;
//        //                deltaX = 0;
//        //                deltaY = 0;
//        //                ball.PlacingSpinRect = true;
//        //            }
//        //        }
//        //        if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Down && ball.PlacingSpinRect)
//        //        {
//        //            deltaX = mc.X - mStartX;
//        //            deltaY = mc.Y - mStartY;
//        //            ball.AdjustSpinMarker(mc.X, mc.Y);
//        //        }
//        //        if (mc.LeftButtonState == UpDownState.Up && mc.LastLeftButtonState == UpDownState.Down && ball.PlacingSpinRect)
//        //        {
//        //            ball.PlacingSpinRect = false;
//        //        }

//        //        // launch / set flight path
//        //        if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up)
//        //        {
//        //            if (ball.IsInLaunchButtonRect(mc.X, mc.Y) && ball.ReadyForLaunch)
//        //            {
//        //                ball.LaunchBall();
//        //            }
//        //            else //if (mc.Y < ball.Y - ball.Height * 2) // uncomment if here to restrict aim to only north side of the ball
//        //            {
//        //                ball.SetFlightPath(mc.X, mc.Y);
//        //            }
//        //        }
//        //    }
//        //    #endregion Setting up the shot
//        //    // unpause
//        //    if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up && ball.BallLaunched == true)
//        //    {
//        //        ball.Pause = false;

//        //    }

//        //    // Reset
//        //    if (mc.RightButtonState == UpDownState.Down && mc.LastRightButtonState == UpDownState.Up)
//        //    {
//        //        ball.Reset();
//        //    }

//        //#endregion ball version 1

//        //Ball2Controls:
//        //    if (ball2 == null) { goto EndControls; }
//        //    #region ball2 only
//        //    #region Setting up the shot
//        //    if (!ball2.BallLaunched)
//        //    {
//        //        if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up && ball2.SettingSpin)
//        //        {
//        //            if (ball2.IsInSpinRect(mc.X, mc.Y))
//        //            {
//        //                mStartX = mc.X;
//        //                mStartY = mc.Y;
//        //                deltaX = 0;
//        //                deltaY = 0;
//        //                ball2.PlacingSpinRect = true;
//        //            }
//        //        }
//        //        if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Down && ball2.PlacingSpinRect)
//        //        {
//        //            deltaX = mc.X - mStartX;
//        //            deltaY = mc.Y - mStartY;
//        //            ball2.AdjustSpinMarker(mc.X, mc.Y);
//        //        }
//        //        if (mc.LeftButtonState == UpDownState.Up && mc.LastLeftButtonState == UpDownState.Down && ball2.PlacingSpinRect)
//        //        {
//        //            ball2.PlacingSpinRect = false;
//        //        }

//        //        // launch / set flight path
//        //        if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up)
//        //        {
//        //            if (ball2.IsInLaunchButtonRect(mc.X, mc.Y) && ball2.ReadyForLaunch)
//        //            {
//        //                ball2.LaunchBall();
//        //            }
//        //            else //if (mc.Y < ball2.Y - ball2.Height * 2) // uncomment if here to restrict aim to only north side of the ball2
//        //            {
//        //                ball2.SetFlightPath(mc.X, mc.Y);
//        //            }
//        //        }
//        //    }
//        //    #endregion Setting up the shot
//        //    // unpause
//        //    if (mc.LeftButtonState == UpDownState.Down && mc.LastLeftButtonState == UpDownState.Up && ball2.BallLaunched == true)
//        //    {
//        //        ball2.Pause = false;
//        //    }

//        //    // Reset
//        //    if (mc.RightButtonState == UpDownState.Down && mc.LastRightButtonState == UpDownState.Up)
//        //    {
//        //        ball2.Reset();
//        //    }
//        //#endregion ball2

//        //EndControls:
//        //    return;

//        //}
//        #endregion UpdateMouseControls versions
//        private void DrawToBuffer()
//        {
//            backbuffer = new Bitmap(width, height);
//            Graphics g = Graphics.FromImage(backbuffer);
//            SolidBrush sb = new SolidBrush(Color.CornflowerBlue);
//            g.FillRectangle(sb, 0, 0, width, height);
//            DrawBlockList(g);

//            //if (ball != null) { ball.Draw(g); }
//            //if (ball2 != null) { ball2.Draw(g); }

//            if (ball4 != null) { ball4.Draw(g); }

//            DrawCollisionPointList(g);

//            DbgFuncs.DrawDbgStrList(g);
//            sb.Dispose();
//            g.Dispose();
//        }
//        public void Draw(Graphics g)
//        {
//            g.DrawImage(backbuffer, new PointF(0, 0));
//            backbuffer.Dispose();
//        }

//        void DrawCollisionPointList(Graphics g)
//        {
//            for (int i = 0; i < cplist.Count; i++)
//            {
//                cplist[i].Draw(g);
//            }
//        }

//        #region BlockList Functions
//        private void LoadBlockList()
//        {
//            int a, r, g, b;
//            int x = 0;
//            int y = 0;
//            a = 255;
//            r = 150;
//            g = 0;
//            b = 0;
//            Color c = Color.FromArgb(a, r, g, b);
//            for (int i = 0; i < 10; i++)
//            {
//                AddBlock(x, y, c);
//                if (x < width) { x = blockList[blockList.Count - 1].Width * i; }
//                else
//                {
//                    x = 0;
//                    y += blockList[blockList.Count - 1].Height;
//                }
//                if (g + i * 10 < 255) { g += i * 10; }
//                else { g = 255; }
//                c = Color.FromArgb(a, r, g, b);
//            }
//        }
//        private void AddBlock(int x, int y, Color c)
//        {
//            BasicBlock01 b = new BasicBlock01();
//            b.Load(x, y);
//            b.DrawColor = c;
//            b.SetNumberOfHits(1, 20);
//            blockList.Add(b);
//        }
//        private void UpdateBlockList() { for (int i = 0; i < blockList.Count; i++) { blockList[i].Update(); } }
//        private void DrawBlockList(Graphics g) { for (int i = 0; i < blockList.Count; i++) { blockList[i].Draw(g); } }
//        private void CleanUpBlockList()
//        {
//            for (int i = 0; i < blockList.Count; i++) { blockList[i].CleanUp(); }
//            blockList.RemoveRange(0, blockList.Count);
//        }
//        #endregion BlockList Functions

//        public void CleanUp()
//        {
//            //if (ball != null) { ball.CleanUp(); }
//            //if (ball2 != null) { ball2.CleanUp(); }
//            //if (ball4 != null) { ball4.CleanUp(); }
//            CleanUpBlockList();

//        }


//    }
//}
#endregion full back up before removing gameball1 and gameball2

#region built collision detection checking into GameBall04.Update. 2020-01-04
// UpdateCollisionPointList for GameBall04 ball4
//void UpdateCollisionPointList(GameBall04 b)
//{
//    CollisionPoint tempcp;
//    int ballCpListCount = b.CollisionPointList.Count;
//    for (int i = 0; i < cplist.Count; i++)
//    {
//        for (int j = 0; j < ballCpListCount; j++)
//        {
//            tempcp = b.CollisionPointList[j];
//            if (cplist[i].CheckForCollision(tempcp.Pos))
//            {
//                tempcp.PointHit = true;
//                return;
//                //continue;
//            }
//            tempcp.PointHit = false;
//        }
//    }
//}
#endregion built collision detection checking into GameBall04.Update. 2020-01-04

#region old update collision point logic for GameBall3
// UpdateCollisionPointList3 for ball 3
//void UpdateCollisionPointList3()
//{
//    CollisionPoint tempcp;
//    for (int i = 0; i < cplist.Count; i++)
//    {
//        for (int j = 0; j < ball3.CollisionPointList.Count; j++)
//        {
//            tempcp = ball3.CollisionPointList[j];
//            if (cplist[i].CheckForCollision(tempcp.Pos.X, tempcp.Pos.Y))
//            {
//                tempcp.PointHit = true;
//                //ball3.Collide = true;
//                //ball3.PublicHitIdxList.Add(i);
//                return;
//                //continue;
//            }
//            tempcp.PointHit = false;
//        }
//    }
//}
#endregion old update collision point logic for GameBall3