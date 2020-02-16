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


        
        List<Button03> btnList;
        string testMsg = "";

        
        public MainMenu(int gameWindowWidth, int gameWindowHeight, bool active = false)
            : base(gameWindowWidth, gameWindowHeight, active)
        {
            clsName = "MainMenu";
            backgroundColor = Color.CornflowerBlue;

            btnList = new List<Button03>();

        }
        public override void Load()
        {
            base.Load();
            PointD pos = new PointD(gameScreenRect.Center);
            
            AddButton("TestGameScreen01", pos.X, pos.Y,true,300, 50);

        }
        

        public override void Update(MouseControls mcontrols, KeyboardControls01 kcontrols)
        {
            string fnId = AssistFunctions.FnId(clsName, "Update");
            base.Update(mcontrols, kcontrols);
            for (int i = 0; i < btnList.Count; i++)
            {
                btnList[i].Update(mcontrols);
            }
            DbgFuncs.AddStr(fnId, $"testMsg: {testMsg}");


        }

        

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            for (int i = 0; i < btnList.Count; i++)
            {
                btnList[i].Draw(g);
            }
            
        }
        public override void Reset()
        {
            base.Reset();
            for (int i = 0; i < btnList.Count; i++)
            {
                btnList[i].Reset();
            }
        }
        public override void CleanUp()
        {
            base.CleanUp();
            for (int i = 0; i < btnList.Count; i++)
            {
                btnList[i].CleanUp();
            }
        }


        private void AddButton(string btnText, double x, double y, bool centerOnPos = true, double width = 0, double height = 0)
        {
            RectangleD r = new RectangleD(x, y, width, height);
            Button03 b = new Button03();
            b.Load(btnText, x, y, width, height);
            b.CenterOnPos(centerOnPos);
            b.BtnEvent += delegate
            {
                testMsg = "Delegate worked!";
                changeScreenRequest = true;
                requestedScreen = b.ClickValue;
            };
            btnList.Add(b);
        }





    }
}
