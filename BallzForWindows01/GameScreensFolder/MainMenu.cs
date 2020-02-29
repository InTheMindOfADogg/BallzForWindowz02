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

            SizeD btnSize = new SizeD(300, 50);
            Button03 b;
            double btnSpaceY = 10;
            AddBtnTestGameScreen01(ref pos, btnSpaceY, btnSize);

            AddBtnTestGameScreen02(ref pos, btnSpaceY, btnSize);

            /// 2020-02-22 probably should not put this here, but doing it for now. I have a feeling I will regret putting this here later.
            /// This is a quick fix for hiding the btnMainMenu when on the main menu.
            btnMainMenu.Visible = false;

        }
        private void AddBtnTestGameScreen01(ref PointD pos, double btnSpaceY, SizeD btnSize)
        {
            Button03 b;
            b = CreateButtonInList("TestGameScreen01", pos.X, pos.Y, true, btnSize.Width, btnSize.Height);
            b.Text = "Screen: Launch ball";
            b.ClickValue = "TestGameScreen01";  
            b.BtnEvent += SetBtnEventChangeScreen(b);
            pos.Y += b.Rect.Height + btnSpaceY;
        }
        private void AddBtnTestGameScreen02(ref PointD pos, double btnSpaceY, SizeD btnSize)
        {
            Button03 b;
            b = CreateButtonInList("TestGameScreen02", pos.X, pos.Y, true, btnSize.Width, btnSize.Height);
            b.Text = "Screen: Manually moving ball";
            b.ClickValue = "TestGameScreen02"; 
            b.BtnEvent += SetBtnEventChangeScreen(b);
            pos.Y += b.Rect.Height + btnSpaceY;
        }

        public override void Update(MouseControls mcontrols, KeyboardControls01 kcontrols)
        {
            string fnId = AssistFunctions.FnId(clsName, "Update");
            base.Update(mcontrols, kcontrols);
            //DbgFuncs.AddStr(fnId, $"testMsg: {testMsg}");
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

        public override void Activate()
        {
            base.Activate();
            btnMainMenu.Visible = false;
        }
        public override void Deactivate(bool forceReset = false)
        {
            base.Deactivate(forceReset);
            btnMainMenu.Visible = true;
        }








    }
}

#region CreateButtonInList - moved to BaseGameScreen01 2020-02-22
//private Button03 CreateButtonInList(string btnText, double x, double y, bool centerOnPos = true, double width = 0, double height = 0)
//{
//    Button03 b = new Button03();
//    b.Load(btnText, x, y, width, height);
//    b.CenterOnPos(centerOnPos);
//    btnList.Add(b);
//    return b;
//}
#endregion CreateButtonInList - moved to BaseGameScreen01 2020-02-22

#region 2020-02-22 AddButton replaced by CreateButtonInList to get reference to button after creating for further set up (ie, set click value and btn event, etc)
//private void AddButton(string btnText, double x, double y, bool centerOnPos = true, double width = 0, double height = 0)
//{
//    Button03 b = new Button03();
//    b.Load(btnText, x, y, width, height);
//    b.CenterOnPos(centerOnPos);
//    b.BtnEvent += delegate
//    {
//        testMsg = "Delegate worked!";
//        changeScreenRequest = true;
//        requestedScreen = b.ClickValue;
//    };
//    btnList.Add(b);
//}
#endregion 2020-02-22 AddButton replaced by CreateButtonInList to get reference to button after creating for further set up (ie, set click value and btn event, etc)

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