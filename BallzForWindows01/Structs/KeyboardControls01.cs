using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BallzForWindows01.Structs
{
    using static AssistFunctions;

    #region KeyboardEventType - Moved to EnumFiles. 2020-02-26
    //public enum KeyboardEventType
    //{
    //    KeyDown,
    //    KeyUp
    //}
    #endregion KeyboardEventType - Moved to EnumFiles. 2020-02-26
    class KeyboardControls01
    {
        string clsName = "KeyboardControls01";

        List<KeyControl01> trackedKeys = new List<KeyControl01>();

        public KeyboardControls01()
        {
            AddTrackedKey(KbKeys.Space);
            AddTrackedKey(KbKeys.Escape);

            AddTrackedKey(KbKeys.Up);
            AddTrackedKey(KbKeys.Down);
            AddTrackedKey(KbKeys.Left);
            AddTrackedKey(KbKeys.Right);
        }


        public void ReadKeyEvents(KeyEventArgs e, KeyState keyState)
        {
            for (int i = 0; i < trackedKeys.Count; i++)
            {
                if (trackedKeys[i].KeyIntValue == e.KeyValue)
                {
                    trackedKeys[i].ReadCurrentState(keyState);
                }
            }
        }
        public void Update() { for (int i = 0; i < trackedKeys.Count; i++) { if (!trackedKeys[i].DefaultState) { trackedKeys[i].CheckForReset(); } } }

        public bool KeyPressed(KbKeys key) { return _KeyPressed((int)key); }
        public bool KeyPressed(Keys key) { return _KeyPressed((int)key); }
        

        public void Reset()
        {
            for (int i = 0; i < trackedKeys.Count; i++) { trackedKeys[i].Reset(); }
        }


        public void DbgPrint()
        {
            string fnId = FnId(clsName, "DbgPrint");
            for (int i = 0; i < trackedKeys.Count; i++)
            {
                DbgFuncs.AddStr($"{fnId} {trackedKeys[i].Key}({trackedKeys[i].KeyIntValue})");
                DbgFuncs.AddStr($"{fnId} state/lastState: {trackedKeys[i].State}/{trackedKeys[i].LastState}");
                DbgFuncs.AddStr($"{fnId} action/lastAction: {trackedKeys[i].Action}/{trackedKeys[i].LastAction}");
                DbgFuncs.AddStr($"{fnId} EventThisFrame: {trackedKeys[i].EventThisFrame}");
                DbgFuncs.AddStr($"{fnId} DefaultState: {trackedKeys[i].DefaultState}");
            }

        }


        public void AddTrackedKey(int keyValue) { _AddTrackedKey(keyValue); }
        public void AddTrackedKey(Keys key) { _AddTrackedKey((int)key); }
        public void AddTrackedKey(KbKeys key) { _AddTrackedKey((int)key); }
        public void ClearTrackedKeys() { trackedKeys.RemoveRange(0, trackedKeys.Count); }



        private bool _KeyPressed(int keyval)
        {
            for (int i = 0; i < trackedKeys.Count; i++)
            {
                if (trackedKeys[i].KeyIntValue == keyval)
                {
                    if (trackedKeys[i].Action == KeyAction.Pressed) { return true; }
                }
            }
            return false;
        }
        private void _AddTrackedKey(int keyValue)
        {
            if (CheckIfKeyTracked(keyValue)) { return; }     // prevent adding key twice
            trackedKeys.Add(new KeyControl01(keyValue, trackedKeys.Count));
        }
        private bool CheckIfKeyTracked(int keyValue)
        {
            for (int i = 0; i < trackedKeys.Count; i++)
            {
                if (trackedKeys[i].KeyIntValue == keyValue) { return true; }
            }
            return false;
        }

    }
}
