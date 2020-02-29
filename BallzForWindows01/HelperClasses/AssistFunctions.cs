using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01
{
    public static class AssistFunctions
    {
        public static void cwl(string str = "") { Console.WriteLine(str); }


        public static SizeF DrawString(Graphics g, Font f, string str, PointF fontPos)
        {
            g.DrawString(str, f, Brushes.Black, fontPos);
            return g.MeasureString(str, f);
        }

        public static string FnId(string clsName, string fnName) { return $"[{clsName}.{fnName}]"; }

        #region string helper functions
        public static bool inows(string str) { return string.IsNullOrWhiteSpace(str); }


        /// <summary>
        /// Compares lowercase version of both strings and returns true if match.
        /// Created 2020-02-08
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool streql(string str1, string str2 = "")
        {
            return (string.Equals(str1.ToLower(), str2.ToLower()));
        }
        #endregion string helper functions

        public static void dbgPrintAngle(string fnId, string text, double angle)
        {
            DbgFuncs.AddStr($"{fnId} {text}: {angle:N3} ({(angle * 180 / Math.PI):N3})");
        }
        


    }

}
