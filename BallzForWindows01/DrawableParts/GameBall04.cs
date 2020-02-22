﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace BallzForWindows01.DrawableParts
{
    using static AssistFunctions;
    using GamePhysicsParts;
    using Structs;

    class GameBall04 : CollisionCircleDV2
    {

        //public bool Launched { get { return launched; } }


        Size gameScreenSize;

        Trajectory03 aimTraj;
        Trajectory03 spinTraj;
        BounceController bc;
        //Button02 btnLaunch2;
        Button03 btnLaunch3;

        PointD startPosition;
        PointD mousePos;

        GameTimer gtimer;

        double speed = 1.0;
        double startingSpeed = 1.0;
        double startingRotation = 0;

        double driftFactor = 0;
        double initialDriftPerSecond = 0;
        double updatedAtSeconds = 0;

        bool adjustingPosition = false;
        bool adjustingAim = false;
        bool adjustingSpin = false;
        bool launched = false;


        public GameBall04(Size gameScreenSize)
            : base()
        {
            clsName = "GameBall04";
            this.gameScreenSize = gameScreenSize;
            //btnLaunch2 = new Button02();
            btnLaunch3 = new Button03();

            startPosition = new PointD();
            mousePos = new PointD();
            gtimer = new GameTimer();
            aimTraj = new Trajectory03();
            spinTraj = new Trajectory03();
            spinTraj.SetXColor(Color.AntiqueWhite);
            aimTraj.NameTag = "aimTraj";
            spinTraj.NameTag = "spinTraj";
            aimTraj.ShowStartMakrer = false;
            spinTraj.ShowStartMakrer = false;

            bc = new BounceController();

            speed = startingSpeed = 1.5;
        }

        new public void Load(double x, double y, double hitBoxSideLength, double radius, double rotation, int collisionPoints)
        {
            base.Load(x, y, hitBoxSideLength, radius, rotation, collisionPoints);
            startPosition.Set(x, y);
            ccDistance = radius * 2;    // set collision check distance
            PositionLaunchButton();
        }


        public void Update(MouseControls mc, KeyboardControls01 kc, List<CollisionPoint> blockCpList)
        {
            bool dbgtxt = true;
            string fnId = FnId(clsName, "Update");
            HandleMouseInput(mc);
            HandleKeyboardInput(kc);

            if (dbgtxt) DbgFuncs.AddStr($"{fnId} launched: {launched}");

            if (!launched)
            {
                if (adjustingPosition)
                {
                    position.Set(mousePos);
                    startPosition.Set(mousePos);
                }
                if (adjustingAim) { aimTraj.SetEndPoint(mousePos); }
                if (adjustingSpin) { spinTraj.SetEndPoint(mousePos); }

                SetInitialTrajectory();
                CalculateDriftFactor(aimTraj.Rotation, spinTraj.Rotation);
                CalculateInitialDriftPerSecond();
            }


            CheckForBlockCollision02(blockCpList);

            if (launched)
            {
                gtimer.Update();

                
                if (collisionDetected)
                {
                    CalculateBounceAngle();
                    rotation += bounceAngle;
                }

                ApplyDriftPerSecond(gtimer.TotalSeconds);
                position.Move(speed, rotation);

                if (bounceAngle != 0)
                {
                    lastBounceAngle = bounceAngle;
                    bounceAngle = 0;
                }
            }

            //MoveCollisionPoints(position.X, position.Y, radius, rotation);
            MoveCollisionPoints(position.X, position.Y, rotation);

            ccEndPoint = position.PointAt(rotation, ccDistance);


            if (dbgtxt && DrawDbgTxt)
            {
                //gtimer.DbgTxt();
                dbgPrintAngle(fnId, "driftFactor", driftFactor);
                dbgPrintAngle(fnId, "rotation", rotation);
                dbgPrintAngle(fnId, "initialDriftPerSecond", initialDriftPerSecond);
                DbgFuncs.AddStr($"{fnId} gtimer.TotalSeconds: {gtimer.TotalSeconds}");
                DbgFuncs.AddStr($"{fnId} ~~~~ CHECK FOR BOUNCE DBG LOGIC ~~~");
                DbgFuncs.AddStr($"{fnId} shouldBounce: {collisionDetected}");
                DbgFuncs.AddStr($"{fnId} firstPointHit (index): {firstPointHit}");
                dbgPrintAngle(fnId, "bounceAngle", bounceAngle);
                dbgPrintAngle(fnId, "lastBounceAngle", lastBounceAngle);
                dbgPrintAngle(fnId, "testBounceAngle", testBounceAngle);
                DbgFuncs.AddStr($"{fnId} collision check distance (ccDistance) : {ccDistance}");
                DbgFuncs.AddStr($"{fnId} hz: {hz}");
                DbgFuncs.AddStr($"{fnId} ballCollisionPointsHit: {CollisionPointsHitCount()}");
                DbgFuncs.AddStr($"{fnId} ball collision points hit: {HitIndexList.Count}");


            }
            if (firstPointHit > -1) { launched = aimTraj.Visible = spinTraj.Visible = false; } // For testing, stopping ball and hiding aim and spin markers   
        }

        public void Draw(Graphics g)
        {
            Pen p = new Pen(Color.Black);
            SolidBrush sb = new SolidBrush(Color.Black);

            //base.Draw(g, p, sb);            
            DrawCollisionPoints(g, p, sb);
            DrawCircle(g, p, sb);

            p.Color = Color.Black;
            p.Width = 2;
            g.DrawLine(p, position.fX, position.fY, ccEndPoint.fX, ccEndPoint.fY);  // debug, drawing cc distance
            if (testBounceResult.X != 0 && testBounceResult.Y != 0)
            {
                p.Color = Color.FromArgb(150, 200, 20, 20);
                g.DrawLine(p, position.fX, position.fY, testBounceResult.fX, testBounceResult.fY);  // debug, drawing test bounce angle
            }


            //btnLaunch2.Draw(g);
            btnLaunch3.Draw(g);
            aimTraj.Draw(g);
            spinTraj.Draw(g);

            p.Dispose();
            sb.Dispose();
        }
        public void Reset()
        {
            launched = false;
            gtimer.Stop();
            gtimer.Reset();
            position.Set(startPosition);
            speed = startingSpeed;
            adjustingPosition = false;
            adjustingAim = false;
            adjustingSpin = false;

            updatedAtSeconds = 0;
            driftFactor = 0;
            initialDriftPerSecond = 0;
            startingRotation = 0;
            aimTraj.Reset();
            spinTraj.Reset();

            ResetPointsHit();

            btnLaunch3.Reset();

            // temporary/testing resets
            bounceAngle = 0;
            firstPointHit = -1;
            collisionDetected = false;
            hz = HitZones.None;
            testBounceResult.Zero();

        }
        public void CleanUp()
        {
            //btnLaunch2.CleanUp();
            btnLaunch3.CleanUp();
        }

        #region building bounce logic. Started 2020-01-01
        double bounceAngle = 0;
        double lastBounceAngle = 0;
        double testBounceAngle = 0;
        bool collisionDetected = false;
        int firstPointHit = -1;
        HitZones hz = HitZones.None;


        double ccDistance = 0;      // collision check distance
        PointD ccEndPoint = new PointD();


        // Checks if the ball has collided with any points passed in and sets HitZone hz.
        // HitZone hz is based off the ball position (center) relative to the sides of the
        // CollisionPoint rectangle.
        /// <summary>
        /// Version 1
        /// </summary>
        /// <param name="blockCpList"></param>
        //public void CheckForBlockCollision(List<CollisionPoint> blockCpList)
        //{

        //    for (int i = 0; i < blockCpList.Count; i++)
        //    {
        //        // If the block is not within set distance of ball, do not check for collision with that block. 2020-01-31
        //        if (position.DistanceTo(blockCpList[i].Pos) > ccDistance) { blockCpList[i].Collision = false; continue; }

        //        // Checking for ball collision points intersecting with blocks within set distance (ccDistance as of 2020-01-31)
        //        for (int j = 0; j < CircleCPList.Count; j++)
        //        {
        //            if (blockCpList[i].CheckForCollision(CircleCPList[j].Pos))
        //            {
        //                //collisionDetected = true;
        //                //CollisionPointList[j].PointHit = true;
        //                return;
        //            }
        //            //CollisionPointList[j].PointHit = false;
        //        }

        //    }
        //}
        public void CheckForBlockCollision02(List<CollisionPoint> blockCpList)
        {

            for(int i = 0; i < CircleCPList.Count; i++)
            {
                /// Not sure if this could happen, but going to put in for now in case I need to use it later.
                /// This will skip checking for collision if the circle cp has already detected collision.
                if(CircleCPList[i].PointHit) { continue; }

                for(int j = 0; j < blockCpList.Count; j++)
                {
                    if(position.DistanceTo(blockCpList[j].Pos) > ccDistance) { blockCpList[j].Collision = false; continue; }
                    //if(CircleCPList[i].CheckForCollision(blockCpList[i].Pos))
                    //{
                    //    AddPointHitIndex(i);
                    //}
                    if(blockCpList[j].CheckForCollision(CircleCPList[i].Pos))
                    {
                        AddPointHitIndex(i);
                    }

                }
            }


            return;
            for (int i = 0; i < blockCpList.Count; i++)
            {
                // If the block is not within set distance of ball, do not check for collision with that block. 2020-01-31
                if (position.DistanceTo(blockCpList[i].Pos) > ccDistance) { blockCpList[i].Collision = false; continue; }

                // Checking for ball collision points intersecting with blocks within set distance (ccDistance as of 2020-01-31)
                for (int j = 0; j < CircleCPList.Count; j++)
                {
                    if (blockCpList[i].CheckForCollision(CircleCPList[j].Pos))
                    {
                        //collisionDetected = true;
                        //CollisionPointList[j].PointHit = true;
                        return;
                    }
                    //CollisionPointList[j].PointHit = false;
                }
            }
        }


        // TODO: build CalculateBounceAngle. I am planning for this to be the location where the bounce angle
        //       will be calculated if collision is detected in CheckForBlockCollision
        void CalculateBounceAngle()
        {
            for (int i = 0; i < CircleCPList.Count; i++)
            {
                // if a collision point on the ball registers a hit, do bounce stuff
                if (CircleCPList[i].PointHit)
                {
                    //collisionDetected = true;
                    firstPointHit = i;

                    SetBounceAngle(bc.RowHit(hz), bc.ColumnHit(hz));
                    return;
                }
            }
        }

        PointD testBounceResult = new PointD();
        double testBountLineLength = 40;
        void SetBounceAngle(AboveMiddleBelow rowHit, LeftMiddleRight columnHit)
        {
            // Heading south
            if (rotation == Math.PI / 2)
            {
                testBounceResult = position.PointAt((testBounceAngle = (3 * Math.PI / 2)), testBountLineLength);
                return;
            }

            // Heading west
            if (rotation == Math.PI)
            {
                testBounceResult = position.PointAt(testBounceAngle = 0, testBountLineLength);
                return;
            }

            // Heading between south and east
            if (rotation < Math.PI / 2 && rotation < 0)
            {
                // bounce if hit top side of block
                if (rowHit == AboveMiddleBelow.Above)
                {
                    testBounceResult = position.PointAt(testBounceAngle = ((2 * Math.PI) - rotation), testBountLineLength);
                    return;
                }

                // bounce if hit left side of block
                testBounceResult = position.PointAt(testBounceAngle = (Math.PI / 2) + ((Math.PI / 2) - rotation), testBountLineLength);
                return;
            }

            // Heading between west and south
            if (rotation < Math.PI)
            {
                testBounceResult = position.PointAt(testBounceAngle = (Math.PI + (Math.PI - rotation)), testBountLineLength);
                return;
            }

            // Heading between north and west
            if (rotation < (3 * Math.PI / 2))
            {
                //testBounceAngle = 3 * Math.PI / 2 + ((3 * Math.PI / 2) - rotation);
                //testBounceResult = position.PointAt(testBounceAngle, testBountLineLength);

                //testBounceAngle = ((3 * Math.PI / 2) + ((3 * Math.PI / 2) - rotation));
                testBounceResult = position.PointAt(testBounceAngle = ((3 * Math.PI / 2) + ((3 * Math.PI / 2) - rotation)), testBountLineLength);
                return;
            }

            // Heading between north and west
            if (rotation < 2 * Math.PI)
            {
                if (rowHit == AboveMiddleBelow.Below)
                {
                    testBounceResult = position.PointAt(testBounceAngle = ((2 * Math.PI) + ((2 * Math.PI) - rotation)), testBountLineLength);
                }




            }

        }


        #endregion building bounce logic. Started 2020-01-01

        void SetInitialTrajectory() { startingRotation = rotation = aimTraj.Rotation; }
        void CalculateDriftFactor(double aim, double spin)
        {
            //string fnId = FnId(clsName, "CalculateDriftFactor");
            //dbgPrintAngle(fnId, "aim", aim);
            //dbgPrintAngle(fnId, "spin", spin);
            //DbgFuncs.AddStr($"{fnId} Calculating drift factor");
            RotationDirection defautRotationDirection = (aim < spin) ? RotationDirection.Clockwise : RotationDirection.CounterClockwise;
            double defaultDifference = (aim < spin) ? spin - aim : aim - spin;      // get positive difference
            double oppositeDifference = (2 * Math.PI) - defaultDifference;          // find remainder of 360 - (positive difference between aim and spin)
            RotationDirection oppositeRotationDirection = (aim < spin) ? RotationDirection.CounterClockwise : RotationDirection.Clockwise;  // set opposite direction. Might rework later.
            RotationDirection shortestRotationDirection = (defaultDifference < oppositeDifference) ? defautRotationDirection : oppositeRotationDirection;   // set which rot direction is shortest
            double smallestDifference = (defaultDifference < oppositeDifference) ? defaultDifference : oppositeDifference;      // set shortest rot distance number
            double rslt = (shortestRotationDirection == RotationDirection.Clockwise) ? smallestDifference : (smallestDifference * (-1));    // calculate drift factor
            driftFactor = rslt;
        }
        void CalculateInitialDriftPerSecond() { initialDriftPerSecond = driftFactor / 100; }
        void ApplyDriftPerSecond(double elapsedSeconds)
        {
            if (elapsedSeconds > updatedAtSeconds)
            {
                rotation += initialDriftPerSecond;                            
                updatedAtSeconds = elapsedSeconds;
            }
        }
        void HandleKeyboardInput(KeyboardControls01 kc)
        {
            if (kc.KeyPressed(Keys.Space)) { if (ReadyForLaunch()) { LaunchBall(); return; } }
        }
        void HandleMouseInput(MouseControls mc)
        {
            //bool dbgtxt = true;
            //string fnId = $"[{clsName}.HandleMouseInput]";
            //if (dbgtxt) DbgFuncs.AddStr($"{fnId} mc.LeftButtonState: {mc.LeftButtonState}");
            //if (dbgtxt) DbgFuncs.AddStr($"{fnId} mc.LastLeftButtonState: {mc.LastLeftButtonState}");

            mousePos.Set(mc.X, mc.Y);
            if (mc.RightButtonClicked()) { Reset(); return; }

            // Prevents aim and spin boxes from continuing to drag if mouse is not down
            if (AdjustingAimSpinOrPos() && mc.LeftButtonUp()) { StopAdjusting(); return; }


            // Setting up the shot
            if (!launched)
            {
                // Logic to move ball before the aim marker is placed (aimTraj.Placed)
                if (!aimTraj.Placed && mc.LeftButtonClicked() && InCircle(mc.Position)) { adjustingPosition = true; return; }

                // Place aim marker
                if (!aimTraj.Placed && mc.LeftButtonClicked()) { PlaceAimMarker(mc); return; }

                // Adjust aim or spin
                if (CanAdjustAimOrSpin() && mc.LeftButtonClicked())
                {
                    if (aimTraj.InEndRect(mc.X, mc.Y)) { adjustingAim = true; return; }
                    if (spinTraj.InEndRect(mc.X, mc.Y)) { adjustingSpin = true; return; }
                }


                #region 2020-01-19 might not need this. going to try commenting it out. will remove later if no issued occur
                /// I added a condition before here to stop adjusting aim, spin, and ball position if left button is up.
                /// That condition up top should make this one no longer needed.
                // Stop adjusting aim or spin
                //if ((adjustingAim || adjustingSpin)
                //    && mc.LeftButtonState == UpDownState.Up
                //    && mc.LastLeftButtonState == UpDownState.Down)
                //{
                //    adjustingAim = adjustingSpin = false;
                //    return;
                //}
                #endregion 2020-01-19 might not need this. going to try commenting it out

                // Launch the ball and start the timer (when launch button is clicked)
                //if ((!adjustingAim && !adjustingSpin) && mc.LeftButtonClicked() && btnLaunch2.InBox(mc.Position)) { LaunchBall(); }
                if ((!adjustingAim && !adjustingSpin) && mc.LeftButtonClicked() && btnLaunch3.InBox(mc.Position)) { LaunchBall(); }
            }

        }
        void PlaceAimMarker(MouseControls mc)
        {
            aimTraj.SetStartPoint(position);
            aimTraj.SetEndPoint(mc.Position);

            spinTraj.SetStartPoint(position);
            PointD spinStartPos = new PointD();
            spinStartPos = position.HalfWayTo(mc.Position.X, mc.Position.Y);
            spinTraj.SetEndPoint(spinStartPos);
        }
        void LaunchBall()
        {
            launched = true;
            gtimer.Start();
        }
        void PositionLaunchButton()     // after gameScreenSize is set b.c it is used to place button
        {
            Point position = new Point();
            Size size = new Size();
            size.Width = 100;
            size.Height = 40;
            position.X = gameScreenSize.Width / 2 - size.Width / 2;
            position.Y = gameScreenSize.Height - size.Height * 2 - 5;
            
            //btnLaunch2.Load("Launch", position.X, position.Y, size.Width, size.Height);
            btnLaunch3.Load("Launch", position.X, position.Y, size.Width, size.Height);
        }

        void StopAdjusting() { adjustingAim = adjustingSpin = adjustingPosition = false; }
        // Condition checks
        bool ReadyForLaunch() { return (!launched && aimTraj.Placed && (!adjustingAim || !adjustingPosition || !adjustingSpin)) ? true : false; }
        bool CanAdjustAimOrSpin() { return (aimTraj.Placed && (!adjustingAim || !adjustingSpin)) ? true : false; }
        bool AdjustingAimSpinOrPos() { return (adjustingAim || adjustingSpin || adjustingPosition) ? true : false; }

        #region might try adding in mouse controls to condition checks later.
        // Might try adding in mouse control later. Only thing here would be I would like to see the mouse button
        // being used in the handle mouse controls (so I can see what mouse button triggers the action)
        //bool CanAdjustAimOrSpin(MouseControls mc) { return ((aimTraj.Placed && (!adjustingAim || !adjustingSpin)) && mc.LeftButtonClicked()) ? true : false; }
        #endregion might try adding in mouse controls to condition checks later.
    }
}

#region old Update function
//public void Update(MouseControls mc)
//{
//    bool dbgtxt = true;
//    string fnId = FnId(clsName, "Update");
//    HandleMouseInput(mc);

//    if (dbgtxt) DbgFuncs.AddStr($"{fnId} launched: {launched}");

//    if (!launched)
//    {
//        if (adjustingPosition)
//        {
//            position.Set(mousePos);
//            startPosition.Set(mousePos);
//        }
//        if (adjustingAim) { aimTraj.SetEndPoint(mousePos); }
//        if (adjustingSpin) { spinTraj.SetEndPoint(mousePos); }

//        SetInitialTrajectory();
//        CalculateDriftFactor(aimTraj.Rotation, spinTraj.Rotation);
//        CalculateInitialDriftPerSecond();
//    }
//    ApplyDriftPerSecond(gtimer.TotalSeconds);
//    if (launched)
//    {
//        gtimer.Update();
//        // 2020-01-01 Going to try applying bounce logic here. Collision is determined in the previous frame currently.
//        CalculateBounceAngle();
//        rotation += bounceAngle;
//        position.X = position.X + speed * Math.Cos(rotation);
//        position.Y = position.Y + speed * Math.Sin(rotation);
//        if (bounceAngle != 0)
//        {
//            lastBounceAngle = bounceAngle;
//            bounceAngle = 0;
//        }
//    }
//    MoveCollisionPoints(position.X, position.Y, radius, rotation);
//    if (dbgtxt && DrawDbgTxt)
//    {
//        //gtimer.DbgTxt();
//        dbgPrintAngle(fnId, "driftFactor", driftFactor);
//        dbgPrintAngle(fnId, "rotation", rotation);
//        dbgPrintAngle(fnId, "initialDriftPerSecond", initialDriftPerSecond);
//        DbgFuncs.AddStr($"{fnId} gtimer.TotalSeconds: {gtimer.TotalSeconds}");
//        DbgFuncs.AddStr($"{fnId} ~~~~ CHECK FOR BOUNCE DBG LOGIC ~~~");
//        DbgFuncs.AddStr($"{fnId} shouldBounce: {shouldBounce}");
//        DbgFuncs.AddStr($"{fnId} firstPointHit (index): {firstPointHit}");
//        dbgPrintAngle(fnId, "bounceAngle", bounceAngle);
//        dbgPrintAngle(fnId, "lastBounceAngle", lastBounceAngle);
//        dbgPrintAngle(fnId, "testBounceAngle", testBounceAngle);

//        DbgFuncs.AddStr($"{fnId} hz: {hz}");
//        DbgFuncs.AddStr($"{fnId} lastHz: {lastHz}");


//        if (firstPointHit > -1)
//        {
//            launched = false;
//            aimTraj.Visible = false;
//            spinTraj.Visible = false;
//        }
//    }
//}
#endregion old Update function

#region GameBall04 full backup 2020-01-04 before continuing to build bounce logic
//namespace BallzForWindows01.DrawableParts
//{
//    using static AssistFunctions;
//    using GamePhysicsParts;
//    using Structs;

//    class GameBall04 : CollisionCircleDV2
//    {
//        Size gameScreenSize;

//        Trajectory03 aimTraj;
//        Trajectory03 spinTraj;

//        Button01 btnLaunch;

//        PointD startPosition;
//        PointD mousePos;

//        GameTimer gtimer;

//        double speed = 1.0;
//        double startingSpeed = 1.0;
//        double startingRotation = 0;

//        double driftFactor = 0;
//        double initialDriftPerSecond = 0;
//        double updatedAtSeconds = 0;

//        bool adjustingAim = false;
//        bool adjustingSpin = false;
//        bool launched = false;


//        public GameBall04(Size gameScreenSize)
//            : base()
//        {
//            clsName = "GameBall04";
//            this.gameScreenSize = gameScreenSize;
//            btnLaunch = new Button01();
//            startPosition = new PointD();
//            mousePos = new PointD();
//            gtimer = new GameTimer();
//            aimTraj = new Trajectory03();
//            spinTraj = new Trajectory03();
//            spinTraj.SetXColor(Color.AntiqueWhite);
//            aimTraj.NameTag = "aimTraj";
//            spinTraj.NameTag = "spinTraj";
//        }

//        new public void Load(double x, double y, double hitBoxSideLength, double radius, double rotation, int collisionPoints)
//        {
//            base.Load(x, y, hitBoxSideLength, radius, rotation, collisionPoints);
//            startPosition.Set(x, y);
//            PositionLaunchButton();
//        }

//        public void Update(MouseControls mc)
//        {
//            bool dbgtxt = true;
//            string fnId = FnId(clsName, "Update");
//            HandleMouseInput(mc);

//            if (dbgtxt) DbgFuncs.AddStr($"{fnId} launched: {launched}");

//            if (!launched)
//            {
//                if (adjustingAim) { aimTraj.SetEndPoint(mousePos); }
//                if (adjustingSpin) { spinTraj.SetEndPoint(mousePos); }
//                SetInitialTrajectory();
//                CalculateDriftFactor(aimTraj.Rotation, spinTraj.Rotation);
//                CalculateInitialDriftPerSecond();
//            }


//            ApplyDriftPerSecond(gtimer.TotalSeconds);


//            if (launched)
//            {
//                gtimer.Update();

//                // 2020-01-01 Going to try applying bounce logic here. Collision is determined in the previous frame currently.
//                CalculateBounceAngle();
//                rotation += bounceAngle;
//                position.X = position.X + speed * Math.Cos(rotation);
//                position.Y = position.Y + speed * Math.Sin(rotation);

//                if (bounceAngle != 0) { lastBounceAngle = bounceAngle; }
//                bounceAngle = 0;
//            }

//            MoveCollisionPoints(position.X, position.Y, radius, rotation);

//            if (dbgtxt && DrawDbgTxt)
//            {
//                //gtimer.DbgTxt();
//                if (dbgtxt) dbgPrintAngle(fnId, "driftFactor", driftFactor);
//                if (dbgtxt) dbgPrintAngle(fnId, "rotation", rotation);
//                if (dbgtxt) dbgPrintAngle(fnId, "initialDriftPerSecond", initialDriftPerSecond);
//                DbgFuncs.AddStr($"{fnId} gtimer.TotalSeconds: {gtimer.TotalSeconds}");
//                DbgFuncs.AddStr($"{fnId} ~~~~ CHECK FOR BOUNCE DBG LOGIC ~~~");
//                dbgPrintAngle(fnId, "bounceAngle", bounceAngle);
//                dbgPrintAngle(fnId, "lastBounceAngle", lastBounceAngle);
//                dbgPrintAngle(fnId, "testBounceAngle", testBounceAngle);
//                DbgFuncs.AddStr($"{fnId} shouldBounce: {shouldBounce}");

//            }
//        }
//        new public void Draw(Graphics g)
//        {
//            base.Draw(g);
//            btnLaunch.Draw(g);
//            aimTraj.Draw(g);
//            spinTraj.Draw(g);
//        }
//        public void Reset()
//        {
//            launched = false;
//            gtimer.Stop();
//            gtimer.Reset();
//            position.Set(startPosition);
//            speed = startingSpeed;
//            adjustingAim = false;
//            adjustingSpin = false;
//            updatedAtSeconds = 0;
//            driftFactor = 0;
//            initialDriftPerSecond = 0;
//            startingRotation = 0;
//            aimTraj.Reset();
//            spinTraj.Reset();

//            // temporary/testing resets
//            bounceAngle = 0;
//        }
//        public void CleanUp()
//        {
//            btnLaunch.CleanUp();
//        }

//        #region building bounce logic. Started 2020-01-01
//        double bounceAngle = 0;
//        double lastBounceAngle = 0;
//        double testBounceAngle = 0;
//        bool shouldBounce = false;
//        void CalculateBounceAngle()
//        {
//            //string fnId = FnId(clsName, "CheckForBounce");
//            for (int i = 0; i < CollisionPointList.Count; i++)
//            {
//                if (CollisionPointList[i].PointHit)
//                {
//                    shouldBounce = true;
//                    CollisionPoint cp = CollisionPointList[i];



//                }
//            }
//        }

//        #endregion building bounce logic. Started 2020-01-01

//        void SetInitialTrajectory() { startingRotation = rotation = aimTraj.Rotation; }
//        void CalculateDriftFactor(double aim, double spin)
//        {
//            string fnId = FnId(clsName, "CalculateDriftFactor");
//            //dbgPrintAngle(fnId, "aim", aim);
//            //dbgPrintAngle(fnId, "spin", spin);
//            DbgFuncs.AddStr($"{fnId} Calculating drift factor");
//            RotationDirection defautRotationDirection = (aim < spin) ? RotationDirection.Clockwise : RotationDirection.CounterClockwise;
//            double defaultDifference = (aim < spin) ? spin - aim : aim - spin;
//            double oppositeDifference = (2 * Math.PI) - defaultDifference;
//            RotationDirection oppositeRotationDirection = (aim < spin) ? RotationDirection.CounterClockwise : RotationDirection.Clockwise;
//            RotationDirection shortestRotationDirection = (defaultDifference < oppositeDifference) ? defautRotationDirection : oppositeRotationDirection;
//            double smallestDifference = (defaultDifference < oppositeDifference) ? defaultDifference : oppositeDifference;
//            double rslt = (shortestRotationDirection == RotationDirection.Clockwise) ? smallestDifference : (smallestDifference * (-1));
//            driftFactor = rslt;
//        }
//        void CalculateInitialDriftPerSecond() { initialDriftPerSecond = driftFactor / 100; }
//        void ApplyDriftPerSecond(double elapsedSeconds)
//        {
//            if (elapsedSeconds > updatedAtSeconds)
//            {
//                rotation += initialDriftPerSecond;

//                // Applying decay to drift per second // Possible future feature
//                //if (Math.Abs(initialDriftPerSecond) > 0) { initialDriftPerSecond = initialDriftPerSecond - (initialDriftPerSecond * 0.01); }                  

//                // Update updatedAtSeconds
//                updatedAtSeconds = elapsedSeconds;
//            }
//        }

//        void HandleMouseInput(MouseControls mc)
//        {
//            bool dbgtxt = true;
//            string fnId = $"[{clsName}.HandleMouseInput]";
//            if (dbgtxt) DbgFuncs.AddStr($"{fnId} mc.LeftButtonState: {mc.LeftButtonState}");
//            if (dbgtxt) DbgFuncs.AddStr($"{fnId} mc.LastLeftButtonState: {mc.LastLeftButtonState}");

//            mousePos.Set(mc.X, mc.Y);

//            // Reset
//            if (mc.RightButtonState == UpDownState.Down && mc.LastRightButtonState == UpDownState.Up)
//            {
//                //_Reset();
//                Reset();
//                return;
//            }

//            // Prevents aim and spin boxes from continuing to drag if mouse is not down
//            if (mc.LeftButtonState == UpDownState.Up
//                && mc.LastLeftButtonState == UpDownState.Up
//                && (adjustingAim || adjustingSpin))
//            {
//                adjustingAim = adjustingSpin = false;
//                return;
//            }


//            // Setting up the shot
//            if (!launched)
//            {

//                // Place aim marker
//                if (!aimTraj.Placed
//                    && mc.LeftButtonState == UpDownState.Down
//                    && mc.LastLeftButtonState == UpDownState.Up)
//                {
//                    aimTraj.SetStartPoint(position);
//                    aimTraj.SetEndPoint(mc.Position);

//                    spinTraj.SetStartPoint(position);
//                    PointD spinStartPos = new PointD();
//                    spinStartPos = position.HalfWayTo(mc.Position.X, mc.Position.Y);
//                    spinTraj.SetEndPoint(spinStartPos);

//                    return;
//                }

//                // Adjust aim or spin
//                if (aimTraj.Placed
//                    && (!adjustingAim || !adjustingSpin)
//                    && mc.LeftButtonState == UpDownState.Down
//                    && mc.LastLeftButtonState == UpDownState.Up)
//                {

//                    if (aimTraj.InEndRect(mc.X, mc.Y)) { adjustingAim = true; return; }
//                    if (spinTraj.InEndRect(mc.X, mc.Y)) { adjustingSpin = true; return; }

//                }

//                // Stop adjusting aim or spin
//                if ((adjustingAim || adjustingSpin)
//                    && mc.LeftButtonState == UpDownState.Up
//                    && mc.LastLeftButtonState == UpDownState.Down)
//                {
//                    adjustingAim = adjustingSpin = false;
//                    return;
//                }

//                // Launch the ball and start the timer (when launch button is clicked)
//                if ((!adjustingAim && !adjustingSpin)
//                    && mc.LastLeftButtonState == UpDownState.Up
//                    && mc.LeftButtonState == UpDownState.Down
//                    //&& InLaunchButtonRect(mc.X, mc.Y)
//                    && btnLaunch.InBoundingRect(mc.X, mc.Y))
//                {
//                    launched = true;
//                    gtimer.Start();
//                }
//            }

//        }

//        void PositionLaunchButton()     // after gameScreenSize is set b.c it is used to place button
//        {
//            Point position = new Point();
//            Size size = new Size();
//            size.Width = 100;
//            size.Height = 40;
//            position.X = gameScreenSize.Width / 2 - size.Width / 2;
//            position.Y = gameScreenSize.Height - size.Height * 2 - 5;
//            btnLaunch.Load(position.X, position.Y, size.Width, size.Height, "Launch");
//        }


//    }
//}
#endregion GameBall04 full backup 2020-01-04 before continuing to build bounce logic

#region SetInitialTrajectory previous version
//void SetInitialTrajectory()
//{
//    rotation = aimTraj.Rotation;
//    startingRotation = rotation;
//}
#endregion SetInitialTrajectory previous version

#region GameBall04 before adding collision/bounce logic 2020-01-01
//namespace BallzForWindows01.DrawableParts
//{
//    using static AssistFunctions;
//    using GamePhysicsParts;
//    using Structs;

//    class GameBall04 : CollisionCircleDV2
//    {
//        Size gameScreenSize;

//        Trajectory03 aimTraj;
//        Trajectory03 spinTraj;

//        Button01 btnLaunch;

//        PointD startPosition;
//        PointD mousePos;

//        GameTimer gtimer;

//        double speed = 1.0;
//        double startingSpeed = 1.0;
//        double startingRotation = 0;

//        double driftFactor = 0;
//        double initialDriftPerSecond = 0;

//        bool adjustingAim = false;
//        bool adjustingSpin = false;
//        bool launched = false;


//        public GameBall04(Size gameScreenSize)
//            : base()
//        {
//            clsName = "GameBall04";
//            this.gameScreenSize = gameScreenSize;
//            btnLaunch = new Button01();
//            startPosition = new PointD();
//            mousePos = new PointD();
//            gtimer = new GameTimer();
//            aimTraj = new Trajectory03();
//            spinTraj = new Trajectory03();
//            spinTraj.SetXColor(Color.AntiqueWhite);
//            aimTraj.NameTag = "aimTraj";
//            spinTraj.NameTag = "spinTraj";
//        }

//        new public void Load(double x, double y, double hitBoxSideLength, double radius, double rotation, int collisionPoints)
//        {
//            base.Load(x, y, hitBoxSideLength, radius, rotation, collisionPoints);
//            startPosition.Set(x, y);
//            PositionLaunchButton();
//        }



//        public void Update(MouseControls mc)
//        {
//            bool dbgtxt = true;
//            string fnId = FnId(clsName, "Update");
//            HandleMouseInput(mc);

//            if (dbgtxt) DbgFuncs.AddStr($"{fnId} launched: {launched}");

//            if (!launched)
//            {
//                if (adjustingAim)
//                {
//                    aimTraj.SetEndPoint(mousePos);
//                }
//                if (adjustingSpin)
//                {
//                    spinTraj.SetEndPoint(mousePos);
//                }
//                SetInitialTrajectory();
//                CalculateDriftFactor(aimTraj.Rotation, spinTraj.Rotation);
//                CalculateInitialDriftPerSecond();

//            }


//            ApplyDriftPerSecond(gtimer.TotalSeconds);


//            if (launched)
//            {
//                gtimer.Update();

//                // 2020-01-01 Going to try applying bounce logic here. Collision is determined in the previous frame currently.

//                position.X = position.X + speed * Math.Cos(rotation);
//                position.Y = position.Y + speed * Math.Sin(rotation);
//            }

//            UpdateCollisionPoints(position.X, position.Y, radius, rotation);

//            if (dbgtxt && DrawDbgTxt)
//            {
//                gtimer.DbgTxt();
//                if (dbgtxt) dbgPrintAngle(fnId, "driftFactor", driftFactor);
//                if (dbgtxt) dbgPrintAngle(fnId, "rotation", rotation);
//                if (dbgtxt) dbgPrintAngle(fnId, "initialDriftPerSecond", initialDriftPerSecond);
//                DbgFuncs.AddStr($"{fnId} gtimer.TotalSeconds: {gtimer.TotalSeconds}");

//            }
//        }

//        new public void Draw(Graphics g)
//        {
//            base.Draw(g);
//            btnLaunch.Draw(g);
//            aimTraj.Draw(g);
//            spinTraj.Draw(g);
//        }

//        public void Reset()
//        {
//            launched = false;
//            gtimer.Stop();
//            gtimer.Reset();
//            position.Set(startPosition);
//            speed = startingSpeed;
//            adjustingAim = false;
//            adjustingSpin = false;
//            updatedAtSeconds = 0;
//            driftFactor = 0;
//            initialDriftPerSecond = 0;
//            startingRotation = 0;
//            aimTraj.Reset();
//            spinTraj.Reset();
//        }

//        public void CleanUp()
//        {
//            btnLaunch.CleanUp();
//        }


//        void SetInitialTrajectory()
//        {
//            rotation = aimTraj.Rotation;
//            startingRotation = rotation;
//        }


//        void CalculateDriftFactor(double aim, double spin)
//        {
//            string fnId = FnId(clsName, "CalculateDriftFactor");
//            //dbgPrintAngle(fnId, "aim", aim);
//            //dbgPrintAngle(fnId, "spin", spin);
//            DbgFuncs.AddStr($"{fnId} Calculating drift factor");
//            RotationDirection defautRotationDirection = (aim < spin) ? RotationDirection.Clockwise : RotationDirection.CounterClockwise;
//            double defaultDifference = (aim < spin) ? spin - aim : aim - spin;
//            double oppositeDifference = (2 * Math.PI) - defaultDifference;
//            RotationDirection oppositeRotationDirection = (aim < spin) ? RotationDirection.CounterClockwise : RotationDirection.Clockwise;
//            RotationDirection shortestRotationDirection = (defaultDifference < oppositeDifference) ? defautRotationDirection : oppositeRotationDirection;
//            double smallestDifference = (defaultDifference < oppositeDifference) ? defaultDifference : oppositeDifference;
//            double rslt = (shortestRotationDirection == RotationDirection.Clockwise) ? smallestDifference : (smallestDifference * (-1));
//            driftFactor = rslt;
//        }
//        void CalculateInitialDriftPerSecond()
//        {
//            initialDriftPerSecond = driftFactor / 100;
//        }

//        double updatedAtSeconds = 0;
//        void ApplyDriftPerSecond(double elapsedSeconds)
//        {
//            if (elapsedSeconds > updatedAtSeconds)
//            {
//                rotation += initialDriftPerSecond;

//                // Applying decay to drift per second // Possible future feature
//                //if (Math.Abs(initialDriftPerSecond) > 0) { initialDriftPerSecond = initialDriftPerSecond - (initialDriftPerSecond * 0.01); }                  

//                // Update updatedAtSeconds
//                updatedAtSeconds = elapsedSeconds;
//            }
//        }


//        void HandleMouseInput(MouseControls mc)
//        {
//            bool dbgtxt = true;
//            string fnId = $"[{clsName}.HandleMouseInput]";
//            if (dbgtxt) DbgFuncs.AddStr($"{fnId} mc.LeftButtonState: {mc.LeftButtonState}");
//            if (dbgtxt) DbgFuncs.AddStr($"{fnId} mc.LastLeftButtonState: {mc.LastLeftButtonState}");

//            mousePos.Set(mc.X, mc.Y);

//            // Reset
//            if (mc.RightButtonState == UpDownState.Down && mc.LastRightButtonState == UpDownState.Up)
//            {
//                //_Reset();
//                Reset();
//                return;
//            }

//            // Prevents aim and spin boxes from continuing to drag if mouse is not down
//            if (mc.LeftButtonState == UpDownState.Up
//                && mc.LastLeftButtonState == UpDownState.Up
//                && (adjustingAim || adjustingSpin))
//            {
//                adjustingAim = adjustingSpin = false;
//                return;
//            }


//            // Setting up the shot
//            if (!launched)
//            {

//                // Place aim marker
//                if (!aimTraj.Placed
//                    && mc.LeftButtonState == UpDownState.Down
//                    && mc.LastLeftButtonState == UpDownState.Up)
//                {
//                    aimTraj.SetStartPoint(position);
//                    aimTraj.SetEndPoint(mc.Position);

//                    spinTraj.SetStartPoint(position);
//                    PointD spinStartPos = new PointD();
//                    spinStartPos = position.HalfWayTo(mc.Position.X, mc.Position.Y);
//                    spinTraj.SetEndPoint(spinStartPos);

//                    return;
//                }

//                // Adjust aim or spin
//                if (aimTraj.Placed
//                    && (!adjustingAim || !adjustingSpin)
//                    && mc.LeftButtonState == UpDownState.Down
//                    && mc.LastLeftButtonState == UpDownState.Up)
//                {

//                    if (aimTraj.InEndRect(mc.X, mc.Y)) { adjustingAim = true; return; }
//                    if (spinTraj.InEndRect(mc.X, mc.Y)) { adjustingSpin = true; return; }

//                }

//                // Stop adjusting aim or spin
//                if ((adjustingAim || adjustingSpin)
//                    && mc.LeftButtonState == UpDownState.Up
//                    && mc.LastLeftButtonState == UpDownState.Down)
//                {
//                    adjustingAim = adjustingSpin = false;
//                    return;
//                }

//                // Launch the ball and start the timer (when launch button is clicked)
//                if ((!adjustingAim && !adjustingSpin)
//                    && mc.LastLeftButtonState == UpDownState.Up
//                    && mc.LeftButtonState == UpDownState.Down
//                    //&& InLaunchButtonRect(mc.X, mc.Y)
//                    && btnLaunch.InBoundingRect(mc.X, mc.Y))
//                {
//                    launched = true;
//                    gtimer.Start();
//                }
//            }

//        }


//        void PositionLaunchButton()     // after gameScreenSize is set b.c it is used to place button
//        {
//            Point position = new Point();
//            Size size = new Size();
//            size.Width = 100;
//            size.Height = 40;
//            position.X = gameScreenSize.Width / 2 - size.Width / 2;
//            position.Y = gameScreenSize.Height - size.Height * 2 - 5;
//            btnLaunch.Load(position.X, position.Y, size.Width, size.Height, "Launch");
//        }


//    }
//}
#endregion GameBall04 before adding collision/bounce logic 2020-01-01

#region Removed InLaunchButtonRect and just called function directly in mouse controls (launch ball section) 2020-01-01
//public bool InLaunchButtonRect(int x, int y) { return (btnLaunch.InBoundingRect(x, y)); }
#endregion Removed InLaunchButtonRect and just called function directly in mouse controls (launch ball section) 2020-01-01