using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BallzForWindows01.DebugForm
{
    public partial class DebugForm01 : Form
    {
        public DebugForm01()
        {
            InitializeComponent();
            this.Width = 800;
            this.Height = 400;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(20, 20);
            tbDebug.TextChanged += TbDebug_TextChanged;
        }

        private void TbDebug_TextChanged(object sender, EventArgs e)
        {
            tbDebug.SelectionStart = tbDebug.TextLength;
            tbDebug.ScrollToCaret();
        }
        public void writeLine(string msg = "")
        {
            tbDebug.Text += msg + Environment.NewLine; 
        }
        public void write(string msg = "")
        {
            tbDebug.Text += msg;
        }
        public void clear()
        {
            tbDebug.Text = "";
        }
    }
}
