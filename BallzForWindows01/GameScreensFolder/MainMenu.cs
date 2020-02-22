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
    class MainMenu : BaseGameScreen01
    {
        
        
        //string testMsg = "";

        
        public MainMenu(int gameWindowWidth, int gameWindowHeight, bool active = false)
            : base(gameWindowWidth, gameWindowHeight, active)
        {
            clsName = "MainMenu";

        }
        public override void Load()
        {
            base.Load();
            PointD pos = new PointD(gameScreenRect.Center);

            //AddButton("TestGameScreen01", pos.X, pos.Y,true,300, 50);
            Button03 b;            
            double btnSpaceY = 10;
            b = CreateButtonInList("TestGameScreen01", pos.X, pos.Y, true, 300, 50);
            b.BtnEvent += SetBtnEventChangeScreen(b);      
            pos.Y += b.Rect.Height + btnSpaceY;

            b = CreateButtonInList("TestGameScreen02", pos.X, pos.Y, true, 300, 50);
            b.BtnEvent += SetBtnEventChangeScreen(b);
            pos.Y += b.Rect.Height + btnSpaceY;

        }

        public override void Update(MouseControls mcontrols, KeyboardControls01 kcontrols)
        {
            string fnId = AssistFunctions.FnId(clsName, "Update");
            base.Update(mcontrols, kcontrols);
            DbgFuncs.AddStr(fnId, $"testMsg: {testMsg}");
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);            
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

        private Button03 CreateButtonInList(string btnText, double x, double y, bool centerOnPos = true, double width = 0, double height = 0)
        {
            Button03 b = new Button03();
            b.Load(btnText, x, y, width, height);
            b.CenterOnPos(centerOnPos);
            btnList.Add(b);
            return b;
        }
        private void AddButton(string btnText, double x, double y, bool centerOnPos = true, double width = 0, double height = 0)
        {
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



#region SetBtnEventChangeScreen Moved to BaseGameScreen01
//private Button03.delButtonEvent SetBtnEventChangeScreen(Button03 b)
//{
//    Button03.delButtonEvent evnt = delegate ()
//    {
//        changeScreenRequest = true;
//        requestedScreen = b.ClickValue;
//        testMsg = $"Request to change screen to {requestedScreen}";
//    };
//    return evnt;
//}
#endregion SetBtnEventChangeScreen Moved to BaseGameScreen01