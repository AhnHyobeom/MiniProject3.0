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
    public partial class OpenDB : Form
    {
        String connStr = "Server=192.168.56.101;Uid=winuser;Pwd=4321;Database=image_db;Charset=UTF8";
        MySqlConnection conn; // 교량
        MySqlCommand cmd; // 트럭
        String sql = "";  // 물건박스
        MySqlDataReader reader; // 트럭이 가져올 끈
        const int RGB = 3, RR = 0, GG = 1, BB = 2;
        public byte[,,] db_inImage;
        public OpenDB()
        {
            InitializeComponent();
        }
        public int id_index, w_index, h_index;
        String selectedFname;
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count <= 0)
            {
                return;
            }
            int indexnum;
            indexnum = listView1.FocusedItem.Index;//인덱스 추출(선택한 열 위치 정보)
            id_index = int.Parse(listView1.Items[indexnum].SubItems[0].Text.ToString());
            selectedFname = listView1.Items[indexnum].SubItems[1].Text.ToString();
            selectedFname += "." + listView1.Items[indexnum].SubItems[2].Text.ToString();
            w_index = int.Parse(listView1.Items[indexnum].SubItems[3].Text.ToString());
            h_index = int.Parse(listView1.Items[indexnum].SubItems[4].Text.ToString());
            tb_selectFileName.Text = selectedFname;
        }

        private void OpenDB_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();
            cmd = new MySqlCommand("", conn);
            sql = "SELECT i_id, i_fname, i_extname, i_width, i_height FROM image"; // 짐 싸기
            cmd.CommandText = sql;  // 짐을 트럭에 싣기
            reader = cmd.ExecuteReader(); // 짐을 서버에 부어넣고, 끈으로 묶어서 끈만 가져옴.
            int i_id, i_width, i_height;
            String i_fname, i_extname;
            // 끈을 당기기
            listView1.View = View.Details;
            listView1.Columns.Add("일련번호");
            listView1.Columns.Add("파일이름");
            listView1.Columns.Add("확장명");
            listView1.Columns.Add("가로");
            listView1.Columns.Add("세로");
            ListViewItem item; // 한 행의 값
            while (reader.Read())
            {
                i_id = (int)reader["i_id"];
                i_fname = (String)reader["i_fname"];
                i_extname = (String)reader["i_extname"];
                i_width = (int)reader["i_width"];
                i_height = (int)reader["i_height"];

                item = new ListViewItem(i_id.ToString());
                item.SubItems.Add(i_fname);
                item.SubItems.Add(i_extname);
                item.SubItems.Add(i_width.ToString());
                item.SubItems.Add(i_height.ToString());
                listView1.Items.Add(item);
            }
            // 폭 조절하기 (열 사이즈에 맞춤)
            for (int i = 0; i < listView1.Columns.Count; i++)
            {
                listView1.Columns[i].TextAlign = HorizontalAlignment.Center;
                listView1.Columns[i].Width = -2;
            }
            reader.Close();
        }
        private void OpenDB_FormClosed(object sender, FormClosedEventArgs e)
        {
            conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void btn_open_Click(object sender, EventArgs e)
        {
            // <3> 물건을 준비해서, 트럭에 실어서 다리 건너 부어넣기.
            sql = "SELECT p_row, p_col, p_valueR, p_valueG, p_valueB FROM pixel WHERE i_id = " + id_index; // 짐 싸기
            cmd.CommandText = sql;  // 짐을 트럭에 싣기
            reader = cmd.ExecuteReader(); // 짐을 서버에 부어넣고, 끈으로 묶어서 끈만 가져옴.
            int row, col;
            db_inImage = new byte[RGB, h_index, w_index]; // 메모리 할당
            while (reader.Read())
            {
                row = int.Parse(reader["p_row"].ToString());
                col = int.Parse(reader["p_col"].ToString());
                db_inImage[RR, row, col] = (byte)(int.Parse(reader["p_valueR"].ToString()));
                db_inImage[GG, row, col] = (byte)(int.Parse(reader["p_valueG"].ToString()));
                db_inImage[BB, row, col] = (byte)(int.Parse(reader["p_valueB"].ToString()));
            }
            reader.Close();
            this.DialogResult = DialogResult.OK;
        }
        private void btn_close_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}