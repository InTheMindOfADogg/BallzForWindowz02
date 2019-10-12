using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01
{
    public static class DbgFuncs
    {
        static List<string> dbgStrList = new List<string>();
        static DateTime start, end;

        public static void MarkStart() { start = DateTime.Now; }
        public static void MarkEnd() { end = DateTime.Now; }
        public static TimeSpan ElapsedTime() { return (end - start); }
        public static void AddStr(string str) { dbgStrList.Add(str); }
        static Font CreateFont() { return new Font("Arial", 20, FontStyle.Regular, GraphicsUnit.Pixel); }
        public static void DrawDbgStrList(Graphics g)
        {

            if (dbgStrList.Count > 0)
            {
                AddStr($"dbgStrList.Count: {dbgStrList.Count + 1}");  // + 1 to account for the string that is added here.
                Font f = CreateFont();
                Point pos = new Point(10, 100);
                SizeF strSize = new SizeF();
                for (int i = 0; i < dbgStrList.Count; i++)
                {
                    strSize = g.MeasureString(dbgStrList[i], f);
                    g.DrawString(dbgStrList[i], f, Brushes.Black, pos);
                    pos.Y += (int)(strSize.Height + 0.49);
                }                
                //dbgStrList.RemoveRange(0, dbgStrList.Count);  
                f.Dispose();
            }
        }
        public static void FrameCleanUp()
        {
            //dbgStrList.RemoveRange(0, dbgStrList.Count);
            if (dbgStrList != null && dbgStrList.Count > 0)
            {
                dbgStrList.RemoveRange(0, dbgStrList.Count);
            }
        }
    }
}
