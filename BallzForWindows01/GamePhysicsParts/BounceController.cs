﻿using System;
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
        //LeftMiddleRight lmr;
        //AboveMiddleBelow amb;
        
        public BounceController()
        {
            //lmr = LeftMiddleRight.Left;
            //amb = AboveMiddleBelow.Above;

        }

        public void CalculateBounce()
        {

        }

        public HitZones SetHitZone(PointD ballPos, RectangleD block)
        {
            int lmr;
            int amb;
            if (ballPos.X < block.Left) { lmr = (int)LeftMiddleRight.Left; }
            else if (ballPos.X > block.Right) { lmr = (int)LeftMiddleRight.Right; }
            else lmr = (int)LeftMiddleRight.Middle;
            if (ballPos.Y < block.Top) { amb = (int)AboveMiddleBelow.Above; }
            else if (ballPos.Y > block.Bottom) { amb = (int)AboveMiddleBelow.Below; }
            else amb = (int)AboveMiddleBelow.Middle;            
            return (HitZones)(lmr + (amb * 3));            
        }
    }
}
