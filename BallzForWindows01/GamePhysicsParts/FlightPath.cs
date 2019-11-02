using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using BallzForWindows01.DrawableParts;

namespace BallzForWindows01.GamePhysicsParts
{
    using static AssistFunctions;

    class FlightPath
    {
        XMarker originMarker;
        XMarker aimMarker;
        XMarker spinMarker;

        Trajectory aimTraj;
        Trajectory spinTraj;

        Color lineColor = Color.FromArgb(255, 255, 0, 0);

        bool connectMarkers = false;
        bool calculateSpin = false;
        double angle = 0;

        public bool ConnectMarkers { get { return connectMarkers; } }
        public bool CalculatingSpin { get { return calculateSpin; } }        
        public double Angle { get { return angle; } set { angle = value; } }
        

        public FlightPath()
        {
            originMarker = new XMarker();
            aimMarker = new XMarker();
            spinMarker = new XMarker();
            spinMarker.DrawColor = Color.Green;
            aimTraj = new Trajectory("aimTraj");
            spinTraj = new Trajectory("spinTraj");
        }
        public void Load()
        {

        }
        public void PlaceStartMarker(int x, int y)
        {
            originMarker.Place(x, y);
            aimTraj.Load(x, y);
            spinTraj.Load(x, y);

        }
        public void PlaceEndMarker(int x, int y)
        {
            aimMarker.Place(x, y);
            aimTraj.SetEndPoint(x, y);
            //connectMarkers = true;
            calculateSpin = true;
            int spinX = (originMarker.X + aimMarker.X) / 2;
            int spinY = (originMarker.Y + aimMarker.Y) / 2;
            SetSpinMarker(spinX, spinY);
        }
        public void SetSpinMarker(int x, int y) { _SetSpinMarker(x, y); }
        void _SetSpinMarker(int x, int y)
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

        public void AddDebugStrings()
        {

        }
        double drift = 0;
        public double Drift { get { return drift; } set { drift = value; } }
        private void AddSpin()
        {
            angle = aimTraj.RotAngle;
            drift = spinTraj.RotAngle;
        }




        public bool IsInBoundingRect(int mPosX, int mPosY) { if (spinMarker.IsInBoundingRect(mPosX, mPosY)) { return true; } else { return false; } }

        public void Draw(Graphics g)
        {
            if (originMarker.IsPlaced && aimMarker.IsPlaced)
            {
                originMarker.Draw(g);
                spinMarker.Draw(g);
                aimMarker.Draw(g);                                
                spinTraj.Draw(g);
                aimTraj.Draw(g,450,20);
            }
            if (connectMarkers)
            {
                Pen p = new Pen(lineColor, 5);
                DrawConnectorLine(g, p);
            }
        }

        private void DrawConnectorLine(Graphics g, Pen p)
        {
            Point[] pArray = { originMarker.Center, spinMarker.Center, aimMarker.Center };
            g.DrawCurve(p, pArray);

        }
        public void Reset()
        {
            originMarker.Remove();
            aimMarker.Remove();
            spinMarker.Remove();
            calculateSpin = false;
            connectMarkers = false;
        }
    }
}

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
