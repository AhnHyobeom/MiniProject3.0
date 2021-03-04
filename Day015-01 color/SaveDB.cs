using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Day015_01_color
{
    public partial class SaveDB : Form
    {
        String connStr = "Server=192.168.56.101;Uid=winuser;Pwd=4321;Database=image_db;Charset=UTF8";
        MySqlConnection conn; // 교량
        MySqlCommand cmd; // 트럭
        String sql = "";  // 물건박스
        MySqlDataReader reader; // 트럭이 가져올 끈
        const int RGB = 3, RR = 0, GG = 1, BB = 2;
        byte[,,] db_outImage;
        int outH, outW;
        public SaveDB(byte[,,] db_outImage, int outH, int outW)
        {
            InitializeComponent();
            this.db_outImage = (byte[,,])db_outImage.Clone();
            this.outH = outH;
            this.outW = outW;
        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int i_id = rnd.Next(0, int.MaxValue);
            String[] fname = tb_fname.Text.ToString().Split('.');
            String i_fname = fname[0];
            String i_extname = fname[1];
            long i_fsize = outH * outW;
            int i_width = outW;
            int i_height = outH;
            String i_user = "AHB";
            sql = "INSERT INTO image(i_id, i_fname, i_extname, i_fsize, i_width, i_height, i_user) VALUES (";
            sql += i_id + ", '" + i_fname + "', '" + i_extname + "', " + i_fsize + ", ";
            sql += i_width + ", " + i_height + ", '" + i_user + "')";
            cmd.CommandText = sql;  // 짐을 트럭에 싣기
            cmd.ExecuteNonQuery();
            int p_row, p_col, p_valueR, p_valueG, p_valueB;
            progressBar1.Value = 0;
            cmd = new MySqlCommand("", conn);
            progressBar1.Maximum = (int)i_fsize;
            lb_ing.Text = "0% 진행중...";
            lb_ing.Refresh();
            int ing = 0, percent = progressBar1.Maximum / 100, percentSum = progressBar1.Maximum / 100;
            for (int i = 0; i < i_width; i++)
            {
                for (int k = 0; k < i_height; k++)
                {
                    p_row = i;
                    p_col = k;
                    p_valueR = (int)db_outImage[RR, i, k];
                    p_valueG = (int)db_outImage[GG, i, k];
                    p_valueB = (int)db_outImage[BB, i, k];
                    sql = "INSERT INTO pixel(i_id, p_row, p_col, p_valueR, p_valueG, p_valueB) VALUES(";
                    sql += i_id + ", " + p_row + ", " + p_col + ", " + p_valueR + ", " + p_valueG + ", " + p_valueB + ")";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    progressBar1.Value++;
                    if (ing < 99)
                    {
                        if (int.Parse(progressBar1.Value.ToString()) >= percentSum)
                        {
                            ing++;
                            percentSum += percent;
                            lb_ing.Text = ing + "% 진행중...";
                            lb_ing.Refresh();
                        }
                    }

                }
            }
            this.DialogResult = DialogResult.OK;
        }
        /*public string LabelText
        {
            get { return lb_ing.Text; }
            set { lb_ing.Text = value; }
        }*/
        private void SaveDB_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();
            cmd = new MySqlCommand("", conn);
        }
        private void SaveDB_FormClosed(object sender, FormClosedEventArgs e)
        {
            conn.Close();
        }

    }
}
