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
using System.IO;

namespace Day015_01_color
{
    public partial class OpenDBFull : Form
    {
        public OpenDBFull()
        {
            InitializeComponent();
        }
        // 전역 변수
        String connStr = "Server=192.168.56.101;Uid=winuser;Pwd=4321;Database=blob_db;Charset=UTF8";
        MySqlConnection conn; // 교량
        MySqlCommand cmd; // 트럭
        String sql = "";  // 물건박스
        MySqlDataReader reader; // 트럭이 가져올 끈
        const int RGB = 3, RR = 0, GG = 1, BB = 2;
        public byte[,,] db_inImage;
        public int id_index;
        public long fsize_index;
        public int inH, inW;
        String selectedFname;
        private void OpenDBFull_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();
            cmd = new MySqlCommand("", conn);
            sql = "SELECT f_id, f_fname, f_extname, f_fsize FROM blob_table";
            cmd.CommandText = sql;  // 짐을 트럭에 싣기
            reader = cmd.ExecuteReader(); // 짐을 서버에 부어넣고, 끈으로 묶어서 끈만 가져옴.
            int f_id;
            ulong f_fsize;
            String f_fname, f_extname;
            // 끈을 당기기
            listView1.View = View.Details;
            listView1.Columns.Add("일련번호");
            listView1.Columns.Add("파일이름");
            listView1.Columns.Add("확장명");
            listView1.Columns.Add("크기");
            ListViewItem item; // 한 행의 값
            while (reader.Read())
            {
                f_id = (int)reader["f_id"];
                f_fname = (String)reader["f_fname"];
                f_extname = (String)reader["f_extname"];
                f_fsize = (ulong)reader["f_fsize"];

                item = new ListViewItem(f_id.ToString());
                item.SubItems.Add(f_fname);
                item.SubItems.Add(f_extname);
                item.SubItems.Add(f_fsize.ToString());
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
        private void OpenDBFull_FormClosed(object sender, FormClosedEventArgs e)
        {
            conn.Close();
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            // <3> 물건을 준비해서, 트럭에 실어서 다리 건너 부어넣기.
            sql = "SELECT f_id, f_fname, f_extname, f_fsize, f_data FROM blob_table";
            sql += " WHERE f_id =" + id_index;
            cmd.CommandText = sql;  // 짐을 트럭에 싣기
            reader = cmd.ExecuteReader();
            reader.Read();  // 한건만 조회됨.
            String f_fname = reader["f_fname"].ToString();
            String f_extname = reader["f_extname"].ToString();
            int f_fsize = int.Parse(reader["f_fsize"].ToString());
            byte[] f_data = new byte[f_fsize];
            reader.GetBytes(reader.GetOrdinal("f_data"), 0, f_data, 0, f_fsize);
            Random r = new Random();
            int random = r.Next(0, 999999);
            String full_name = "C:\\TempImagesDBOpen\\" + f_fname + random + "." + f_extname;

            FileStream fs = new FileStream(full_name, FileMode.OpenOrCreate, FileAccess.Write);
            fs.Write(f_data, 0, (int)f_fsize);
            fs.Close();
            reader.Close();
            Bitmap bitmap = new Bitmap(full_name);
            inH = bitmap.Width;
            inW = bitmap.Height;
            db_inImage = new byte[RGB, inH, inW];
            for(int i = 0; i < inH; i++)
            {
                for(int j = 0; j < inW; j++)
                {
                    Color c = bitmap.GetPixel(i, j);
                    db_inImage[RR, i, j] = c.R;
                    db_inImage[GG, i, j] = c.G;
                    db_inImage[BB, i, j] = c.B;
                }
            }
            this.DialogResult = DialogResult.OK;
        }
        private void btn_close_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
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
            fsize_index = long.Parse(listView1.Items[indexnum].SubItems[3].Text.ToString());
            tb_selectFileName.Text = selectedFname;
        }
    }
}
