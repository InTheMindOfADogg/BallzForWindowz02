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
    class GameBall3 : CollisionCircleDV2
    {
        Size gameScreenSize;

        FlightPath flightPath;
        Button01 launchButton;

        PointD startPosition;


        double speed = 1.0;
        double startingSpeed = 1.0;
        double fpAngle = 0;
        double fpDrift = 0;
        double driftFactor = 0;
        double calculatedAngle = 0;
        double timedriftModifier = 0;
        double drifthardness = 0.5;

        bool placingAim = false;
        bool settingSpin = false;
        bool readyForLaunch = false;    // same as setting spin, might need to adjust later
        bool placingSpinRect = false;
        bool ballLaunched = false;


        public bool ReadyForLaunch { get { return readyForLaunch; } }
        public bool PlacingAim { get { return placingAim; } set { placingAim = value; } }
        public bool SettingSpin { get { return settingSpin; } }
        public bool PlacingSpinRect { get { return placingSpinRect; } set { placingSpinRect = value; } }
        public bool BallLaunched { get { return ballLaunched; } }


        public GameBall3(Size gameScreenSize)
            : base()
        {
            _Init(gameScreenSize);
        }
        public GameBall3(Size gameScreenSize, double x, double y, double radius = 20, double rotation = 0, int collisionPoints = 5)
            : base(x, y, radius, rotation, collisionPoints)
        {
            _Init(gameScreenSize);
        }
        new public void Load(double x, double y, double hitBoxSideLength, double radius, double rotation, int collisionPoints)
        {
            base.Load(x, y, radius, rotation, collisionPoints);
            _Load(x, y);
        }
        public void Update()
        {
            _Update();            
        }
        new public void Draw(Graphics g)
        {
            _Draw(g);
        }
        public void Reset()
        {
            _Reset();
        }
        public void CleanUp()
        {
            _CleanUp();
        }

        public bool IsInLaunchButtonRect(int x, int y) { return (launchButton.IsInBoundingRect(x, y)); }



        void _Init(Size gameScreenSize)
        {
            clsName = "GameBall3";
            this.gameScreenSize = gameScreenSize;
            flightPath = new FlightPath();
            launchButton = new Button01();
            startPosition = new PointD();
        }
        void _Load(double x, double y)
        {
            DebugConfigure(false);
            flightPath.Load();
            startPosition.Set(x, y);

            PositionLaunchButton();
        }
        void _Update()
        {
            string fnId = $"[{clsName}._Update]: ";
            if (!ballLaunched)
            {
                fpAngle = flightPath.Angle;
                fpDrift = flightPath.Drift;
                driftFactor = (fpAngle - fpDrift) * drifthardness;
                calculatedAngle = fpAngle - (driftFactor * timedriftModifier);
            }
            UpdateCollisionPoints(position.X, position.Y, radius, rotation);

            if (DrawDbgTxt)
            {
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
                DebugTextCollisionCircle();

            }
        }
        void _Draw(Graphics g)
        {
            //base.Draw(g);
            DrawCircle(g);
            DrawCollisionPointList(g);
            launchButton.Draw(g);
        }

        void _Reset()
        {
            ballLaunched = false;
            //pause = false;
            //endTime = DateTime.Now; // not using at the moment, but might use later 2019-10-12.
            readyForLaunch = false;
            position = startPosition;
            flightPath.Reset();
            timedriftModifier = 0;
            speed = startingSpeed;
            //bounceCount = 0;
            //calculatedBounceAngle = 0;
            //collided = false;
        }
        void _CleanUp()
        {
            launchButton.CleanUp();
        }

        void DebugConfigure(bool debugValue = false)
        {
            //flightPath.ConnectMarkers = debugValue;
            flightPath.DebugConfigure(debugValue);
            //circ2.DrawDbgTxt = DrawDbgTxt;
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



    }
}
