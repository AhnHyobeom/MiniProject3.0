using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Day015_01_color
{
    public partial class InRangeColor : Form
    {
        public InRangeColor()
        {
            InitializeComponent();
        }
        public int colorValue = -1;
        private void btn_Red_Click(object sender, EventArgs e)
        {
            colorValue = 0;
            this.DialogResult = DialogResult.OK;
        }
        private void btn_orange_Click(object sender, EventArgs e)
        {
            colorValue = 2;
            this.DialogResult = DialogResult.OK;
        }
        private void btn_yellow_Click(object sender, EventArgs e)
        {
            colorValue = 4;
            this.DialogResult = DialogResult.OK;
        }
        private void btn_ChartreuseGreen_Click(object sender, EventArgs e)
        {
            colorValue = 6;
            this.DialogResult = DialogResult.OK;
        }
        private void btn_Green_Click(object sender, EventArgs e)
        {
            colorValue = 8;
            this.DialogResult = DialogResult.OK;
        }
        private void btn_SpringGreen_Click(object sender, EventArgs e)
        {
            colorValue = 10;
            this.DialogResult = DialogResult.OK;
        }
        private void btn_Cyan_Click(object sender, EventArgs e)
        {
            colorValue = 12;
            this.DialogResult = DialogResult.OK;
        }
        private void Azure_Click(object sender, EventArgs e)
        {
            colorValue = 14;
            this.DialogResult = DialogResult.OK;
        }
        private void btn_Blue_Click(object sender, EventArgs e)
        {
            colorValue = 16;
            this.DialogResult = DialogResult.OK;
        }
        private void btn_violet_Click(object sender, EventArgs e)
        {
            colorValue = 18;
            this.DialogResult = DialogResult.OK;
        }
        private void btn_Magenta_Click(object sender, EventArgs e)
        {
            colorValue = 20;
            this.DialogResult = DialogResult.OK;
        }
        private void btn_Rose_Click(object sender, EventArgs e)
        {
            colorValue = 22;
            this.DialogResult = DialogResult.OK;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
