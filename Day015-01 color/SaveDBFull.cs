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
    public partial class SaveDBFull : Form
    {
        String connStr = "Server=192.168.56.101;Uid=winuser;Pwd=4321;Database=blob_db;Charset=UTF8";
        MySqlConnection conn; // 교량
        MySqlCommand cmd; // 트럭
        String sql = "";  // 물건박스
        int outH, outW, saveIndex;
        const int RGB = 3;
        public SaveDBFull(int outH, int outW, int saveIndex)
        {
            InitializeComponent();
            this.outH = outH;
            this.outW = outW;
            this.saveIndex = saveIndex - 1;
        }
        private void SaveDBFull_Load(object sender, EventArgs e)
        {
            //<1> 데이터베이스 연결 (교량 건설) + <2> 트럭 준비
            conn = new MySqlConnection(connStr);
            conn.Open();
            cmd = new MySqlCommand("", conn);
        }
        private void SaveDBFull_FormClosed(object sender, FormClosedEventArgs e)
        {
            conn.Close();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            /*
            CREATE TABLE blod_table (
               f_id INT NOT NULL PRIMARY KEY,		-- UUID, GUID (MySQL, C#) char(36)
               f_fname VARCHAR(50) NOT NULL,			-- 파일명
               f_extname VARCHAR(10) NOT NULL,		-- 확장명
               f_fsize BIGINT UNSIGNED NOT NULL,				-- 파일 크기
               f_data LONGBLOB
            );
            */
            Random rnd = new Random();
            int f_id = rnd.Next(int.MinValue, int.MaxValue);
            String[] fname = tb_saveFileName.Text.ToString().Split('.');
            String f_fname = fname[0];
            String f_extname = fname[1];
            long f_fsize = RGB * outH * outW;
            // 이미지 테이블(부모 테이블)에 Insert
            // <3> 물건을 준비해서, 트럭에 실어서 다리 건너 부어넣기.
            sql = "INSERT INTO blob_table(f_id, f_fname, f_extname, f_fsize, f_data) VALUES (";
            sql += f_id + ", '" + f_fname + "', '" + f_extname + "', " + f_fsize + ",";
            sql += "@BLOB_DATA" + ")";
            // 파일을 준비 임시 폴더에 저장
            String full_name = "C:\\TempImages\\" + saveIndex + "번째 이미지.png";
            FileStream fs = new FileStream(full_name, FileMode.Open, FileAccess.Read);
            byte[] blob_data = new byte[f_fsize];
            fs.Read(blob_data, 0, (int)f_fsize);
            fs.Close();
            // SET GLOBAL max_allowed_packet = 1024 * 1024 * 1024; 쿼리 최대길이 제한 수정
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@BLOB_DATA", blob_data);
            cmd.CommandText = sql;  // 짐을 트럭에 싣기
            cmd.ExecuteNonQuery();
            this.DialogResult = DialogResult.OK;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
