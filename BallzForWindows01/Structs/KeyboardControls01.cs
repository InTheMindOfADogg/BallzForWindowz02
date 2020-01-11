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

        
        public void Update(KeyEventArgs e, KeyState keyState)
        {
            for (int i = 0; i < trackedKeys.Count; i++)
            {
                trackedKeys[i].Update(keyState);
            }
        }

        public void DbgPrint()
        {
            string fnId = FnId(clsName, "DbgPrint");

            for (int i = 0; i < trackedKeys.Count; i++)
            {
                DbgFuncs.AddStr($"{fnId} {trackedKeys[i].Key}({trackedKeys[i].Value}) state/lastState: {trackedKeys[i].State}/{trackedKeys[i].LastState} action: {trackedKeys[i].Action}");
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
