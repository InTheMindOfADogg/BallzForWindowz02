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
        double rot = 0;

        public RotationD() { rot = 0; }
        public RotationD(double rotation) { this.rot = rotation; }

        public void Set(double rotation) { this.rot = rotation; }
        
        // Not tested in this location yet, but this is code from Trajectory03
        public void Set(PointD startPoint, PointD endPoint)
        {
            bool north, south;
            double anglePreRotation = 0;
            double rotation = 0;
            double oppLen = 0, hypLen = 0;
            PointD rightPoint = new PointD(endPoint.X, startPoint.Y);
            oppLen = rightPoint.DistanceTo(endPoint);
            hypLen = startPoint.DistanceTo(endPoint);
            north = south = false;
            if (endPoint.Y < startPoint.Y) { north = true; }
            if (endPoint.Y > startPoint.Y) { south = true; }
            anglePreRotation = Math.Asin(oppLen / hypLen);
            // north
            if (endPoint.X == startPoint.X && north) { rotation = (3 * Math.PI) / 2; return; }
            // south
            if (endPoint.X == startPoint.X && south) { rotation = (Math.PI) / 2; return; }
            // east
            if (endPoint.X > startPoint.X && endPoint.Y == startPoint.Y) { rotation = 0; return; }
            // west
            if (endPoint.X < startPoint.X && endPoint.Y == startPoint.Y) { rotation = Math.PI; return; }
            // northwest
            if (endPoint.X > startPoint.X && north) { rotation = Math.PI * 2 - anglePreRotation; return; }
            //northeast
            if (endPoint.X < startPoint.X && north) { rotation = Math.PI + anglePreRotation; return; }
            // southwest
            if (endPoint.X > startPoint.X && south) { rotation = anglePreRotation; return; }
            // southeast
            if (endPoint.X < startPoint.X && south) { rotation = Math.PI - anglePreRotation; return; }
        }

    }
}
