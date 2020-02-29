﻿using System;
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
        Color indicatorLineColor = Color.Red;
        PointD position;
        PointD indicatorPoint;
        PointD tetherPoint;
        RotationD rotation;
        


        public RotationIndicator01()
            :base()
        {
            position = new PointD(30, 30);
            indicatorPoint = new PointD();
            tetherPoint = new PointD();
            rotation = new RotationD();
            length = 15;
            color = Color.Black;
            
        }
        public void Load(PointD pos, double len, double rot)
        {
            position.Set(pos);
            position.SetStartingPosition(pos);
            length = len;
            rotation.Set(rot);
            rotation.SetStartingRotation(rot);
            indicatorPoint.Set(rotation.PointDFrom(pos, len));
            indicatorPoint.SetStartingPosition(rotation.PointDFrom(pos, len));
            tetherPoint.Set(rotation.PointDFrom(pos, len));
        }
        public void Update(double speed, double rotChange)
        {
            //string fnId = AssistFunctions.FnId(clsName, "Update");
            //rotation.Set(rot);
            rotation.Adjust(rotChange);
            position.Move(speed, rotation.Value);
            //indicatorPoint.Move(speed, rot);
            indicatorPoint.Set(rotation.PointDFrom(position, length));

            //DbgFuncs.AddStr(fnId, $"speed: {speed}");
            //DbgFuncs.AddStr(fnId, $"rotation: {rotation.Value}");


            
        }
        
        
        public void Draw(Graphics g, Pen p, SolidBrush sb)
        {
            p.Color = color;
            p.Width = 3;
            g.DrawLine(p, position.fX, position.fY, tetherPoint.fX, tetherPoint.fY);

            p.Color = indicatorLineColor;
            p.Width = 1;
            g.DrawLine(p, position.fX, position.fY, indicatorPoint.fX, indicatorPoint.fY);

            
        }
        public void Reset()
        {
            position.Reset();
            rotation.Reset();
            indicatorPoint.Reset();
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
