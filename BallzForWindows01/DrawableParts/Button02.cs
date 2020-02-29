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
        public bool IsCenteredOnPos { get { return centeredOnPos; } }

        RectangleD rect;
        string text;

        Font font = null;
        Color fontColor;
        int fontSize = 20;
        string fontFamily = "Arial";

        bool centeredOnPos = false;
        

        public Button02()
        {
            clsName = "Button02";
            rect = new RectangleD();
            text = clsName;
            fontColor = Color.Black;
            SetColor(255, 255, 0, 0);   // button border color
            ConfigureFont();
        }
        public void Load(string buttonText, double x, double y, double width = 0, double height = 0)
        {

            rect.SetPosition(x, y);
            if ((width == 0 || height == 0) && (!AssistFunctions.inows(buttonText))) { SetSizeFromText(buttonText); }
            else { rect.SetSize(width, height); }
            text = buttonText;
        }
        public void Load(string buttonText, RectangleD btnRect)
        {

            rect.SetPosition(btnRect.X, btnRect.Y);
            if ((btnRect.Width == 0 || btnRect.Height == 0) && (!AssistFunctions.inows(buttonText))) { SetSizeFromText(buttonText); }
            else { rect.SetSize(btnRect.Width, btnRect.Height); }
            text = buttonText;
        }
        public void Update() 
        { 
        
        }
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

        /// <summary>
        /// Centers button on rectangle position if true passed in.<br/>
        /// Otherwise, the top left corner is set to the rectnagle position.
        /// </summary>
        /// <param name="cop"></param>
        public void CenterOnPos(bool cop)
        {
            string fnId = AssistFunctions.FnId(clsName, "CenterOnPos");
            if (rect.Width == 0 || rect.Height == 0) { throw new Exception($"{fnId} Can not center because width or height of button is zero."); }
            if (centeredOnPos == cop) { return; }    // already set to bool passed in

            centeredOnPos = cop;
            if (centeredOnPos) { rect.CenterOnXY(rect.X, rect.Y); return; }
            // un center if already centered (and cop == false)
            rect.X -= rect.Width / 2;
            rect.Y -= rect.Height / 2;

        }
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
