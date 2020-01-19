using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.DrawableParts
{
    using GamePhysicsParts;

    class Button02 : DrawableObject
    {
        RectangleD rect;
        string text;

        Font font = null;
        Color fontColor;
        int fontSize = 20;
        string fontFamily = "Arial";

        public Button02()
        {
            clsName = "Button02";
            rect = new RectangleD();
            text = clsName;
            fontColor = Color.Black;
            SetColor(255, 255, 0, 0);   // button border color
            ConfigureFont();
        }
        public void Load(double x, double y, double width, double height, string buttonText)
        {
            rect.SetPosition(x, y);
            if ((width == 0 || height == 0) && (!AssistFunctions.inows(buttonText))) { SetSizeFromText(buttonText); }
            else { rect.SetSize(width, height); }
            text = buttonText;
        }
        public void Update() { }
        public void Draw(Graphics g)
        {
            SolidBrush sb = new SolidBrush(fontColor);
            Pen p = new Pen(color, 5);
            PointF txtPos = new PointF();
            txtPos.X = rect.Center.fX - g.MeasureString(text, font).Width / 2;
            txtPos.Y = rect.Center.fY - g.MeasureString(text, font).Height / 2;
            if (visible)
            {
                g.DrawRectangle(p, rect.fX, rect.fY, rect.fWidth, rect.fHeight);
                g.DrawString(text, font, sb, txtPos);
            }
            p.Dispose();
            sb.Dispose();
        }

        public void CleanUp() { if (font != null) { font.Dispose(); } }

        public bool InBox(PointD p) { return rect.InBox(p.X, p.Y); }

        void SetSizeFromText(string btnText)
        {
            Bitmap bmp = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(bmp);
            SizeF strSize = g.MeasureString(btnText, font);
            rect.Width = (int)(strSize.Width + 1.5);
            rect.Height = (int)(strSize.Height + 1.5);
            g.Dispose();
            bmp.Dispose();
        }
        void ConfigureFont()
        {
            font = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            fontColor = Color.Black;
        }
    }
}
