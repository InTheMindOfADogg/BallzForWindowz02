using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace BallzForWindows01.DrawableParts
{
    using GamePhysicsParts;
    class CollisionLineMoveable01 : CollisionLine02
    {

        public CollisionLineMoveable01()
        {
            clsName = "CollisionLineMoveable01";
            SetColor(255, 150, 50, 100);
        }


        
        // TODO: adjust collision points with line.
        public void Update(double adjustRotation = 0)
        {
            string fnId = AssistFunctions.FnId(clsName, "Update");
            if (adjustRotation != 0)
            {
                rot.Adjust(adjustRotation);
                endPoint = rot.PointDFrom(startPoint, length);

                AdjustCollisionPoints();
                
            }
            DbgFuncs.AddStr($"{fnId} rot(deg): {(rot.AsDegrees):N2}");

            
            
        }

        PointD tempPos = new PointD();
        void AdjustCollisionPoints()
        {
            for(int i = 0; i < cplist.Count; i++)
            {
                tempPos = rot.PointDFrom(startPoint, startPoint.DistanceTo(cplist[i].Pos));
                cplist[i].Set(tempPos);
            }
        }

    }
}
