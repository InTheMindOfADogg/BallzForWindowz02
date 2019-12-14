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
        double elapsedTime = 0; // elapsedTime just place holder for now. I am planning to use it for furture ball moving logic. 2019-12-07

        bool adjustingAim = false;
        bool adjustingSpin = false;
        //bool readyForLaunch = false;    // same as setting spin, might need to adjust later
        //bool placingSpinRect = false;
        bool launched = false;


        public GameBall3(Size gameScreenSize)
            : base() { _Init(gameScreenSize); }

        new public void Load(double x, double y, double hitBoxSideLength, double radius, double rotation, int collisionPoints)
        {base.Load(x, y, hitBoxSideLength, radius, rotation, collisionPoints);_Load(x, y);}

        public void Update(MouseControls mc){HandleMouseInput(mc);_Update();}

        new public void Draw(Graphics g) { _Draw(g); }

        public void Reset() { _Reset(); }

        public void CleanUp() { _CleanUp(); }

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
            flightPath.Load();
            flightPath.DriftHardness = 0.5;
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
            }
            
            rotation = flightPath.CalculatedAngle(elapsedTime);
            flightPath.DbgPlotPath();

            

            if (launched)
            {
                position.X = position.X + speed * Math.Cos(rotation);
                position.Y = position.Y + speed * Math.Sin(rotation);
            }

            UpdateCollisionPoints(position.X, position.Y, radius, rotation);

            if (DrawDbgTxt)
            {

                DbgFuncs.AddStr($"{fnId} ");
                DbgFuncs.AddStr($"{fnId} launched: {launched}");
                DbgFuncs.AddStr($"{fnId} flightPath.AimMarkerPlaced: {flightPath.AimMarkerPlaced}");
                DbgFuncs.AddStr($"{fnId} rotation: {(rotation * 180 / Math.PI):N2}");

            }
        }
        
        void _Draw(Graphics g)
        {
            base.Draw(g);
            flightPath.Draw(g, !launched);
            launchButton.Draw(g);
        }
        void _Reset()
        {
            launched = false;
            //readyForLaunch = false;
            position.Set(startPosition);
            flightPath.Reset();
            speed = startingSpeed;
            adjustingAim = false;
            adjustingSpin = false;
        }
        void _CleanUp()
        {
            launchButton.CleanUp();
        }


        void HandleMouseInput(MouseControls mc)
        {
            string fnId = $"[{clsName}.HandleMouseInput]";
            DbgFuncs.AddStr($"{fnId} mc.LeftButtonState: {mc.LeftButtonState}");
            DbgFuncs.AddStr($"{fnId} mc.LastLeftButtonState: {mc.LastLeftButtonState}");

            mousePos.Set(mc.X, mc.Y);

            // Reset
            if (mc.RightButtonState == UpDownState.Down && mc.LastRightButtonState == UpDownState.Up)
            {
                _Reset();
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
                if (!flightPath.AimMarkerPlaced
                    && mc.LeftButtonState == UpDownState.Down
                    && mc.LastLeftButtonState == UpDownState.Up)
                {
                    flightPath.PlaceOriginMarker(position.X, position.Y);
                    flightPath.PlaceAimMarker(mc.X, mc.Y);
                    return;
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
                    return;
                }

                // TODO: set up launch
                if ((!adjustingAim && !adjustingSpin)
                    && mc.LastLeftButtonState == UpDownState.Up
                    && mc.LeftButtonState == UpDownState.Down
                    && InLaunchButtonRect(mc.X, mc.Y))
                {
                    launched = true;
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
            launchButton.Load(position.X, position.Y, size.Width, size.Height, "Launch");
        }

        public bool InLaunchButtonRect(int x, int y) { return (launchButton.InBoundingRect(x, y)); }



    }
}

#region previous Update function
//void _Update()
//{
//    string fnId = $"[{clsName}._Update]";
//    if (!launched)
//    {
//        if (adjustingAim)
//        {
//            flightPath.PlaceAimMarker(mousePos.X, mousePos.Y);
//        }
//        if (adjustingSpin)
//        {
//            flightPath.PlaceSpinMarker(mousePos.X, mousePos.Y);
//        }
//        fpAngle = flightPath.Angle;
//        fpDrift = flightPath.Drift;
//        driftFactor = (fpAngle - fpDrift) * driftHardness;
//        calculatedAngle = fpAngle - (driftFactor * timeDriftModifier);
//        //rotation = calculatedAngle;
//    }
//    flightPath.Update();
//    UpdateCollisionPoints(position.X, position.Y, radius, rotation);

//    if (DrawDbgTxt)
//    {

//        DbgFuncs.AddStr($"{fnId} ");
//        DbgFuncs.AddStr($"{fnId} launched: {launched}");
//        DbgFuncs.AddStr($"{fnId} rotation: {(rotation * 180 / Math.PI):N2}");
//        DbgFuncs.AddStr($"{fnId} calculatedAngle: {(calculatedAngle * 180 / Math.PI):N2}");

//    }
//}
#endregion previous Update function

#region removed 2019-12-07

#region old debug text from if DrawDbgTxt statement in _Update
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
#endregion old debug text from if DrawDbgTxt statement in _Update

//void DebugConfigure(bool debugValue = false)
//{
//    //flightPath.ConnectMarkers = debugValue;
//    //flightPath.DebugConfigure(debugValue);
//    //circ2.DrawDbgTxt = DrawDbgTxt;
//}

//public GameBall3(Size gameScreenSize, double x, double y, double radius = 20, double rotation = 0, int collisionPoints = 5)
//    : base(x, y, radius, rotation, collisionPoints) 
//{ _Init(gameScreenSize); }    // 0 Refs as of 2019-12-07



#endregion removed 2019-12-07





