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
        KeyboardControls01 kcontrols = new KeyboardControls01();
        MainGame01 ballzGame01;

        public BallzForm()
        {
            InitializeForm();
            #region Mouse Control Events
            MouseMove += BallzForm_MouseMove;
            MouseDown += BallzForm_MouseDown;
            MouseUp += BallzForm_MouseUp;
            #endregion Mouse Control Events

            #region Keyboard Control Events
            KeyDown += BallzForm_KeyDown;
            KeyUp += BallzForm_KeyUp;
            #endregion Keyboard Control Events

        }


        #region Keyboard events
        private void BallzForm_KeyDown(object sender, KeyEventArgs e)
        {
            kcontrols.Update(e, KeyState.Down);
        }

        private void BallzForm_KeyUp(object sender, KeyEventArgs e)
        {
            kcontrols.Update(e, KeyState.Up);
        }
        #endregion Keyboard events

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
            this.ClientSize = new System.Drawing.Size(1000, 900);
            this.Name = "BallzForm";
            this.Text = "Ballz Game01";
            this.StartPosition = FormStartPosition.CenterScreen;

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
                FrameCleanUp();
                Application.DoEvents();
            }
            CleanUp();

        }


        private void GameLogic()    //Update
        {
            ballzGame01.Update(mcontrols, kcontrols);   // update game and draw to back buffer
        }
        private void RenderScene()
        {
            Graphics g = CreateGraphics();
            ballzGame01.Draw(g);     // currently just drawing back buffer, going with this for now.            
            g.Dispose();

        }
        private void FrameCleanUp()
        {
            DbgFuncs.FrameCleanUp();
        }

        private void CleanUp()
        {
            ballzGame01.CleanUp();
        }

        #region Clearing string list test 2019-10-12
        //private void ClearingListTest()
        //{
        //    List<string> list = new List<string>();
        //    int testcnt = 1;
        //    int cnt = 50000;
        //    string[] arr = new string[cnt];
        //    for (int i = 0; i < arr.Length; i++) { arr[i] = $"Here is test string number {i + 1}"; }


        //    Console.WriteLine($"Clear test {testcnt++}: using list.Clear()");
        //    list.AddRange(arr);
        //    Console.WriteLine($"list set to {list.Count} elements");
        //    DbgFuncs.MarkStart();
        //    list.Clear();
        //    DbgFuncs.MarkEnd();
        //    Console.WriteLine($"Elements in list: {list.Count}");
        //    Console.WriteLine($"Test {testcnt} results: Clear time for a string list with {cnt} elements: {DbgFuncs.ElapsedTime().ToString()}");
        //    Console.WriteLine("\n");

        //    Console.WriteLine($"Clear test {testcnt++}: using list.RemoveRange()");
        //    list.AddRange(arr);
        //    Console.WriteLine($"list set to {list.Count} elements");
        //    DbgFuncs.MarkStart();
        //    list.RemoveRange(0, list.Count);
        //    DbgFuncs.MarkEnd();
        //    Console.WriteLine($"Elements in list: {list.Count}");
        //    Console.WriteLine($"Test {testcnt} results: Clear time for a string list with {cnt} elements: {DbgFuncs.ElapsedTime().ToString()}");
        //    Console.WriteLine("\n");

        //    Console.WriteLine($"Clear test {testcnt++}: using for loop");
        //    list.AddRange(arr);
        //    DbgFuncs.MarkStart();
        //    Console.WriteLine($"list set to {list.Count} elements");
        //    for (int i = list.Count - 1; i >= 0; i--)
        //    {
        //        list.RemoveAt(i);
        //    }
        //    DbgFuncs.MarkEnd();
        //    Console.WriteLine($"Elements in list: {list.Count}");
        //    Console.WriteLine($"Test {testcnt} results: Clear time for a string list with {cnt} elements: {DbgFuncs.ElapsedTime().ToString()}");
        //    Console.WriteLine("\n");

        //    // VERY SLOW
        //    //Console.WriteLine($"Clear test {testcnt++}: using for loop version 2");
        //    //list.AddRange(arr);
        //    //DbgFuncs.MarkStart();
        //    //Console.WriteLine($"list set to {list.Count} elements");
        //    //for(int i = 0; i < list.Count; i++)
        //    //{
        //    //    list.RemoveAt(0);
        //    //}
        //    //DbgFuncs.MarkEnd();
        //    //Console.WriteLine($"Elements in list: {list.Count}");
        //    //Console.WriteLine($"Test {testcnt} results: Clear time for a string list with {cnt} elements: {DbgFuncs.ElapsedTime().ToString()}");
        //    //Console.WriteLine("\n");
        //}
        #endregion Clearing string list test 2019-10-12

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