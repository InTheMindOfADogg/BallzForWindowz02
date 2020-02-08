using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.MainGameParts
{
    using DrawableParts;
    using GamePhysicsParts;
    using Structs;
    class MainMenu : BaseGameScreen01
    {
        List<Button02> btnList;
        public MainMenu(int gameWindowWidth, int gameWindowHeight, bool active = false)
            : base(gameWindowWidth, gameWindowHeight, active)
        {
            clsName = "MainMenu";
            backgroundColor = Color.CornflowerBlue;

            btnList = new List<Button02>();

        }
        public override void Load()
        {

            PointD pos = new PointD(gameWindowRect.Center);
            AddButton("test", pos.X, pos.Y);




        }
        private void AddButton(string btnText, double x, double y, bool centerOnPos = true, double width = 0, double height = 0)
        {

            RectangleD r = new RectangleD(x, y, width, height);

            Button02 b = new Button02();
            b.Load(btnText, x, y, width, height);
            b.CenterOnPos(centerOnPos);
            btnList.Add(b);
        }
        public override void Update(MouseControls mcontrols, KeyboardControls01 kcontrols)
        {
            for (int i = 0; i < btnList.Count; i++)
            {
                btnList[i].Update();
            }
        }
        public override void Draw(Graphics g)
        {
            base.Draw(g);
            string fnId = AssistFunctions.FnId(clsName, "Draw");
            DbgFuncs.AddStr(fnId, $"Active game screen");
            for (int i = 0; i < btnList.Count; i++)
            {
                btnList[i].Draw(g);
            }

            
        }
        public override void Reset()
        {
            for (int i = 0; i < btnList.Count; i++)
            {

            }
        }
        public override void CleanUp()
        {
            for (int i = 0; i < btnList.Count; i++)
            {
                btnList[i].CleanUp();
            }
        }



    }
}
