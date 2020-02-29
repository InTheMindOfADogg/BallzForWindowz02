using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BallzForWindows01.Structs
{
    #region Keys and Keyboard related enums
    public enum KeyboardEventType
    {
        KeyDown,
        KeyUp
    }
    public enum KeyState
    {
        Up,
        Down
    }
    public enum KeyAction
    {
        None,
        Pressed,
        Held,
        Released,

    }
    public enum KbKeys
    {
        Escape = Keys.Escape,
        Space = Keys.Space,
        Up = Keys.Up,
        Down = Keys.Down,
        Left = Keys.Left,
        Right = Keys.Right
        
    }

    #endregion Keys and Keyboard related enums

    public enum RotationDirection
    {
        Clockwise,
        CounterClockwise
    }

    public enum Heading
    {        
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
        North,
        NorthEast
    }


}
