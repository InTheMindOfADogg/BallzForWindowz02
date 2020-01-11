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
        string clsName = "KeyControl01";

        public int Index { get { return idx; } }
        public Keys Key { get { return key; } }
        public int Value { get { return val; } }
        public KeyState State { get { return state; } }
        public KeyState LastState { get { return lastState; } }
        public KeyAction Action { get { return action; } }



        int idx = 0;
        int val = 0;
        Keys key;
        KeyState state = KeyState.Up;
        KeyState lastState = KeyState.Up;
        KeyAction action = KeyAction.None;

        public KeyControl01(int keyValue, int idx) { _Init((Keys)keyValue, idx); }
        public KeyControl01(Keys key, int idx) { _Init(key, idx); }        

        public void Update(KeyState currentState)
        {
            lastState = state;
            state = currentState;
            SetKeyAction();

        }
        
        void _Init(Keys key, int idx)
        {
            this.idx = idx;
            this.key = key;
            this.val = (int)key;
            state = KeyState.Up;
            lastState = KeyState.Up;
            action = KeyAction.None;

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
