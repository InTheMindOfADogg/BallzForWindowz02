using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallzForWindows01.GamePhysicsParts
{
    public enum LeftMiddleRight
    {
        Left,
        Middle,
        Right
    }
    public enum AboveMiddleBelow
    {
        Above,
        Middle,
        Below
    }
    public enum HitZones
    {
        LA,
        MA,
        RA,
        LM,
        MM,
        RM,
        LB,
        MB,
        RB
    }
    class BounceController
    {
        LeftMiddleRight lmr;
        AboveMiddleBelow amb;
        
        public BounceController()
        {
            lmr = LeftMiddleRight.Left;
            amb = AboveMiddleBelow.Above;

        }

        public void CalculateBounce()
        {

        }

        HitZones SetHitZone(PointD ballPos, RectangleD block)
        {
            HitZones hz = HitZones.MM;
            if(ballPos.X < )

            return hz;
        }
    }
}
