using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace BallzForWindows01.DebugParts
{
    using static AssistFunctions;
    using GamePhysicsParts;

    class DbgPath
    {
        string clsName = "DbgPath";

        List<PointD> pathList;
        List<PointD> testPathList;
        int points = 10;
        double distance = 25;


        public DbgPath()
        {
            pathList = new List<PointD>();
            testPathList = new List<PointD>();
        }

        public void DbgText()
        {
            string fnId = $"[{clsName}.DbgText]";
            DbgFuncs.AddStr($"{fnId} angle: {angle} ({(angle * 180 / Math.PI):N3})");
            DbgFuncs.AddStr($"{fnId} drift: {drift} ({(drift * 180 / Math.PI):N3})");
            DbgFuncs.AddStr($"{fnId} driftHardness: {driftHardness}");
            DbgFuncs.AddStr($"{fnId} driftFactor: {driftFactor} ({(driftFactor * 180 / Math.PI):N3})");
        }
        double angle, drift, driftHardness, driftFactor;
        public void PlotPoints(PointD startPos, double angle, double drift, double timeDegradeModifier, double hardness)
        {
            this.angle = angle;
            this.drift = drift;
            this.driftHardness = hardness;


            //TestPlot02(startPos, angle, drift, timeDegradeModifier, hardness);            
            //TestPlot02d1(startPos, angle, drift, timeDegradeModifier, hardness);
            TestPlot04(startPos, angle, drift, timeDegradeModifier, hardness);
            //TestPlot04d1(startPos, angle, drift, timeDegradeModifier, hardness);
        }
        #region testing different paths
        // TestPlot01 is the original version
        void TestPlot01(PointD startPos, double angle, double drift, double timeDegradeModifier, double hardness)
        {
            double calcAngle = 0;
            double driftFactor = 0;
            pathList.RemoveRange(0, pathList.Count);
            PointD tmp = new PointD();

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
            }
        }
        void TestPlot02(PointD startPos, double angle, double drift, double timeDegradeModifier, double hardness)
        {
            string fnId = FnId(clsName, "TestPlot02");
            double calcAngle = 0;
            double driftFactor = 0;
            pathList.RemoveRange(0, pathList.Count);
            PointD tmp = new PointD();

            tmp.Set(startPos);
            // for testing, i can represent time, ex 0 sec, 1 sec ...
            driftFactor = (angle - drift);
            driftFactor *= hardness;



            for (int i = 0; i < points; i++)
            {
                calcAngle = (angle - (driftFactor * i));
                tmp.X = tmp.X + distance * Math.Cos(calcAngle);
                tmp.Y = tmp.Y + distance * Math.Sin(calcAngle);
                pathList.Add(new PointD(tmp.X, tmp.Y));
                DbgFuncs.AddStr($"{fnId} calcAngle: {calcAngle}");
            }
        }
        void TestPlot02d1(PointD startPos, double angle, double drift, double timeDegradeModifier, double hardness)
        {
            string fnId = FnId(clsName, "TestPlot02");
            double driftFactor = 0;
            pathList.RemoveRange(0, pathList.Count);
            PointD tmp = new PointD();


            List<double> tmpAngleList = new List<double>();
            double tmpCalcAngle = 0;
            driftFactor = angle - drift;
            dbgPrintAngle(fnId, "driftFactor", driftFactor);
            for (int i = 0; i < points; i++)
            {
                //driftFactor = angle - drift;
                tmpCalcAngle = angle - (driftFactor * i);
                tmpAngleList.Add(tmpCalcAngle);
                //dbgPrintAngle(fnId, "angle at {i} seconds", tmpCalcAngle);
            }

            tmp.Set(startPos);
            for (int i = 0; i < points; i++)
            {
                tmp.X = tmp.X + distance * Math.Cos(tmpAngleList[i]);
                tmp.Y = tmp.Y + distance * Math.Sin(tmpAngleList[i]);
                pathList.Add(new PointD(tmp.X, tmp.Y));
                //DbgFuncs.AddStr($"{fnId} tmpAngleList[{i}]: {tmpAngleList[i]}");
            }
        }

        void TestPlot03(PointD startPos, double angle, double drift, double timeDegradeModifier, double hardness)
        {
            string fnId = FnId(clsName, "TestPlot03");

            pathList.RemoveRange(0, pathList.Count);
            testPathList.RemoveRange(0, testPathList.Count);

            PointD tmp = new PointD();

            double calcAngle = 0;

            double adjustToAngle = 0;

            double angleAdjustAmount = 0;
            double driftAdjustAmount = 0;
            double adjustedAngle = 0;
            double adjustedDrift = 0;

            double maxDriftAdjust = 0;

            double maxDrift = 0;
            double minDrift = 0;

            double angleFinal = 0;

            //adjustToAngle = (Math.PI / 4);   // 45 degrees
            adjustToAngle = (Math.PI);   // 180 degrees

            maxDriftAdjust = 178 * Math.PI / 720;
            dbgPrintAngle(fnId, "maxDriftAdjust", maxDriftAdjust);

            angleAdjustAmount = ((adjustToAngle) - angle) * -1;
            driftAdjustAmount = ((adjustToAngle) - drift) * -1;

            dbgPrintAngle(fnId, "angleAdjustmentAmount", angleAdjustAmount);
            dbgPrintAngle(fnId, "driftAdjustmentAmount", driftAdjustAmount);


            maxDrift = adjustToAngle + maxDriftAdjust;
            minDrift = adjustToAngle - maxDriftAdjust;
            dbgPrintAngle(fnId, "maxDrift", maxDrift);
            dbgPrintAngle(fnId, "minDrift", minDrift);

            adjustedAngle = (adjustToAngle) + angleAdjustAmount;
            adjustedDrift = (adjustToAngle) + driftAdjustAmount;

            dbgPrintAngle(fnId, "adjustedAngle", adjustedAngle);
            dbgPrintAngle(fnId, "adjustedDrift", adjustedDrift);

            dbgPrintAngle(fnId, "angleAdjustmentAmount after restriction", angleAdjustAmount);
            dbgPrintAngle(fnId, "driftAdjustmentAmount after restriction", driftAdjustAmount);

            // Restricting adjusted drift amount
            if (adjustedDrift > maxDrift) adjustedDrift = maxDrift;
            if (adjustedDrift < minDrift) adjustedDrift = minDrift;

            dbgPrintAngle(fnId, "adjustedAngle after restriction", adjustedAngle);
            dbgPrintAngle(fnId, "adjustedDrift after restriction", adjustedDrift);

            List<double> tmpAngleList = new List<double>();
            double tmpCalcAngle = 0;
            for (int i = 0; i < points; i++)
            {
                driftFactor = angle - drift;
                tmpCalcAngle = angle - (driftFactor * i);
                tmpAngleList.Add(tmpCalcAngle);
            }

            // Plotting tmpPathList with tmpAngleList
            tmp.Set(startPos);
            for (int i = 0; i < points; i++)
            {
                tmp.X = tmp.X + distance * Math.Cos(tmpAngleList[i]);
                tmp.Y = tmp.Y + distance * Math.Sin(tmpAngleList[i]);
                testPathList.Add(new PointD(tmp.X, tmp.Y));
            }

            angle = angle - angleAdjustAmount;

            angleFinal = angle - angleAdjustAmount;
            dbgPrintAngle(fnId, "angleFinal", angleFinal);

            driftFactor = (angle - drift);
            driftFactor *= hardness;


            dbgPrintAngle(fnId, "angle", angle);
            tmp.Set(startPos);
            for (int i = 0; i < points; i++)
            {
                calcAngle = (angle);
                tmp.X = tmp.X + distance * Math.Cos(calcAngle);
                tmp.Y = tmp.Y + distance * Math.Sin(calcAngle);
                pathList.Add(new PointD(tmp.X, tmp.Y));
                //DbgFuncs.AddStr($"{fnId} calcAngle: {calcAngle}");
            }
        }
        void TestPlot03_GOOD_PROGRESS(PointD startPos, double angle, double drift, double timeDegradeModifier, double hardness)
        {
            string fnId = FnId(clsName, "TestPlot03_GOOD_PROGRESS");

            pathList.RemoveRange(0, pathList.Count);
            testPathList.RemoveRange(0, testPathList.Count);

            PointD tmp = new PointD();



            double calcAngle = 0;

            double adjustToAngle = 0;

            double angleAdjustAmount = 0;
            double driftAdjustAmount = 0;
            double adjustedAngle = 0;
            double adjustedDrift = 0;

            double maxDriftAdjust = 0;

            double maxDrift = 0;
            double minDrift = 0;

            double angleFinal = 0;

            //adjustToAngle = (Math.PI / 4);   // 45 degrees
            adjustToAngle = (Math.PI);   // 180 degrees

            maxDriftAdjust = 178 * Math.PI / 720;
            dbgPrintAngle(fnId, "maxDriftAdjust", maxDriftAdjust);

            angleAdjustAmount = ((adjustToAngle) - angle) * -1;
            driftAdjustAmount = ((adjustToAngle) - drift) * -1;

            dbgPrintAngle(fnId, "angleAdjustmentAmount", angleAdjustAmount);
            dbgPrintAngle(fnId, "driftAdjustmentAmount", driftAdjustAmount);


            maxDrift = adjustToAngle + maxDriftAdjust;
            minDrift = adjustToAngle - maxDriftAdjust;
            dbgPrintAngle(fnId, "maxDrift", maxDrift);
            dbgPrintAngle(fnId, "minDrift", minDrift);

            adjustedAngle = (adjustToAngle) + angleAdjustAmount;
            adjustedDrift = (adjustToAngle) + driftAdjustAmount;

            dbgPrintAngle(fnId, "adjustedAngle", adjustedAngle);
            dbgPrintAngle(fnId, "adjustedDrift", adjustedDrift);

            dbgPrintAngle(fnId, "angleAdjustmentAmount after restriction", angleAdjustAmount);
            dbgPrintAngle(fnId, "driftAdjustmentAmount after restriction", driftAdjustAmount);

            // Restricting adjusted drift amount
            if (adjustedDrift > maxDrift) adjustedDrift = maxDrift;
            if (adjustedDrift < minDrift) adjustedDrift = minDrift;

            dbgPrintAngle(fnId, "adjustedAngle after restriction", adjustedAngle);
            dbgPrintAngle(fnId, "adjustedDrift after restriction", adjustedDrift);

            List<double> tmpAngleList = new List<double>();
            double tmpCalcAngle = 0;
            for (int i = 0; i < points; i++)
            {
                driftFactor = angle - drift;
                tmpCalcAngle = angle - (driftFactor * i);
                tmpAngleList.Add(tmpCalcAngle);
            }

            // Plotting tmpPathList with tmpAngleList
            tmp.Set(startPos);
            for (int i = 0; i < points; i++)
            {
                tmp.X = tmp.X + distance * Math.Cos(tmpAngleList[i]);
                tmp.Y = tmp.Y + distance * Math.Sin(tmpAngleList[i]);
                testPathList.Add(new PointD(tmp.X, tmp.Y));
            }

            angle = angle - angleAdjustAmount;

            angleFinal = angle - angleAdjustAmount;
            dbgPrintAngle(fnId, "angleFinal", angleFinal);

            driftFactor = (angle - drift);
            driftFactor *= hardness;


            dbgPrintAngle(fnId, "angle", angle);
            tmp.Set(startPos);
            for (int i = 0; i < points; i++)
            {
                calcAngle = (angle);
                tmp.X = tmp.X + distance * Math.Cos(calcAngle);
                tmp.Y = tmp.Y + distance * Math.Sin(calcAngle);
                pathList.Add(new PointD(tmp.X, tmp.Y));
                //DbgFuncs.AddStr($"{fnId} calcAngle: {calcAngle}");
            }
        }

        void TestPlot04(PointD startPos, double angle, double drift, double timeDegradeModifier, double hardness)
        {
            string fnId = FnId(clsName, "TestPlot04");

            pathList.RemoveRange(0, pathList.Count);
            testPathList.RemoveRange(0, testPathList.Count);

            PointD tmp = new PointD();

            double calcAngle = 0;

            double adjustToAngle = 0;

            double angleAdjustAmount = 0;
            double driftAdjustAmount = 0;
            double adjustedAngle = 0;
            double adjustedDrift = 0;

            double maxDriftAdjust = 0;

            double maxDrift = 0;
            double minDrift = 0;

            double angleFinal = 0;

            //adjustToAngle = (Math.PI / 4);   // 45 degrees
            adjustToAngle = (Math.PI);   // 180 degrees

            maxDriftAdjust = 178 * Math.PI / 720;
            dbgPrintAngle(fnId, "maxDriftAdjust", maxDriftAdjust);

            angleAdjustAmount = ((adjustToAngle) - angle) * -1;
            driftAdjustAmount = ((adjustToAngle) - drift) * -1;
            dbgPrintAngle(fnId, "angleAdjustmentAmount", angleAdjustAmount);
            dbgPrintAngle(fnId, "driftAdjustmentAmount", driftAdjustAmount);


            maxDrift = adjustToAngle + maxDriftAdjust;
            minDrift = adjustToAngle - maxDriftAdjust;
            dbgPrintAngle(fnId, "maxDrift", maxDrift);
            dbgPrintAngle(fnId, "minDrift", minDrift);

            adjustedAngle = (adjustToAngle) + angleAdjustAmount;
            adjustedDrift = (adjustToAngle) + driftAdjustAmount;
            dbgPrintAngle(fnId, "adjustedAngle", adjustedAngle);
            dbgPrintAngle(fnId, "adjustedDrift", adjustedDrift);

            dbgPrintAngle(fnId, "angleAdjustmentAmount after restriction", angleAdjustAmount);
            dbgPrintAngle(fnId, "driftAdjustmentAmount after restriction", driftAdjustAmount);

            // Restricting adjusted drift amount
            if (adjustedDrift > maxDrift) adjustedDrift = maxDrift;
            if (adjustedDrift < minDrift) adjustedDrift = minDrift;

            dbgPrintAngle(fnId, "adjustedAngle after restriction", adjustedAngle);
            dbgPrintAngle(fnId, "adjustedDrift after restriction", adjustedDrift);


            // Plotting the angles for tmpPathList
            List<double> tmpAngleList = new List<double>();
            double tmpCalcAngle = 0;
            driftFactor = angle - drift;
            for (int i = 0; i < points; i++)
            {
                //driftFactor = angle - drift;
                tmpCalcAngle = angle - (driftFactor * i);
                tmpAngleList.Add(tmpCalcAngle);
            }

            // Plotting tmpPathList with tmpAngleList
            tmp.Set(startPos);
            for (int i = 0; i < points; i++)
            {
                tmp.X = tmp.X + distance * Math.Cos(tmpAngleList[i]);
                tmp.Y = tmp.Y + distance * Math.Sin(tmpAngleList[i]);
                testPathList.Add(new PointD(tmp.X, tmp.Y));
            }

            angle = angle - angleAdjustAmount;

            angleFinal = angle - angleAdjustAmount;
            dbgPrintAngle(fnId, "angleFinal", angleFinal);

            driftFactor = (angle - drift);
            driftFactor *= hardness;


            dbgPrintAngle(fnId, "angle", angle);
            tmp.Set(startPos);
            for (int i = 0; i < points; i++)
            {
                calcAngle = (angle);
                tmp.X = tmp.X + distance * Math.Cos(calcAngle);
                tmp.Y = tmp.Y + distance * Math.Sin(calcAngle);
                pathList.Add(new PointD(tmp.X, tmp.Y));
                //DbgFuncs.AddStr($"{fnId} calcAngle: {calcAngle}");
            }
        }

        void TestPlot04d1(PointD startPos, double angle, double drift, double timeDegradeModifier, double hardness)
        {
            string fnId = FnId(clsName, "TestPlot04");

            pathList.RemoveRange(0, pathList.Count);
            testPathList.RemoveRange(0, testPathList.Count);

            PointD tmp = new PointD();

            double calcAngle = 0;

            double adjustToAngle = 0;

            double angleAdjustAmount = 0;
            double driftAdjustAmount = 0;
            double adjustedAngle = 0;
            double adjustedDrift = 0;

            double maxDriftAdjust = 0;

            double maxDrift = 0;
            double minDrift = 0;

            double angleFinal = 0;

            //adjustToAngle = (Math.PI / 4);   // 45 degrees
            adjustToAngle = (Math.PI);   // 180 degrees

            maxDriftAdjust = 178 * Math.PI / 720;
            dbgPrintAngle(fnId, "maxDriftAdjust", maxDriftAdjust);

            angleAdjustAmount = ((adjustToAngle) - angle) * -1;
            driftAdjustAmount = ((adjustToAngle) - drift) * -1;
            dbgPrintAngle(fnId, "angleAdjustmentAmount", angleAdjustAmount);
            dbgPrintAngle(fnId, "driftAdjustmentAmount", driftAdjustAmount);


            maxDrift = adjustToAngle + maxDriftAdjust;
            minDrift = adjustToAngle - maxDriftAdjust;
            dbgPrintAngle(fnId, "maxDrift", maxDrift);
            dbgPrintAngle(fnId, "minDrift", minDrift);

            adjustedAngle = (adjustToAngle) + angleAdjustAmount;
            adjustedDrift = (adjustToAngle) + driftAdjustAmount;
            dbgPrintAngle(fnId, "adjustedAngle", adjustedAngle);
            dbgPrintAngle(fnId, "adjustedDrift", adjustedDrift);

            dbgPrintAngle(fnId, "angleAdjustmentAmount after restriction", angleAdjustAmount);
            dbgPrintAngle(fnId, "driftAdjustmentAmount after restriction", driftAdjustAmount);

            // Restricting adjusted drift amount
            if (adjustedDrift > maxDrift) adjustedDrift = maxDrift;
            if (adjustedDrift < minDrift) adjustedDrift = minDrift;

            dbgPrintAngle(fnId, "adjustedAngle after restriction", adjustedAngle);
            dbgPrintAngle(fnId, "adjustedDrift after restriction", adjustedDrift);


            // Plotting the angles for tmpPathList
            List<double> tmpAngleList = new List<double>();
            double tmpCalcAngle = 0;
            driftFactor = angle - drift;
            for (int i = 0; i < points; i++)
            {
                tmpCalcAngle = angle - (driftFactor * i);
                tmpAngleList.Add(tmpCalcAngle);
            }

            // Plotting tmpPathList with tmpAngleList
            tmp.Set(startPos);
            for (int i = 0; i < points; i++)
            {
                tmp.X = tmp.X + distance * Math.Cos(tmpAngleList[i]);
                tmp.Y = tmp.Y + distance * Math.Sin(tmpAngleList[i]);
                testPathList.Add(new PointD(tmp.X, tmp.Y));
            }

            angle = angle - angleAdjustAmount;

            angleFinal = angle - angleAdjustAmount;
            dbgPrintAngle(fnId, "angleFinal", angleFinal);

            driftFactor = (angle - drift);
            driftFactor *= hardness;


            dbgPrintAngle(fnId, "angle", angle);
            tmp.Set(startPos);
            for (int i = 0; i < points; i++)
            {
                calcAngle = (angle);
                tmp.X = tmp.X + distance * Math.Cos(calcAngle);
                tmp.Y = tmp.Y + distance * Math.Sin(calcAngle);
                pathList.Add(new PointD(tmp.X, tmp.Y));
                //DbgFuncs.AddStr($"{fnId} calcAngle: {calcAngle}");
            }
        }
        #endregion testing different paths

        
        public void Draw(Graphics g)
        {
            SolidBrush sb = new SolidBrush(Color.Red);
            float width = 4;
            // Plotting pathList
            for (int i = 0; i < pathList.Count; i++)
            {
                g.FillRectangle(sb, pathList[i].fX - (width / 2), pathList[i].fY - (width / 2), width, width);
            }
            DrawPathLine(g, pathList, sb.Color);

            // Plotting testPathList
            if (testPathList != null && testPathList.Count > 0)
            {
                sb.Color = Color.Green;
                for (int i = 0; i < testPathList.Count; i++)
                {
                    g.FillRectangle(sb, testPathList[i].fX - (width / 2), testPathList[i].fY - (width / 2), width, width);
                }
                DrawPathLine(g, testPathList, sb.Color);
                sb.Dispose();
            }

        }

        private void DrawPathLine(Graphics g, List<PointD> pathList, Color c)
        {
            List<PointF> plotPoints = new List<PointF>();
            for (int i = 0; i < pathList.Count; i++)
            {
                plotPoints.Add(pathList[i].ToPointF());
            }
            Pen p = new Pen(c, 1);
            g.DrawCurve(p, plotPoints.ToArray());
            p.Dispose();
        }
        private void DrawPathLine(Graphics g)
        {
            List<PointF> plotPoints = new List<PointF>();
            for (int i = 0; i < pathList.Count; i++)
            {
                plotPoints.Add(pathList[i].ToPointF());
            }
            Pen p = new Pen(Color.Red, 1);
            g.DrawCurve(p, plotPoints.ToArray());
            p.Dispose();
        }
    }
}


#region dbgPrintAngle moved to AssistFunctions
//private void dbgPrintAngle(string fnId, string text, double angle)
//{
//    DbgFuncs.AddStr($"{fnId} {text}: {angle:N3} ({(angle * 180 / Math.PI):N3})");
//}
#endregion moved to AssistFunctions