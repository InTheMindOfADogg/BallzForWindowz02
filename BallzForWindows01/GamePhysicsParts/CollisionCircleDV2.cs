using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.GamePhysicsParts
{
    using DrawableParts;

    class CollisionCircleDV2 : CircleDV3
    {

        // Properties
        public List<CollisionPoint> CollisionPointList { get { return cplist; } }
        public double MiddleCPIdx { get { return middleCpidx; } }


        List<CollisionPoint> cplist;
        int cpCount = 5;
        double cpBoxSideLength = 2;
        Color dfltColorCollisionPoints = Color.FromArgb(255, 175, 30, 50);

        double middleCpidx = 0;
        bool[] cpHitArr = null;

        double spaceBetween = 0;
        double firstCpAngle = 0;

        public CollisionCircleDV2() : base()
        {
            clsName = "CollisionCircleDV2";
            cplist = new List<CollisionPoint>();
        }
        public void Load(double x, double y, double cpBoxSideLength, double radius, double rotation, int collisionPointCount)
        {
            base.Load(x, y, radius, rotation);

            cpCount = collisionPointCount;
            this.cpBoxSideLength = cpBoxSideLength;
            if (cpCount % 2 == 0) { cpCount++; }      // adds 1 collision point if there is an even number
            middleCpidx = (int)(((double)cpCount / 2.0) + 0.5) - 1;
            cpHitArr = new bool[cpCount];
            double tempRot = rotation;
            tempRot = 0;    // for testing            
            spaceBetween = (Math.PI / 2) / ((double)cpCount / 2.0);
            tempRot -= (spaceBetween * middleCpidx);    // adjusting first point so that the points line up on front of ball
            Color cpColor = Color.Blue;
            for (int i = 0; i < cpCount; i++)
            {
                cplist.Add(CreateCollisionPoint(position.X, position.Y, this.cpBoxSideLength, radius, tempRot, cpColor));
                tempRot += spaceBetween;
                if (i == 0) { cpColor = dfltColorCollisionPoints; }
            }
        }
        public void Draw(Graphics g)
        {
            DrawCircle(g);
            DrawCollisionPoints(g);
        }

        CollisionPoint CreateCollisionPoint(double x, double y, double boxSize, double radius, double rotation, Color c)
        {
            PointD cppos = new PointD();
            cppos.X = (x + radius * Math.Cos(rotation));
            cppos.Y = (y + radius * Math.Sin(rotation));
            CollisionPoint cp = new CollisionPoint(x, y, boxSize, boxSize, c);
            return cp;
        }

        // I should probably rework this to just move the points instead of recalculating the points all over again.
        protected void UpdateCollisionPoints(double x, double y, double radius, double rotation)
        {
            PointD cppos = new PointD();
            double tempRot = rotation;
            spaceBetween = (Math.PI / 2) / ((double)cpCount / 2.0);
            tempRot -= (spaceBetween * middleCpidx);

            for (int i = 0; i < cplist.Count; i++)
            {
                cppos.X = x + radius * Math.Cos(tempRot);
                cppos.Y = y + radius * Math.Sin(tempRot);
                cplist[i].Set(cppos.X, cppos.Y, cpBoxSideLength, cpBoxSideLength);
                tempRot += spaceBetween;
            }
            //DebugTextCollisionCircle();
        }

        protected void DebugTextCollisionCircle()
        {
            if (DrawDbgTxt)
            {
                DbgFuncs.AddStr($"[{clsName}._Update]: middleCpidx: {middleCpidx}");
                DbgFuncs.AddStr($"[{clsName}._Update]: spaceBetween: {spaceBetween * 180 / Math.PI}");
                DbgFuncs.AddStr($"[{clsName}._Update]: firstCpAngle: {firstCpAngle * 180 / Math.PI}");
                DbgFuncs.AddStr($"[{clsName}._Update]: cpBoxSize: {cpBoxSideLength}");
            }
        }

        protected void DrawCollisionPoints(Graphics g)
        {
            for (int i = 0; i < cplist.Count; i++) { cplist[i].Draw(g); }
        }

        public List<int> TriggeredHitPointIdxList()
        {
            List<int> triggeredHitIdx = new List<int>();
            for (int i = 0; i < cplist.Count; i++)
            {
                cpHitArr[i] = false;
                if (cplist[i].PointHit)
                {
                    cpHitArr[i] = true;
                    triggeredHitIdx.Add(i);
                }
            }
            return triggeredHitIdx;
        }
        public int CollisionPointHit()
        {
            for (int i = 0; i < cplist.Count; i++) { if (cplist[i].PointHit && i != middleCpidx) { return i; } }
            return -1;
        }
        public int CollisionPointHit2()
        {
            for (int i = 0; i < cplist.Count; i++)
            {
                if (cplist[i].PointHit) { return i; }
            }
            return -1;
        }


    }

}

#region _Init, moved into constructor and removed parts not needed at this time 2020-01-01
//void _Init(int collisionPoints, double cpBoxSideLength)
//{
//    clsName = "CollisionCircleDV2";
//    cplist = new List<CollisionPoint>();
//    cpCount = collisionPoints;
//    this.cpBoxSideLength = cpBoxSideLength;
//}
#endregion _Init, moved into constructor and removed parts not needed at this time 2020-01-01

#region _Load, moved into Load 2020-01-01
//void _Load(int collisionPointCount, double cpBoxSideLength)
//{
//    cpCount = collisionPointCount;
//    this.cpBoxSideLength = cpBoxSideLength;
//    if (cpCount % 2 == 0) { cpCount++; }      // adds 1 collision point if there is an even number
//    middleCpidx = (int)(((double)cpCount / 2.0) + 0.5) - 1;
//    cpHitArr = new bool[cpCount];
//    double tempRot = rotation;
//    tempRot = 0;    // for testing            
//    spaceBetween = (Math.PI / 2) / ((double)cpCount / 2.0);
//    tempRot -= (spaceBetween * middleCpidx);    // adjusting first point so that the points line up on front of ball
//    Color cpColor = Color.Blue;
//    for (int i = 0; i < cpCount; i++)
//    {
//        cplist.Add(CreateCollisionPoint(position.X, position.Y, this.cpBoxSideLength, radius, tempRot, cpColor));
//        tempRot += spaceBetween;
//        if (i == 0) { cpColor = dfltColorCollisionPoints; }
//    }
//}
#endregion _Load, moved into Load 2020-01-01

#region CollisionCircleDV2 full back up before reworking some. Removing private version of funcs if only one public version 2020-01-01
//namespace BallzForWindows01.GamePhysicsParts
//{
//    using DrawableParts;

//    class CollisionCircleDV2 : CircleDV3
//    {

//        // Properties
//        public List<CollisionPoint> CollisionPointList { get { return cplist; } }
//        public double MiddleCPIdx { get { return middleCpidx; } }


//        List<CollisionPoint> cplist;
//        int cpCount = 5;
//        double cpBoxSideLength = 2;
//        Color dfltColorCollisionPoints = Color.FromArgb(255, 175, 30, 50);

//        double middleCpidx = 0;
//        bool[] cpHitArr = null;

//        double spaceBetween = 0;
//        double firstCpAngle = 0;

//        public CollisionCircleDV2() : base() { _Init(cpCount, cpBoxSideLength); }

//        public void Load(double x, double y, double cpBoxSideLength, double radius, double rotation, int collisionPointCount)
//        {
//            base.Load(x, y, radius, rotation);
//            _Load(collisionPointCount, cpBoxSideLength);
//        }

//        public void Draw(Graphics g)
//        {
//            DrawCircle(g);
//            DrawCollisionPoints(g);
//        }

//        void _Init(int collisionPoints, double cpBoxSideLength)
//        {
//            clsName = "CollisionCircleDV2";
//            cplist = new List<CollisionPoint>();
//            cpCount = collisionPoints;
//            this.cpBoxSideLength = cpBoxSideLength;
//        }
//        void _Load(int collisionPointCount, double cpBoxSideLength)
//        {
//            cpCount = collisionPointCount;
//            this.cpBoxSideLength = cpBoxSideLength;
//            if (cpCount % 2 == 0) { cpCount++; }      // adds 1 collision point if there is an even number
//            middleCpidx = (int)(((double)cpCount / 2.0) + 0.5) - 1;
//            cpHitArr = new bool[cpCount];
//            double tempRot = rotation;
//            tempRot = 0;    // for testing            
//            spaceBetween = (Math.PI / 2) / ((double)cpCount / 2.0);
//            tempRot -= (spaceBetween * middleCpidx);    // adjusting first point so that the points line up on front of ball
//            Color cpColor = Color.Blue;
//            for (int i = 0; i < cpCount; i++)
//            {
//                cplist.Add(CreateCollisionPoint(position.X, position.Y, this.cpBoxSideLength, radius, tempRot, cpColor));
//                tempRot += spaceBetween;
//                if (i == 0) { cpColor = dfltColorCollisionPoints; }
//            }
//        }

//        CollisionPoint CreateCollisionPoint(double x, double y, double boxSize, double radius, double rotation, Color c)
//        {
//            PointD cppos = new PointD();
//            cppos.X = (x + radius * Math.Cos(rotation));
//            cppos.Y = (y + radius * Math.Sin(rotation));
//            CollisionPoint cp = new CollisionPoint(x, y, boxSize, boxSize, c);
//            return cp;
//        }

//        protected void UpdateCollisionPoints(double x, double y, double radius, double rotation)
//        {
//            PointD cppos = new PointD();
//            double tempRot = rotation;
//            spaceBetween = (Math.PI / 2) / ((double)cpCount / 2.0);
//            tempRot -= (spaceBetween * middleCpidx);

//            for (int i = 0; i < cplist.Count; i++)
//            {
//                cppos.X = x + radius * Math.Cos(tempRot);
//                cppos.Y = y + radius * Math.Sin(tempRot);
//                cplist[i].Set(cppos.X, cppos.Y, cpBoxSideLength, cpBoxSideLength);
//                tempRot += spaceBetween;
//            }
//            //DebugTextCollisionCircle();
//        }

//        protected void DebugTextCollisionCircle()
//        {
//            if (DrawDbgTxt)
//            {
//                DbgFuncs.AddStr($"[{clsName}._Update]: middleCpidx: {middleCpidx}");
//                DbgFuncs.AddStr($"[{clsName}._Update]: spaceBetween: {spaceBetween * 180 / Math.PI}");
//                DbgFuncs.AddStr($"[{clsName}._Update]: firstCpAngle: {firstCpAngle * 180 / Math.PI}");
//                DbgFuncs.AddStr($"[{clsName}._Update]: cpBoxSize: {cpBoxSideLength}");
//            }
//        }

//        protected void DrawCollisionPoints(Graphics g)
//        {
//            for (int i = 0; i < cplist.Count; i++)
//            {
//                cplist[i].Draw(g);
//            }
//        }

//        public List<int> TriggeredHitPointIdxList()
//        {
//            List<int> triggeredHitIdx = new List<int>();
//            for (int i = 0; i < cplist.Count; i++)
//            {
//                cpHitArr[i] = false;
//                if (cplist[i].PointHit)
//                {
//                    cpHitArr[i] = true;
//                    triggeredHitIdx.Add(i);
//                }
//            }
//            return triggeredHitIdx;
//        }
//        public int CollisionPointHit()
//        {
//            for (int i = 0; i < cplist.Count; i++) { if (cplist[i].PointHit && i != middleCpidx) { return i; } }
//            return -1;
//        }
//        public int CollisionPointHit2()
//        {
//            for (int i = 0; i < cplist.Count; i++)
//            {
//                if (cplist[i].PointHit) { return i; }
//            }
//            return -1;
//        }


//    }

//}
#endregion CollisionCircleDV2 full back up before reworking some. Removing private version of funcs if only one public version 2020-01-01

#region removed 2019-12-07
//public override void Load(double x, double y, double radius, double rotation)
//{
//    base.Load(x, y, radius, rotation);
//    _Load(5, cpBoxSideLength);
//}
//public void Load(double x, double y, double radius, double rotation, int collisionPoints)
//{
//    base.Load(x, y, radius, rotation);
//    _Load(collisionPoints, cpBoxSideLength);
//}
#endregion removed 2019-12-07

#region 0 ref constructors, removing 2019-12-07
//public CollisionCircleDV2(double x, double y, double radius, double rotation = 0) : base(x, y, radius, rotation) { _Init(cpCount, cpBoxSideLength); }
//public CollisionCircleDV2(double x, double y, double radius, double rotation = 0, int collisionPointCount = 5, double cpBoxSideLength = 2) : base(x, y, radius, rotation) { _Init(collisionPointCount, cpBoxSideLength); }
#endregion 0 ref constructors, removing 2019-12-07

#region Same as UpdateCollisionPoints, removing 2019-12-07
//void UpdateCollisionPointList(double x, double y, double radius, double rotation)
//{
//    PointD cppos = new PointD();
//    double tempRot = rotation;
//    //double rotAdjust = (Math.PI * cplist.Count) / 2;
//    spaceBetween = (Math.PI / 2) / ((double)cpCount / 2.0);
//    tempRot -= (spaceBetween * middleCpidx);

//    for (int i = 0; i < cplist.Count; i++)
//    {
//        cppos.X = x + radius * Math.Cos(tempRot);
//        cppos.Y = y + radius * Math.Sin(tempRot);
//        cplist[i].Set(cppos.X, cppos.Y, cpBoxSideLength, cpBoxSideLength);
//        tempRot += spaceBetween;
//    }
//}
#endregion Same as UpdateCollisionPoints, removing 2019-12-07

#region previous UpdateCollisionPoints before cleaning up. looks to be same as UpdateCollisionPointList 2019-12-07
//protected void UpdateCollisionPoints(double x, double y, double radius, double rotation)
//{
//    //base._Update(x, y, radius, rotation);
//    //position.Set(x, y);
//    //this.radius = radius;
//    //this.rotation = rotation;
//    //UpdateCollisionPointList(x, y, radius, rotation);

//    PointD cppos = new PointD();
//    double tempRot = rotation;
//    //double rotAdjust = (Math.PI * cplist.Count) / 2;
//    spaceBetween = (Math.PI / 2) / ((double)cpCount / 2.0);
//    tempRot -= (spaceBetween * middleCpidx);

//    for (int i = 0; i < cplist.Count; i++)
//    {
//        cppos.X = x + radius * Math.Cos(tempRot);
//        cppos.Y = y + radius * Math.Sin(tempRot);
//        cplist[i].Set(cppos.X, cppos.Y, cpBoxSideLength, cpBoxSideLength);
//        tempRot += spaceBetween;
//    }

//    //DebugTextCollisionCircle();
//}
#endregion previous UpdateCollisionPoints before cleaning up. looks to be same as UpdateCollisionPointList 2019-12-07

#region CollisionCircleDV2 before rearranging functions 2019-12-07
//class CollisionCircleDV2 : CircleDV3
//{

//    // Properties
//    public List<CollisionPoint> CollisionPointList { get { return cplist; } }
//    public double MiddleCPIdx { get { return middleCpidx; } }


//    List<CollisionPoint> cplist;
//    int cpCount = 5;
//    double cpBoxSideLength = 2;
//    Color dfltColorCollisionPoints = Color.FromArgb(255, 175, 30, 50);

//    double middleCpidx = 0;
//    bool[] cpHitArr = null;

//    double spaceBetween = 0;
//    double firstCpAngle = 0;


//    public CollisionCircleDV2() : base() { _Init(cpCount, cpBoxSideLength); }
//    public CollisionCircleDV2(double x, double y, double radius, double rotation = 0) : base(x, y, radius, rotation) { _Init(cpCount, cpBoxSideLength); }
//    public CollisionCircleDV2(double x, double y, double radius, double rotation = 0, int collisionPointCount = 5, double cpBoxSideLength = 2) : base(x, y, radius, rotation) { _Init(collisionPointCount, cpBoxSideLength); }
//    void _Init(int collisionPoints, double cpBoxSideLength)
//    {
//        clsName = "CollisionCircleDV2";
//        cplist = new List<CollisionPoint>();
//        cpCount = collisionPoints;
//        this.cpBoxSideLength = cpBoxSideLength;
//    }

//    #region public Load signatures
//    public override void Load(double x, double y, double radius, double rotation)
//    {
//        base.Load(x, y, radius, rotation);
//        _Load(5, cpBoxSideLength);
//    }
//    public void Load(double x, double y, double radius, double rotation, int collisionPoints)
//    {
//        base.Load(x, y, radius, rotation);
//        _Load(collisionPoints, cpBoxSideLength);
//    }
//    public void Load(double x, double y, double hitBoxSideLength, double radius, double rotation, int collisionPoints)
//    {
//        base.Load(x, y, radius, rotation);
//        _Load(collisionPoints, hitBoxSideLength);
//    }
//    #endregion public Load signatures
//    void _Load(int collisionPointCount, double cpBoxSideLength)
//    {
//        cpCount = collisionPointCount;
//        this.cpBoxSideLength = cpBoxSideLength;
//        if (cpCount % 2 == 0) { cpCount++; }      // adds 1 collision point if there is an even number
//        middleCpidx = (int)(((double)cpCount / 2.0) + 0.5) - 1;
//        cpHitArr = new bool[cpCount];
//        double tempRot = rotation;
//        tempRot = 0;    // for testing            
//        spaceBetween = (Math.PI / 2) / ((double)cpCount / 2.0);
//        tempRot -= (spaceBetween * middleCpidx);    // adjusting first point so that the points line up on front of ball
//        Color cpColor = Color.Blue;
//        for (int i = 0; i < cpCount; i++)
//        {
//            cplist.Add(CreateCollisionPoint(position.X, position.Y, this.cpBoxSideLength, radius, tempRot, cpColor));
//            tempRot += spaceBetween;
//            if (i == 0) { cpColor = dfltColorCollisionPoints; }
//        }
//    }

//    #region CreateCollisionPoint signatures
//    //CollisionPoint CreateCollisionPoint(double x, double y, double radius, double rotation)
//    //{
//    //    return _CreateCollisionPoint(x, y, radius, cpBoxSideLength, rotation, dfltColorCollisionPoints);
//    //}
//    //CollisionPoint CreateCollisionPoint(double x, double y, double radius, double rotation, Color c)
//    //{
//    //    return _CreateCollisionPoint(x, y, cpBoxSideLength, radius, rotation, c);
//    //}
//    CollisionPoint CreateCollisionPoint(double x, double y, double boxSize, double radius, double rotation, Color c)
//    {
//        return _CreateCollisionPoint(x, y, boxSize, radius, rotation, c);
//    }
//    #endregion CreateCollisionPoint signatures
//    CollisionPoint _CreateCollisionPoint(double x, double y, double boxSize, double radius, double rotation, Color c)
//    {
//        PointD cppos = new PointD();
//        cppos.X = (x + radius * Math.Cos(rotation));
//        cppos.Y = (y + radius * Math.Sin(rotation));
//        CollisionPoint cp = new CollisionPoint(x, y, boxSize, boxSize, c);
//        return cp;
//    }



//    protected void UpdateCollisionPoints(double x, double y, double radius, double rotation)
//    {
//        //base._Update(x, y, radius, rotation);
//        //position.Set(x, y);
//        //this.radius = radius;
//        //this.rotation = rotation;
//        //UpdateCollisionPointList(x, y, radius, rotation);

//        PointD cppos = new PointD();
//        double tempRot = rotation;
//        //double rotAdjust = (Math.PI * cplist.Count) / 2;
//        spaceBetween = (Math.PI / 2) / ((double)cpCount / 2.0);
//        tempRot -= (spaceBetween * middleCpidx);

//        for (int i = 0; i < cplist.Count; i++)
//        {
//            cppos.X = x + radius * Math.Cos(tempRot);
//            cppos.Y = y + radius * Math.Sin(tempRot);
//            cplist[i].Set(cppos.X, cppos.Y, cpBoxSideLength, cpBoxSideLength);
//            tempRot += spaceBetween;
//        }

//        //DebugTextCollisionCircle();
//    }
//    protected void DebugTextCollisionCircle()
//    {
//        if (DrawDbgTxt)
//        {
//            DbgFuncs.AddStr($"[{clsName}._Update]: middleCpidx: {middleCpidx}");
//            DbgFuncs.AddStr($"[{clsName}._Update]: spaceBetween: {spaceBetween * 180 / Math.PI}");
//            DbgFuncs.AddStr($"[{clsName}._Update]: firstCpAngle: {firstCpAngle * 180 / Math.PI}");
//            DbgFuncs.AddStr($"[{clsName}._Update]: cpBoxSize: {cpBoxSideLength}");
//        }
//    }
//    void UpdateCollisionPointList(double x, double y, double radius, double rotation)
//    {
//        PointD cppos = new PointD();
//        double tempRot = rotation;
//        double rotAdjust = (Math.PI * cplist.Count) / 2;
//        spaceBetween = (Math.PI / 2) / ((double)cpCount / 2.0);
//        tempRot -= (spaceBetween * middleCpidx);

//        for (int i = 0; i < cplist.Count; i++)
//        {
//            cppos.X = x + radius * Math.Cos(tempRot);
//            cppos.Y = y + radius * Math.Sin(tempRot);
//            cplist[i].Set(cppos.X, cppos.Y, cpBoxSideLength, cpBoxSideLength);
//            tempRot += spaceBetween;
//        }
//    }

//    public int CollisionPointHit()
//    {
//        for (int i = 0; i < cplist.Count; i++) { if (cplist[i].PointHit && i != middleCpidx) { return i; } }
//        return -1;
//    }
//    public int CollisionPointHit2()
//    {
//        for (int i = 0; i < cplist.Count; i++)
//        {
//            if (cplist[i].PointHit) { return i; }
//        }
//        return -1;
//    }

//    public List<int> TriggeredHitPointIdxList()
//    {
//        List<int> triggeredHitIdx = new List<int>();

//        for (int i = 0; i < cplist.Count; i++)
//        {
//            cpHitArr[i] = false;
//            if (cplist[i].PointHit)
//            {
//                cpHitArr[i] = true;
//                triggeredHitIdx.Add(i);
//            }
//        }
//        return triggeredHitIdx;
//    }
//    new public void Draw(Graphics g)
//    {
//        DrawCircle(g);
//        DrawCollisionPointList(g);
//    }
//    protected void DrawCollisionPointList(Graphics g)
//    {
//        for (int i = 0; i < cplist.Count; i++)
//        {
//            cplist[i].Draw(g);
//        }
//    }



//}
#endregion CollisionCircleDV2 before rearranging functions 2019-12-07