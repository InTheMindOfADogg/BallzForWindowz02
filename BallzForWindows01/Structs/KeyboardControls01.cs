using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BallzForWindows01.Structs
{
    public enum KeyboardEventType
    {
        KeyDown,
        KeyUp
    }

    class KeyboardControls01
    {

        List<KeyControl01> trackedKeys = new List<KeyControl01>();
        public KeyboardControls01()
        {

        }
        public void Update(KeyEventArgs e, KeyboardEventType t)
        {

            
        }


        public void AddTrackedKey(int keyCode)
        {
            if (CheckIfKeyTracked(keyCode)) { return; }     // prevent adding key twice
            trackedKeys.Add(new KeyControl01(keyCode, trackedKeys.Count));
        }

        bool CheckIfKeyTracked(int keyCode)
        {
            for(int i = 0; i < trackedKeys.Count; i++)
            {
                if(trackedKeys[i].KeyCode == keyCode) { return true; }
            }
            return false;
        }

    }
}
