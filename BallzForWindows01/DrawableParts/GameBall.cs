using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using BallzForWindows01.GamePhysicsParts;

namespace BallzForWindows01.DrawableParts
{
    class GameBall : DrawableObject
    {
        Point startPosition;
        Point center;
        Color color;

        Font font;
        Color fontColor = Color.Black;
        int fontSize = 20;
        string fontFamily = "Arial";

        FlightPath flightPath;
        Button01 launchButton;


        Size gameScreenSize = new Size();

        bool placingAim = false;

        bool settingSpin = false;
        bool readyForLaunch = false;    // same as setting spin, might need to adjust later
        bool placingSpinRect = false;
        bool ballLaunched = false;

        #region properties
        public Size GameScreenSize { get { return gameScreenSize; } set { gameScreenSize = value; } }
        public Color DrawColor { get { return color; } set { color = value; } }
        public bool ReadyForLaunch { get { return readyForLaunch; } }
        public bool PlacingAim { get { return placingAim; } set { placingAim = value; } }
        public bool SettingSpin { get { return settingSpin; } }
        public bool PlacingSpinRect { get { return placingSpinRect; } set { placingSpinRect = value; } }

        public bool BallLaunched { get { return ballLaunched; } }
        #endregion

        public GameBall()
        {
            startPosition = new Point(0, 0);
            x = 0;
            y = 0;
            width = 15;
            height = 15;
            alpha = 255;
            red = 0;
            green = 0;
            blue = 0;
            color = Color.FromArgb(alpha, red, green, blue);
            font = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            center = new Point(x - width / 2, y - height / 2);
            placingAim = false;
            settingSpin = false;
            readyForLaunch = false;    // same as setting spin, might need to adjust later
            placingSpinRect = false;
            ballLaunched = false;
            InitAndLoadBallParts();
        }
        private void InitAndLoadBallParts()
        {
            flightPath = new FlightPath();
            flightPath.Load();
            launchButton = new Button01();
        }
        public void Load(int x, int y)
        {
            this.x = x;
            this.y = y;
            
            center = new Point(x + width / 2, y + height / 2);
            startPosition = new Point(x, y);
            PositionLaunchButton();
        }
        public void Load(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            center = new Point(x + width / 2, y + height / 2);
            startPosition = new Point(x, y);
            PositionLaunchButton();
        }
        private void PositionLaunchButton()     // place in load or after
        {
            Point position = new Point();
            Size size = new Size();
            size.Width = 100;
            size.Height = 40;
            position.X = gameScreenSize.Width / 2 - size.Width / 2;
            position.Y = gameScreenSize.Height - size.Height * 2 - 5;
            launchButton.Load(position.X, position.Y, size.Width, size.Height);
        }


        float angle = 2;
        float speed = 5;
        PointF flightPos = new PointF();
        bool startPosSet = false;
        public void Update()
        {
            
            if (ballLaunched)
            {
                //y -= (int)(1 * speed);
                //x = x + (int)(Math.Cos(angle));
                //y = y + (int)(Math.Sin(angle));
                float xfloat = speed * (float)Math.Cos(angle);
                float yfloat = speed * (float)Math.Sin(angle);
                x = (int)xfloat;
                y = (int)yfloat;
                
                if (y <= 0)
                {
                    //ballLaunched = false;
                    Reset();
                }
                if(x <= 0 || x >= gameScreenSize.Width)
                {
                    Reset();
                }


            }
        }
        public void SetFlightPath(int x, int y)
        {
            if (!flightPath.CalculatingSpin)
            {
                PlaceAimMarker(x, y);
                settingSpin = true;
                readyForLaunch = true;
            }
        }


        public bool IsInSpinRect(int x, int y)
        {
            if (flightPath.IsInBoundingRect(x, y))
            {
                return true;
            }                
            else
            {
                return false;
            }
                
        }
        public bool IsInLaunchButtonRect(int x, int y)
        {
            if (launchButton.IsInBoundingRect(x, y))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public void AdjustSpinMarker(int x, int y)
        {
            flightPath.AdjustSpinMarker(x, y);
        }
        private void PlaceAimMarker(int endMarkerX, int endMarkerY)
        {
            flightPath.PlaceStartMarker(this.x, this.y);
            flightPath.PlaceEndMarker(endMarkerX, endMarkerY);
            //settingSpin = true;
        }
        public void LaunchBall()
        {
            ballLaunched = true;
        }
        public void Draw(Graphics g)
        {
            string description = "Ball";
            SizeF strSize = g.MeasureString(description, font);
            Point strPos = new Point();
            strPos.X = (int)center.X - (int)strSize.Width / 2;
            strPos.Y = (int)center.Y - (int)strSize.Height / 2;
            using (SolidBrush sb = new SolidBrush(color))
            {
                launchButton.Draw(g);
                flightPath.Draw(g);
                g.FillEllipse(sb, x - width / 2, y - height / 2, width, height);

                g.DrawEllipse(Pens.Red, x - width / 2, y - height / 2, width, height);
            }
        }

        public void Reset()
        {
            ballLaunched = false;
            readyForLaunch = false;
            x = startPosition.X;
            y = startPosition.Y;
            flightPath.Reset();

        }

    }
}
