using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.DrawableParts
{
    using GamePhysicsParts;
    using Structs;

    class ControllableBall01 : DrawableObject
    {

        public ControllableBall01()
            :base()
        {
            clsName = "ControllableBall01";

        }
        public void Load()
        {

        }
        public void Update(MouseControls mc, KeyboardControls01 kc)
        {

        }
        public void Draw(Graphics g)
        {
            Pen p = new Pen(Color.Black);
            SolidBrush sb = new SolidBrush(Color.Black);

            

            p.Dispose();
            sb.Dispose();
        }

        
        public void Reset()
        {

        }
        public void CleanUp()
        {

        }


    }
}
