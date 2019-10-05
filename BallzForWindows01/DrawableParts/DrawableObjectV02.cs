using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.DrawableParts
{
    class DrawableObjectV02 : DrawableObject
    {

        Rectangle clickRectangle;
        Color color;
        Point center;
        Font font;
        int fontSize = 20;
        string fontFamily = "Arial";
        Color fontColor;
        string text;
        bool placed;// = false;
        bool showClickRect;// = false;
        //bool isClicked = false;

        public Rectangle ClickRectangle { get { return clickRectangle; } }
        public Point Center { get { return center; } }
        public Color DrawColor { get { return color; } set { color = value; } }
        public Color FontColor { get { return fontColor; } set { fontColor = value; } }
        public bool IsPlaced { get { return placed; } }
        public bool ShowClickRectangle { get { return showClickRect; } set { showClickRect = value; } }
        //public bool IsClicked { get { return isClicked; } set { isClicked = value; } }

        public DrawableObjectV02()
        {
            x = 0;
            y = 0;
            width = 50;
            height = 50;
            alpha = 255;
            red = 255;
            green = 0;
            blue = 0;
            color = Color.FromArgb(alpha, red, green, blue);
            font = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            center = new Point(x + width / 2, y + height / 2);
            clickRectangle = new Rectangle(x, y, width, height);
            visible = true;
            placed = false;
            showClickRect = false;
            text = "DrawableObjectV02";
        }
        public virtual void Load()
        {

        }
        public virtual void Update()
        {

        }
        public virtual void Draw(Graphics g)
        {

        }

    }
}
