using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.GameScreensFolder
{
    using DrawableParts;
    using GamePhysicsParts;
    using Structs;
    using MainGameParts;

    class TestGameScreen02 : BaseGameScreen01
    {
        CircleDV4 circle;
        Square01 square;
        public TestGameScreen02(int gameWindowWidth, int gameWindowHeight, bool active = false)
            : base(gameWindowWidth, gameWindowHeight, active)
        {
            clsName = "TestGameScreen02";
            circle = new CircleDV4();
            square = new Square01();
        }

        public override void Load()
        {
            base.Load();
            circle.Load(400, 500, 15, 0);
        }
        public override void Update(MouseControls mcontrols, KeyboardControls01 kcontrols)
        {
            //string fnId = AssistFunctions.FnId(clsName, "Update");
            base.Update(mcontrols, kcontrols);

            //DbgFuncs.AddStr(fnId, "Active screen");
        }
        public override void Draw(Graphics g)
        {
            base.Draw(g);
            circle.Draw(g, p, sb);
        }
        public override void Reset()
        {
            base.Reset();
        }
        public override void CleanUp()
        {
            base.CleanUp();
        }
    }
}
