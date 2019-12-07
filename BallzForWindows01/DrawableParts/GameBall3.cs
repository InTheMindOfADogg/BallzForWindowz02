using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace BallzForWindows01.DrawableParts
{
    using GamePhysicsParts;
    using Structs;

    class GameBall3 : CollisionCircleDV2
    {
        Size gameScreenSize;

        FlightPath2 flightPath;
        Button01 launchButton;

        PointD startPosition;
        PointD mousePos;

        double speed = 1.0;
        double startingSpeed = 1.0;
        double fpAngle = 0;
        double fpDrift = 0;
        double driftFactor = 0;
        double calculatedAngle = 0;
        double timedriftModifier = 0;
        double drifthardness = 0.5;

        bool adjustingAim = false;
        bool adjustingSpin = false;
        bool readyForLaunch = false;    // same as setting spin, might need to adjust later
        bool placingSpinRect = false;
        bool launched = false;


        public GameBall3(Size gameScreenSize)
            : base() { _Init(gameScreenSize); }

        public GameBall3(Size gameScreenSize, double x, double y, double radius = 20, double rotation = 0, int collisionPoints = 5)
            : base(x, y, radius, rotation, collisionPoints) { _Init(gameScreenSize); }

        new public void Load(double x, double y, double hitBoxSideLength, double radius, double rotation, int collisionPoints)
        { base.Load(x, y, radius, rotation, collisionPoints); _Load(x, y); }

        public void Update(MouseControls mc) { HandleMouseInput(mc); _Update(); }

        new public void Draw(Graphics g) { _Draw(g); }

        public void Reset() { _Reset(); }

        public void CleanUp() { _CleanUp(); }


        public bool InLaunchButtonRect(int x, int y) { return (launchButton.InBoundingRect(x, y)); }

        //public bool InSpinRect(double x, double y) { return flightPath.InSpinRect(x, y); }

        void HandleMouseInput(MouseControls mc)
        {
            string fnId = $"[{clsName}.HandleMouseInput]";
            DbgFuncs.AddStr($"{fnId} mc.LeftButtonState: {mc.LeftButtonState}");
            DbgFuncs.AddStr($"{fnId} mc.LastLeftButtonState: {mc.LastLeftButtonState}");

            mousePos.Set(mc.X, mc.Y);

            // Setting up the shot
            if (!launched)
            {
                // Place aim marker
                if (!flightPath.AimMarkerPlaced
                    && mc.LeftButtonState == UpDownState.Down
                    && mc.LastLeftButtonState == UpDownState.Up)
                {
                    flightPath.PlaceOriginMarker(position.X, position.Y);
                    flightPath.PlaceAimMarker(mc.X, mc.Y);
                }

                // Adjust aim or spin
                if (flightPath.AimMarkerPlaced
                    && (!adjustingAim || !adjustingSpin)
                    && mc.LeftButtonState == UpDownState.Down
                    && mc.LastLeftButtonState == UpDownState.Up)
                {
                    if (flightPath.InAimRect(mc.X, mc.Y)) { adjustingAim = true; return; }
                    if (flightPath.InSpinRect(mc.X, mc.Y)) { adjustingSpin = true; return; }
                }

                // Stop adjusting aim or spin
                if ((adjustingAim || adjustingSpin)
                    && mc.LeftButtonState == UpDownState.Up
                    && mc.LastLeftButtonState == UpDownState.Down)
                {
                    adjustingAim = adjustingSpin = false;
                }

                // TODO: set up launch


            }

            // Reset
            if (mc.RightButtonState == UpDownState.Down && mc.LastRightButtonState == UpDownState.Up)
            {
                _Reset();
            }

        }



        void _Init(Size gameScreenSize)
        {
            clsName = "GameBall3";
            this.gameScreenSize = gameScreenSize;
            flightPath = new FlightPath2();
            launchButton = new Button01();
            startPosition = new PointD();
            mousePos = new PointD();
        }
        void _Load(double x, double y)
        {
            //DebugConfigure(true);
            flightPath.Load();
            startPosition.Set(x, y);
            PositionLaunchButton();
        }
        void _Update()
        {
            string fnId = $"[{clsName}._Update]";
            if (!launched)
            {

                if (adjustingAim)
                {
                    flightPath.PlaceAimMarker(mousePos.X, mousePos.Y);
                }
                if (adjustingSpin)
                {
                    flightPath.PlaceSpinMarker(mousePos.X, mousePos.Y);
                }
                fpAngle = flightPath.Angle;
                fpDrift = flightPath.Drift;
                driftFactor = (fpAngle - fpDrift) * drifthardness;
                calculatedAngle = fpAngle - (driftFactor * timedriftModifier);
            }
            UpdateCollisionPoints(position.X, position.Y, radius, rotation);

            if (DrawDbgTxt)
            {

                DbgFuncs.AddStr($"{fnId} ");
                //DbgFuncs.AddStr($"{fnId} collisionPointHit (index): {cpHitIdx}");
                //DbgFuncs.AddStr($"{fnId} outerAngle: {(outerAngle * 180 / Math.PI):N2}");
                //DbgFuncs.AddStr($"{fnId} calculatedBouneAngle: {(calculatedBounceAngle * 180 / Math.PI):N2}");
                //DbgFuncs.AddStr($"{fnId} middle cp hit idx: {circ2.MiddleCPIdx}");           
                //DbgFuncs.AddStr($"{fnId} fpAngle: {fpAngle:N3} ({(fpAngle * 180 / Math.PI):N2})");
                //DbgFuncs.AddStr($"{fnId}timeIn90: {timesIn90}");
                //DbgFuncs.AddStr($"{fnId} toNext90: {toNext90:N3} ({(toNext90 * 180 / Math.PI):N2})");
                //DbgFuncs.AddStr($"{fnId} toPrev90: {toPrev90:N3} ({(toPrev90 * 180 / Math.PI):N2})");
                //DbgFuncs.AddStr($"{fnId} bounceAngle: {bounceAngle:N3} ({(bounceAngle * 180 / Math.PI):N2})");
                //DbgFuncs.AddStr($"{fnId} angleAfterBounce: {angleAfterBounce:N3} ({(angleAfterBounce * 180 / Math.PI):N2})");
                //DbgFuncs.AddStr($"{fnId} dbgInfoCpIdxHit: {dbgtxtCpIdxHit}");
                //DbgFuncs.AddStr($"{fnId} PublicHitIdxList: {dbgtxtPublicHitIdxList}");

                //DebugTextCollisionCircle();

            }
        }

        void _Draw(Graphics g)
        {
            //base.Draw(g);
            DrawCircle(g);
            DrawCollisionPoints(g);
            flightPath.Draw(g, !launched);
            launchButton.Draw(g);
        }

        void _Reset()
        {
            launched = false;
            //pause = false;
            //endTime = DateTime.Now; // not using at the moment, but might use later 2019-10-12.
            readyForLaunch = false;
            position = startPosition;
            flightPath.Reset();
            timedriftModifier = 0;
            speed = startingSpeed;
            adjustingAim = false;
            adjustingSpin = false;
            //bounceCount = 0;
            //calculatedBounceAngle = 0;
            //collided = false;
        }
        void _CleanUp()
        {
            launchButton.CleanUp();
        }


        void PositionLaunchButton()     // after gameScreenSize is set b.c it is used to place button
        {
            Point position = new Point();
            Size size = new Size();
            size.Width = 100;
            size.Height = 40;
            position.X = gameScreenSize.Width / 2 - size.Width / 2;
            position.Y = gameScreenSize.Height - size.Height * 2 - 5;
            launchButton.Load(position.X, position.Y, size.Width, size.Height, "Launch");
        }

        void DebugConfigure(bool debugValue = false)
        {
            //flightPath.ConnectMarkers = debugValue;
            //flightPath.DebugConfigure(debugValue);
            //circ2.DrawDbgTxt = DrawDbgTxt;
        }



    }
}
