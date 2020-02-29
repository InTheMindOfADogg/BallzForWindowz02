using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BallzForWindows01.Structs
{
    class KeyControl02
    {
        string clsName = "KeyControl02";

        public int Index { get { return idx; } }
        public Keys Key { get { return key; } }
        public KbKeys KbKey { get { return kbkey; } }
        public int KeyIntValue { get { return val; } }
        public KeyState State { get { return state; } }
        public KeyState LastState { get { return lastState; } }
        public KeyAction Action { get { return action; } }
        public KeyAction LastAction { get { return lastAction; } }

        public bool EventThisFrame { get { return eventThisFrame; } }
        public bool DefaultState { get { return defaultState; } }


        int idx = 0;
        int val = 0;
        Keys key;
        KbKeys kbkey;
        KeyState state = KeyState.Up;
        KeyState lastState = KeyState.Up;
        KeyAction action = KeyAction.None;
        KeyAction lastAction = KeyAction.None;

        bool eventThisFrame = false;
        bool defaultState = false;
        bool resetNextFrame = false;

        public KeyControl02(int keyValue, int idx) { _Init(keyValue, idx); }
        public KeyControl02(Keys key, int idx) { _Init((int)key, idx); }
        public KeyControl02(KbKeys key, int idx) { _Init((int)key, idx); }

        private void _Init(int keyValue, int idx)
        {
            this.idx = idx;
            key = (Keys)keyValue;
            kbkey = (KbKeys)keyValue;
            val = keyValue;
            state = KeyState.Up;
            lastState = KeyState.Up;
            action = KeyAction.None;
            lastAction = KeyAction.None;
        }


        public void ReadCurrentState(KeyState currentState)
        {
            lastState = state;
            state = currentState;
            SetKeyAction();
            eventThisFrame = true;
            defaultState = false;
            resetNextFrame = false;
        }



        public void CheckForReset()
        {
            if (state == KeyState.Up && lastState != KeyState.Up)
            {
                if (!resetNextFrame) { resetNextFrame = true; return; }
                eventThisFrame = false;
                lastState = state; // At this point this is [Up/Down], [Released/[Held or Pressed]]                
                SetKeyAction(); // At this point this is [Up/Up] [None/Released].
                //AssistFunctions.cwl($"state/lastState: [{state}/{lastState}] action/lastAction: [{action}/{lastAction}]");
                return;
            }
            if (state == KeyState.Up && lastState == KeyState.Up)
            {
                defaultState = true;
                SetKeyAction();
                return;
            }
        }
        public void Reset()
        {
            state = lastState = KeyState.Up;
        }

        private void SetKeyAction()
        {
            lastAction = action;
            if (state == KeyState.Up && lastState == KeyState.Up) { action = KeyAction.None; return; }
            if (state == KeyState.Down && lastState == KeyState.Up) { action = KeyAction.Pressed; return; }
            if (state == KeyState.Down && lastState == KeyState.Down) { action = KeyAction.Held; return; }
            if (state == KeyState.Up && lastState == KeyState.Down) { action = KeyAction.Released; return; }
        }
    }
}
