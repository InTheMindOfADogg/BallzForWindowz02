using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using BallzForWindows01.DrawableParts;

namespace BallzForWindows01.GamePhysicsParts
{
    class FlightPath
    {
        
        XMarker startMarker;
        XMarker endMarker;
        XMarker spinMarker;
        bool connectMarkers = false;
        bool calculateSpin = false;
        public bool ConnectMarkers { get { return connectMarkers; } }
        public bool CalculatingSpin { get { return calculateSpin; } }

        Point lineStart = new Point();
        Point lineMiddle = new Point();
        Point lineEnd = new Point();

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
            lineStart.X = x;
            lineStart.Y = y;
        }
        public void PlaceEndMarker(int x, int y)
        {
            endMarker.Place(x, y);
            lineEnd.X = x;
            lineEnd.Y = y;
            connectMarkers = true;
            calculateSpin = true;
            int spinX = (startMarker.X + endMarker.X) / 2;
            int spinY = (startMarker.Y + endMarker.Y) / 2;
            lineMiddle.X = spinX;
            lineMiddle.Y = spinY;
            
            PlaceSpinMarker(spinX, spinY);
        }
        //int initialSpinX = 0;
        //int initialSpinY = 0;
        int startingSpinX = 0;
        int startingSpinY = 0;
        int deltaSpinX = 0;
        int deltaSpinY = 0;
        private void PlaceSpinMarker(int x, int y)
        {
            startingSpinX = x;
            startingSpinY = y;
            deltaSpinX = 0;
            deltaSpinY = 0;
            spinMarker.Place(x, y);
            spinMarker.ShowClickRectangle = true;
        }
        
        public bool IsInBoundingRect(int mPosX, int mPosY)
        {
            if(spinMarker.IsInBoundingRect(mPosX, mPosY))
            {
                return true;
            }
            else
                return false;
        }
        public void AdjustSpinMarker(int offsetX, int offsetY)
        {
            deltaSpinX = offsetX - startingSpinX;
            deltaSpinY = offsetY - startingSpinY;
            lineMiddle.X = offsetX;
            lineMiddle.Y = offsetY;
            spinMarker.AdjustPosition(offsetX, offsetY);
        }
        
        public void Draw(Graphics g)
        {
            if (startMarker.IsPlaced && endMarker.IsPlaced)
            {
                startMarker.Draw(g);
                endMarker.Draw(g);
            }
            if(connectMarkers)
            {
                Color lineColor;
                int a = startMarker.Alpha;
                int r = startMarker.Red;
                int grn = startMarker.Green;
                int b = startMarker.Blue;
                lineColor = Color.FromArgb(a, r, grn, b);
                Pen p = new Pen(lineColor, 5);
                //g.DrawLine(p, startMarker.Center, endMarker.Center);
                spinMarker.Draw(g);
                DrawConnectorLine(g, p);
            }
        }

        private void DrawConnectorLine(Graphics g, Pen p)
        {
            Point[] pArray = { lineStart, lineMiddle, lineEnd };
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
