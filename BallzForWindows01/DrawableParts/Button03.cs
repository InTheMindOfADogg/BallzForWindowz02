using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BallzForWindows01.DrawableParts
{

    // Created 2020-02-08


    using GamePhysicsParts;
    using Structs;

    


    /// <summary>
    /// Currently only used in MainMenu
    /// </summary>
    class Button03 : DrawableObject
    {

        public delegate void delButtonEvent();
        

        delButtonEvent btnEvent;
        public delButtonEvent BtnEvent { get { return btnEvent; } set { btnEvent = value; } }



        public bool IsCenteredOnPos { get { return centeredOnPos; } }
        public bool Clicked { get { return clicked; } }

        public string Text { get { return text; } set { text = value; } }

        /// <summary>
        /// String that can be used in button click event
        /// </summary>
        public string ClickValue { get { return clickValue; } set { clickValue = value; } }



        public RectangleD Rect { get { return rect; } }

        RectangleD rect;
        string text;

        Font font = null;
        Color fontColor;
        int fontSize = 20;
        string fontFamily = "Arial";
        string clickValue = "";

        bool centeredOnPos = false;
        bool mouseOver = false;
        bool clicked = false;
        

        Color hoverBackgroundColor;


        public Button03()
        {
            clsName = "Button03";
            rect = new RectangleD();
            text = clsName;
            fontColor = Color.Black;
            SetColor(255, 255, 0, 0);   // button border color
            ConfigureFont();
            ConfigureColors();
        }

        public void Load(string buttonText, double x, double y, double width = 0, double height = 0)
        {
            //pvtLoad(buttonText, x, y, width, height);
            rect.SetPosition(x, y);
            if ((width == 0 || height == 0) && (!AssistFunctions.inows(buttonText))) { SetSizeFromText(buttonText); }
            else { rect.SetSize(width, height); }
            text = buttonText;
            clickValue = buttonText;
        }

        public void Update(MouseControls mc)
        {
            //string fnId = AssistFunctions.FnId(clsName, $"({text}) Update");
            if (mc.RightButtonClicked()) { Reset(); }

            mouseOver = InBox(mc.Position);

            if (mouseOver && mc.LeftButtonClicked()) 
            { 
                clicked = true;
                btnEvent();
            }

            //DbgFuncs.AddStr(fnId, $"mouseOver: {mouseOver}");
            //DbgFuncs.AddStr(fnId, $"clicked: {clicked}");
        }
        public void Draw(Graphics g)
        {
            if (!visible) { return; }
            SolidBrush sb = new SolidBrush(fontColor);
            Pen p = new Pen(color, 2);
            PointF txtPos = new PointF();
            txtPos.X = rect.Center.fX - g.MeasureString(text, font).Width / 2;
            txtPos.Y = rect.Center.fY - g.MeasureString(text, font).Height / 2;

            if (mouseOver) { DrawHoverEffect(g, sb, p); }
            g.DrawRectangle(p, rect.fX, rect.fY, rect.fWidth, rect.fHeight);
            g.DrawString(text, font, sb, txtPos);


            p.Dispose();
            sb.Dispose();
        }


        public void Reset()
        {
            mouseOver = false;
            clicked = false;
        }

        public void CleanUp() { if (font != null) { font.Dispose(); } }
        public void ClickHandled() { clicked = false; }

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


        private void SetSizeFromText(string btnText)
        {
            Bitmap bmp = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(bmp);
            SizeF strSize = g.MeasureString(btnText, font);
            rect.Width = (int)(strSize.Width + 1.5);
            rect.Height = (int)(strSize.Height + 1.5);
            g.Dispose();
            bmp.Dispose();
        }

        private void ConfigureColors()
        {
            hoverBackgroundColor = Color.FromArgb(255, 200, 150, 100);
        }
        private void ConfigureFont()
        {
            font = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            fontColor = Color.Black;
        }

        private void DrawHoverEffect(Graphics g, SolidBrush sb, Pen p)
        {
            Color origSbColor = sb.Color;
            Color origPColor = p.Color;
            sb.Color = hoverBackgroundColor;
            g.FillRectangle(sb, rect.fX, rect.fY, rect.fWidth, rect.fHeight);
            sb.Color = origSbColor;
            p.Color = origPColor;
        }
    }
}
