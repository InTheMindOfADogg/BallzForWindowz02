using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace BallzForWindows01.GamePhysicsParts
{
    using DrawableParts;

    class CircleDV2
    {
        protected PointD center;
        protected double radius = 0;
        protected double rot = 0;

        protected CollisionPoint maincp;
        protected double cpBoxSize = 2;


        public CircleDV2() { _Init(0, 0, 0, 0); }
        public CircleDV2(double x, double y, double radius, double rotation = 0) { _Init(x, y, radius, rotation); }
        void _Init(double x, double y, double radius, double rotation)
        {
            center = new PointD(x, y);
            maincp = new CollisionPoint(x, y, cpBoxSize, cpBoxSize, Color.Red);
        }
        public void Load(double x, double y, double radius, double rotation) { _Load(x, y, radius, rotation); }
        void _Load(double x, double y, double radius, double rotation)
        {
            center.Set(x, y);
            this.radius = radius;
            rot = rotation;
            LoadCollisionPoints();
        }
        void LoadCollisionPoints()
        {
            PointD cppos = new PointD();
            cppos.X = (center.X + radius * Math.Cos(rot));
            cppos.Y = (center.Y + radius * Math.Sin(rot));
            maincp.Set(cppos.X, cppos.Y, cpBoxSize, cpBoxSize);
        }
        public void Update(double x, double y, double radius, double rotation)
        {
            _Update(x, y, radius, rotation);
            //center.Set(x, y);
            //UpdateCollisionPoint(center.X, center.Y, radius, rotation);
        }
        protected virtual void _Update(double x, double y, double radius, double rotation)
        {
            center.Set(x, y);
            UpdateCollisionPoint(center.X, center.Y, radius, rotation);
        }
        void UpdateCollisionPoint(double x, double y, double radius, double rotation)
        {
            PointD cppos = new PointD();    // temp collision point position
            cppos.X = x + radius * Math.Cos(rotation);
            cppos.Y = y + radius * Math.Sin(rotation);
            maincp.Set(cppos.X, cppos.Y, cpBoxSize, cpBoxSize);
        }

        public void Draw(Graphics g)
        {
            _Draw(g);
            //float len = 4;      // length of sides for center marker
            //g.FillRectangle(Brushes.Red, center.fX - (len / 2), center.fY - (len / 2), len, len);   // draw center marker for testing            
            //g.DrawEllipse(Pens.Green, (float)center.fX - ((float)radius), center.fY - ((float)radius), (float)radius * 2, (float)radius * 2);   // draw ball outline
            //maincp.Draw(g);

        }
        protected virtual void _Draw(Graphics g)
        {
            float len = 4;      // length of sides for center marker
            g.FillRectangle(Brushes.Red, center.fX - (len / 2), center.fY - (len / 2), len, len);   // draw center marker for testing            
            g.DrawEllipse(Pens.Green, (float)center.fX - ((float)radius), center.fY - ((float)radius), (float)radius * 2, (float)radius * 2);   // draw ball outline
            maincp.Draw(g);
        }




    }
}

#region CircleD first version - backup - working 2019-11-03
//using DrawableParts;

//    class CircleD
//{
//    PointD center;
//    double radius = 0;
//    double rot = 0;

//    CollisionPoint maincp;
//    double cpBoxSize = 2;


//    public CircleD() { _Init(0, 0, 0, 0); }
//    public CircleD(double x, double y, double radius, double rotation = 0) { _Init(x, y, radius, rotation); }
//    void _Init(double x, double y, double radius, double rotation)
//    {
//        center = new PointD(x, y);
//        maincp = new CollisionPoint(x, y, cpBoxSize, cpBoxSize, Color.Red);
//    }
//    public void Load(double x, double y, double radius, double rotation = 0)
//    {
//        _Load(x, y, radius, rotation);
//    }
//    void _Load(double x, double y, double radius, double rotation)
//    {
//        center.Set(x, y);
//        this.radius = radius;
//        rot = rotation;
//        LoadCollisionPoints();
//    }
//    void LoadCollisionPoints()
//    {
//        PointD cppos = new PointD();
//        cppos.X = (center.X + radius * Math.Cos(rot));
//        cppos.Y = (center.Y + radius * Math.Sin(rot));
//        maincp.Set(cppos.X, cppos.Y, cpBoxSize, cpBoxSize);
//    }
//    public void Update(double x, double y, double radius, double rotation)
//    {
//        center.Set(x, y);
//        UpdateCollisionPoint(center.X, center.Y, radius, rotation);
//    }
//    void UpdateCollisionPoint(double x, double y, double radius, double rotation)
//    {
//        PointD cppos = new PointD();    // temp collision point position
//        cppos.X = x + radius * Math.Cos(rotation);
//        cppos.Y = y + radius * Math.Sin(rotation);
//        maincp.Set(cppos.X, cppos.Y, cpBoxSize, cpBoxSize);
//    }

//    public void Draw(Graphics g)
//    {
//        float len = 4;      // length of sides for center marker
//        g.FillRectangle(Brushes.Red, center.fX - (len / 2), center.fY - (len / 2), len, len);   // draw center marker for testing            
//        g.DrawEllipse(Pens.Green, (float)center.fX - ((float)radius), center.fY - ((float)radius), (float)radius * 2, (float)radius * 2);   // draw ball outline
//        maincp.Draw(g);

//    }




//}
#endregion CircleD first version - backup - working 2019-11-03