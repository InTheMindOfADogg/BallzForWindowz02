using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace BallzForWindows01.Structs
{
    using GamePhysicsParts;
    public enum MouseEventType
    {
        LeftButtonUp,
        LeftButtonDown,
        RightButtonUp,
        RightButtonDown,
        Move
    }

    class MouseControls
    {
        public PointD Position { get { return position; } }
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public int LastX { get { return lastX; } }
        public int LastY { get { return lastY; } }
        public UpDownState LeftButtonState { get { return leftState; } set { leftState = value; } }
        public UpDownState RightButtonState { get { return rightState; } set { rightState = value; } }
        public UpDownState LastLeftButtonState { get { return lastLeftState; } set { lastLeftState = value; } }
        public UpDownState LastRightButtonState { get { return lastRightState; } set { lastRightState = value; } }

        PointD position;
        private int x;
        private int y;
        int lastX;
        int lastY;
        private UpDownState leftState;
        private UpDownState rightState;
        private UpDownState lastLeftState;
        private UpDownState lastRightState;

        Point leftUpPos = new Point();
        Point leftDownPos = new Point();
        Point rightUpPos = new Point();
        Point rightDownPos = new Point();


        public MouseControls() 
        {
            position = new PointD();
        }


        public void Update(MouseEventArgs e, MouseEventType t)
        {
            UpdateLastState();
            position.Set(e.X, e.Y);
            x = e.X;
            y = e.Y;
            switch(t)
            {
                case MouseEventType.LeftButtonUp:
                    leftState = UpDownState.Up;
                    leftUpPos = new Point(e.X, e.Y);
                    break;
                case MouseEventType.LeftButtonDown:
                    leftState = UpDownState.Down;
                    leftDownPos = new Point(e.X, e.Y);
                    break;
                case MouseEventType.RightButtonUp:
                    rightState = UpDownState.Up;
                    rightUpPos = new Point(e.X, e.Y);
                    break;
                case MouseEventType.RightButtonDown:
                    rightState = UpDownState.Down;
                    rightDownPos = new Point(e.X, e.Y);
                    break;
                case MouseEventType.Move:
                    break;
                default:
                    break;
            }
        }
        void UpdateLastState()
        {
            lastX = x;
            lastY = y;
            lastLeftState = leftState;
            lastRightState = rightState;
        }

        
    }
}
