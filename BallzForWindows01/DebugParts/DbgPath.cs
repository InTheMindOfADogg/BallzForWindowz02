using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace BallzForWindows01.DebugParts
{

    using GamePhysicsParts;

    class DbgPath
    {

        List<PointD> pathList;
        int points = 10;
        double distance = 25;

        //GameTimer timer;

        public DbgPath()
        {
            pathList = new List<PointD>();
            //timer = new GameTimer();
        }

        public void PlotPoints(PointD startPos, double angle, double drift, double timeDegradeModifier, double hardness )
        {
            
            double calcAngle = 0;
            double driftFactor = 0;            
            pathList.RemoveRange(0, pathList.Count);
            PointD tmp = new PointD();

            //timer.Update();

            tmp.Set(startPos);
            // for testing, i can represent time, ex 0 sec, 1 sec ...
            for (int i = 0; i < points; i++)
            {
                
                driftFactor = (angle - drift) * hardness;
                //calcAngle = (angle - (driftFactor * timeModifier));
                calcAngle = (angle - (driftFactor * i));
                tmp.X = tmp.X + distance * Math.Cos(calcAngle);
                tmp.Y = tmp.Y + distance * Math.Sin(calcAngle);
                pathList.Add(new PointD(tmp.X, tmp.Y));
                //DbgFuncs.AddStr($"dgbPath {i}: {pathList[i].ToString()}");
            }
        }


        public void Draw(Graphics g)
        {
            SolidBrush sb = new SolidBrush(Color.Red);            
            float width = 4;
            for(int i = 0; i < pathList.Count; i++)
            {                
                g.FillRectangle(sb, pathList[i].fX - (width / 2), pathList[i].fY - (width / 2), width, width);
            }
            DrawPathLine(g);
            sb.Dispose();
        }

        private void DrawPathLine(Graphics g)
        {
            List<PointF> plotPoints = new List<PointF>();
            for(int i = 0; i < pathList.Count; i++)
            {
                plotPoints.Add(pathList[i].ToPointF());
            }
            Pen p = new Pen(Color.Red, 1);
            g.DrawCurve(p, plotPoints.ToArray());
            p.Dispose();
        }
    }
}
