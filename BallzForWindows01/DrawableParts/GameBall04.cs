using System;
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
        double startingAngle = 0;

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
            }
            UpdateTrajectories();

            //flightPath.CalculateDriftFactor();
            //rotation = flightPath.CalculatedAngle(rotation, gtimer.TotalSeconds);
            if (dbgtxt) dbgPrintAngle(fnId, "rotation", rotation);
            //flightPath.DbgPlotPath();

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
                //flightPath.DbgText();


            }
        }


        new public void Draw(Graphics g)
        {
            //_Draw(g); 
            base.Draw(g);
            //flightPath.Draw(g, !launched);
            btnLaunch.Draw(g);

            aimTraj.Draw(g);
            spinTraj.Draw(g);
        }

        public void Reset()
        {
            //_Reset(); 
            launched = false;
            gtimer.Stop();
            gtimer.Reset();
            //readyForLaunch = false;
            position.Set(startPosition);
            //flightPath.Reset();
            speed = startingSpeed;
            adjustingAim = false;
            adjustingSpin = false;

            aimTraj.Reset();
            spinTraj.Reset();
        }

        public void CleanUp()
        {
            //_CleanUp(); 
            btnLaunch.CleanUp();
        }

        void UpdateTrajectories()
        {
            aimTraj.Update();
            spinTraj.Update();
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
