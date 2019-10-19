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
        //public float Angle { get { return (float)(angle * Math.PI / 180); } }

        XMarker startMarker;
        XMarker endMarker;
        XMarker spinMarker;
        bool connectMarkers = false;
        bool calculateSpin = false;
        public bool ConnectMarkers { get { return connectMarkers; } }
        public bool CalculatingSpin { get { return calculateSpin; } }

        Color lineColor = Color.FromArgb(255, 255, 0, 0);

        double angle = 0;    // angle in degrees
        public double AngleDeg() { return angle; }
        //public double Angle() { return angle; }
        public double Angle { get { return angle; } set { angle = value; } }


        public FlightPath()
        {
            startMarker = new XMarker();
            endMarker = new XMarker();
            spinMarker = new XMarker();
            spinMarker.DrawColor = Color.Green;
        }
        public void Load()
        {

        }
        public void PlaceStartMarker(int x, int y)
        {
            startMarker.Place(x, y);
        }
        public void PlaceEndMarker(int x, int y)
        {
            endMarker.Place(x, y);
            //connectMarkers = true;
            calculateSpin = true;
            int spinX = (startMarker.X + endMarker.X) / 2;
            int spinY = (startMarker.Y + endMarker.Y) / 2;
            PlaceSpinMarker(spinX, spinY);
        }
        private void PlaceSpinMarker(int x, int y)
        {
            spinMarker.Place(x, y);
            spinMarker.ShowClickRectangle = true;
            AddSpin();
        }
        


        public void AddDebugStrings()
        {
            //double endMakerAngleRadians = endMarker.AngleFromPoint(startMarker.Center);
            //double spinMakerAngleRadians = spinMarker.AngleFromPoint(startMarker.Center);

            //double endMakerAngleDegrees = endMakerAngleRadians * 180 / Math.PI;
            //double spinMakerAngleDegrees = spinMakerAngleRadians * 180 / Math.PI;

            //DbgFuncs.AddStr($"[FilghtPath]: End marker angle radians(from start marker): {endMakerAngleRadians}");
            //DbgFuncs.AddStr($"[FilghtPath]: End marker angle degrees(from start marker): {endMakerAngleDegrees}");
            //DbgFuncs.AddStr($"[FilghtPath]: Spin marker angle radians(from start marker): {spinMakerAngleRadians}");
            //DbgFuncs.AddStr($"[FilghtPath]: Spin marker angle degrees(from start marker): {spinMakerAngleDegrees}");            
            
        }
        double drift = 0;
        public double Drift { get { return drift; } set { drift = value; } }        
        private void AddSpin()
        {
            angle = endMarker.AngleFromPoint(startMarker.Center);
            drift = spinMarker.AngleFromPoint(startMarker.Center);
             
            // I want to adjust the angle of the ball as the ball moves.
            // At this time the current logic i am thking is to adjust the
            // angle of the ball based off the spin marker. the aim marker will set the 
            // initial angle and then the spin marker will factor into the flight of the ball over
            // the flight time. for instance, if the aim marker and spin marker are at the same angle, the ball 
            // will go straight. If the spin marker is pulled to the right of the aim marker, I will need to do some calculations to 
            // and apply a slight "drift" to the ball as it travels.

        }


        public void AdjustSpinMarker(int x, int y)
        {
            spinMarker.AdjustPosition(x, y);
            AddSpin();
        }

        public bool IsInBoundingRect(int mPosX, int mPosY)
        {
            if (spinMarker.IsInBoundingRect(mPosX, mPosY)) { return true; }
            else { return false; }
        }

        public void Draw(Graphics g)
        {
            if (startMarker.IsPlaced && endMarker.IsPlaced)
            {
                startMarker.Draw(g);
                spinMarker.Draw(g);
                endMarker.Draw(g);
            }
            if (connectMarkers)
            {
                Pen p = new Pen(lineColor, 5);
                //spinMarker.Draw(g);
                DrawConnectorLine(g, p);
            }
        }

        private void DrawConnectorLine(Graphics g, Pen p)
        {
            Point[] pArray = { startMarker.Center, spinMarker.Center, endMarker.Center };
            g.DrawCurve(p, pArray);

        }
        public void Reset()
        {
            startMarker.Remove();
            endMarker.Remove();
            spinMarker.Remove();
            calculateSpin = false;
            connectMarkers = false;
        }
    }
}
