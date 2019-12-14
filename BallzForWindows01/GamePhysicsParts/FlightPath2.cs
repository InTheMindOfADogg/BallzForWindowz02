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
    using DebugParts;

    class FlightPath2
    {
        XMarker2 originMarker;
        XMarker2 aimMarker;
        XMarker2 spinMarker;

        Trajectory2 aimTraj;
        Trajectory2 spinTraj;

        Color lineColor = Color.FromArgb(255, 255, 0, 0);

        DbgPath dpath = new DbgPath();

        bool connectMarkers = false;

        double angle = 0;
        double drift = 0;
        double driftFactor = 0;
        
        // planning for timeDriftModifier to be the decay of drift over time. 
        double timeDriftModifier = 0;
        double driftHardness = 0.5;

        public bool ConnectMarkers { get { return connectMarkers; } set { connectMarkers = value; } }
        public bool AimMarkerPlaced { get { return aimMarker.Placed; } }
        //public double Angle { get { return angle; } set { angle = value; } }
        //public double Drift { get { return drift; } set { drift = value; } }
        //public double DriftFactor { get { return driftFactor; } set { driftFactor = value; } }
        public double TimeDriftModifier { get { return timeDriftModifier; } set { timeDriftModifier = value; } }
        public double DriftHardness { get { return driftHardness; } set { driftHardness = value; } }


        public FlightPath2() { _Init(); }
        public void Load() { _Load(); }
        public void Update() { _Update(); }
        public void Draw(Graphics g, bool render = true) { _Draw(g, render); }
        public void Reset() { _Reset(); }
        public void CleanUp() { _CleanUp(); }

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
        void _Update()
        {
        }
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
                //DrawConnectorLine(g, p);
                p.Dispose();
            }
            dpath.Draw(g);
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

        
        public double CalculatedAngle(double elapsedTime = 0)
        {
            // added elapsedTime here but not used at the moment (2019-12-07)
            // I am thinking of using elapsed time to degrade the amount of drift.
            // For example, the longer the ball is moving, the less drift it will have, so it will have the
            // highest amount of drift right at launch and then it will get closer to a strait line
            // as it moves forward.
            angle = aimTraj.Rotation;
            drift = spinTraj.Rotation;
            driftFactor = (angle - drift) * driftHardness;
            return (angle - (driftFactor * timeDriftModifier));
        }

        public void DbgPlotPath()
        {
            dpath.PlotPoints(originMarker.Center, angle, drift, timeDriftModifier, driftHardness);
        }

        public bool InAimRect(double x, double y){return aimMarker.InClickRect(x, y);}
        public bool InSpinRect(double x, double y){return spinMarker.InClickRect(x, y);}

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
            //angle = aimTraj.Rotation;
            //drift = spinTraj.Rotation;
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
