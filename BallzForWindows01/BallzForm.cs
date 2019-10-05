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

namespace BallzForWindows01
{
    public enum UpDownState
    {
        Up,
        Down
    }
    public struct MouseControls
    {
        public int x;
        public int y;
        public UpDownState leftState;
        public UpDownState rightState;
        public UpDownState lastLeftState;
        public UpDownState lastRightState;
        
    }

    public partial class BallzForm : Form
    {
        public static MouseControls mcontrols = new MouseControls();
        MainGame01 ballzGame01;

        MyTimer timer = new MyTimer();
        long refreshRate = 0;
        public BallzForm()
        {
            InitializeComponent();
            #region Mouse Controls
            MouseMove += BallzForm_MouseMove;
            MouseDown += BallzForm_MouseDown;
            MouseUp += BallzForm_MouseUp;
            #endregion
            SetUpForm();

        }
        public void LoadGame()
        {
            ballzGame01.Load();
        }
        public void GameLoop()
        {
            while(this.Created)
            {
                timer.Reset();
                GameLogic();
                RenderScene();
                Application.DoEvents();
                while (timer.GetTicks() < refreshRate) ;
            }
        }
        

        private void GameLogic()    //Update
        {
            
            //UpdateGame();
            ballzGame01.Update();
            UpdateMouseControlsLastState();
            
        }
        private void RenderScene()
        {
            #region DrawGame
            DrawGame();
            //ballzGame01.Draw(this.CreateGraphics());
            #endregion DrawGame
        }


        private void BallzForm_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                mcontrols.leftState = UpDownState.Up;
            }
            if(e.Button == MouseButtons.Right)
            {
                mcontrols.rightState = UpDownState.Up;
            }
            ballzGame01.UpdateMouseControls(mcontrols);
        }
        private void BallzForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mcontrols.leftState = UpDownState.Down;
            }
            if (e.Button == MouseButtons.Right)
            {
                mcontrols.rightState = UpDownState.Down;
            }
            ballzGame01.UpdateMouseControls(mcontrols);
        }
        private void BallzForm_MouseMove(object sender, MouseEventArgs e)
        {
            mcontrols.x = e.X;
            mcontrols.y = e.Y;
            ballzGame01.UpdateMouseControls(mcontrols);
        }

        private void BallzForm_Loader(object sender, EventArgs e)
        {
            ballzGame01 = new MainGame01(this.Width, this.Height);
            LoadGame();
        }
        private void SetUpForm()
        {
            this.Width = 800;
            this.Height = 700;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        //private void UpdateGame()
        //{
        //    ballzGame01.Update();
        //    UpdateMouseControlsLastState();
        //}

        private void UpdateMouseControlsLastState()
        {
            mcontrols.lastLeftState = mcontrols.leftState;
            mcontrols.lastRightState = mcontrols.rightState;
        }
        private void DrawGame()
        {
            ballzGame01.Draw(this.CreateGraphics());
        }

        
    }

    public class MyTimer
    {
        private long StartTick = 0;
        public MyTimer()
        {
            Reset();
        }
        public long GetTicks()
        {
            long currentTick = 0;
            currentTick = GetTickCount();
            return currentTick - StartTick;
        }
        public void Reset()
        {
            StartTick = GetTickCount();
        }
        [DllImport("Kernel32.dll")]
        private static extern long GetTickCount();
    }
}



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