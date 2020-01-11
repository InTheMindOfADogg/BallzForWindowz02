using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BallzForWindows01.Structs
{
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
    class KeyControl01
    {
        public int Index { get { return idx; } }
        public int KeyCode { get { return keyCode; } }
        public KeyState State { get { return state; } }
        public KeyState LastState { get { return lastState; } }

        int idx = 0;
        int keyCode = 0;
        KeyState state = KeyState.Up;
        KeyState lastState = KeyState.Up;
        KeyAction action = KeyAction.None;

        public KeyControl01(int keyCode, int idx)
        {
            this.idx = idx;
            this.keyCode = keyCode;
            state = KeyState.Up;
            lastState = KeyState.Up;
        }

        public void Update(KeyState currentState)
        {
            lastState = state;
            state = currentState;
            SetKeyAction();

        }

        void SetKeyAction()
        {
            if (state == KeyState.Down && lastState == KeyState.Up) { action = KeyAction.Pressed; return; }
            if (state == KeyState.Down && lastState == KeyState.Down) { action = KeyAction.Held; return; }
            if (state == KeyState.Up && lastState == KeyState.Down) { action = KeyAction.Released; return; }
            action = KeyAction.None;
        }

    }
}
