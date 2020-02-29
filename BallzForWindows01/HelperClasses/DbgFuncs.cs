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

        static Font f = null;
        public static void Init()
        {
            f = CreateFont();
        }

        public static void MarkStart() { start = DateTime.Now; }
        public static void MarkEnd() { end = DateTime.Now; }
        public static TimeSpan ElapsedTime() { return (end - start); }
        public static void AddStr(string str) { dbgStrList.Add(str); }
        public static void AddStr(string fnId, string str) { dbgStrList.Add($"{fnId} {str}"); }
        static Font CreateFont() { return new Font("Arial", 12, FontStyle.Regular/*, GraphicsUnit.Pixel*/); }

        #region DrawDbgStrList versions
        // Creating font every frame
        //public static void DrawDbgStrList(Graphics g)
        //{
        //    if (dbgStrList.Count > 0)
        //    {
        //        //AddStr($"dbgStrList.Count: {dbgStrList.Count + 1}");  // + 1 to account for the string that is added here.
        //        Font f = CreateFont();
        //        Point pos = new Point(10, 100);
        //        SizeF strSize = new SizeF();
        //        for (int i = 0; i < dbgStrList.Count; i++)
        //        {
        //            strSize = g.MeasureString(dbgStrList[i], f);
        //            g.DrawString(dbgStrList[i], f, Brushes.Black, pos);
        //            pos.Y += (int)(strSize.Height + 0.49);
        //        }
        //        f.Dispose();
        //    }
        //}

        // using static font
        public static void DrawDbgStrList(Graphics g)
        {
            if (dbgStrList.Count > 0)
            {
                //AddStr($"dbgStrList.Count: {dbgStrList.Count + 1}");  // + 1 to account for the string that is added here.
                //Font f = CreateFont();
                Point pos = new Point(10, 100);
                SizeF strSize = new SizeF();
                for (int i = 0; i < dbgStrList.Count; i++)
                {
                    strSize = g.MeasureString(dbgStrList[i], f);
                    g.DrawString(dbgStrList[i], f, Brushes.Black, pos);
                    pos.Y += (int)(strSize.Height + 0.49);
                }
                //f.Dispose();
            }
        }
        #endregion DrawDbgStrList versions

        // Remove all strings that were drawn to the screen last frame.
        public static void FrameCleanUp()
        {
            if (dbgStrList != null && dbgStrList.Count > 0)
            {
                dbgStrList.RemoveRange(0, dbgStrList.Count);
            }
        }
        public static void CleanUp()
        {
            if (f != null) { f.Dispose(); f = null; }

        }
    }
}
