//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Runtime.InteropServices;

//namespace BallzForWindows01.Structs
//{
//    public class MyTimer
//    {
//        private long StartTick = 0;
//        public MyTimer()
//        {
//            Reset();
//        }
//        public long GetTicks()
//        {
//            long currentTick = 0;
//            currentTick = GetTickCount();
//            return currentTick - StartTick;
//        }
//        public void Reset()
//        {
//            StartTick = GetTickCount();
//        }
//        [DllImport("Kernel32.dll")]
//        private static extern long GetTickCount();
//    }
//}
