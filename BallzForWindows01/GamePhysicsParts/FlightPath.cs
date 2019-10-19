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
            originMarker = new XMarker();
            aimMarker = new XMarker();
            spinMarker = new XMarker();
            spinMarker.DrawColor = Color.Green;
        }
        public void Load()
        {

        }
        public void PlaceStartMarker(int x, int y)
        {
            originMarker.Place(x, y);
        }
        public void PlaceEndMarker(int x, int y)
        {
            aimMarker.Place(x, y);
            //connectMarkers = true;
            calculateSpin = true;
            int spinX = (originMarker.X + aimMarker.X) / 2;
            int spinY = (originMarker.Y + aimMarker.Y) / 2;
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
            angle = aimMarker.AngleFromPoint(originMarker.Center);
            drift = spinMarker.AngleFromPoint(originMarker.Center);
             
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
            if (originMarker.IsPlaced && aimMarker.IsPlaced)
            {
                originMarker.Draw(g);
                spinMarker.Draw(g);
                aimMarker.Draw(g);
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
