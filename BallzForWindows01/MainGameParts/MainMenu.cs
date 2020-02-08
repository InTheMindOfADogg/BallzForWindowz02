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
            btnList = new List<Button02>();

        }
        public override void Load()
        {

            PointD pos = new PointD(gameWindowRect.Center);

        }
        private void AddButton(double x, double y, double width, double height, string btnText, bool centerOnPos = true)
        {
            
            Button02 b = new Button02();
            b.Load(btnText, x, y, width, height);
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
            for(int i = 0; i < btnList.Count; i++)
            {
                btnList[i].CleanUp();
            }
        }



    }
}
