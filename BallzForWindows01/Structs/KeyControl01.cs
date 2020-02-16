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
        public KeyAction LastAction { get { return lastAction; } }

        

        public bool EventThisFrame { get { return eventThisFrame; } }
        public bool DefaultState { get { return defaultState; } }


        int idx = 0;
        int val = 0;
        Keys key;
        KeyState state = KeyState.Up;
        KeyState lastState = KeyState.Up;
        KeyAction action = KeyAction.None;
        KeyAction lastAction = KeyAction.None;

        bool eventThisFrame = false;
        bool defaultState = false;

        public KeyControl01(int keyValue, int idx) { _Init((Keys)keyValue, idx); }
        public KeyControl01(Keys key, int idx) { _Init(key, idx); }

        public void ReadCurrentState(KeyState currentState)
        {
            lastState = state;
            state = currentState;
            SetKeyAction();
            eventThisFrame = true;
            defaultState = false;
            resetNextFrame = false;
        }


        bool resetNextFrame = false;
        public void CheckForReset()
        {            
            if (state == KeyState.Up && lastState != KeyState.Up)
            {     
                
                if (!resetNextFrame) { resetNextFrame = true; return; }
                eventThisFrame = false;
                // At this point this is [Up/Down], [Released/[Held or Pressed]]                
                lastState = state;
                // At this point this is [Up/Up] [None/Released].
                SetKeyAction();
                //AssistFunctions.cwl($"state/lastState: [{state}/{lastState}] action/lastAction: [{action}/{lastAction}]");
                return;
            }
            if (state == KeyState.Up && lastState == KeyState.Up)
            {
                defaultState = true;
                //lastAction = action;
                SetKeyAction();
                return;
            }
        }
        public void Reset()
        {
            state = lastState = KeyState.Up;
        }



        void _Init(Keys key, int idx)
        {
            this.idx = idx;
            this.key = key;
            this.val = (int)key;
            state = KeyState.Up;
            lastState = KeyState.Up;
            action = KeyAction.None;
            lastAction = KeyAction.None;
        }

        void SetKeyAction()
        {
            lastAction = action;
            if (state == KeyState.Up && lastState == KeyState.Up) { action = KeyAction.None; return; }
            if (state == KeyState.Down && lastState == KeyState.Up) { action = KeyAction.Pressed; return; }
            if (state == KeyState.Down && lastState == KeyState.Down) { action = KeyAction.Held; return; }
            if (state == KeyState.Up && lastState == KeyState.Down) { action = KeyAction.Released; return; }
        }

        

    }
}


#region CheckForReset previous version
//public void CheckForReset()
//{
//    if (state == KeyState.Up && lastState != KeyState.Up && action == KeyAction.Released)
//    {
//        eventThisFrame = false;
//        lastState = state;
//        lastAction = action;
//        SetKeyAction();                
//        return;
//    }
//    if(state == KeyState.Up && lastState == KeyState.Up)
//    {
//        defaultState = true;
//        lastAction = action;
//        SetKeyAction();
//        return;
//    }
//}
#endregion CheckForReset previous version

#region CheckForReset previous version
//public void CheckForReset()
//{
//    if(!resetNextFrame && action == KeyAction.Released && state == KeyState.Up)
//    {
//        resetNextFrame = true;
//        return;
//    }
//    if(resetNextFrame && action == KeyAction.Released && state == KeyState.Up)
//    {
//        resetCount++;
//        lastState = state; 
//        state = KeyState.Up;
//        lastAction = action;
//        SetKeyAction();
//        resetNextFrame = false;
//        defaultState = true;
//        //action = lastAction = KeyAction.None;
//        return;
//    }
//}
#endregion CheckForReset previous version