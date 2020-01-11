// commented out 2020-01-11

//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using System.Drawing;


//namespace BallzForWindows01.GamePhysicsParts
//{
    
//    using DrawableParts;

//    // NOTE: This class is here for reference and in case I need to revert back to this version for some reason.
//    // Use CircleDV2.
//    class CircleD
//    {
//        PointD center;
//        double radius = 0;
//        double rot = 0;
        
//        CollisionPoint maincp;
//        double cpBoxSize = 2;


//        public CircleD() { _Init(0, 0, 0, 0); }
//        public CircleD(double x, double y, double radius, double rotation = 0) { _Init(x, y, radius, rotation); }
//        void _Init(double x, double y, double radius, double rotation)
//        {
//            center = new PointD(x,y);
//            maincp = new CollisionPoint(x,y,cpBoxSize,cpBoxSize, Color.Red);
//        }
//        public void Load(double x, double y, double radius, double rotation = 0)
//        {
//            _Load(x, y, radius, rotation);
//        }
//        void _Load(double x, double y, double radius, double rotation)
//        {
//            center.Set(x, y);
//            this.radius = radius;
//            rot = rotation;
//            LoadCollisionPoints();
//        }
//        void LoadCollisionPoints()
//        {
//            PointD cppos = new PointD();
//            cppos.X = (center.X + radius * Math.Cos(rot));
//            cppos.Y = (center.Y + radius * Math.Sin(rot));
//            maincp.Set(cppos.X, cppos.Y, cpBoxSize, cpBoxSize);
//        }
//        public void Update(double x, double y, double radius, double rotation)
//        {            
//            center.Set(x, y);
//            UpdateCollisionPoint(center.X, center.Y, radius, rotation);
//        }
//        void UpdateCollisionPoint(double x, double y, double radius, double rotation)
//        {
//            PointD cppos = new PointD();    // temp collision point position
//            cppos.X = x + radius * Math.Cos(rotation);
//            cppos.Y = y + radius * Math.Sin(rotation);
//            maincp.Set(cppos.X, cppos.Y, cpBoxSize, cpBoxSize);
//        }

//        public void Draw(Graphics g)
//        {
//            float len = 4;      // length of sides for center marker
//            g.FillRectangle(Brushes.Red, center.fX - (len / 2), center.fY - (len / 2), len, len);   // draw center marker for testing            
//            g.DrawEllipse(Pens.Green, (float)center.fX - ((float)radius), center.fY - ((float)radius), (float)radius * 2, (float)radius * 2);   // draw ball outline
//            maincp.Draw(g);
            
//        }




//    }
//}
//#region Notes
//// 2019-11-03 
//// Removed collision point from here and put collision points in CollisionCircleD
////           
//#endregion Notes

//#region CircleD first version - backup - working 2019-11-03
////using DrawableParts;

////    class CircleD
////{
////    PointD center;
////    double radius = 0;
////    double rot = 0;

////    CollisionPoint maincp;
////    double cpBoxSize = 2;


////    public CircleD() { _Init(0, 0, 0, 0); }
////    public CircleD(double x, double y, double radius, double rotation = 0) { _Init(x, y, radius, rotation); }
////    void _Init(double x, double y, double radius, double rotation)
////    {
////        center = new PointD(x, y);
////        maincp = new CollisionPoint(x, y, cpBoxSize, cpBoxSize, Color.Red);
////    }
////    public void Load(double x, double y, double radius, double rotation = 0)
////    {
////        _Load(x, y, radius, rotation);
////    }
////    void _Load(double x, double y, double radius, double rotation)
////    {
////        center.Set(x, y);
////        this.radius = radius;
////        rot = rotation;
////        LoadCollisionPoints();
////    }
////    void LoadCollisionPoints()
////    {
////        PointD cppos = new PointD();
////        cppos.X = (center.X + radius * Math.Cos(rot));
////        cppos.Y = (center.Y + radius * Math.Sin(rot));
////        maincp.Set(cppos.X, cppos.Y, cpBoxSize, cpBoxSize);
////    }
////    public void Update(double x, double y, double radius, double rotation)
////    {
////        center.Set(x, y);
////        UpdateCollisionPoint(center.X, center.Y, radius, rotation);
////    }
////    void UpdateCollisionPoint(double x, double y, double radius, double rotation)
////    {
////        PointD cppos = new PointD();    // temp collision point position
////        cppos.X = x + radius * Math.Cos(rotation);
////        cppos.Y = y + radius * Math.Sin(rotation);
////        maincp.Set(cppos.X, cppos.Y, cpBoxSize, cpBoxSize);
////    }

////    public void Draw(Graphics g)
////    {
////        float len = 4;      // length of sides for center marker
////        g.FillRectangle(Brushes.Red, center.fX - (len / 2), center.fY - (len / 2), len, len);   // draw center marker for testing            
////        g.DrawEllipse(Pens.Green, (float)center.fX - ((float)radius), center.fY - ((float)radius), (float)radius * 2, (float)radius * 2);   // draw ball outline
////        maincp.Draw(g);

////    }




////}
//#endregion CircleD first version - backup - working 2019-11-03