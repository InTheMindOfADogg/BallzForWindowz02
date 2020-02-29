using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallzForWindows01.GamePhysicsParts
{
    // Created 2020-01-01


    class RotationD
    {
        public double AsDegrees { get { return (rot * 180 / Math.PI); } }
        public double Value { get { return rot; } }
        public double StartingRotation { get { return startingRotation; } }

        double rot = 0;
        double startingRotation = 0;

        

        public RotationD() 
        { 
            rot = 0;
        }
        public RotationD(double rotation) 
        { 
            this.rot = rotation;
            
        }

        

        public PointD PointDFrom(PointD startPoint, double distance)
        {
            PointD tmp = new PointD();
            tmp.X = startPoint.X + distance * Math.Cos(rot);
            tmp.Y = startPoint.Y + distance * Math.Sin(rot);
            return tmp;
        }

        /// <summary>
        /// Adds adjustRotation amount to current rotation
        /// </summary>
        /// <param name="adjustRotation"></param>
        public void Adjust(double adjustRotation)
        {
            rot += adjustRotation;
            rot = rot % (2 * Math.PI);
            if (rot < 0) { rot += (2 * Math.PI); }
        }
        public void Set(double rotation) { this.rot = rotation; }

        // Not tested in this location yet, but this is code from Trajectory03
        public void Set(PointD startPoint, PointD endPoint)
        {
            // Testing as function 2020-02-24
            rot = _RotationFromPoints(startPoint, endPoint);
            #region placed this is function to reuse to set starting rotation the same way. 2020-02-24
            //bool north, south;
            //double anglePreRotation = 0;
            ////double rot = 0;
            //double oppLen = 0, hypLen = 0;
            //PointD rightPoint = new PointD(endPoint.X, startPoint.Y);
            //oppLen = rightPoint.DistanceTo(endPoint);
            //hypLen = startPoint.DistanceTo(endPoint);
            //north = south = false;
            //if (endPoint.Y < startPoint.Y) { north = true; }
            //if (endPoint.Y > startPoint.Y) { south = true; }
            //anglePreRotation = Math.Asin(oppLen / hypLen);
            //// north
            //if (endPoint.X == startPoint.X && north) { rot = (3 * Math.PI) / 2; return; }
            //// south
            //if (endPoint.X == startPoint.X && south) { rot = (Math.PI) / 2; return; }
            //// east
            //if (endPoint.X > startPoint.X && endPoint.Y == startPoint.Y) { rot = 0; return; }
            //// west
            //if (endPoint.X < startPoint.X && endPoint.Y == startPoint.Y) { rot = Math.PI; return; }
            //// northwest
            //if (endPoint.X > startPoint.X && north) { rot = Math.PI * 2 - anglePreRotation; return; }
            ////northeast
            //if (endPoint.X < startPoint.X && north) { rot = Math.PI + anglePreRotation; return; }
            //// southwest
            //if (endPoint.X > startPoint.X && south) { rot = anglePreRotation; return; }
            //// southeast
            //if (endPoint.X < startPoint.X && south) { rot = Math.PI - anglePreRotation; return; }
            #endregion placed this is function to reuse to set starting rotation the same way. 2020-02-24
        }
        public void SetStartingRotation(PointD startPoint, PointD endPoint) { startingRotation = _RotationFromPoints(startPoint, endPoint); }

        // Might rename this face point.. that seems more descriptive of the action.
        private double _RotationFromPoints(PointD startPoint, PointD endPoint)
        {
            bool north, south;
            double anglePreRotation = 0;
            double oppLen = 0, hypLen = 0;
            PointD rightPoint = new PointD(endPoint.X, startPoint.Y);
            oppLen = rightPoint.DistanceTo(endPoint);
            hypLen = startPoint.DistanceTo(endPoint);
            north = south = false;
            if (endPoint.Y < startPoint.Y) { north = true; }
            if (endPoint.Y > startPoint.Y) { south = true; }
            anglePreRotation = Math.Asin(oppLen / hypLen);
            // north
            if (endPoint.X == startPoint.X && north) { return (3 * Math.PI) / 2; }
            // south
            if (endPoint.X == startPoint.X && south) { return (Math.PI) / 2; }
            // east
            if (endPoint.X > startPoint.X && endPoint.Y == startPoint.Y) { return 0; }
            // west
            if (endPoint.X < startPoint.X && endPoint.Y == startPoint.Y) { return Math.PI; }
            // northwest
            if (endPoint.X > startPoint.X && north) { return Math.PI * 2 - anglePreRotation; }
            //northeast
            if (endPoint.X < startPoint.X && north) { return Math.PI + anglePreRotation; }
            // southwest
            if (endPoint.X > startPoint.X && south) { return anglePreRotation; }
            // southeast
            if (endPoint.X < startPoint.X && south) { return Math.PI - anglePreRotation; }
            return 0;
        }

        public void SetStartingRotation(double startingRotation) { this.startingRotation = startingRotation; }
        public void Reset() { rot = startingRotation; }

    }
}
