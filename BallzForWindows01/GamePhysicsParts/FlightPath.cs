using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;



namespace BallzForWindows01.GamePhysicsParts
{
    using static AssistFunctions;
    using DrawableParts;

    class FlightPath
    {
        XMarker originMarker;
        XMarker aimMarker;
        XMarker spinMarker;

        Trajectory aimTraj;
        Trajectory spinTraj;

        //Trajectory2 aimTraj;
        //Trajectory2 spinTraj;

        Color lineColor = Color.FromArgb(255, 255, 0, 0);

        bool connectMarkers = false;
        bool calculateSpin = false;

        double angle = 0;
        double drift = 0;


        public bool ConnectMarkers { get { return connectMarkers; } set { connectMarkers = value; } }
        public bool CalculatingSpin { get { return calculateSpin; } }
        public double Angle { get { return angle; } set { angle = value; } }
        public double Drift { get { return drift; } set { drift = value; } }

        public FlightPath()
        {
            _Init();
        }
        public void Load() { }
        public void Draw(Graphics g, bool render = true)
        {
            _Draw(g, render);
        }
        public void Reset()
        {
            _Reset();
        }

        public void PlaceStartMarker(int x, int y)
        {
            originMarker.Place(x, y);
            aimTraj.Load(x, y);
            spinTraj.Load(x, y);
        }

        public void PlaceAimMarker(int x, int y)
        {
            aimMarker.Place(x, y);
            aimTraj.SetEndPoint(x, y);
            calculateSpin = true;
            int spinX = (originMarker.X + aimMarker.X) / 2;
            int spinY = (originMarker.Y + aimMarker.Y) / 2;
            SetSpinMarker(spinX, spinY);
        }

        public void SetSpinMarker(int x, int y)
        {
            if (!spinMarker.IsPlaced)
            {
                spinMarker.Place(x, y);
                spinTraj.SetEndPoint(x, y);
                spinMarker.ShowClickRectangle = true;
            }
            else
            {
                spinMarker.AdjustPosition(x, y);
                spinTraj.SetEndPoint(x, y);
            }
            AddSpin();
        }


        public void DebugConfigure(bool debugValue = false)
        {
            connectMarkers = debugValue;
            aimTraj.DebugConfigure(debugValue);
            spinTraj.DebugConfigure(debugValue);
            //spinMarker.ShowXMarker = false;

            //aimTraj.ShowDebugLine = debugValue;
            //spinTraj.ShowDebugLine = debugValue;

            //aimMarker.ShowClickRectangle = debugValue;
            //spinMarker.ShowClickRectangle = debugValue;
        }



        public bool IsInBoundingRect(int mPosX, int mPosY) { return (spinMarker.IsInBoundingRect(mPosX, mPosY)); }



        

        void _Init()
        {
            originMarker = new XMarker();
            originMarker.DrawColor = Color.FromArgb(125, Color.Red);
            aimMarker = new XMarker();
            aimMarker.DrawColor = Color.FromArgb(125, Color.Red);
            spinMarker = new XMarker();
            spinMarker.DrawColor = Color.FromArgb(125, Color.Green);
            //aimTraj = new Trajectory("aimTraj");
            //spinTraj = new Trajectory("spinTraj");

            aimTraj = new Trajectory("aimTraj");
            spinTraj = new Trajectory("spinTraj");
        }

        void _Draw(Graphics g, bool render = true)
        {
            if (!render) return;
            if (originMarker.IsPlaced && aimMarker.IsPlaced)
            {
                //originMarker.Draw(g);
                spinMarker.Draw(g);
                aimMarker.Draw(g);
                spinTraj.Draw(g);
                // overload for Trajectory (original) draw to set position for debug text, split out in Trajectory2
                aimTraj.Draw(g, 450, 20);

                // for Trajectory2 drawing. split draw debug info into seperate function
                //aimTraj.Draw(g);
                //aimTraj.DebugDraw(g, 450, 20);

            }
            if (connectMarkers)
            {
                Pen p = new Pen(lineColor, 5);
                DrawConnectorLine(g, p);
                p.Dispose();
            }
        }

        void _Reset()
        {
            originMarker.Remove();
            aimMarker.Remove();
            spinMarker.Remove();
            calculateSpin = false;
            connectMarkers = false;
        }

        private void AddSpin()
        {
            // For original Trajectory.cs
            angle = aimTraj.RotAngle;
            drift = spinTraj.RotAngle;

            // renamed RotAngle to Rotation in Trajectory2
            //angle = aimTraj.Rotation;
            //drift = spinTraj.Rotation;
        }

        private void DrawConnectorLine(Graphics g, Pen p)
        {
            Point[] pArray = { originMarker.Center, spinMarker.Center, aimMarker.Center };
            g.DrawCurve(p, pArray);

        }
    }
}



#region old IsInBoundingRect
//public bool IsInBoundingRect(int mPosX, int mPosY) { if (spinMarker.IsInBoundingRect(mPosX, mPosY)) { return true; } else { return false; } }
#endregion old IsInBoundingRect

#region _SetSpinMarker
//void _SetSpinMarker(int x, int y)
//{
//    if (!spinMarker.IsPlaced)
//    {
//        spinMarker.Place(x, y);
//        spinTraj.SetEndPoint(x, y);
//        spinMarker.ShowClickRectangle = true;
//    }
//    else
//    {
//        spinMarker.AdjustPosition(x, y);
//        spinTraj.SetEndPoint(x, y);
//    }
//    AddSpin();
//}
#endregion _SetSpinMarker

#region FlightPath Full back up before reworking 2019-10-19
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Drawing;

//using BallzForWindows01.DrawableParts;

//namespace BallzForWindows01.GamePhysicsParts
//{
//    using static AssistFunctions;

//    class FlightPath
//    {
//        XMarker originMarker;
//        XMarker aimMarker;
//        XMarker spinMarker;

//        Trajectory aimTraj;
//        Trajectory spinTraj;

//        bool connectMarkers = false;
//        bool calculateSpin = false;
//        public bool ConnectMarkers { get { return connectMarkers; } }
//        public bool CalculatingSpin { get { return calculateSpin; } }

//        Color lineColor = Color.FromArgb(255, 255, 0, 0);

//        double angle = 0;    // angle in degrees
//        public double AngleDeg() { return angle; }
//        public double Angle { get { return angle; } set { angle = value; } }


//        public FlightPath()
//        {
//            originMarker = new XMarker();
//            aimMarker = new XMarker();
//            spinMarker = new XMarker();
//            spinMarker.DrawColor = Color.Green;
//            aimTraj = new Trajectory();
//            spinTraj = new Trajectory();
//        }
//        public void Load()
//        {

//        }
//        public void PlaceStartMarker(int x, int y)
//        {
//            originMarker.Place(x, y);
//            aimTraj.Load(x, y);
//            spinTraj.Load(x, y);

//        }
//        public void PlaceEndMarker(int x, int y)
//        {
//            aimMarker.Place(x, y);
//            aimTraj.SetEndPoint(x, y);
//            //connectMarkers = true;
//            calculateSpin = true;
//            int spinX = (originMarker.X + aimMarker.X) / 2;
//            int spinY = (originMarker.Y + aimMarker.Y) / 2;
//            PlaceSpinMarker(spinX, spinY);
//        }
//        private void PlaceSpinMarker(int x, int y)
//        {
//            _SetSpinMarker(x, y);
//            //spinMarker.Place(x, y);
//            //spinTraj.SetEndPoint(x, y);
//            //spinMarker.ShowClickRectangle = true;
//            //AddSpin();
//        }

//        public void AdjustSpinMarker(int x, int y)
//        {
//            _SetSpinMarker(x, y);
//            //spinMarker.AdjustPosition(x, y);
//            //spinTraj.SetEndPoint(x, y);
//            //AddSpin();
//        }
//        void _SetSpinMarker(int x, int y)
//        {
//            if (!spinMarker.IsPlaced)
//            {
//                spinMarker.Place(x, y);
//                spinTraj.SetEndPoint(x, y);
//                spinMarker.ShowClickRectangle = true;
//            }
//            else
//            {
//                spinMarker.AdjustPosition(x, y);
//                spinTraj.SetEndPoint(x, y);
//            }
//            AddSpin();
//        }

//        public void AddDebugStrings()
//        {

//        }
//        double drift = 0;
//        public double Drift { get { return drift; } set { drift = value; } }
//        private void AddSpin()
//        {
//            angle = aimMarker.AngleFromPoint(originMarker.Center);
//            drift = spinMarker.AngleFromPoint(originMarker.Center);

//            // I want to adjust the angle of the ball as the ball moves.
//            // At this time the current logic i am thking is to adjust the
//            // angle of the ball based off the spin marker. the aim marker will set the 
//            // initial angle and then the spin marker will factor into the flight of the ball over
//            // the flight time. for instance, if the aim marker and spin marker are at the same angle, the ball 
//            // will go straight. If the spin marker is pulled to the right of the aim marker, I will need to do some calculations to 
//            // and apply a slight "drift" to the ball as it travels.

//        }




//        public bool IsInBoundingRect(int mPosX, int mPosY) { if (spinMarker.IsInBoundingRect(mPosX, mPosY)) { return true; } else { return false; } }

//        public void Draw(Graphics g)
//        {
//            if (originMarker.IsPlaced && aimMarker.IsPlaced)
//            {
//                originMarker.Draw(g);
//                spinMarker.Draw(g);
//                aimMarker.Draw(g);
//            }
//            if (connectMarkers)
//            {
//                Pen p = new Pen(lineColor, 5);
//                DrawConnectorLine(g, p);
//            }
//        }

//        private void DrawConnectorLine(Graphics g, Pen p)
//        {
//            Point[] pArray = { originMarker.Center, spinMarker.Center, aimMarker.Center };
//            g.DrawCurve(p, pArray);

//        }
//        public void Reset()
//        {
//            originMarker.Remove();
//            aimMarker.Remove();
//            spinMarker.Remove();
//            calculateSpin = false;
//            connectMarkers = false;
//        }
//    }
//}
#endregion FlightPath Full back up before reworking 2019-10-19
