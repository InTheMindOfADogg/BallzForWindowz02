using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.MainGameParts
{
    using GamePhysicsParts;
    using Structs;
    abstract class BaseScreenBluePrint
    {
        
        public string ClsName { get { return clsName; } }
        public SizeD ScreenSize { get { return size; } }
        public bool Active { get { return active; } set { active = value; } }

        protected string clsName = "BaseGameScreen01";
        protected SizeD size = new SizeD(100, 100);
        protected bool active = false;

        public abstract void Load();
        public abstract void Update(MouseControls mcontrols, KeyboardControls01 kcontrols);
        public abstract void Draw(Graphics g);
        public abstract void CleanUp();
        public abstract void Reset();

    }
}
