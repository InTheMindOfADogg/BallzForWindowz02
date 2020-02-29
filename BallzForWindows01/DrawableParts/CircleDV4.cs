using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace BallzForWindows01.DrawableParts
{

    using GamePhysicsParts;
    using Structs;

    class CircleDV4 : DrawableObject
    {

        public double Rotation { get { return rotInd.Rotation; } }
        public PointD Position { get { return position; } }


        PointD position;
        double radius;
        RotationIndicator01 rotInd;

        Color fillColor = Color.Green;
        Color outlineColor = Color.Green;


        public CircleDV4()
        {
            clsName = "CircleDV4";
            position = new PointD(30, 30);
            radius = 15;
            rotInd = new RotationIndicator01();
        }
        public void Load(double x, double y, double radius, double rotation = 0)
        {
            position.Set(x, y);
            this.radius = radius;
            position.SetStartingPosition(x, y);
            rotInd.Load(position, radius * 2, rotation);
        }


        double speed = 0;
        double acceleration = 0.1;
        double deceleration = 0.1;

        double rotChange = 0;
        double turnRate = 0.01;
        double maxSpeed = 4;
        double slowRate = 0.01;
        double zeroOutValue = 0.0001;   // if abs of speed is less than zeroOutValue, speed gets set to 0 (for handling double precision)
        public void Update(MouseControls mc, KeyboardControls01 kc)
        {
            string fnId = AssistFunctions.FnId(clsName, "Update");

            HandleMouseInput(mc);
            HandleKeyboardInput(kc);



            rotInd.Update(speed, rotChange);
            position.Move(speed, rotInd.Rotation);




            //rotInd.Update(position, rotChange);

            DbgFuncs.AddStr(fnId, $"speed: {speed}");
            DbgFuncs.AddStr(fnId, $"rotation: {rotInd.Rotation}");
            DbgFuncs.AddStr(fnId, $"rotChange: {rotChange}");

            rotChange = 0;
        }

        public void HandleMouseInput(MouseControls mc)
        {
            if (mc.RightButtonClicked()) { Reset(); return; }
        }

        
        public void HandleKeyboardInput(KeyboardControls01 kc)
        {
            bool rateChange = false;
            // Handle keyboard controls
            if (kc.KeyPressed(KbKeys.Up) || kc.KeyHeld(KbKeys.Up))
            {
                if (speed < maxSpeed) { speed += acceleration; }
                // restrict positive speed to max speed
                if(speed > maxSpeed) { speed = maxSpeed; }
                rateChange = true;
            }

            if (kc.KeyPressed(KbKeys.Down) || kc.KeyHeld(KbKeys.Down))
            {
                if (speed > -maxSpeed) { speed -= deceleration; }
                // Restrict negative speed to max speed
                if (speed < -maxSpeed) { speed = -maxSpeed; }
                rateChange = true;
            }

            // If no accel or decel, apply gradual speed shift toward 0
            if(!rateChange)
            {
                if(speed < zeroOutValue && speed > -zeroOutValue ){speed = 0;}
                
                if(speed > 0){speed -= slowRate;}
                else if(speed < 0){speed += slowRate;}
                
            }


            if (kc.KeyPressed(KbKeys.Right) || kc.KeyHeld(KbKeys.Right)) { rotChange += turnRate; }
            if (kc.KeyPressed(KbKeys.Left) || kc.KeyHeld(KbKeys.Left)) { rotChange -= turnRate; }
        }

        public void Draw(Graphics g, Pen p, SolidBrush sb)
        {
            FillCircle(g, sb);
            DrawOutline(g, p);
            rotInd.Draw(g, p, sb);
        }

        public void Reset()
        {
            speed = 0;
            position.Reset();
            rotInd.Reset();
        }
        public void CleanUp()
        {

        }

        public void FillCircle(Graphics g, SolidBrush sb)
        {
            sb.Color = fillColor;
            g.FillEllipse(sb, (float)position.fX - ((float)radius), position.fY - ((float)radius), (float)radius * 2, (float)radius * 2);   // draw ball outline

        }
        public void DrawOutline(Graphics g, Pen p, float width = 1)
        {
            p.Color = outlineColor;
            p.Width = 1;
            g.DrawEllipse(p, (float)position.fX - ((float)radius), position.fY - ((float)radius), (float)radius * 2, (float)radius * 2);   // draw ball outline
        }
        public void DrawCenterMarker(Graphics g, SolidBrush sb)
        {
            float len = 4;      // length of sides for center marker
            sb.Color = Color.Red;
            g.FillRectangle(sb, position.fX - (len / 2), position.fY - (len / 2), len, len);   // draw center marker for testing
        }

        public bool InCircle(PointD p) { return position.DistanceTo(p.X, p.Y) < radius ? true : false; }
        public bool InCircle(double x, double y) { return position.DistanceTo(x, y) < radius ? true : false; }

        /// <summary>
        /// Sets fill color and outline color to color c.
        /// </summary>
        /// <param name="c"></param>
        public void SetCircleColor(Color c) { fillColor = outlineColor = c; }
        public void SetFillColor(Color c) { fillColor = c; }
        public void SetOutlineColor(Color c) { outlineColor = c; }

    }
}
