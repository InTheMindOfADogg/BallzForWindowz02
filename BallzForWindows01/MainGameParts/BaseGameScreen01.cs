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
    class BaseGameScreen01 : BaseScreenBluePrint
    {

        public PointD Center { get { return gameWindowRect.Center; } }

        protected RectangleD gameWindowRect;

        public BaseGameScreen01(int gameWindowWidth, int gameWindowHeight, bool active = false) 
        { 
            clsName = "BaseGameScreen01";
            this.active = active;
            size = new SizeD(gameWindowWidth, gameWindowHeight);
            gameWindowRect = new RectangleD(0, 0, size.Width, size.Height);
            
        }
        public override void Load() { }
        public override void Update(MouseControls mcontrols, KeyboardControls01 kcontrols) { }
        public override void Draw(Graphics g) { }
        public override void Reset() { }
        public override void CleanUp() { }

        

    }
}
