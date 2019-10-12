using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using BallzForWindows01.DrawableParts;
using BallzForWindows01.DebugForm;
using BallzForWindows01.MainGameParts;
using BallzForWindows01.Structs;

namespace BallzForWindows01
{
    public enum UpDownState
    {
        Up,
        Down
    }
    

    public class BallzForm : Form
    {
        MouseControls mcontrols = new MouseControls();
        MainGame01 ballzGame01;

        //Bitmap backBuffer = null;

        
        public BallzForm()
        {
            InitializeForm();
            #region Mouse Control Events
            MouseMove += BallzForm_MouseMove;
            MouseDown += BallzForm_MouseDown;
            MouseUp += BallzForm_MouseUp;
            #endregion Mouse Control Events

        }
        #region mouse events
        private void BallzForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { mcontrols.Update(e, MouseEventType.LeftButtonUp); }
            if (e.Button == MouseButtons.Right) { mcontrols.Update(e, MouseEventType.RightButtonUp); }
        }
        private void BallzForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { mcontrols.Update(e, MouseEventType.LeftButtonDown); }
            if (e.Button == MouseButtons.Right) { mcontrols.Update(e, MouseEventType.RightButtonDown); }
        }
        private void BallzForm_MouseMove(object sender, MouseEventArgs e)
        {
            mcontrols.Update(e, MouseEventType.Move);
        }
        #endregion mouse events
        private void InitializeForm()
        {
            // 
            // BallzForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800,700);
            this.Name = "BallzForm";
            this.Text = "Ballz Game01";
            this.StartPosition = FormStartPosition.CenterScreen;

            //backBuffer = new Bitmap(this.Width, this.Height);
        }
        public void LoadGame()
        {
            ballzGame01 = new MainGame01(this.Width, this.Height);
            ballzGame01.Load();
        }
        public void GameLoop()
        {
            while (this.Created)
            {
                GameLogic();
                RenderScene();
                Application.DoEvents();                
            }
        }


        private void GameLogic()    //Update
        {
            ballzGame01.Update(mcontrols);   // update game and draw to back buffer
        }
        private void RenderScene()
        {

            Graphics g = CreateGraphics();

            ballzGame01.Draw(g);     // currently just drawing back buffer, going with this for now.

            g.Dispose();

        }

        
        


    }
    
}


#region MouseControls - moved to own file and changed to class
//public struct MouseControls
//{
//    public int x;
//    public int y;
//    public UpDownState leftState;
//    public UpDownState rightState;
//    public UpDownState lastLeftState;
//    public UpDownState lastRightState;
//}
#endregion MouseControls - moved to own file and changed to class

#region Old update logic from BallzForm, no longer used, but keeping for reference
//private void Application_Idle(object sender, EventArgs e)
//{
//    if (IsApplicationIdle())
//    {
//        UpdateGame();
//        DrawGame();
//    }
//}
//bool IsApplicationIdle()
//{
//    NativeMessage result;
//    return PeekMessage(out result, IntPtr.Zero, 0, 0, 0) == 0;
//}
//private struct NativeMessage
//{
//    public IntPtr Handle;
//    public uint Message;
//    public IntPtr WParameter;
//    public IntPtr LParameter;
//    public uint Time;
//    public Point Location;
//}
//[DllImport("user32.dll")]
//private static extern int PeekMessage(out NativeMessage message, IntPtr window, uint filterMin, uint filterMax, uint remove);
#endregion Old update logic from BallzForm, no longer used, but keeping for reference



#region MyTimer - moved to own file 2019-10-05
//public class MyTimer
//{
//    private long StartTick = 0;
//    public MyTimer()
//    {
//        Reset();
//    }
//    public long GetTicks()
//    {
//        long currentTick = 0;
//        currentTick = GetTickCount();
//        return currentTick - StartTick;
//    }
//    public void Reset()
//    {
//        StartTick = GetTickCount();
//    }
//    [DllImport("Kernel32.dll")]
//    private static extern long GetTickCount();
//}
#endregion MyTimer - moved to own file 2019-10-05