using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BallzForWindows01.Structs
{

    
    public enum KbKeys
    {
        Escape = Keys.Escape,
        Space = Keys.Space,
        Up = Keys.Up,
        Down = Keys.Down,
        Left = Keys.Left,
        Right = Keys.Right
        
    }
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
