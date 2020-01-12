// commented out 2020-01-11

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Drawing;
//using System.Timers;

//using BallzForWindows01.GamePhysicsParts;


//namespace BallzForWindows01.DrawableParts
//{
//    class GameBall2 : DrawableObject
//    {

//        public List<int> PublicHitIdxList = new List<int>();

//        #region local drawing font settings
//        Font font;
//        Color fontColor = Color.Black;
//        int fontSize = 20;
//        string fontFamily = "Arial";
//        #endregion local drawing font settings


//        FlightPath flightPath;
//        Button01 launchButton;

//        PointD dstartPosition;
//        PointD dcenter;

//        CollisionCircleD circ2;
//        int collisionPointCount = 5;
//        double collisionPointSideLength = 1;


//        bool collided = false;

//        int width = 0;
//        int height = 0;
//        double dx = 0;
//        double dy = 0;
//        double startingSpeed = 1.0;
//        double speed = 1.0;// 0.5;

//        DateTime startTime;
//        DateTime endTime;
//        TimeSpan flightTime = TimeSpan.Zero;
//        double fpAngle = 0;
//        double fpDrift = 0;

//        double timedriftModifier = 0;
//        double totalMs = 0;
//        double driftFactor = 0;

//        double calculatedAngle = 0;

//        double secondsElapsed = 0;
//        double secondsRemaining = 0;
//        double roundTime = 10;
//        double drifthardness = 0.5;

//        Size gameScreenSize = new Size();
//        bool placingAim = false;
//        bool settingSpin = false;
//        bool readyForLaunch = false;    // same as setting spin, might need to adjust later
//        bool placingSpinRect = false;
//        bool ballLaunched = false;

//        bool pause = false;

//        string dbgtxtCpIdxHit = "";
//        string dbgtxtPublicHitIdxList = "";

//        double east, south, west, north, fullCircle;

//        // used in Bounce functions (ex Bounce[X])
//        #region used in bounce function
//        int timesIn90 = 0;  // this is gpAngle / 90. used to "normalize" the angle so that i can work with it as if it were 0-90 degrees
//        double toNext90 = 0;
//        double toPrev90 = 0;
//        double bounceAngle = 0;
//        double angleAfterBounce = 0;


//        double calculatedBounceAngle = 0;
//        double outerAngle = 0;
//        int bounceCount = 0;
//        int cpHitIdx = -1;
//        #endregion used in bounce function

//        List<int> cpIdxHitList;

//        #region Properties
//        public Size GameScreenSize { get { return gameScreenSize; } set { gameScreenSize = value; } }
//        public bool ReadyForLaunch { get { return readyForLaunch; } }
//        public bool PlacingAim { get { return placingAim; } set { placingAim = value; } }
//        public bool SettingSpin { get { return settingSpin; } }
//        public bool PlacingSpinRect { get { return placingSpinRect; } set { placingSpinRect = value; } }
//        public bool BallLaunched { get { return ballLaunched; } }

//        public double X { get { return dx; } }
//        public double Y { get { return dy; } }
//        public int Width { get { return width; } }
//        public int Height { get { return height; } }

//        public List<CollisionPoint> CollisionPointList { get { return circ2.CollisionPointList; } }

//        public bool Collide { get { return collided; } set { collided = value; } }
//        //public double Speed { get { return speed; } set { speed = value; } }

//        public bool Pause { get { return pause; } set { pause = value; } }
//        #endregion Properties



//        public GameBall2(Size gameScreenSize)
//        {
//            this.clsName = "GameBall2";
//            this.gameScreenSize = gameScreenSize;
//            dstartPosition = new PointD(0, 0);
//            SetPosition(0, 0);
//            SetSize(15, 15);
//            SetColor(255, 0, 0, 0);
//            font = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
//            placingAim = false;
//            settingSpin = false;
//            readyForLaunch = false;    // same as setting spin, might need to adjust later
//            placingSpinRect = false;
//            ballLaunched = false;

//            InitBallParts();

//            east = 0;
//            south = east + Math.PI / 2;
//            west = south + Math.PI / 2;
//            north = west + Math.PI / 2;
//            fullCircle = 2 * Math.PI;

//            //DebugConfigure(false);
//        }

//        public void Load(int x, int y) { _Load(x, y, width, height); }
//        public void Load(int x, int y, int width, int height) { _Load(x, y, width, height); }
//        public void Update()
//        {

//            string fnId = $"[{clsName}.Update]: ";
//            if (!ballLaunched)
//            {
//                fpAngle = flightPath.Angle;
//                fpDrift = flightPath.Drift;
//                driftFactor = (fpAngle - fpDrift) * drifthardness;
//                calculatedAngle = fpAngle - (driftFactor * timedriftModifier);
//            }
//            //AddUpdateDebugMsgs();

//            if (ballLaunched && !pause)
//            {
//                flightTime = DateTime.Now - startTime;
//                totalMs = flightTime.TotalMilliseconds;
//                secondsElapsed = flightTime.TotalSeconds;
//                secondsRemaining = roundTime - secondsElapsed;
//                timedriftModifier = totalMs / 250;

//                // bounce logic

                
//                SetInfoCpIdxHit(cpIdxHitList);
//                if (PublicHitIdxList.Count > 0)
//                {
//                    Bounce3(PublicHitIdxList[0]);
//                    collided = false;
//                    PublicHitIdxList.RemoveRange(0, PublicHitIdxList.Count);
//                }

//                // starts looping if angle gets too high
//                calculatedAngle = fpAngle - (driftFactor * timedriftModifier);


//                dx = dx + speed * Math.Cos(calculatedAngle);
//                dy = dy + speed * Math.Sin(calculatedAngle);

//                // commented out for testing. commenting this out should prevent reset based of time
//                //if (secondsRemaining <= 0) { secondsRemaining = 0; Reset(); }
//                if (dy <= 0 || dy >= gameScreenSize.Height) { Reset(); }
//                if (dx <= 0 || dx >= gameScreenSize.Width) { Reset(); }
//            }

//            circ2.Update(dx, dy, width / 2, calculatedAngle);

//            #region collision point and bounce angle dbg logic
//            if (DrawDbgTxt)
//            {
//                DbgFuncs.AddStr($"{fnId} collisionPointHit (index): {cpHitIdx}");
//                DbgFuncs.AddStr($"{fnId} outerAngle: {(outerAngle * 180 / Math.PI):N2}");
//                DbgFuncs.AddStr($"{fnId} calculatedBouneAngle: {(calculatedBounceAngle * 180 / Math.PI):N2}");
//                DbgFuncs.AddStr($"{fnId} middle cp hit idx: {circ2.MiddleCPIdx}");
//            }
//            #endregion collision point and bounce angle dbg logic
//            #region testing logic for bounce
//            if (DrawDbgTxt)
//            {
//                DbgFuncs.AddStr($"{fnId} fpAngle: {fpAngle:N3} ({(fpAngle * 180 / Math.PI):N2})");
//                DbgFuncs.AddStr($"{fnId}timeIn90: {timesIn90}");
//                DbgFuncs.AddStr($"{fnId} toNext90: {toNext90:N3} ({(toNext90 * 180 / Math.PI):N2})");
//                DbgFuncs.AddStr($"{fnId} toPrev90: {toPrev90:N3} ({(toPrev90 * 180 / Math.PI):N2})");
//                DbgFuncs.AddStr($"{fnId} bounceAngle: {bounceAngle:N3} ({(bounceAngle * 180 / Math.PI):N2})");
//                DbgFuncs.AddStr($"{fnId} angleAfterBounce: {angleAfterBounce:N3} ({(angleAfterBounce * 180 / Math.PI):N2})");
//                DbgFuncs.AddStr($"{fnId} dbgInfoCpIdxHit: {dbgtxtCpIdxHit}");
//                DbgFuncs.AddStr($"{fnId} PublicHitIdxList: {dbgtxtPublicHitIdxList}");

//            }
//            #endregion testing logic for bounce
//            PublicHitIdxList.RemoveRange(0, PublicHitIdxList.Count);
//        }
        
//        public void Draw(Graphics g)
//        {
//            SolidBrush sb = new SolidBrush(color);
//            launchButton.Draw(g);
//            flightPath.Draw(g, !ballLaunched);  // do not draw flight path is ball is launched
//            circ2.Draw(g);

//            //dbgDrawBounceAngle(g);
//            sb.Dispose();
//        }
//        public void Reset()
//        {
//            ballLaunched = false;
//            pause = false;
//            endTime = DateTime.Now; // not using at the moment, but might use later 2019-10-12.
//            readyForLaunch = false;
//            dx = dstartPosition.X;
//            dy = dstartPosition.Y;
//            flightPath.Reset();
//            timedriftModifier = 0;
//            speed = startingSpeed;
//            bounceCount = 0;
//            calculatedBounceAngle = 0;
//            collided = false;
//        }
//        public void CleanUp()
//        {
//            if (font != null) font.Dispose();
//            launchButton.CleanUp();
//            //timer.Close();
//            //timer.Dispose();
//        }


//        public bool IsInSpinRect(int x, int y) { if (flightPath.IsInBoundingRect(x, y)) { return true; } else { return false; } }

//        public bool IsInLaunchButtonRect(int x, int y) { return (launchButton.InBoundingRect(x, y)); }

//        public void AdjustSpinMarker(int x, int y) { flightPath.SetSpinMarker(x, y); }

//        public void LaunchBall()
//        {
//            ballLaunched = true;
//            startTime = DateTime.Now;
//            secondsElapsed = 0;
//        }

//        public void SetFlightPath(int x, int y)
//        {
//            if (!flightPath.CalculatingSpin)
//            {
//                PlaceAimMarker(x, y);
//                settingSpin = true;
//                readyForLaunch = true;
//                secondsRemaining = roundTime;
//            }
//        }

//        protected void SetPosition(double x, double y) { dx = x; dy = y; }
//        protected void SetSize(int width, int height) { this.width = width; this.height = height; }

//        private void _Load(int x, int y, int width, int height)
//        {
//            flightPath.Load();
//            DebugConfigure(false);
//            this.width = width;
//            this.height = height;
//            dx = x;
//            dy = y;
//            dcenter = new PointD(dx + width / 2, dy + height / 2);
//            dstartPosition = new PointD(dx, dy);
//            //circ2.Load(dx, dy, (width / 2), 0, collisionPointCount);
//            circ2.Load(dx, dy, collisionPointSideLength, (width / 2), 0, collisionPointCount);
//            circ2.SetCircleColor(this.color);

//            PositionLaunchButton();
//        }
        

//        void Bounce3(int cpHitIdx)
//        {
//            string fnId = $"[{clsName}.Bounce]";
//            bounceCount++;

//            //fpAngle = (320 * Math.PI / 180);
//            timesIn90 = (int)(fpAngle / south);
//            toPrev90 = fpAngle - (timesIn90 * south);
//            toNext90 = ((timesIn90 + 1) * south) - fpAngle;
//            bounceAngle = 0;
//            angleAfterBounce = 0;

//            if (fpAngle > east && fpAngle < south)
//            {
//                if (cpHitIdx < circ2.MiddleCPIdx)
//                {
//                    bounceAngle = toPrev90 * 2;
//                    angleAfterBounce = (fpAngle + bounceAngle) % fullCircle;
//                }
//                if (cpHitIdx > circ2.MiddleCPIdx)
//                {
//                    bounceAngle = toPrev90 * 2;
//                    angleAfterBounce = (fpAngle - bounceAngle) % fullCircle;
//                }
//            }
//            if (fpAngle > south && fpAngle < west)
//            {
//                if (cpHitIdx < circ2.MiddleCPIdx)
//                {
//                    bounceAngle = toNext90 * 2;

//                    angleAfterBounce = (fpAngle + bounceAngle) % fullCircle;
//                }
//                if (cpHitIdx > circ2.MiddleCPIdx)
//                {
//                    bounceAngle = toPrev90 * 2;
//                    angleAfterBounce = (fpAngle - bounceAngle) % fullCircle;
//                }
//            }
//            if (fpAngle > west && fpAngle < north)
//            {
//                if (cpHitIdx < circ2.MiddleCPIdx)
//                {
//                    bounceAngle = toNext90 * 2;
//                    angleAfterBounce = (fpAngle + bounceAngle) % fullCircle;
//                }
//                if (cpHitIdx > circ2.MiddleCPIdx)
//                {
//                    bounceAngle = (toPrev90 * 2);
//                    angleAfterBounce = (fpAngle - bounceAngle) % fullCircle;
//                }
//            }
//            if (fpAngle > north && fpAngle < fullCircle)
//            {
//                if (cpHitIdx < circ2.MiddleCPIdx)
//                {
//                    bounceAngle = toNext90 * 2;
//                    angleAfterBounce = (fpAngle + bounceAngle) % fullCircle;
//                }
//                if (cpHitIdx > circ2.MiddleCPIdx)
//                {
//                    bounceAngle = toPrev90 * 2;
//                    angleAfterBounce = (fpAngle - bounceAngle) % fullCircle;
//                }

//            }

//            //DbgFuncs.AddStr($"{fnId} fpAngle: {fpAngle:N3} ({(fpAngle * 180 / Math.PI):N2})");
//            //DbgFuncs.AddStr($"timeIn90: {timesIn90}");
//            //DbgFuncs.AddStr($"{fnId} toNext90: {toNext90:N3} ({(toNext90 * 180 / Math.PI):N2})");
//            //DbgFuncs.AddStr($"{fnId} toPrev90: {toPrev90:N3} ({(toPrev90 * 180 / Math.PI):N2})");
//            //DbgFuncs.AddStr($"{fnId} bounceAngle: {bounceAngle:N3} ({(bounceAngle * 180 / Math.PI):N2})");
//            //DbgFuncs.AddStr($"{fnId} angleAfterBounce: {angleAfterBounce:N3} ({(angleAfterBounce * 180 / Math.PI):N2})");

//            fpAngle = angleAfterBounce;

//        }

//        private void DebugConfigure(bool debugValue = false)
//        {
//            //flightPath.ConnectMarkers = debugValue;
//            flightPath.DebugConfigure(debugValue);
//            circ2.DrawDbgTxt = DrawDbgTxt;
//        }
//        private void InitBallParts()
//        {
//            circ2 = new CollisionCircleD();
//            flightPath = new FlightPath();
//            launchButton = new Button01();
//        }

//        private void dbgDrawBounceAngle(Graphics g)
//        {
//            PointD bncEndPt = new PointD();
//            bncEndPt.X = dx + 20 * Math.Cos(calculatedBounceAngle);
//            bncEndPt.Y = dy + 20 * Math.Sin(calculatedBounceAngle);
//            g.DrawLine(Pens.Red, (float)dx, (float)dy, bncEndPt.fX, bncEndPt.fY);
//        }
        
//        private void PlaceAimMarker(int endMarkerX, int endMarkerY)
//        {
//            flightPath.PlaceStartMarker((int)dx, (int)dy);
//            flightPath.PlaceAimMarker(endMarkerX, endMarkerY);
//        }

//        private void SetInfoCpIdxHit(List<int> cpIdxHitList)
//        {
//            dbgtxtCpIdxHit = "";
//            if (cpIdxHitList != null)
//            {
//                for (int i = 0; i < cpIdxHitList.Count; i++)
//                {
//                    dbgtxtCpIdxHit += $"{cpIdxHitList[i]}";
//                    if (i < cpIdxHitList.Count - 1) { dbgtxtCpIdxHit += ","; }
//                }
//            }

//            if (PublicHitIdxList.Count > 0)
//            {
//                dbgtxtPublicHitIdxList = "";
//            }
//            for (int i = 0; i < PublicHitIdxList.Count; i++)
//            {
//                dbgtxtPublicHitIdxList += $"{PublicHitIdxList[i]}  ";
//            }

//        }
        
//        private void PositionLaunchButton()     // after gameScreenSize is set b.c it is used to place button
//        {
//            Point position = new Point();
//            Size size = new Size();
//            size.Width = 100;
//            size.Height = 40;
//            position.X = gameScreenSize.Width / 2 - size.Width / 2;
//            position.Y = gameScreenSize.Height - size.Height * 2 - 5;
//            launchButton.Load(position.X, position.Y, size.Width, size.Height, "Launch");
//        }
//        private void DrawBallLabel(Graphics g) // 0 refs as of 2019-10-26, might use later
//        {
//            string description = "Ball";
//            SizeF strSize = g.MeasureString(description, font);
//            PointF strPos = new Point();
//            strPos.X = (int)dcenter.X - (int)strSize.Width / 2;
//            strPos.Y = (int)dcenter.Y - (int)strSize.Height / 2;
//            g.DrawString(description, font, Brushes.Black, strPos);
//        }
//        private void AddUpdateDebugMsgs()
//        {
//            string fnId = $"[{clsName}.AddUpdateDebugMsgs]";
//            if (DrawDbgTxt)
//            {
//                //DbgFuncs.AddStr($"game window size: {{ {gameScreenSize.Width}, {gameScreenSize.Height} }}");
//                DbgFuncs.AddStr($"{fnId} ballLaunched: {ballLaunched}");
//                DbgFuncs.AddStr($"{fnId} angle(degrees) from flightpath: {(fpAngle * 180 / Math.PI):N2}");
//                DbgFuncs.AddStr($"{fnId} drift(degrees) from flightpath: {fpDrift * 180 / Math.PI:N2}");
//                DbgFuncs.AddStr($"{fnId} driftFactor(degrees): {driftFactor * 180 / Math.PI:N2}");
//                DbgFuncs.AddStr($"{fnId} speed(of ball): {speed:N2}");
//                DbgFuncs.AddStr($"{fnId} angle(of ball degrees): {(calculatedAngle * 180 / Math.PI):N2}");
//                DbgFuncs.AddStr($"{fnId} flightTime: {flightTime.ToString(@"mm\:ss\:fff")}");
//                DbgFuncs.AddStr($"{fnId} ydriftModifier: {timedriftModifier:N2}");
//                DbgFuncs.AddStr($"{fnId} secondsRemaining: {(secondsRemaining):N3}");
//                //DbgFuncs.AddStr($"east: {east:N3} ({(east * 180 / Math.PI)})");
//                //DbgFuncs.AddStr($"south: {south:N3} ({(south * 180 / Math.PI)})");
//                //DbgFuncs.AddStr($"west: {west:N3} ({(west * 180 / Math.PI)})");
//                //DbgFuncs.AddStr($"north: {north:N3} ({(north * 180 / Math.PI)})");
//            }
//        }
//    }
//}


//#region Bounce 3 versions

//#region bounce3 v3
////void Bounce3(List<int> cpIdxHitList)
////{
////    string fnId = $"[{clsName}.Bounce]";
////    bounceCount++;

////    //cpHitIdx = circ2.CollisionPointHit();
////    //cpHitIdx = circ2.CollisionPointHit2();
////    //fpAngle = (320 * Math.PI / 180);
////    timesIn90 = (int)(fpAngle / south);
////    toPrev90 = fpAngle - (timesIn90 * south);
////    toNext90 = ((timesIn90 + 1) * south) - fpAngle;
////    bounceAngle = 0;
////    angleAfterBounce = 0;

////    if (fpAngle > east && fpAngle < south)
////    {
////        if (cpHitIdx < circ2.MiddleCPIdx)
////        {
////            bounceAngle = toPrev90 * 2;
////            angleAfterBounce = (fpAngle + bounceAngle) % fullCircle;
////        }
////        if (cpHitIdx > circ2.MiddleCPIdx)
////        {
////            bounceAngle = toPrev90 * 2;
////            angleAfterBounce = (fpAngle - bounceAngle) % fullCircle;
////        }
////    }
////    if (fpAngle > south && fpAngle < west)
////    {
////        if (cpHitIdx < circ2.MiddleCPIdx)
////        {
////            bounceAngle = toNext90 * 2;

////            angleAfterBounce = (fpAngle + bounceAngle) % fullCircle;
////        }
////        if (cpHitIdx > circ2.MiddleCPIdx)
////        {
////            bounceAngle = toPrev90 * 2;
////            angleAfterBounce = (fpAngle - bounceAngle) % fullCircle;
////        }
////    }
////    if (fpAngle > west && fpAngle < north)
////    {
////        if (cpHitIdx < circ2.MiddleCPIdx)
////        {
////            bounceAngle = toNext90 * 2;
////            angleAfterBounce = (fpAngle + bounceAngle) % fullCircle;
////        }
////        if (cpHitIdx > circ2.MiddleCPIdx)
////        {
////            bounceAngle = (toPrev90 * 2);
////            angleAfterBounce = (fpAngle - bounceAngle) % fullCircle;
////        }
////    }
////    if (fpAngle > north && fpAngle < fullCircle)
////    {
////        if (cpHitIdx < circ2.MiddleCPIdx)
////        {
////            bounceAngle = toNext90 * 2;
////            angleAfterBounce = (fpAngle + bounceAngle) % fullCircle;
////        }
////        if (cpHitIdx > circ2.MiddleCPIdx)
////        {
////            bounceAngle = toPrev90 * 2;
////            angleAfterBounce = (fpAngle - bounceAngle) % fullCircle;
////        }

////    }

////    //DbgFuncs.AddStr($"{fnId} fpAngle: {fpAngle:N3} ({(fpAngle * 180 / Math.PI):N2})");
////    //DbgFuncs.AddStr($"timeIn90: {timesIn90}");
////    //DbgFuncs.AddStr($"{fnId} toNext90: {toNext90:N3} ({(toNext90 * 180 / Math.PI):N2})");
////    //DbgFuncs.AddStr($"{fnId} toPrev90: {toPrev90:N3} ({(toPrev90 * 180 / Math.PI):N2})");
////    //DbgFuncs.AddStr($"{fnId} bounceAngle: {bounceAngle:N3} ({(bounceAngle * 180 / Math.PI):N2})");
////    //DbgFuncs.AddStr($"{fnId} angleAfterBounce: {angleAfterBounce:N3} ({(angleAfterBounce * 180 / Math.PI):N2})");

////    fpAngle = angleAfterBounce;

////}
//#endregion bounce3 v3

//#region bounce3 v2
////void Bounce3(int cpHitIdx)
////{
////    string fnId = $"[{clsName}.Bounce]";
////    bounceCount++;

////    //fpAngle = (320 * Math.PI / 180);
////    timesIn90 = (int)(fpAngle / south);
////    toPrev90 = fpAngle - (timesIn90 * south);
////    toNext90 = ((timesIn90 + 1) * south) - fpAngle;
////    bounceAngle = 0;
////    angleAfterBounce = 0;

////    if (fpAngle > east && fpAngle < south)
////    {
////        if (cpHitIdx < circ2.MiddleCPIdx)
////        {
////            bounceAngle = toPrev90 * 2;
////            angleAfterBounce = (fpAngle + bounceAngle) % fullCircle;
////        }
////        if (cpHitIdx > circ2.MiddleCPIdx)
////        {
////            bounceAngle = toPrev90 * 2;
////            angleAfterBounce = (fpAngle - bounceAngle) % fullCircle;
////        }
////    }
////    if (fpAngle > south && fpAngle < west)
////    {
////        if (cpHitIdx < circ2.MiddleCPIdx)
////        {
////            bounceAngle = toNext90 * 2;

////            angleAfterBounce = (fpAngle + bounceAngle) % fullCircle;
////        }
////        if (cpHitIdx > circ2.MiddleCPIdx)
////        {
////            bounceAngle = toPrev90 * 2;
////            angleAfterBounce = (fpAngle - bounceAngle) % fullCircle;
////        }
////    }
////    if (fpAngle > west && fpAngle < north)
////    {
////        if (cpHitIdx < circ2.MiddleCPIdx)
////        {
////            bounceAngle = toNext90 * 2;
////            angleAfterBounce = (fpAngle + bounceAngle) % fullCircle;
////        }
////        if (cpHitIdx > circ2.MiddleCPIdx)
////        {
////            bounceAngle = (toPrev90 * 2);
////            angleAfterBounce = (fpAngle - bounceAngle) % fullCircle;
////        }
////    }
////    if (fpAngle > north && fpAngle < fullCircle)
////    {
////        if (cpHitIdx < circ2.MiddleCPIdx)
////        {
////            bounceAngle = toNext90 * 2;
////            angleAfterBounce = (fpAngle + bounceAngle) % fullCircle;
////        }
////        if (cpHitIdx > circ2.MiddleCPIdx)
////        {
////            bounceAngle = toPrev90 * 2;
////            angleAfterBounce = (fpAngle - bounceAngle) % fullCircle;
////        }

////    }

////    //DbgFuncs.AddStr($"{fnId} fpAngle: {fpAngle:N3} ({(fpAngle * 180 / Math.PI):N2})");
////    //DbgFuncs.AddStr($"timeIn90: {timesIn90}");
////    //DbgFuncs.AddStr($"{fnId} toNext90: {toNext90:N3} ({(toNext90 * 180 / Math.PI):N2})");
////    //DbgFuncs.AddStr($"{fnId} toPrev90: {toPrev90:N3} ({(toPrev90 * 180 / Math.PI):N2})");
////    //DbgFuncs.AddStr($"{fnId} bounceAngle: {bounceAngle:N3} ({(bounceAngle * 180 / Math.PI):N2})");
////    //DbgFuncs.AddStr($"{fnId} angleAfterBounce: {angleAfterBounce:N3} ({(angleAfterBounce * 180 / Math.PI):N2})");

////    fpAngle = angleAfterBounce;

////}
//#endregion bounce3 v2

//#region bounce3 v1
////void Bounce3()
////{
////    string fnId = $"[GameBall.Bounce]";
////    bounceCount++;

////    //cpHitIdx = circ2.CollisionPointHit();
////    //cpHitIdx = circ2.CollisionPointHit2();
////    //fpAngle = (320 * Math.PI / 180);
////    timesIn90 = (int)(fpAngle / south);
////    toPrev90 = fpAngle - (timesIn90 * south);
////    toNext90 = ((timesIn90 + 1) * south) - fpAngle;
////    bounceAngle = 0;
////    angleAfterBounce = 0;

////    if (fpAngle > east && fpAngle < south)
////    {
////        if (cpHitIdx < circ2.MiddleCPIdx)
////        {
////            bounceAngle = toPrev90 * 2;
////            angleAfterBounce = (fpAngle + bounceAngle) % fullCircle;
////        }
////        if (cpHitIdx > circ2.MiddleCPIdx)
////        {
////            bounceAngle = toPrev90 * 2;
////            angleAfterBounce = (fpAngle - bounceAngle) % fullCircle;
////        }
////    }
////    if (fpAngle > south && fpAngle < west)
////    {
////        if (cpHitIdx < circ2.MiddleCPIdx)
////        {
////            bounceAngle = toNext90 * 2;

////            angleAfterBounce = (fpAngle + bounceAngle) % fullCircle;
////        }
////        if (cpHitIdx > circ2.MiddleCPIdx)
////        {
////            bounceAngle = toPrev90 * 2;
////            angleAfterBounce = (fpAngle - bounceAngle) % fullCircle;
////        }
////    }
////    if (fpAngle > west && fpAngle < north)
////    {
////        if (cpHitIdx < circ2.MiddleCPIdx)
////        {
////            bounceAngle = toNext90 * 2;
////            angleAfterBounce = (fpAngle + bounceAngle) % fullCircle;
////        }
////        if (cpHitIdx > circ2.MiddleCPIdx)
////        {
////            bounceAngle = (toPrev90 * 2);
////            angleAfterBounce = (fpAngle - bounceAngle) % fullCircle;
////        }
////    }
////    if (fpAngle > north && fpAngle < fullCircle)
////    {
////        if (cpHitIdx < circ2.MiddleCPIdx)
////        {
////            bounceAngle = toNext90 * 2;
////            angleAfterBounce = (fpAngle + bounceAngle) % fullCircle;
////        }
////        if (cpHitIdx > circ2.MiddleCPIdx)
////        {
////            bounceAngle = toPrev90 * 2;
////            angleAfterBounce = (fpAngle - bounceAngle) % fullCircle;
////        }

////    }

////    //DbgFuncs.AddStr($"{fnId} fpAngle: {fpAngle:N3} ({(fpAngle * 180 / Math.PI):N2})");
////    //DbgFuncs.AddStr($"timeIn90: {timesIn90}");
////    //DbgFuncs.AddStr($"{fnId} toNext90: {toNext90:N3} ({(toNext90 * 180 / Math.PI):N2})");
////    //DbgFuncs.AddStr($"{fnId} toPrev90: {toPrev90:N3} ({(toPrev90 * 180 / Math.PI):N2})");
////    //DbgFuncs.AddStr($"{fnId} bounceAngle: {bounceAngle:N3} ({(bounceAngle * 180 / Math.PI):N2})");
////    //DbgFuncs.AddStr($"{fnId} angleAfterBounce: {angleAfterBounce:N3} ({(angleAfterBounce * 180 / Math.PI):N2})");

////    fpAngle = angleAfterBounce;

////}
//#endregion bounce3 v1
//#endregion Bounce 3 versions

//#region previous bounce attempts
////void Bounce()
////{
////    string fnId = $"[GameBall.Bounce]";
////    bounceCount++;
////    cpHitIdx = circ2.CollisionPointHit();
////    if (fpAngle > Math.PI && fpAngle < ((3 * Math.PI) / 2))
////    {
////        outerAngle = fpAngle - Math.PI;
////    }
////    calculatedBounceAngle = fpAngle + (((Math.PI / 2) - outerAngle) * 2);
////    fpAngle = calculatedBounceAngle;
////}
////void Bounce2()
////{
////    string fnId = $"[GameBall.Bounce]";
////    bounceCount++;
////    cpHitIdx = circ2.CollisionPointHit();

////    if (cpHitIdx < CollisionPointList.Count / 2)
////    {
////        outerAngle = fpAngle - Math.PI;
////        calculatedBounceAngle = fpAngle + (((Math.PI / 2) - outerAngle) * 2);
////        goto SetfpAngle;
////    }
////    if (cpHitIdx > CollisionPointList.Count / 2)
////    {
////        outerAngle = fpAngle - (Math.PI);
////        //calculatedBounceAngle = fpAngle - (((Math.PI / 2) - outerAngle) * 2);
////        calculatedBounceAngle = fpAngle - ((outerAngle * 2));
////        goto SetfpAngle;
////    }
////SetfpAngle:
////    fpAngle = calculatedBounceAngle;
////}
//#endregion previous bounce attempts

//#region snippit - first version of calculating degress from previous 90 and to next 90
////// building for calculating bounce angle
////if (fpAngle > east && fpAngle < south)
////{
////    toNext90 = south - fpAngle;
////    toPrev90 = fpAngle;
////}
////if (fpAngle > south && fpAngle < west)
////{
////    toNext90 = west - fpAngle;
////    toPrev90 = fpAngle - south;
////}
////if (fpAngle > west && fpAngle < north)
////{
////    toNext90 = north - fpAngle;
////    toPrev90 = fpAngle - west;
////}
////if (fpAngle > north && fpAngle < 2 * Math.PI)
////{
////    toNext90 = (2 * Math.PI) - fpAngle;
////    toPrev90 = fpAngle - north;
////}
//#endregion snippit - first version of calculating degress from previous 90 and to next 90

//#region original version using int

////flightTime = DateTime.Now - startTime;
////totalMs = flightTime.TotalMilliseconds;
////secondsElapsed = flightTime.TotalSeconds;
////secondsRemaining = roundTime - secondsElapsed;
////timedriftModifier = totalMs / 250;

////// starts looping if angle gets too high
////calculatedAngle = fpAngle - (driftFactor * timedriftModifier);

////xdub = x + speed * Math.Cos(calculatedAngle) * 180 / Math.PI;
////ydub = y + speed * Math.Sin(calculatedAngle) * 180 / Math.PI;

////x = (int)xdub;
////y = (int)ydub;
////lastPosx = x;
////lastPosy = y;
////if (secondsRemaining <= 0)
////{
////    secondsRemaining = 0;
////    Reset();
////}
////if (y <= 0 || y >= gameScreenSize.Height)
////{
////    Reset();
////}
////if (x <= 0 || x >= gameScreenSize.Width)
////{
////    Reset();
////}

//#endregion original version using int

//#region Update - origianl version not using trajectory class
////public void Update()
////{
////    DbgFuncs.AddStr($"[GameBall.Update] ballLaunched: {ballLaunched}");

////    if (!ballLaunched)
////    {
////        fpAngle = flightPath.Angle;
////        fpDrift = flightPath.Drift;

////        #region adjusting fpAngle to how I want it
////        //fpAngle = (fpAngle + (90 * Math.PI / 180)) * -1;
////        //fpDrift = (fpDrift + (90 * Math.PI / 180)) * -1;

////        fpAngle = (fpAngle + (Math.PI / 2)) * -1;
////        fpDrift = (fpDrift + (Math.PI / 2)) * -1;
////        #endregion adjusting fpAngle to how I want it

////        driftFactor = (fpAngle - fpDrift) * drifthardness;


////    }

////    DbgFuncs.AddStr($"[GameBall.Update] angle(degrees) from flightpath: {360 + (fpAngle * 180 / Math.PI):N2}");
////    DbgFuncs.AddStr($"[GameBall.Update] drift(degrees) from flightpath: {fpDrift * 180 / Math.PI:N2}");
////    DbgFuncs.AddStr($"[GameBall.Update] driftFactor(degrees): {driftFactor * 180 / Math.PI:N2}");

////    DbgFuncs.AddStr($"[GameBall.Update] speed(of ball): {speed:N2}");
////    DbgFuncs.AddStr($"[GameBall.Update] angle(of ball): {(calculatedAngle * 180 / Math.PI) % 360:N2}");
////    DbgFuncs.AddStr($"[GameBall.Update] flightTime: {flightTime}");
////    DbgFuncs.AddStr($"[GameBall.Update] ydriftModifier: {timedriftModifier:N2}");
////    //DbgFuncs.AddStr($"[GameBall.Update] secondsElapsed: {secondsElapsed}");
////    //DbgFuncs.AddStr($"[GameBall.Update] secondsRemaining: {(roundTime - secondsElapsed):N2}");
////    DbgFuncs.AddStr($"[GameBall.Update] secondsRemaining: {(secondsRemaining):N2}");
////    //DbgFuncs.AddStr($"[GameBall.Update] totalMs: {totalMs}");
////    DbgFuncs.AddStr($"ball pos: {{ {lastPosx}, {lastPosy} }}");
////    DbgFuncs.AddStr($"game window size: {{ {gameScreenSize.Width}, {gameScreenSize.Height} }}");


////    if (ballLaunched)
////    {

////        flightTime = DateTime.Now - startTime;
////        totalMs = flightTime.TotalMilliseconds;
////        secondsElapsed = flightTime.TotalSeconds;
////        secondsRemaining = roundTime - secondsElapsed;
////        timedriftModifier = totalMs / 250;

////        #region accidently discovered effects
////        // up and down on y axis
////        //xdub = x + speed * Math.Cos(fpAngle) * 180 / Math.PI;
////        //ydub = y + speed * Math.Sin(fpAngle - (driftFactor * ydriftModifier)) * 180 / Math.PI;

////        #region crazySnake
////        //double xFpAngle, yFpAngle;
////        //double xFpDrift, yFpDirft;
////        //double xDriftFactor, yDriftFactor;
////        //driftFactor = (fpAngle - fpDrift) * drifthardness;

////        //xDriftFactor = Math.Cos(driftFactor);
////        //yDriftFactor = Math.Sin(driftFactor);

////        //xFpAngle = Math.Cos(fpAngle);
////        //yFpAngle = Math.Sin(fpAngle);

////        //xFpDrift = Math.Cos(fpDrift);
////        //yFpDirft = Math.Sin(fpDrift);
////        //xdub = x + speed * Math.Cos(xFpAngle -(xDriftFactor * timedriftModifier)) * 180 / Math.PI;
////        //ydub = y + speed * Math.Sin(yFpDirft - (yDriftFactor * timedriftModifier)) * 180 / Math.PI;
////        #endregion crazySnake

////        #region crazySnake 2
////        //double xFpAngle, yFpAngle;
////        //double xFpDrift, yFpDirft;
////        //double xDriftFactor, yDriftFactor;
////        //xDriftFactor = Math.Cos(driftFactor);
////        //yDriftFactor = Math.Sin(driftFactor);

////        //xFpAngle = Math.Cos(fpAngle);
////        //yFpAngle = Math.Sin(fpAngle);

////        //xFpDrift = Math.Cos(fpDrift);
////        //yFpDirft = Math.Sin(fpDrift);
////        //xdub = x + speed * Math.Cos(xFpAngle - (xDriftFactor * timedriftModifier)) * 180 / Math.PI;
////        //ydub = y + speed * Math.Sin(yFpAngle - (yDriftFactor * timedriftModifier)) * 180 / Math.PI;
////        #endregion crazySnake 2

////        #region goes into spiral
////        //double driftDegradetion = 0, driftDegradeRate = 0.01;
////        //if(driftFactor > 0)
////        //{
////        //    driftDegradetion += driftDegradeRate;
////        //    //driftFactor -= driftDegradeRate;
////        //}
////        //else if(driftFactor < 0)
////        //{
////        //    driftDegradetion -= driftDegradeRate;
////        //    //driftFactor += driftDegradeRate;
////        //}
////        //calculatedAngle = fpAngle - ((driftFactor - driftDegradetion) * timedriftModifier);
////        #endregion goes into spiral

////        #endregion accidently discovered effects

////        // starts looping if angle gets too high
////        calculatedAngle = fpAngle - (driftFactor * timedriftModifier);

////        xdub = x + speed * Math.Cos(calculatedAngle) * 180 / Math.PI;
////        ydub = y + speed * Math.Sin(calculatedAngle) * 180 / Math.PI;

////        x = (int)xdub;
////        y = (int)ydub;
////        lastPosx = x;
////        lastPosy = y;
////        if (secondsRemaining <= 0)
////        {
////            secondsRemaining = 0;
////            Reset();
////        }
////        if (y <= 0 || y >= gameScreenSize.Height)
////        {
////            Reset();
////        }
////        if (x <= 0 || x >= gameScreenSize.Width)
////        {
////            Reset();
////        }
////    }

////}
//#endregion Update - origianl version not using trajectory class

//#region DrawDebugInfo - Old logic to draw debug info to game screen.
////void DrawDebugInfo(Graphics g)
////{
////    Point pos = new Point(10, 100);
////    SizeF strSize = new SizeF();
////    for(int i = 0; i < dbgStrList.Count; i++)
////    {
////        strSize = g.MeasureString(dbgStrList[i], font);
////        g.DrawString(dbgStrList[i], font, Brushes.Black, pos);
////        pos.Y += (int)(strSize.Height + 0.49);

////    }
////    dbgStrList = new List<string>();
////}
//#endregion DrawDebugInfo - Old logic to draw debug info to game screen.