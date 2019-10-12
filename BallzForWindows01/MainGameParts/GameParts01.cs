//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Drawing;
//using System.Timers;
//using BallzForWindows01.DebugForm;


//namespace BallzForWindows01.MainGameParts
//{
//    class GameParts01
//    {
//        //protected MouseControls mcontrols;
        
//        //protected Timer t;
//        protected int seconds;
//        protected int width;
//        protected int height;
//        protected Bitmap bufferMap;
//        protected Bitmap drawMap;

//        public void InitializeGameParts(int width, int height)
//        {
//            this.width = width;
//            this.height = height;
//            //InitTimer();
//            InitBitmaps(width, height);            
//        }

//        //private void InitTimer()
//        //{
//        //    seconds = 0;
//        //    t = new Timer();
//        //    t.Interval = 1000;
//        //    t.Elapsed += GameTime_Elapsed;
//        //    t.Enabled = true;
//        //}
//        //private void GameTime_Elapsed(object sender, ElapsedEventArgs e)
//        //{
//        //    TimerElapsed();
//        //}
//        //protected virtual void TimerElapsed()
//        //{
//        //    seconds++;
//        //}
//        private void InitBitmaps(int w, int h)
//        {
//            bufferMap = new Bitmap(w, h);
//            drawMap = new Bitmap(w, h);
//        }
        
        
//    }
//}

//#region removed debug form parts - InitDebugForm was called in GameParts01.InitializeGameParts
////protected DebugForm01 debug;
////private void InitDebugForm()
////{
////    debug = new DebugForm01();
////    debug.Show();
////}
//#endregion removed debug form parts
