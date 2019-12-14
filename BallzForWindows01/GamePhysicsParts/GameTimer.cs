using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallzForWindows01.GamePhysicsParts
{
    class GameTimer
    {

        TimeSpan elapsed;
        DateTime lastFrameTime;


        public GameTimer()
        {
            elapsed = new TimeSpan();
            
        }

        public void Update()
        {
            DbgFuncs.AddStr($"lastFrameTime: {lastFrameTime}");
            //elapsed += (DateTime.Now - lastFrameTime);
        }


    }
}
