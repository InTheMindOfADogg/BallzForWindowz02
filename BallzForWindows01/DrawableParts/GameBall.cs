using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

using BallzForWindows01.GamePhysicsParts;

namespace BallzForWindows01.DrawableParts
{
    class GameBall : DrawableObject
    {

        #region properties
        public Size GameScreenSize { get { return gameScreenSize; } set { gameScreenSize = value; } }
        public bool ReadyForLaunch { get { return readyForLaunch; } }
        public bool PlacingAim { get { return placingAim; } set { placingAim = value; } }
        public bool SettingSpin { get { return settingSpin; } }
        public bool PlacingSpinRect { get { return placingSpinRect; } set { placingSpinRect = value; } }
        public bool BallLaunched { get { return ballLaunched; } }
        #endregion


        Point startPosition;
        Point center;
        //Color color;

        Font font;
        Color fontColor = Color.Black;
        int fontSize = 20;
        string fontFamily = "Arial";

        FlightPath flightPath;
        Button01 launchButton;
        Trajectory traj;

        Size gameScreenSize = new Size();

        bool placingAim = false;
        bool settingSpin = false;
        bool readyForLaunch = false;    // same as setting spin, might need to adjust later
        bool placingSpinRect = false;
        bool ballLaunched = false;


        public GameBall(Size gameScreenSize)
        {
            this.gameScreenSize = gameScreenSize;
            startPosition = new Point(0, 0);
            SetPosition(0, 0);
            SetSize(15, 15);
            SetColor(255, 0, 0, 0);

            //color = Color.FromArgb(alpha, red, green, blue);

            font = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);

            //center = new Point(x - width / 2, y - height / 2);
            placingAim = false;
            settingSpin = false;
            readyForLaunch = false;    // same as setting spin, might need to adjust later
            placingSpinRect = false;
            ballLaunched = false;
            InitAndLoadBallParts();
        }
        private void InitAndLoadBallParts()
        {
            flightPath = new FlightPath();
            flightPath.Load();
            launchButton = new Button01();
            traj = new Trajectory();
        }
        public void Load(int x, int y) { _Load(x, y, width, height); }
        public void Load(int x, int y, int width, int height) { _Load(x, y, width, height); }
        private void _Load(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            center = new Point(x + width / 2, y + height / 2);
            startPosition = new Point(x, y);
            traj.Load(x, y);
            PositionLaunchButton();
        }
        private void PositionLaunchButton()     // place in load 
        {
            Point position = new Point();
            Size size = new Size();
            size.Width = 100;
            size.Height = 40;
            position.X = gameScreenSize.Width / 2 - size.Width / 2;
            position.Y = gameScreenSize.Height - size.Height * 2 - 5;
            launchButton.Load(position.X, position.Y, size.Width, size.Height);
        }



        double speed = 0.1;

        DateTime startTime, endTime;
        TimeSpan flightTime = TimeSpan.Zero;
        double fpAngle, fpDrift;

        double timedriftModifier;
        double totalMs = 0;
        double driftFactor = 0;

        double xdub, ydub;
        double calculatedAngle = 0;

        double secondsElapsed = 0;
        double secondsRemaining = 0;
        double roundTime = 10;
        double drifthardness = 0.5;

        int lastPosx, lastPosy;
        public void Update()
        {
            DbgFuncs.AddStr($"[GameBall.Update] ballLaunched: {ballLaunched}");

            if (!ballLaunched)
            {
                fpAngle = flightPath.Angle;
                fpDrift = flightPath.Drift;

                #region adjusting fpAngle to how I want it
                //fpAngle = (fpAngle + (90 * Math.PI / 180)) * -1;
                //fpDrift = (fpDrift + (90 * Math.PI / 180)) * -1;
                fpAngle = (fpAngle + (Math.PI / 2)) * -1;
                fpDrift = (fpDrift + (Math.PI / 2)) * -1;
                #endregion adjusting fpAngle to how I want it

                driftFactor = (fpAngle - fpDrift) * drifthardness;


            }

            DbgFuncs.AddStr($"[GameBall.Update] angle(degrees) from flightpath: {360 + (fpAngle * 180 / Math.PI):N2}");
            DbgFuncs.AddStr($"[GameBall.Update] drift(degrees) from flightpath: {fpDrift * 180 / Math.PI:N2}");
            DbgFuncs.AddStr($"[GameBall.Update] driftFactor(degrees): {driftFactor * 180 / Math.PI:N2}");

            DbgFuncs.AddStr($"[GameBall.Update] speed(of ball): {speed:N2}");
            DbgFuncs.AddStr($"[GameBall.Update] angle(of ball): {(calculatedAngle * 180 / Math.PI) % 360:N2}");
            DbgFuncs.AddStr($"[GameBall.Update] flightTime: {flightTime}");
            DbgFuncs.AddStr($"[GameBall.Update] ydriftModifier: {timedriftModifier:N2}");
            //DbgFuncs.AddStr($"[GameBall.Update] secondsElapsed: {secondsElapsed}");
            //DbgFuncs.AddStr($"[GameBall.Update] secondsRemaining: {(roundTime - secondsElapsed):N2}");
            DbgFuncs.AddStr($"[GameBall.Update] secondsRemaining: {(secondsRemaining):N2}");
            //DbgFuncs.AddStr($"[GameBall.Update] totalMs: {totalMs}");
            DbgFuncs.AddStr($"ball pos: {{ {lastPosx}, {lastPosy} }}");
            DbgFuncs.AddStr($"game window size: {{ {gameScreenSize.Width}, {gameScreenSize.Height} }}");


            if (ballLaunched)
            {

                flightTime = DateTime.Now - startTime;
                totalMs = flightTime.TotalMilliseconds;
                secondsElapsed = flightTime.TotalSeconds;
                secondsRemaining = roundTime - secondsElapsed;
                timedriftModifier = totalMs / 250;

                #region accidently discovered effects
                // up and down on y axis
                //xdub = x + speed * Math.Cos(fpAngle) * 180 / Math.PI;
                //ydub = y + speed * Math.Sin(fpAngle - (driftFactor * ydriftModifier)) * 180 / Math.PI;

                #region crazySnake
                //double xFpAngle, yFpAngle;
                //double xFpDrift, yFpDirft;
                //double xDriftFactor, yDriftFactor;
                //driftFactor = (fpAngle - fpDrift) * drifthardness;

                //xDriftFactor = Math.Cos(driftFactor);
                //yDriftFactor = Math.Sin(driftFactor);

                //xFpAngle = Math.Cos(fpAngle);
                //yFpAngle = Math.Sin(fpAngle);

                //xFpDrift = Math.Cos(fpDrift);
                //yFpDirft = Math.Sin(fpDrift);
                //xdub = x + speed * Math.Cos(xFpAngle -(xDriftFactor * timedriftModifier)) * 180 / Math.PI;
                //ydub = y + speed * Math.Sin(yFpDirft - (yDriftFactor * timedriftModifier)) * 180 / Math.PI;
                #endregion crazySnake

                #region crazySnake 2
                //double xFpAngle, yFpAngle;
                //double xFpDrift, yFpDirft;
                //double xDriftFactor, yDriftFactor;
                //xDriftFactor = Math.Cos(driftFactor);
                //yDriftFactor = Math.Sin(driftFactor);

                //xFpAngle = Math.Cos(fpAngle);
                //yFpAngle = Math.Sin(fpAngle);

                //xFpDrift = Math.Cos(fpDrift);
                //yFpDirft = Math.Sin(fpDrift);
                //xdub = x + speed * Math.Cos(xFpAngle - (xDriftFactor * timedriftModifier)) * 180 / Math.PI;
                //ydub = y + speed * Math.Sin(yFpAngle - (yDriftFactor * timedriftModifier)) * 180 / Math.PI;
                #endregion crazySnake 2

                #region goes into spiral
                //double driftDegradetion = 0, driftDegradeRate = 0.01;
                //if(driftFactor > 0)
                //{
                //    driftDegradetion += driftDegradeRate;
                //    //driftFactor -= driftDegradeRate;
                //}
                //else if(driftFactor < 0)
                //{
                //    driftDegradetion -= driftDegradeRate;
                //    //driftFactor += driftDegradeRate;
                //}
                //calculatedAngle = fpAngle - ((driftFactor - driftDegradetion) * timedriftModifier);
                #endregion goes into spiral

                #endregion accidently discovered effects

                // starts looping if angle gets too high
                calculatedAngle = fpAngle - (driftFactor * timedriftModifier);

                xdub = x + speed * Math.Cos(calculatedAngle) * 180 / Math.PI;
                ydub = y + speed * Math.Sin(calculatedAngle) * 180 / Math.PI;

                x = (int)xdub;
                y = (int)ydub;
                lastPosx = x;
                lastPosy = y;
                if (secondsRemaining <= 0)
                {
                    secondsRemaining = 0;
                    Reset();
                }
                if (y <= 0 || y >= gameScreenSize.Height)
                {
                    Reset();
                }
                if (x <= 0 || x >= gameScreenSize.Width)
                {
                    Reset();
                }
            }

        }
        public void LaunchBall()
        {
            ballLaunched = true;
            startTime = DateTime.Now;
            secondsElapsed = 0;
        }

        public void SetFlightPath(int x, int y)
        {
            if (!flightPath.CalculatingSpin)
            {
                PlaceAimMarker(x, y);
                settingSpin = true;
                readyForLaunch = true;
                secondsRemaining = roundTime;
            }
        }

        public bool IsInSpinRect(int x, int y) { if (flightPath.IsInBoundingRect(x, y)) { return true; } else { return false; } }
        public bool IsInLaunchButtonRect(int x, int y) { if (launchButton.IsInBoundingRect(x, y)) { return true; } else { return false; } }
        public void AdjustSpinMarker(int x, int y) { flightPath.AdjustSpinMarker(x, y); }
        private void PlaceAimMarker(int endMarkerX, int endMarkerY)
        {
            flightPath.PlaceStartMarker(this.x, this.y);
            flightPath.PlaceEndMarker(endMarkerX, endMarkerY);
            traj.SetEndPoint(endMarkerX, endMarkerY);
            //settingSpin = true;
        }




        public void Draw(Graphics g)
        {
            //DrawBallLabel(g);

            SolidBrush sb = new SolidBrush(color);

            launchButton.Draw(g);
            flightPath.Draw(g);
            g.FillEllipse(sb, x - width / 2, y - height / 2, width, height);
            g.DrawEllipse(Pens.Red, x - width / 2, y - height / 2, width, height);

            traj.Draw(g);

            sb.Dispose();
        }
        void DrawBallLabel(Graphics g)
        {
            string description = "Ball";
            SizeF strSize = g.MeasureString(description, font);
            Point strPos = new Point();
            strPos.X = (int)center.X - (int)strSize.Width / 2;
            strPos.Y = (int)center.Y - (int)strSize.Height / 2;
            g.DrawString(description, font, Brushes.Black, strPos);
        }

        public void Reset()
        {
            ballLaunched = false;
            endTime = DateTime.Now; // not using at the moment, but might use later 2019-10-12.
            readyForLaunch = false;
            x = startPosition.X;
            y = startPosition.Y;
            flightPath.Reset();

        }

        public void CleanUp()
        {
            //timer.Close();
            //timer.Dispose();
        }

    }
}


#region DrawDebugInfo - Old logic to draw debug info to game screen.
//void DrawDebugInfo(Graphics g)
//{
//    Point pos = new Point(10, 100);
//    SizeF strSize = new SizeF();
//    for(int i = 0; i < dbgStrList.Count; i++)
//    {
//        strSize = g.MeasureString(dbgStrList[i], font);
//        g.DrawString(dbgStrList[i], font, Brushes.Black, pos);
//        pos.Y += (int)(strSize.Height + 0.49);

//    }
//    dbgStrList = new List<string>();
//}
#endregion DrawDebugInfo - Old logic to draw debug info to game screen.