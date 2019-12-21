using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallzForWindows01.GamePhysicsParts
{
    using DrawableParts;
    class GameTimer
    {

        public bool Running { get { return running; } }

        public double TotalMilliseconds { get { return elapsed.TotalMilliseconds; } }
        public double TotalSeconds { get { return elapsed.TotalSeconds; } }

        string clsName = "GameTimer";
        TimeSpan elapsed;
        DateTime currentFrameTime;
        DateTime lastFrameTime;
        bool running = false;

        

        public GameTimer()
        {
            elapsed = new TimeSpan();
            currentFrameTime = DateTime.Now;
            lastFrameTime = DateTime.Now;
            //btnToggleTimer = new Button01();
        }

        public void Start()
        {
            currentFrameTime = DateTime.Now;
            lastFrameTime = DateTime.Now;
            running = true;
        }
        public void Stop()
        {
            running = false;
        }
        public void Reset()
        {
            elapsed -= elapsed;
        }
        public void Update()
        {
            string fnId = $"[{clsName}.Update]";
            if(!running) { return; }
            currentFrameTime = DateTime.Now;
            elapsed += (currentFrameTime - lastFrameTime);
            lastFrameTime = currentFrameTime;
        }

        public void DbgTxt()
        {
            string fnId = $"[{clsName}.DbgTxt]";
            DbgFuncs.AddStr($"{fnId} elapsed: {elapsed}");
            DbgFuncs.AddStr($"{fnId} lastFrameTime: {lastFrameTime}");
        }


    }
}
