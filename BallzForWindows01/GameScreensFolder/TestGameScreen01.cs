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

    class TestGameScreen01 : BaseGameScreen01
    {

        List<BasicBlock01> blockList;

        GameBall04 ball4 = null;

        List<CollisionPoint> cplist = new List<CollisionPoint>();

        CollisionLine02 cline2;
        CollisionLineMoveable01 clm;
        CollisionLineMoveable01 clm2;

        CollisionPoint02 cp02;

        public TestGameScreen01(int gameWindowWidth, int gameWindowHeight, bool active = false)
            : base(gameWindowWidth, gameWindowHeight, active)
        {
            clsName = "TestGameScreen01";


            blockList = new List<BasicBlock01>();

            ball4 = new GameBall04(size.ToSize());
            if (ball4 != null) { ball4.DrawDbgTxt = true; }

            //cline = new CollisionLine();
            cline2 = new CollisionLine02();
            clm = new CollisionLineMoveable01();
            clm2 = new CollisionLineMoveable01();

            cp02 = new CollisionPoint02();
        }
        public override void Load()
        {
            //LoadBlockList();
            // Load game ball starting position
            int ballStartX, ballStartY;


            ballStartX = 525;
            ballStartY = size.iHeight / 2 - 50;

            if (ball4 != null)
            {
                ball4.Load(ballStartX, ballStartY, 2, 10, 0, 7);
                ball4.SetCircleColor(Color.FromArgb(255, 255, 50, 50));
            }

            TESTLoadCollisionLine02();  // cline2 is greenish color

            TESTLoadCollisionMoveable01();  // clm is dark red color

            TESTLoadCollisionMoveable02();  // clm2 is bright yellowish color

            TESTLoadCollisionPoint02();
        }
        void TESTLoadCollisionLine02()
        {
            double
                startx = 550, starty = 550,
                length = 50,
                rotation = 0,
                spaceBtCp = -10;
            int
                thickness = 5;

            cline2.Load(startx, starty, length, rotation, thickness, spaceBtCp);
            cplist.AddRange(cline2.CpList);
        } // cline2
        void TESTLoadCollisionMoveable01()
        {
            double
                startx = 650,
                starty = 550,
                length = 50,
                rotation = 0,
                spaceBtCp = -10;
            int
                thickness = 5;

            clm.Load(startx, starty, length, rotation, thickness, spaceBtCp);

            cplist.AddRange(clm.CpList);
        } // clm
        void TESTLoadCollisionMoveable02()
        {
            double
                startx = 550,
                starty = 550,
                length = 50,
                rotation = 0,
                spaceBtCp = 10;
            int
                thickness = 5;

            clm2.Load(startx, starty, length, rotation, thickness, spaceBtCp);
            clm2.Color = Color.FromArgb(255, 250, 200, 150);

            cplist.AddRange(clm2.CpList);
        } // clm2
        void TESTLoadCollisionPoint02()
        {
            double
                x = 500,
                y = 500,
                detectionRadius = 10;
            cp02.Load(x, y, detectionRadius);
        }

        double lineRotationSpeed = 0; //0.005;
        public override void Update(MouseControls mc, KeyboardControls01 kc)
        {
            base.Update(mc, kc);
            // GameBall04 ball4 has mouse controls and collision detection logic built into Update function
            if (ball4 != null) { ball4.Update(mc, kc, cplist); }

            //cline.Update(lineRotationSpeed);
            //cline.Update();
            //cline2.Update();


            clm.Update(lineRotationSpeed);
            clm2.Update(-lineRotationSpeed);
        }
        public override void Draw(Graphics g)
        {
            //SolidBrush sb = new SolidBrush(Color.CornflowerBlue);
            //Pen p = new Pen(Color.Black);            
            //g.FillRectangle(sb, 0, 0, size.iWidth, size.iHeight);
            base.Draw(g);
            
            
            //DrawBlockList(g);

            if (ball4 != null) { ball4.Draw(g); }

            //DrawCollisionPointList(g);

            //cline.Draw(g, false);
            cline2.Draw(g, true);   // green
            clm.Draw(g, true);      // dark red
            clm2.Draw(g, true);     // yellowish color


            cp02.Draw(g, p, sb);

            sb.Dispose();
            p.Dispose();
        }
        public override void Reset()
        {
            ball4.Reset();
        }
        public override void CleanUp()
        {
            if (ball4 != null) { ball4.CleanUp(); }
            //CleanUpBlockList();
        }


    }
}
