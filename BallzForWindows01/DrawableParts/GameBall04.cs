﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.DrawableParts
{
    using static AssistFunctions;
    using GamePhysicsParts;
    using Structs;

    class GameBall04 : CollisionCircleDV2
    {
        Size gameScreenSize;

        Trajectory03 aimTraj;
        Trajectory03 spinTraj;

        Button01 btnLaunch;

        PointD startPosition;
        PointD mousePos;

        GameTimer gtimer;


        double speed = 1.0;
        double startingSpeed = 1.0;
        double startingRotation = 0;

        double driftFactor = 0;
        double initialDriftPerSecond = 0;

        bool adjustingAim = false;
        bool adjustingSpin = false;
        bool launched = false;


        public GameBall04(Size gameScreenSize)
            : base()
        {

            clsName = "GameBall04";
            this.gameScreenSize = gameScreenSize;
            btnLaunch = new Button01();
            startPosition = new PointD();
            mousePos = new PointD();
            gtimer = new GameTimer();

            aimTraj = new Trajectory03();
            spinTraj = new Trajectory03();
            spinTraj.SetXColor(Color.AntiqueWhite);
            aimTraj.NameTag = "aimTraj";
            spinTraj.NameTag = "spinTraj";

        }

        new public void Load(double x, double y, double hitBoxSideLength, double radius, double rotation, int collisionPoints)
        {
            base.Load(x, y, hitBoxSideLength, radius, rotation, collisionPoints);

            startPosition.Set(x, y);
            PositionLaunchButton();
        }


        public void Update(MouseControls mc)
        {
            bool dbgtxt = true;
            string fnId = FnId(clsName, "Update");
            HandleMouseInput(mc);

            if (dbgtxt) DbgFuncs.AddStr($"{fnId} launched: {launched}");


            if (!launched)
            {
                if (adjustingAim)
                {
                    aimTraj.SetEndPoint(mousePos);
                }
                if (adjustingSpin)
                {
                    spinTraj.SetEndPoint(mousePos);
                }
                SetInitialTrajectory();
                CalculateDriftFactor(aimTraj.Rotation, spinTraj.Rotation);
                CalculateInitialDriftPerSecond();

            }

            // Put here for building, after finished building, move to pre launch logic        
            //CalculateInitialDriftPerSecond();
            ApplyDriftPerSecond(gtimer.TotalSeconds);
            

            if (launched)
            {
                gtimer.Update();
                position.X = position.X + speed * Math.Cos(rotation);
                position.Y = position.Y + speed * Math.Sin(rotation);
            }

            UpdateCollisionPoints(position.X, position.Y, radius, rotation);

            if (dbgtxt && DrawDbgTxt)
            {
                gtimer.DbgTxt();
                if (dbgtxt) dbgPrintAngle(fnId, "driftFactor", driftFactor);
                if (dbgtxt) dbgPrintAngle(fnId, "rotation", rotation);
                if (dbgtxt) dbgPrintAngle(fnId, "initialDriftPerSecond", initialDriftPerSecond);
                DbgFuncs.AddStr($"{fnId} gtimer.TotalSeconds: {gtimer.TotalSeconds}");

            }
        }


        new public void Draw(Graphics g)
        {
            base.Draw(g);
            btnLaunch.Draw(g);

            aimTraj.Draw(g);
            spinTraj.Draw(g);
        }

        public void Reset()
        {
            launched = false;
            gtimer.Stop();
            gtimer.Reset();
            position.Set(startPosition);
            speed = startingSpeed;
            adjustingAim = false;
            adjustingSpin = false;
            updatedAtSeconds = 0;
            driftFactor = 0;
            initialDriftPerSecond = 0;
            //driftForSecond = 0;
            //previousRotation = 0;
            startingRotation = 0;
            aimTraj.Reset();
            spinTraj.Reset();
        }

        public void CleanUp()
        {
            btnLaunch.CleanUp();
        }

        
        void SetInitialTrajectory()
        {
            rotation = aimTraj.Rotation;
            startingRotation = rotation;
        }

        
        void CalculateDriftFactor(double aim, double spin)
        {
            string fnId = FnId(clsName, "CalculateDriftFactor");
            //dbgPrintAngle(fnId, "aim", aim);
            //dbgPrintAngle(fnId, "spin", spin);
            DbgFuncs.AddStr($"{fnId} Calculating drift factor");
            RotationDirection defautRotationDirection = (aim < spin) ? RotationDirection.Clockwise : RotationDirection.CounterClockwise;
            double defaultDifference = (aim < spin) ? spin - aim : aim - spin;
            double oppositeDifference = (2 * Math.PI) - defaultDifference;
            RotationDirection oppositeRotationDirection = (aim < spin) ? RotationDirection.CounterClockwise : RotationDirection.Clockwise;
            RotationDirection shortestRotationDirection = (defaultDifference < oppositeDifference) ? defautRotationDirection : oppositeRotationDirection;
            double smallestDifference = (defaultDifference < oppositeDifference) ? defaultDifference : oppositeDifference;
            double rslt = (shortestRotationDirection == RotationDirection.Clockwise) ? smallestDifference : (smallestDifference * (-1));
            driftFactor = rslt;

            //dbgPrintAngle(fnId, "driftFactor", driftFactor);
        }
        void CalculateInitialDriftPerSecond()
        {
            initialDriftPerSecond = driftFactor / 100;
        }

        //double previousRotation = 0;
        double updatedAtSeconds = 0;
        //double driftForSecond = 0;
        void ApplyDriftPerSecond(double elapsedSeconds)
        {
            
            if(elapsedSeconds > updatedAtSeconds)
            {
                rotation += initialDriftPerSecond;
                
                // Applying decay to drift per second
                //if (Math.Abs(initialDriftPerSecond) > 0) { initialDriftPerSecond = initialDriftPerSecond - (initialDriftPerSecond * 0.01); }  
                
                
                
                // Update updatedAtSeconds
                updatedAtSeconds = elapsedSeconds;
            }
        }


        void HandleMouseInput(MouseControls mc)
        {
            bool dbgtxt = true;
            string fnId = $"[{clsName}.HandleMouseInput]";
            if (dbgtxt) DbgFuncs.AddStr($"{fnId} mc.LeftButtonState: {mc.LeftButtonState}");
            if (dbgtxt) DbgFuncs.AddStr($"{fnId} mc.LastLeftButtonState: {mc.LastLeftButtonState}");

            mousePos.Set(mc.X, mc.Y);

            // Reset
            if (mc.RightButtonState == UpDownState.Down && mc.LastRightButtonState == UpDownState.Up)
            {
                //_Reset();
                Reset();
                return;
            }

            // Prevents aim and spin boxes from continuing to drag if mouse is not down
            if (mc.LeftButtonState == UpDownState.Up
                && mc.LastLeftButtonState == UpDownState.Up
                && (adjustingAim || adjustingSpin))
            {
                adjustingAim = adjustingSpin = false;
                return;
            }


            // Setting up the shot
            if (!launched)
            {

                // Place aim marker
                if (!aimTraj.Placed
                    && mc.LeftButtonState == UpDownState.Down
                    && mc.LastLeftButtonState == UpDownState.Up)
                {
                    aimTraj.SetStartPoint(position);
                    aimTraj.SetEndPoint(mc.Position);

                    spinTraj.SetStartPoint(position);
                    PointD spinStartPos = new PointD();
                    spinStartPos = position.HalfWayTo(mc.Position.X, mc.Position.Y);
                    spinTraj.SetEndPoint(spinStartPos);

                    return;
                }

                // Adjust aim or spin
                if (aimTraj.Placed
                    && (!adjustingAim || !adjustingSpin)
                    && mc.LeftButtonState == UpDownState.Down
                    && mc.LastLeftButtonState == UpDownState.Up)
                {

                    if (aimTraj.InEndRect(mc.X, mc.Y)) { adjustingAim = true; return; }
                    if (spinTraj.InEndRect(mc.X, mc.Y)) { adjustingSpin = true; return; }

                }

                // Stop adjusting aim or spin
                if ((adjustingAim || adjustingSpin)
                    && mc.LeftButtonState == UpDownState.Up
                    && mc.LastLeftButtonState == UpDownState.Down)
                {
                    adjustingAim = adjustingSpin = false;
                    return;
                }

                // TODO: set up launch
                if ((!adjustingAim && !adjustingSpin)
                    && mc.LastLeftButtonState == UpDownState.Up
                    && mc.LeftButtonState == UpDownState.Down
                    && InLaunchButtonRect(mc.X, mc.Y))
                {
                    launched = true;
                    gtimer.Start();
                }
            }

        }


        void PositionLaunchButton()     // after gameScreenSize is set b.c it is used to place button
        {
            Point position = new Point();
            Size size = new Size();
            size.Width = 100;
            size.Height = 40;
            position.X = gameScreenSize.Width / 2 - size.Width / 2;
            position.Y = gameScreenSize.Height - size.Height * 2 - 5;
            btnLaunch.Load(position.X, position.Y, size.Width, size.Height, "Launch");
        }

        public bool InLaunchButtonRect(int x, int y) { return (btnLaunch.InBoundingRect(x, y)); }



    }
}