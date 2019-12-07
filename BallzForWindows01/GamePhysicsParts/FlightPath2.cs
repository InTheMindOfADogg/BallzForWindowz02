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

    class FlightPath2
    {
        XMarker2 originMarker;
        XMarker2 aimMarker;
        XMarker2 spinMarker;

        Trajectory2 aimTraj;
        Trajectory2 spinTraj;

        Color lineColor = Color.FromArgb(255, 255, 0, 0);

        bool connectMarkers = false;
        //bool calculateSpin = false;

        double angle = 0;
        double drift = 0;


        public bool ConnectMarkers { get { return connectMarkers; } set { connectMarkers = value; } }
        //public bool CalculatingSpin { get { return calculateSpin; } }
        public bool AimMarkerPlaced { get { return aimMarker.Placed; } }
        public double Angle { get { return angle; } set { angle = value; } }
        public double Drift { get { return drift; } set { drift = value; } }


        public FlightPath2() { _Init(); }
        public void Load() { _Load(); }
        public void Update() { _Update(); }
        public void Draw(Graphics g, bool render = true) { _Draw(g, render); }
        public void Reset() { _Reset(); }
        public void CleanUp() { _CleanUp(); }

        public bool InAimRect(double x, double y)
        {
            return aimMarker.InClickRect(x, y);
        }
        public bool InSpinRect(double x, double y)
        {
            return spinMarker.InClickRect(x, y);
        }

        public void PlaceOriginMarker(double x, double y)
        {
            originMarker.Place(x, y);
            aimTraj.SetStartPoint(x, y);
            spinTraj.SetStartPoint(x, y);
        }

        public void PlaceAimMarker(double x, double y)
        {
            aimMarker.Place(x, y);
            aimTraj.SetEndPoint(x, y);
            //calculateSpin = true;

            if(!spinMarker.Placed)
            {
                double spinX = (originMarker.Position.X + aimMarker.Position.X) / 2;
                double spinY = (originMarker.Position.Y + aimMarker.Position.Y) / 2;
                PlaceSpinMarker(spinX, spinY);
                connectMarkers = true;
            }
            
            
        }

        public void PlaceSpinMarker(double x, double y)
        {
            spinMarker.Place(x, y);
            spinTraj.SetEndPoint(x, y);
            if (!spinMarker.ShowClickRectangle) spinMarker.ShowClickRectangle = true;
            //AddSpin();
            angle = aimTraj.Rotation;
            drift = spinTraj.Rotation;
        }

        void _Init()
        {
            originMarker = new XMarker2();
            aimMarker = new XMarker2();
            spinMarker = new XMarker2();
            
            originMarker.DrawColor = Color.FromArgb(125, Color.Red);            
            aimMarker.DrawColor = Color.FromArgb(125, Color.Red);            
            spinMarker.DrawColor = Color.FromArgb(125, Color.Green);

            aimTraj = new Trajectory2("aimTraj");
            spinTraj = new Trajectory2("spinTraj");
        }

        void _Load() { }
        void _Update() { }
        void _Draw(Graphics g, bool render)
        {
            if (!render) return;
            if (originMarker.Placed && aimMarker.Placed)
            {
                //originMarker.Draw(g);
                spinMarker.Draw(g);
                aimMarker.Draw(g);
                spinTraj.Draw(g);

                // for Trajectory2 drawing. split draw debug info into seperate function
                aimTraj.Draw(g);
                aimTraj.DebugDraw(g, 450, 20);
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
            originMarker.Reset();
            aimMarker.Reset();
            spinMarker.Reset();
            //calculateSpin = false;
            connectMarkers = false;
        }
        void _CleanUp()
        {

        }

        private void DrawConnectorLine(Graphics g, Pen p)
        {
            PointF[] pArray = { originMarker.Center.ToPointF(), spinMarker.Center.ToPoint(), aimMarker.Center.ToPoint() };
            g.DrawCurve(p, pArray);

        }


    }

}


#region old PlaceSpinMarker
//public void PlaceSpinMarker(double x, double y)
//{
//    //if (!spinMarker.Placed)
//    //{
//    //    spinMarker.Place(x, y);
//    //    spinTraj.SetEndPoint(x, y);
//    //    spinMarker.ShowClickRectangle = true;
//    //}
//    //else
//    //{
//    //    spinMarker.Place(x, y);
//    //    spinTraj.SetEndPoint(x, y);
//    //}
//    spinMarker.Place(x, y);
//    spinTraj.SetEndPoint(x, y);
//    spinMarker.ShowClickRectangle = true;
//    //AddSpin();
//    angle = aimTraj.Rotation;
//    drift = spinTraj.Rotation;
//}
#endregion old PlaceSpinMarker
