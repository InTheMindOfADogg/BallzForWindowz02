using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.GamePhysicsParts
{
    using DrawableParts;
    class RotationIndicator01 : DrawableObject
    {
        public double Rotation { get { return rotation.Value; } }

        // Might move rotation indicator to own class.
        double length = 10;
        //Color color = Color.Black;
        PointD position;
        RotationD rotation;
        PointD endPoint;


        public RotationIndicator01()
            :base()
        {
            position = new PointD(30, 30);
            endPoint = new PointD();
            rotation = new RotationD();
            length = 15;
            color = Color.Black;
            
        }
        public void Load(PointD position, double length, double rotation)
        {
            this.position.Set(position);
            this.length = length;
            this.rotation.Set(rotation);
            this.rotation.SetStartingRotation(rotation);
        }
        public void Update(PointD position, double rotChange)
        {
            this.position.Set(position);
            this.rotation.Set(rotChange);
            endPoint.Set(rotation.PointDFrom(position, length));
        }
        public void Draw(Graphics g, Pen p, SolidBrush sb)
        {
            p.Color = color;
            g.DrawLine(p, position.fX, position.fY, endPoint.fX, endPoint.fY);
        }
        public void Reset()
        {
            position.Reset();
            rotation.Reset();
        }
        public void CleanUp()
        {

        }
        public void SetStartingPosRot(double x, double y, double rot)
        {
            position.SetStartingPosition(x, y);
            rotation.SetStartingRotation(rot);
        }

        public void SetStartingPosition(double x, double y)
        {
            position.SetStartingPosition(x, y);
        }
        public void SetStartingRotation(double rot)
        {
            rotation.SetStartingRotation(rot);
        }

    }
}
