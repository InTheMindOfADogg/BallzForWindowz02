using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BallzForWindows01.Structs
{
    using static AssistFunctions;
    public enum KeyboardEventType
    {
        KeyDown,
        KeyUp
    }

    class KeyboardControls01
    {
        string clsName = "KeyboardControls01";

        List<KeyControl01> trackedKeys = new List<KeyControl01>();

        public KeyboardControls01()
        {
            AddTrackedKey(Keys.Space);
        }


        public void ReadKeyEvents(KeyEventArgs e, KeyState keyState)
        {
            for (int i = 0; i < trackedKeys.Count; i++)
            {
                if (trackedKeys[i].Value == e.KeyValue)
                {
                    trackedKeys[i].ReadCurrentState(keyState);
                }

            }
        }
        public void Update()
        {
            DbgPrint();
            for (int i = 0; i < trackedKeys.Count; i++)
            {
                if (!trackedKeys[i].DefaultState) 
                { 
                    trackedKeys[i].CheckForReset();
                    
                    //cwl($"state/lastState: {trackedKeys[i].State}/{trackedKeys[i].LastState} [{trackedKeys[i].Action}/{trackedKeys[i].LastAction}]");     // For debugging
                }

            }
        }


        public void DbgPrint()
        {
            string fnId = FnId(clsName, "DbgPrint");

            for (int i = 0; i < trackedKeys.Count; i++)
            {
                DbgFuncs.AddStr($"{fnId} {trackedKeys[i].Key}({trackedKeys[i].Value})");
                DbgFuncs.AddStr($"{fnId} state/lastState: {trackedKeys[i].State}/{trackedKeys[i].LastState}");
                DbgFuncs.AddStr($"{fnId} action/lastAction: {trackedKeys[i].Action}/{trackedKeys[i].LastAction}");
                DbgFuncs.AddStr($"{fnId} EventThisFrame: {trackedKeys[i].EventThisFrame}");
                DbgFuncs.AddStr($"{fnId} DefaultState: {trackedKeys[i].DefaultState}");
                


            }

        }


        public void AddTrackedKey(int keyValue) { _AddTrackedKey(keyValue); }
        public void AddTrackedKey(Keys key) { _AddTrackedKey((int)key); }

        void _AddTrackedKey(int keyValue)
        {
            if (CheckIfKeyTracked(keyValue)) { return; }     // prevent adding key twice
            trackedKeys.Add(new KeyControl01(keyValue, trackedKeys.Count));
        }
        bool CheckIfKeyTracked(int keyValue)
        {
            for (int i = 0; i < trackedKeys.Count; i++)
            {
                if (trackedKeys[i].Value == keyValue) { return true; }
            }
            return false;
        }

    }
}
