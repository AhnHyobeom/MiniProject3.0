using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using MySql.Data.MySqlClient;
using OpenCvSharp;

namespace Day015_01_color
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //전역 변수
        byte[,,] inImage = null, outImage = null;
        int inH, inW, outH, outW;
        string fileName;
        Bitmap paper, bitmap;
        const int RGB = 3, RR = 0, GG = 1, BB = 2, LISTSIZE = 7;
        // 마우스로 지정
        bool mouseYN = false, isUndo = false;
        int mouseSX, mouseSY, mouseEX, mouseEY;
        // Redo, Undo
        List<byte[,,]> undoList = new List<byte[,,]>();
        List<byte[,,]> redoList = new List<byte[,,]>();
        // DB
        String connStr = "Server=192.168.56.101;Uid=winuser;Pwd=4321;Database=image_db;Charset=UTF8";
        MySqlConnection conn; // 교량
        MySqlCommand cmd; // 트럭
        String sql = "";  // 물건박스
        MySqlDataReader reader; // 트럭이 가져올 끈
        int saveIndex = 0;
        String[] fileNameAry = { }; // 자동 업로드 파일 저장 배열
        // OpenCv
        Mat inCvImage, outCvImage;

        //메뉴 이벤트 처리부
        private void 블러링ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            blurrImage_CV();
        }
        private void 이진화ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            bwImage_CV();
        }
        private void 적응형이진화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bwImageA_CV();
        }
        private void 밝기조절ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            brightImage_CV();
        }
        private void 그레이스케일ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            grayscale_CV();
        }
        private void 업로드자동화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fdb = new FolderBrowserDialog(); // 새로운 폴더 선택 Dialog 를 생성합니다. 
            if (fdb.ShowDialog() != DialogResult.OK) // 폴더 선택이 정상적으로 되면 아래 코드를 실행합니다. 
            {
                return;
            }
            dfs_autoUpload(fdb.SelectedPath.ToString()); // dfs 시작
            autoUpload();
            MessageBox.Show("업로드 모두 완료");
        }
        private void 특정확장자만업로드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileNameAry = new string[0];
            FolderBrowserDialog fdb = new FolderBrowserDialog(); // 새로운 폴더 선택 Dialog 를 생성합니다. 
            if (fdb.ShowDialog() != DialogResult.OK) // 폴더 선택이 정상적으로 되면 아래 코드를 실행합니다. 
            {
                return;
            }
            String selectExt = strGetValue();
            dfs_extAutoUpload(fdb.SelectedPath.ToString(), "." + selectExt); // dfs 시작
            autoUpload();
            MessageBox.Show("업로드 모두 완료");
        }
        private void 다운로드자동화ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void dB에서통째로불러오기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDBFull odbf = new OpenDBFull();
            if (odbf.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            inImage = (byte[,,])odbf.db_inImage.Clone();
            inW = odbf.inW;
            inH = odbf.inH;
            equal_image();
        }
        private void dB에서통째로저장하기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDBFull sdbf = new SaveDBFull(outH, outW, saveIndex);
            if(sdbf.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            MessageBox.Show("업로드 완료!");
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            conn.Close();
            // 스레드 일시 중단 (사용중인 파일 삭제를 위해) 
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //폴더 삭제 (display() 마다 생성)
            String srcPath = "C:\\TempImages\\";
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            System.IO.FileInfo[] files = dir.GetFiles("*.*", SearchOption.AllDirectories);
            foreach (System.IO.FileInfo file in files)
                file.Attributes = FileAttributes.Normal;
            Directory.Delete(srcPath, true);
            //폴더 삭제 (openDBFull 마다 생성)
            srcPath = "C:\\TempImagesDBOpen\\";
            DirectoryInfo dir2 = new DirectoryInfo(srcPath);
            System.IO.FileInfo[] files2 = dir2.GetFiles("*.*", SearchOption.AllDirectories);
            foreach (System.IO.FileInfo file in files2)
                file.Attributes = FileAttributes.Normal;
            Directory.Delete(srcPath, true);
        }
        private void dB로열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDB odb = new OpenDB();
            if (odb.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            inImage = (byte[,,])odb.db_inImage.Clone();
            inW = odb.w_index;
            inH = odb.h_index;
            equal_image();
        }
        private void dB로저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDB sdb = new SaveDB(outImage, outH, outW);
            if (sdb.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            MessageBox.Show("저장 완료");
        }
        private void 채도변경ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            change_satur();
        }
        private void 실행취소ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undoImage();
        }
        private void 다시실행ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            redoImage();
        }
        private void 저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveImage();
        }
        private void 스트레칭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stretching();
        }
        private void 평활화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            equalized();
        }
        private void 엔드인탐색ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            endInSearch();
        }
        private void 히스토그램그리기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawHistogram();
        }
        private void 미디언필터ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            medianFilter();
        }
        private void erosionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            erosion();
        }
        private void dilationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dilation();
        }
        private void openingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            opening();
        }
        private void closingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closing();
        }
        private void 반전이미지ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reverseImage();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.O://열기
                        openImage_CV();
                        break;
                    case Keys.S://저장
                        break;
                    case Keys.Z://실행 취소
                        undoImage();
                        break;
                    case Keys.Y://다시 실행
                        redoImage();
                        break;
                }
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!mouseYN)
            {
                return;
            }
            mouseEX = e.X;
            mouseEY = e.Y;

            if (mouseSX > mouseEX)
            {
                int tmp = mouseSX;
                mouseSX = mouseEX;
                mouseEX = tmp;
            }
            if (mouseSY > mouseEY)
            {
                int tmp = mouseSY;
                mouseSY = mouseEY;
                mouseEY = tmp;
            }

            reverseImage();
            mouseYN = false;
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!mouseYN)
            {
                return;
            }
            mouseSX = e.X; mouseSY = e.Y;
        }
        private void 반전선택ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mouseYN = true;
        }
        private void 동일이미지ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            equal_image();
        }
        private void 이진화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bwImage();
        }
        private void 열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openImage_CV();
        }
        private void 확대ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sizeUpImage();
        }
        private void 축소ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sizeDownImage();
        }
        private void 밝게어둡게ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            brightImage();
        }
        private void 그레이스케일ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grayScale();
        }
        private void 회전ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rotateImage();
        }
        //공통 함수부
        void saveImage()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "";
            sfd.Filter = "PNG File(*.png) | *.png";
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            String saveFname = sfd.FileName;
            Bitmap image = new Bitmap(outH, outW); // 빈 비트맵(종이) 준비
            for (int i = 0; i < outH; i++)
                for (int k = 0; k < outW; k++)
                {
                    Color c;
                    int r, g, b;
                    r = outImage[RR, i, k];
                    g = outImage[GG, i, k];
                    b = outImage[BB, i, k];
                    c = Color.FromArgb(r, g, b);
                    image.SetPixel(i, k, c);  // 종이에 콕콕 찍기
                }
            // 상단에 using System.Drawing.Imaging; 추가해야 함
            image.Save(saveFname, ImageFormat.Png); // 종이를 PNG로 저장
            toolStripStatusLabel1.Text = saveFname + "으로 저장됨.";
        }
        void openImage_CV()
        {
            OpenFileDialog ofd = new OpenFileDialog();  // 객체 생성
            ofd.DefaultExt = "";
            ofd.Filter = "칼라 필터 | *.png; *.jpg; *.bmp; *.tif";
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            fileName = ofd.FileName;
            // 파일 --> openCv용 Matrix
            inCvImage = Cv2.ImRead(fileName);
            Cv2.Transpose(inCvImage, inCvImage);
            inH = inCvImage.Height;
            inW = inCvImage.Width;
            inImage = new byte[RGB, inH, inW]; // 메모리 할당
            // openCV 이미지 --> 메모리 (로딩)
            for (int i = 0; i < inH; i++)
            {
                for (int j = 0; j < inW; j++)
                {
                    var c = inCvImage.At<Vec3b>(i, j);
                    inImage[RR, i, j] = c.Item2;
                    inImage[GG, i, j] = c.Item1;
                    inImage[BB, i, j] = c.Item0;
                }
            }
            equal_image();
        }
        void equal_image()
        {
            if (inImage == null)
            {
                return;
            }
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            if(isUndo)
            {
                outH = inImage.GetLength(1);
                outW = inImage.GetLength(2);
            }
            outImage = new byte[RGB, outH, outW];
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < outH; i++)
                {
                    for (int j = 0; j < outW; j++)
                    {
                        outImage[rgb, i, j] = inImage[rgb, i, j];
                    }
                }
            }
            displayImage();
        }
        void displayImage()
        {
            if (!isUndo)//undo로 접근하지 않았을때
            {
                redoList.Clear();//새로운 작업시 리스트를 초기화
                if (undoList.Count == LISTSIZE)
                {//LISTSIZE이상 저장하지 않음
                    undoList.RemoveAt(0);
                }
                undoList.Add(outImage);
            }
            /*byte[] f_data = new byte[RGB * outH * outW];
            System.Buffer.BlockCopy(outImage, 0, f_data, 0, RGB * sizeof(byte) * outH * outW);*/
            String full_name = "C:\\TempImages\\" + saveIndex++ + "번째 이미지.png";
            Bitmap image = new Bitmap(outH, outW); // 빈 비트맵(종이) 준비
            for (int i = 0; i < outH; i++)
            {
                for (int k = 0; k < outW; k++)
                {
                    Color c;
                    int r, g, b;
                    r = outImage[0, i, k];
                    g = outImage[1, i, k];
                    b = outImage[2, i, k];
                    c = Color.FromArgb(r, g, b);
                    image.SetPixel(i, k, c);  // 종이에 콕콕 찍기
                }
            }
            image.Save(full_name, ImageFormat.Png); // 종이를 PNG로 저장
            // 벽, 게시판, 종이 크기 조절    900, 600
            paper = new Bitmap(outH, outW); // 종이
            pictureBox1.Size = new System.Drawing.Size(outH, outW); // 캔버스
            this.Size = new System.Drawing.Size(outH + 20, outW + 80); // 벽
            Color pen; // 펜(콕콕 찍을 용도)
            for (int i = 0; i < outH; i++)
            {
                for (int j = 0; j < outW; j++)
                {
                    byte r = outImage[RR,i, j]; // 잉크(색상값)
                    byte g = outImage[GG,i, j]; // 잉크(색상값)
                    byte b = outImage[BB,i, j]; // 잉크(색상값)
                    pen = Color.FromArgb(r, g, b); // 펜에 잉크 묻히기
                    paper.SetPixel(i, j, pen); // 종이에 콕 찍기
                }
            }
            pictureBox1.Image = paper; // 게시판에 종이를 붙이기.
            //실행 취소 -> 파일경로 구현 못함
            toolStripStatusLabel1.Text =
                outH.ToString() + "x" + outW.ToString() + "  " + fileName;
        }
        double getValue()
        {
            subform sub = new subform();//서브폼 준비
            sub.lb_input.Visible = true;
            sub.numUp_value.Visible = true;
            sub.lb_ext.Visible = false;
            sub.tb_GetString.Visible = false;
            if (sub.ShowDialog() == DialogResult.Cancel)
            {
                return 0.0;
            }
            double value = (double)sub.numUp_value.Value;
            return value;
        }
        String strGetValue()
        {
            subform sub = new subform();//서브폼 준비
            sub.lb_ext.Visible = true;
            sub.tb_GetString.Visible = true;
            sub.lb_input.Visible = false;
            sub.numUp_value.Visible = false;
            if (sub.ShowDialog() == DialogResult.Cancel)
            {
                return "";
            }
            String value = sub.tb_GetString.Text.ToString();
            return value;
        }
        private void 엠보싱ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            embossImage();
        }
        private void 블러링ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            blurrImage();
        }
        private void 샤프닝ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sharpImage();
        }
        //영상처리 함수부
        private void dfs_autoUpload(String fullName)
        { // dfs 알고리즘
            DirectoryInfo di = new DirectoryInfo(fullName);
            foreach (FileInfo File in di.GetFiles()) // 선택 폴더의 파일 목록을 스캔합니다.
            {
                Array.Resize(ref fileNameAry, fileNameAry.Length + 1);
                fileNameAry[fileNameAry.Length - 1] = File.DirectoryName.ToString() + "\\" + File.Name.ToString();// 개별 파일 별로 정보를 추가합니다.
            }
            DirectoryInfo[] di_sub = di.GetDirectories(); // 하위 폴더 목록들의 정보 가져옵니다.
            if (di_sub.Length > 0)
            { // dfs
                for (int i = 0; i < di_sub.Length; i++)
                {
                    dfs_autoUpload(di_sub[i].FullName);
                }
            }
        }
        private void autoUpload()
        {
            String connStr2 = "Server=192.168.56.101;Uid=winuser;Pwd=4321;Database=blob_db;Charset=UTF8";
            MySqlConnection conn2 = new MySqlConnection(connStr2); // 교량
            MySqlCommand cmd2; // 트럭
            conn2.Open();
            cmd2 = new MySqlCommand("", conn2);
            Random rnd = new Random();
            for (int i = 0; i < fileNameAry.Length; i++)
            {
                string fullname = fileNameAry[i];
                int f_id = rnd.Next(int.MinValue, int.MaxValue);
                String f_fname = Path.GetFileNameWithoutExtension(fullname);
                String f_extname = Path.GetExtension(fullname);
                f_extname = f_extname.Replace(".", "");
                Bitmap tempB = new Bitmap(fullname);
                ulong f_fsize = (ulong)(tempB.Width * tempB.Height * RGB);
                sql = "INSERT INTO blob_table(f_id, f_fname, f_extname, f_fsize, f_data) VALUES (";
                sql += f_id + ", '" + f_fname + "', '" + f_extname + "', " + f_fsize + ",";
                sql += "@BLOB_DATA" + ")";
                FileStream fs = new FileStream(fullname, FileMode.Open, FileAccess.Read);
                byte[] blob_data = new byte[f_fsize];
                fs.Read(blob_data, 0, (int)f_fsize);
                fs.Close();

                cmd2.Parameters.Clear();
                cmd2.Parameters.AddWithValue("@BLOB_DATA", blob_data);
                cmd2.CommandText = sql;  // 짐을 트럭에 싣기
                cmd2.ExecuteNonQuery();
            }
            conn2.Close();
        }
        private void dfs_extAutoUpload(String fullName, String ext)
        {
            DirectoryInfo di = new DirectoryInfo(fullName);
            foreach (FileInfo File in di.GetFiles()) // 선택 폴더의 파일 목록을 스캔합니다.
            {
                if (File.Extension == ext)
                {
                    Array.Resize(ref fileNameAry, fileNameAry.Length + 1);
                    fileNameAry[fileNameAry.Length - 1] = File.DirectoryName.ToString() + "\\" + File.Name.ToString();// 개별 파일 별로 정보를 추가합니다.
                }
            }
            DirectoryInfo[] di_sub = di.GetDirectories(); // 하위 폴더 목록들의 정보 가져옵니다.
            if (di_sub.Length > 0)
            { // dfs
                for (int i = 0; i < di_sub.Length; i++)
                {
                    dfs_extAutoUpload(di_sub[i].FullName, ext);
                }
            }
        }
        void brightImage()
        {
            if (inImage == null)
            {
                return;
            }
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            int value = (int)getValue();
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < inH; i++)
                {
                    for (int j = 0; j < inW; j++)
                    {
                        if(inImage[rgb, i, j] + value > 255)
                        {
                            outImage[rgb, i, j] = 255;
                        } else if(inImage[rgb, i, j] + value < 0)
                        {
                            outImage[rgb, i, j] = 0;
                        } else
                        {
                            outImage[rgb, i, j] = (byte)(inImage[rgb, i, j] + value);
                        }
                    }
                }
            }
            displayImage();
        }
        void grayScale()
        {
            if (inImage == null)
            {
                return;
            }
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            for (int i = 0; i < inH; i++)
            {
                for (int j = 0; j < inW; j++)
                {
                    int hap = inImage[RR, i, j] + inImage[GG, i, j] + inImage[BB, i, j];
                    byte rgb = (byte)(hap / 3.0); 
                    outImage[RR, i, j] = rgb;
                    outImage[GG, i, j] = rgb;
                    outImage[BB, i, j] = rgb;
                }
            }
            displayImage();
        }
        void reverseImage()
        {
            if (inImage == null)
            {
                return;
            }
            outH = inH;
            outW = inW;
            outImage = new byte[RGB, outH, outW];

            if (!mouseYN)
            {
                mouseSX = 0; mouseEX = inH;
                mouseSY = 0; mouseEY = inW;
            }

            for (int i = 0; i < inH; i++)
            {
                for (int j = 0; j < inW; j++)
                {
                    if ((mouseSX <= i && i <= mouseEX) && (mouseSY <= j && j <= mouseEY))
                    {
                        outImage[RR, i, j] = (byte)(255 - inImage[RR, i, j]);
                        outImage[GG, i, j] = (byte)(255 - inImage[GG, i, j]);
                        outImage[BB, i, j] = (byte)(255 - inImage[BB, i, j]);
                    }
                    else
                    {
                        outImage[RR, i, j] = inImage[RR, i, j];
                        outImage[GG, i, j] = inImage[GG, i, j];
                        outImage[BB, i, j] = inImage[BB, i, j];
                    }
                }
            }
            displayImage();
        }
        void bwImage()
        {
            if (inImage == null)
            {
                return;
            }
            outH = inH;
            outW = inW;
            outImage = new byte[RGB, outH, outW];
            for (int i = 0; i < inH; i++)
            {
                for (int j = 0; j < inW; j++)
                {
                    if (inImage[RR, i, j] > 127)
                    {
                        outImage[RR, i, j] = 255;
                    } else if(inImage[GG, i, j] > 127)
                    {
                        outImage[GG, i, j] = 255;
                    } else if (inImage[BB, i, j] > 127)
                    {
                        outImage[BB, i, j] = 255;
                    }
                    else
                    {
                        outImage[RR, i, j] = 0;
                        outImage[GG, i, j] = 0;
                        outImage[BB, i, j] = 0;
                    }
                }
            }
            displayImage();
        }
        void change_satur()
        {
            if (inImage == null)
            {
                return;
            }
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            Color c;//한점 색상
            double hh, ss, vv;//색상, 채도, 밝기
            int rr, gg, bb;//red, green, blue
            double value = getValue();
            for (int i = 0; i < outH; i++)
            {
                for (int j = 0; j < outW; j++)
                {
                    rr = inImage[RR, i, j];
                    gg = inImage[GG, i, j];
                    bb = inImage[BB, i, j];
                    //RGB -> HSV(HSB)
                    c = Color.FromArgb(rr, gg, bb);
                    hh = c.GetHue();
                    ss = c.GetSaturation();
                    vv = c.GetBrightness();

                    //(핵심!)채도 올리기
                    ss += value;
                    //HSV -> RGB
                    HsvToRgb(hh, ss, vv, out rr, out gg, out bb);
                    outImage[RR, i, j] = (byte)rr;
                    outImage[GG, i, j] = (byte)gg;
                    outImage[BB, i, j] = (byte)bb;
                }
            }
            displayImage();
        }
        void HsvToRgb(double h, double S, double V, out int r, out int g, out int b)
        {
            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {
                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;
                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;
                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;
                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;
                    default:
                        R = G = B = V;
                        break;
                }
            }
            r = CheckRange((int)(R * 255.0));
            g = CheckRange((int)(G * 255.0));
            b = CheckRange((int)(B * 255.0));

            int CheckRange(int i)
            {
                if (i < 0) return 0;
                if (i > 255) return 255;
                return i;
            }
        }
        void sizeUpImage()
        {//확대 알고리즘
            if (inImage == null)
            {
                return;
            }
            int mul = (int)getValue();
            outH = inH * mul;
            outW = inW * mul;
            outImage = new byte[RGB, outH, outW];
            for (int i = 0; i < outH; i++)
            {
                for (int j = 0; j < outW; j++)
                {
                    outImage[RR, i, j] = inImage[RR, i / mul, j / mul];
                    outImage[GG, i, j] = inImage[GG, i / mul, j / mul];
                    outImage[BB, i, j] = inImage[BB, i / mul, j / mul];
                }
            }
            displayImage();
        }
        void sizeDownImage()
        {//축소 알고리즘
            if (inImage == null)
            {
                return;
            }
            int div;
            div = (int)getValue();
            outH = inH / div;
            outW = inW / div;
            outImage = new byte[RGB, outH, outW];
            int sumR, sumG, sumB;
            for (int i = 0; i < outH; i++)
            {//평균값으로 계산
                for (int j = 0; j < outW; j++)
                {
                    sumR = 0;
                    sumG = 0;
                    sumB = 0;
                    for (int k = 0; k < div; k++)
                    {
                        for (int m = 0; m < div; m++)
                        {
                            sumR = sumR + inImage[RR, i * div + k, j * div + m];
                            sumG = sumG + inImage[GG, i * div + k, j * div + m];
                            sumB = sumB + inImage[BB, i * div + k, j * div + m];
                        }
                    }
                    outImage[RR, i, j] = (byte)(sumR / (double)(div * div));
                    outImage[GG, i, j] = (byte)(sumG / (double)(div * div));
                    outImage[BB, i, j] = (byte)(sumB / (double)(div * div));
                }
            }
            displayImage();
        }
        void rotateImage()
        {//회전 알고리즘
            if (inImage == null)
            {
                return;
            }
            int degree = (int)getValue();
            outH = inH;
            outW = inW;
            outImage = new byte[RGB, outH, outW];
            int center_w = inW / 2;//중심축 계산
            int center_h = inH / 2;
            int new_w;
            int new_h;
            double pi = 3.141592;
            double seta = pi / (180.0 / degree);
            //회전 알고리즘
            for (int i = 0; i < inH; i++)
            {
                for (int j = 0; j < inW; j++)
                {
                    new_w = (int)((i - center_h) * Math.Sin(seta) + (j - center_w) * Math.Cos(seta) + center_w);
                    new_h = (int)((i - center_h) * Math.Cos(seta) - (j - center_w) * Math.Sin(seta) + center_h);
                    if (new_w < 0) continue;
                    if (new_w >= inW) continue;
                    if (new_h < 0) continue;
                    if (new_h >= inH) continue;
                    outImage[RR, i, j] = inImage[RR, new_h, new_w];
                    outImage[GG, i, j] = inImage[GG, new_h, new_w];
                    outImage[BB, i, j] = inImage[BB, new_h, new_w];
                }
            }
            displayImage();
        }
        void fillEdges(double[,,] tmpInput, double[,,] inputCopy, int maskLength)
        {//가장자리 처리 알고리즘, 정사각형 마스크는 모두 처리 가능
            int full = maskLength - 1;
            int half = maskLength / 2;
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < inH + full; i++)
                {
                    for (int j = 0; j < inW + full; j++)
                    {
                        if (i < half && j < half)//왼쪽 위 모서리
                        {
                            tmpInput[rgb, i, j] = inputCopy[rgb, i, j];
                        }
                        else if (i < half && j > inW + half - 1) //오른쪽 위 모서리
                        {
                            tmpInput[rgb, i, j] = inputCopy[rgb, i, j - full];
                        }
                        else if (i < half)//맨 위 2줄
                        {
                            tmpInput[rgb, i, j] = inputCopy[rgb, i, j - 2];
                        }
                        else if (i > inH + half - 1 && j < half)//왼쪽 아래 모서리
                        {
                            tmpInput[rgb, i, j] = inputCopy[rgb, i - full, j];
                        }
                        else if (i > inH + half - 1 && j > inW + half - 1)//오른쪽 아래 모서리
                        {
                            tmpInput[rgb, i, j] = inputCopy[rgb, i - full, j - full];
                        }
                        else if (j < half)//맨 왼쪽 2줄
                        {
                            tmpInput[rgb, i, j] = inputCopy[rgb, i - half, j];
                        }
                        else if (j > inW + half - 1)//맨 오른쪽 2줄
                        {
                            tmpInput[rgb, i, j] = inputCopy[rgb, i - half, j - full];
                        }
                        else if (i > inH + half - 1)//맨 아래 2줄
                        {
                            tmpInput[rgb, i, j] = inputCopy[rgb, i - full, j - half];
                        }
                        else
                        {
                            tmpInput[rgb, i, j] = inputCopy[rgb, i - half, j - half];
                        }
                    }
                }
            }
        }
        void maskOP(double[,,] tmpInput, double[,,] tmpOutput, double[,] mask)
        {
            double sumR = 0.0;
            double sumG = 0.0;
            double sumB = 0.0;
            for (int i = 2; i < inH + 2; i++)
            {
                for (int j = 2; j < inW + 2; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        for (int m = 0; m < 5; m++)
                        {
                            sumR += tmpInput[RR, i - 2 + k, j - 2 + m] * mask[k, m];
                            sumG += tmpInput[GG, i - 2 + k, j - 2 + m] * mask[k, m];
                            sumB += tmpInput[BB, i - 2 + k, j - 2 + m] * mask[k, m];
                        }
                    }
                    tmpOutput[RR, i - 2, j - 2] = sumR;
                    tmpOutput[GG, i - 2, j - 2] = sumG;
                    tmpOutput[BB, i - 2, j - 2] = sumB;
                    sumR = 0.0;
                    sumG = 0.0;
                    sumB = 0.0;
                }
            }
        }
        void outCopy(double[,,] tmpOutput)
        { //임시 출력 -> 원래 출력
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < inH; i++)
                {
                    for (int j = 0; j < inW; j++)
                    {
                        if (tmpOutput[rgb, i, j] > 255.0)
                        {
                            outImage[rgb, i, j] = (byte)255;
                        }
                        else if (tmpOutput[rgb, i, j] < 0.0)
                        {
                            outImage[rgb, i, j] = (byte)0;
                        }
                        else
                        {
                            outImage[rgb, i, j] = (byte)tmpOutput[rgb, i, j];
                        }
                    }
                }
            }
        }
        void embossImage()
        {
            if (inImage == null)
            {
                return;
            }
            outH = inH;
            outW = inW;
            outImage = new byte[RGB, outH, outW];
            //마스크 결정
            double[,] mask = {
                { -1.0, 0.0, 0.0, 0.0, 0.0},
                { 0.0, 0.0, 0.0, 0.0, 0.0},
                { 0.0, 0.0, 0.0, 0.0, 0.0},
                { 0.0, 0.0, 0.0, 0.0, 0.0},
                { 0.0, 0.0, 0.0, 0.0, 1.0} };
            //임시 입력 출력 메모리 할당
            double[,,] tmpInput = new double[RGB ,inH + 4, inW + 4];//확장된 메모리
            double[,,] inputCopy = new double[RGB, inH, inW];//가장자리 처리를 위한 복사본 메모리
            double[,,] tmpOutput = new double[RGB, inH, inW];//후처리를 위한 출력 메모리
            //입력 -> 복사본 복사
            for (int i = 0; i < inH; i++)
            {
                for (int j = 0; j < inW; j++)
                {
                    inputCopy[RR, i, j] = inImage[RR, i, j];
                    inputCopy[GG, i, j] = inImage[GG, i, j];
                    inputCopy[BB, i, j] = inImage[BB, i, j];
                }
            }
            //가장자리 처리
            int maskLength = mask.GetLength(0);
            fillEdges(tmpInput, inputCopy, maskLength);
            //마스크연산
            maskOP(tmpInput, tmpOutput, mask);
            var findAvg = calculImage();
            //후처리
            for (int i = 0; i < inH; i++)
            {
                for (int j = 0; j < inW; j++)
                {
                    tmpOutput[RR, i, j] += findAvg.rAvg;
                    tmpOutput[GG, i, j] += findAvg.gAvg;
                    tmpOutput[BB, i, j] += findAvg.bAvg;
                }
            }
            //임시 출력 -> 원래 출력
            outCopy(tmpOutput);
            displayImage();
        }
        void blurrImage()
        {
            if (inImage == null)
            {
                return;
            }
            outH = inH;
            outW = inW;
            outImage = new byte[RGB, outH, outW];
            //마스크 결정
            double[,] mask = {
                { 1.0/25.0, 1.0/25.0, 1.0/25.0, 1.0/25.0, 1.0/25.0},
                { 1.0/25.0, 1.0/25.0, 1.0/25.0, 1.0/25.0, 1.0/25.0},
                { 1.0/25.0, 1.0/25.0, 1.0/25.0, 1.0/25.0, 1.0/25.0},
                { 1.0/25.0, 1.0/25.0, 1.0/25.0, 1.0/25.0, 1.0/25.0},
                { 1.0/25.0, 1.0/25.0, 1.0/25.0, 1.0/25.0, 1.0/25.0} };
            //임시 입력 출력 메모리 할당
            double[,,] tmpInput = new double[RGB, inH + 4, inW + 4];//확장된 메모리
            double[,,] inputCopy = new double[RGB, inH, inW];//가장자리 처리를 위한 복사본 메모리
            double[,,] tmpOutput = new double[RGB, inH, inW];//후처리를 위한 출력 메모리
            //입력 -> 복사본 복사
            for (int i = 0; i < inH; i++)
            {
                for (int j = 0; j < inW; j++)
                {
                    inputCopy[RR, i, j] = inImage[RR, i, j];
                    inputCopy[GG, i, j] = inImage[GG, i, j];
                    inputCopy[BB, i, j] = inImage[BB, i, j];
                }
            }
            //가장자리 처리
            int maskLength = mask.GetLength(0);
            fillEdges(tmpInput, inputCopy, maskLength);
            //마스크 연산
            maskOP(tmpInput, tmpOutput, mask);
            //임시 출력 -> 원래 출력
            outCopy(tmpOutput);
            displayImage();
        }
        void sharpImage()
        {
            if (inImage == null)
            {
                return;
            }
            outH = inH;
            outW = inW;
            outImage = new byte[RGB, outH, outW];
            //마스크 결정
            double[,] mask = {
                { -1, -1, -1, -1, -1},
                { -1, -1, -1, -1, -1},
                { -1, -1, 24, -1, -1},
                { -1, -1, -1, -1, -1},
                { -1, -1, -1, -1, -1} };
            //임시 입력 출력 메모리 할당
            double[,,] tmpInput = new double[RGB, inH + 4, inW + 4];//확장된 메모리
            double[,,] inputCopy = new double[RGB, inH, inW];//가장자리 처리를 위한 복사본 메모리
            double[,,] tmpOutput = new double[RGB, inH, inW];//후처리를 위한 출력 메모리
            //입력 -> 복사본 복사
            for (int i = 0; i < inH; i++)
            {
                for (int j = 0; j < inW; j++)
                {
                    inputCopy[RR, i, j] = inImage[RR, i, j];
                    inputCopy[GG, i, j] = inImage[GG, i, j];
                    inputCopy[BB, i, j] = inImage[BB, i, j];
                }
            }
            //가장자리 처리
            int maskLength = mask.GetLength(0);
            fillEdges(tmpInput, inputCopy, maskLength);
            //마스크 연산
            maskOP(tmpInput, tmpOutput, mask);
            //임시 출력 -> 원래 출력
            outCopy(tmpOutput);
            displayImage();
        }
        void morphologyOP(double[,,] tmpInput, double[,,] tmpOutput, int isErosion)
        { //침식(isErosion = 1) min 팽창(isErosion = 0) max 
            double min, max;
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 2; i < inH + 2; i++)
                {
                    for (int j = 2; j < inW + 2; j++)
                    {
                        min = 255;
                        max = 0;
                        for (int k = 0; k < 5; k++)
                        {
                            for (int m = 0; m < 5; m++)
                            {
                                if (isErosion == 1)//침식
                                {
                                    if (tmpInput[rgb, i - 2 + k, j - 2 + m] < min)
                                    {
                                        min = tmpInput[rgb, i - 2 + k, j - 2 + m];
                                    }
                                }
                                else//팽창
                                {
                                    if (tmpInput[rgb, i - 2 + k, j - 2 + m] > max)
                                    {
                                        max = tmpInput[rgb, i - 2 + k, j - 2 + m];
                                    }
                                }
                            }
                        }
                        if (isErosion == 1)//침식
                        {
                            tmpOutput[rgb, i - 2, j - 2] = min;
                        }
                        else
                        {
                            tmpOutput[rgb, i - 2, j - 2] = max;
                        }
                    }
                }
            }
        }
        void erosion()
        {
            if (inImage == null)
            {
                return;
            }
            outH = inH;
            outW = inW;
            outImage = new byte[RGB, outH, outW];
            double[,,] tmpInput = new double[RGB, inH + 4, inW + 4];//확장된 메모리
            double[,,] inputCopy = new double[RGB, inH, inW];//가장자리 처리를 위한 복사본 메모리
            double[,,] tmpOutput = new double[RGB, inH, inW];//후처리를 위한 출력 메모리
            //입력 -> 복사본 복사
            for (int i = 0; i < inH; i++)
            {
                for (int j = 0; j < inW; j++)
                {
                    inputCopy[RR, i, j] = inImage[RR, i, j];
                    inputCopy[GG, i, j] = inImage[GG, i, j];
                    inputCopy[BB, i, j] = inImage[BB, i, j];
                }
            }
            //가장자리 처리
            int maskLength = 5;
            fillEdges(tmpInput, inputCopy, maskLength);
            morphologyOP(tmpInput, tmpOutput, 1);
            //임시 출력 -> 원래 출력
            outCopy(tmpOutput);
            displayImage();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection(connStr);
            conn.Open();
            cmd = new MySqlCommand("", conn);
            String srcPath = "C:\\TempImages";
            DirectoryInfo di1 = new DirectoryInfo(srcPath);
            if(di1.Exists == false)
            {
                di1.Create();
            }
            srcPath = "C:\\TempImagesDBOpen";
            DirectoryInfo di2 = new DirectoryInfo(srcPath);
            if (di2.Exists == false)
            {
                di2.Create();
            }
        }
        void dilation()
        {
            if (inImage == null)
            {
                return;
            }
            outH = inH;
            outW = inW;
            outImage = new byte[RGB, outH, outW];
            double[,,] tmpInput = new double[RGB, inH + 4, inW + 4];//확장된 메모리
            double[,,] inputCopy = new double[RGB, inH, inW];//가장자리 처리를 위한 복사본 메모리
            double[,,] tmpOutput = new double[RGB, inH, inW];//후처리를 위한 출력 메모리
            //입력 -> 복사본 복사
            for (int i = 0; i < inH; i++)
            {
                for (int j = 0; j < inW; j++)
                {
                    inputCopy[RR, i, j] = inImage[RR, i, j];
                    inputCopy[GG, i, j] = inImage[GG, i, j];
                    inputCopy[BB, i, j] = inImage[BB, i, j];
                }
            }
            //가장자리 처리
            int maskLength = 5;
            fillEdges(tmpInput, inputCopy, maskLength);
            morphologyOP(tmpInput, tmpOutput, 0);
            //임시 출력 -> 원래 출력
            outCopy(tmpOutput);
            displayImage();
        }
        void opening()
        {
            if (inImage == null)
            {
                return;
            }
            outH = inH;
            outW = inW;
            outImage = new byte[RGB, outH, outW];
            double[,,] tmpInput = new double[RGB, inH + 4, inW + 4];//확장된 메모리
            double[,,] inputCopy = new double[RGB, inH, inW];//가장자리 처리를 위한 복사본 메모리
            double[,,] tmpOutput = new double[RGB, inH, inW];//후처리를 위한 출력 메모리
            //입력 -> 복사본 복사
            for (int i = 0; i < inH; i++)
            {
                for (int j = 0; j < inW; j++)
                {
                    inputCopy[RR, i, j] = inImage[RR, i, j];
                    inputCopy[GG, i, j] = inImage[GG, i, j];
                    inputCopy[BB, i, j] = inImage[BB, i, j];
                }
            }
            //가장자리 처리
            int maskLength = 5;
            fillEdges(tmpInput, inputCopy, maskLength);
            morphologyOP(tmpInput, tmpOutput, 1);
            //2번 가장차리 처리 연산을 위한 메모리 
            double[,,] outBufImage = new double[RGB, outH + 4, outW + 4];
            //가장자리 처리
            fillEdges(outBufImage, tmpOutput, maskLength);
            morphologyOP(outBufImage, tmpOutput, 0);
            //임시 출력 -> 원래 출력
            outCopy(tmpOutput);
            displayImage();
        }
        void closing()
        {
            if (inImage == null)
            {
                return;
            }
            outH = inH;
            outW = inW;
            outImage = new byte[RGB, outH, outW];
            double[,,] tmpInput = new double[RGB, inH + 4, inW + 4];//확장된 메모리
            double[,,] inputCopy = new double[RGB, inH, inW];//가장자리 처리를 위한 복사본 메모리
            double[,,] tmpOutput = new double[RGB, inH, inW];//후처리를 위한 출력 메모리
            //입력 -> 복사본 복사
            for (int i = 0; i < inH; i++)
            {
                for (int j = 0; j < inW; j++)
                {
                    inputCopy[RR, i, j] = inImage[RR, i, j];
                    inputCopy[GG, i, j] = inImage[GG, i, j];
                    inputCopy[BB, i, j] = inImage[BB, i, j];
                }
            }
            //가장자리 처리
            int maskLength = 5;
            fillEdges(tmpInput, inputCopy, maskLength);
            morphologyOP(tmpInput, tmpOutput, 0);
            //2번 가장차리 처리 연산을 위한 메모리 
            double[,,] outBufImage = new double[RGB, outH + 4, outW + 4];
            //가장자리 처리
            fillEdges(outBufImage, tmpOutput, maskLength);
            morphologyOP(outBufImage, tmpOutput, 1);
            //임시 출력 -> 원래 출력
            outCopy(tmpOutput);
            displayImage();
        }
        void medianFilter()
        {//노이즈 제거 알고리즘 
            if (inImage == null)
            {
                return;
            }
            outH = inH;
            outW = inW;
            outImage = new byte[RGB, outH, outW];
            double[,,] tmpInput = new double[RGB, inH + 4, inW + 4];//확장된 메모리
            double[,,] inputCopy = new double[RGB, inH, inW];//가장자리 처리를 위한 복사본 메모리
            double[,,] tmpOutput = new double[RGB, inH, inW];//후처리를 위한 출력 메모리
            //입력 -> 복사본 복사
            for (int i = 0; i < inH; i++)
            {
                for (int j = 0; j < inW; j++)
                {
                    inputCopy[RR, i, j] = inImage[RR, i, j];
                    inputCopy[GG, i, j] = inImage[GG, i, j];
                    inputCopy[BB, i, j] = inImage[BB, i, j];
                }
            }
            int amount = 13;//잡음 개수 조절
                            //가로 x 세로 x (amout / 100)
            int noiseCount = (int)(inH * inW * ((double)amount / 100));
            salt_pepper(noiseCount, inputCopy);//영상에 잡음추가
            //잡음 이미지 출력
            for (int i = 0; i < inH; i++)
            {
                for (int j = 0; j < inW; j++)
                {
                    outImage[RR, i, j] = (byte)inputCopy[RR, i, j];
                    outImage[GG, i, j] = (byte)inputCopy[GG, i, j];
                    outImage[BB, i, j] = (byte)inputCopy[BB, i, j];
                }
            }
            displayImage();
            Delay(2000);
            int maskLength = 5;
            fillEdges(tmpInput, inputCopy, maskLength);
            int sortSize = 5;
            byte[] medianSort = new byte[sortSize * sortSize];//정렬을 위한 1차원 배열
            //inImage 값이 아닌 노이즈가 생긴 tmpInput 값을 가져온다.
            int temp = 0;//임시변수(배열 인덱스 값)
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 2; i < inH + 2; i++)
                {//엣지는 처리하지 않음
                    for (int j = 2; j < inW + 2; j++)
                    {
                        temp = 0;
                        for (int k = 0; k < sortSize; k++)
                        {
                            for (int m = 0; m < sortSize; m++)
                            {
                                medianSort[temp++] = (byte)tmpInput[rgb, i - 2 + k, j - 2 + m];
                            }
                        }
                        Array.Sort(medianSort);
                        outImage[rgb, i - 2, j - 2] = medianSort[(sortSize * sortSize) / 2];//중간 값으로 출력
                    }
                }
            }
            //제거 이미지 출력
            displayImage();
        }
        //프로그램 ms만큼 대기
        private DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }

            return DateTime.Now;
        }
        void salt_pepper(int noiseCount, double[,,] inputCopy)
        {//영상에 잡음 추가
            Random r = new Random();
            int salt_or_pepper;
            int row, col;
            //잡음 추가
            for (int i = 0; i < noiseCount; i++)
            {
                row = r.Next(0, inH);
                col = r.Next(0, inW);
                // 랜덤하게 0 또는 255, 0이면 후추, 255면 소금
                salt_or_pepper = r.Next(0, 2) * 255;
                inputCopy[RR, row, col] = salt_or_pepper;
                inputCopy[GG, row, col] = salt_or_pepper;
                inputCopy[BB, row, col] = salt_or_pepper;
            }
        }
        void drawHistogram()
        {
            long[] rHisto = new long[256];
            long[] gHisto = new long[256];
            long[] bHisto = new long[256];
            for (int i = 0; i < outH; i++)
            {
                for (int j = 0; j < outW; j++)
                {
                    rHisto[outImage[RR, i, j]]++;
                    gHisto[outImage[GG, i, j]]++;
                    bHisto[outImage[BB, i, j]]++;
                }
            }
            HistoForm hform = new HistoForm(rHisto, gHisto, bHisto);
            hform.ShowDialog();
        }
        //이미지 최대, 최소, 합계, 평균값 반환 함수
        private (byte rMin, byte rMax, byte gMin, 
            byte gMax, byte bMin, byte bMax, 
            long rSum, long gSum, long bSum, 
            double rAvg, double gAvg, double bAvg) calculImage()
        {
            byte rMin = 255;
            byte rMax = 0;
            byte gMin = 255;
            byte gMax = 0;
            byte bMin = 255;
            byte bMax = 0;
            long rSum = 0;
            long gSum = 0;
            long bSum = 0;
            for(int i = 0; i < inH; i++)
            {
                for(int j = 0; j < inW; j++)
                {
                    rSum += inImage[RR, i, j];
                    gSum += inImage[GG, i, j];
                    bSum += inImage[BB, i, j];
                    if (rMin > inImage[RR, i, j])
                    {
                        rMin = inImage[RR, i, j];
                    }
                    if (gMin > inImage[GG, i, j])
                    {
                        gMin = inImage[GG, i, j];
                    }
                    if (bMin > inImage[BB, i, j])
                    {
                        bMin = inImage[BB, i, j];
                    }
                    if (rMax < inImage[RR, i, j])
                    {
                        rMax = inImage[RR, i, j];
                    }
                    if (gMax < inImage[GG, i, j])
                    {
                        gMax = inImage[GG, i, j];
                    }
                    if (bMax < inImage[BB, i, j])
                    {
                        bMax = inImage[BB, i, j];
                    }
                }
            }
            double rAvg = rSum / ((double)inH * inW);
            double gAvg = gSum / ((double)inH * inW);
            double bAvg = bSum / ((double)inH * inW);
            return (rMin, rMax, gMin, gMax, bMin, bMax, rSum, gSum, bSum, rAvg, gAvg, bAvg);
        }
        void stretching()
        {
            if (inImage == null)
                return;
            outH = inH;
            outW = inW;
            outImage = new byte[RGB, outH, outW];
            var findMinMax = calculImage();
            byte rMin = findMinMax.rMin;
            byte rMax = findMinMax.rMax;
            byte gMin = findMinMax.gMin;
            byte gMax = findMinMax.gMax;
            byte bMin = findMinMax.bMin;
            byte bMax = findMinMax.bMax;
            for (int i = 0; i < inH; i++)
            {
                for (int j = 0; j < inW; j++)
                {
                    outImage[RR, i, j] = (byte)(((double)(inImage[RR, i, j] - rMin) / (rMax - rMin)) * 255);
                    outImage[GG, i, j] = (byte)(((double)(inImage[GG, i, j] - gMin) / (gMax - gMin)) * 255);
                    outImage[BB, i, j] = (byte)(((double)(inImage[BB, i, j] - bMin) / (bMax - bMin)) * 255);
                }
            }
            displayImage();
        }
        void endInSearch()
        {
            if (inImage == null)
                return;
            outH = inH;
            outW = inW;
            outImage = new byte[RGB, outH, outW];
            var findMinMax = calculImage();
            int rMin = findMinMax.rMin - 50;
            int rMax = findMinMax.rMax + 50;
            int gMin = findMinMax.gMin - 50;
            int gMax = findMinMax.gMax + 50;
            int bMin = findMinMax.bMin - 50;
            int bMax = findMinMax.bMax + 50;
            //min max값을 강제로 변경
            for (int i = 0; i < inH; i++)
            {
                for (int j = 0; j < inW; j++)
                {
                    double rValue = ((double)(inImage[RR, i, j] - rMin) / (rMax - rMin)) * 255;
                    double gValue = ((double)(inImage[GG, i, j] - gMin) / (gMax - gMin)) * 255;
                    double bValue = ((double)(inImage[BB, i, j] - bMin) / (bMax - bMin)) * 255;
                    if (rValue > 255)
                    {
                        rValue = 255;
                    }
                    else if (rValue < 0)
                    {
                        rValue = 0;
                    }
                    if (gValue > 255)
                    {
                        gValue = 255;
                    }
                    else if (gValue < 0)
                    {
                        gValue = 0;
                    }
                    if (bValue > 255)
                    {
                        bValue = 255;
                    }
                    else if (bValue < 0)
                    {
                        bValue = 0;
                    }
                    outImage[RR, i, j] = (byte)rValue;
                    outImage[GG, i, j] = (byte)gValue;
                    outImage[BB, i, j] = (byte)bValue;
                }
            }
            displayImage();
        }
        void equalized()
        {
            if (inImage == null)
                return;
            outH = inH;
            outW = inW;
            outImage = new byte[RGB, outH, outW];
            //히스토그램 생성
            long[] rHist = new long[256];
            long[] rHistSum = new long[256];
            long[] gHist = new long[256];
            long[] gHistSum = new long[256];
            long[] bHist = new long[256];
            long[] bHistSum = new long[256];
            for (int i = 0; i < rHist.Length; i++)
            {
                rHist[i] = 0;
                rHistSum[i] = 0;
                gHist[i] = 0;
                gHistSum[i] = 0;
                bHist[i] = 0;
                bHistSum[i] = 0;
            }
            for (int i = 0; i < inH; i++)
            {
                for (int j = 0; j < inW; j++)
                {
                    rHist[inImage[RR, i, j]]++;
                    gHist[inImage[GG, i, j]]++;
                    bHist[inImage[BB, i, j]]++;
                }
            }
            //누적합 생성
            long copy = rHist[0];
            rHistSum[0] = copy;
            copy = gHist[0];
            gHistSum[0] = copy;
            copy = bHist[0];
            bHistSum[0] = copy;
            for (int i = 1; i < rHistSum.Length; i++)
            {
                rHistSum[i] = (rHistSum[i - 1] + rHist[i]);
                gHistSum[i] = (gHistSum[i - 1] + gHist[i]);
                bHistSum[i] = (bHistSum[i - 1] + bHist[i]);
            }
            //정규화된 누적합 생성
            for (int i = 0; i < inH; i++)
            {
                for (int j = 0; j < inW; j++)
                {
                    outImage[RR, i, j] = (byte)(rHistSum[inImage[RR, i, j]] / (double)(inH * inW) * 255);
                    outImage[GG, i, j] = (byte)(rHistSum[inImage[GG, i, j]] / (double)(inH * inW) * 255);
                    outImage[BB, i, j] = (byte)(rHistSum[inImage[BB, i, j]] / (double)(inH * inW) * 255);
                }
            }
            displayImage();
        }
        void undoImage()
        {
            if(undoList.Count < 2)
            {
                return;
            }
            redoList.Add(undoList[undoList.Count - 1]);
            undoList.RemoveAt(undoList.Count - 1);
            inImage = (byte[,,])undoList[undoList.Count - 1].Clone();
            isUndo = true;
            equal_image();
            isUndo = false;
        }
        void redoImage()
        {
            if (redoList.Count < 1)
            {
                return;
            }
            undoList.Add(redoList[redoList.Count - 1]);
            inImage = (byte[,,])undoList[undoList.Count - 1].Clone();
            redoList.RemoveAt(redoList.Count - 1);
            isUndo = true;
            equal_image();
            isUndo = false;
        }
        // OpenCv용 함수
        void Cv2ToOutImage()
        {
            // 출력 이미지 메모리 확보
            outH = outCvImage.Height;
            outW = outCvImage.Width;
            outImage = new byte[RGB, outH, outW];
            // openCV 이미지 --> 메모리 (로딩)
            for (int i = 0; i < outH; i++)
            {
                for (int j = 0; j < outW; j++)
                {
                    var c = outCvImage.At<Vec3b>(i, j);
                    outImage[RR, i, j] = c.Item2;
                    outImage[GG, i, j] = c.Item1;
                    outImage[BB, i, j] = c.Item0;
                }
            }
            displayImage();
        }
        void grayscale_CV()
        {
            // 출력 openCV 이미지 크기 결정 (알고리즘)
            /*int oH, oW; // outCvImage 크기
            oH = inCvImage.Height;
            oW = inCvImage.Width;
            outCvImage = Mat.Ones(new OpenCvSharp.Size(oH, oW), MatType.CV_8UC1);*/
            outCvImage = new Mat();
            // 진짜 openCv용 알고리즘
            Cv2.CvtColor(inCvImage, outCvImage, ColorConversionCodes.BGR2GRAY);
            Cv2ToOutImage();
        }
        void brightImage_CV()
        {
            int bright = (int)(getValue());
            Mat val;
            outCvImage = new Mat();
            if(bright >= 0)
            {
                val = new Mat(inCvImage.Size(), MatType.CV_8UC3, new Scalar(bright, bright, bright));
                Cv2.Add(inCvImage, val, outCvImage);
            } else
            {
                bright = Math.Abs(bright);
                val = new Mat(inCvImage.Size(), MatType.CV_8UC3, new Scalar(bright, bright, bright));
                Cv2.Subtract(inCvImage, val, outCvImage);
            }
            Cv2ToOutImage();
        }
        void bwImage_CV()
        {
            outCvImage = new Mat();
            // 진짜 openCv용 알고리즘
            Cv2.CvtColor(inCvImage, outCvImage, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(outCvImage, outCvImage, 127, 255, ThresholdTypes.Binary);
            Cv2ToOutImage();
        }
        void bwImageA_CV()
        {
            outCvImage = new Mat();
            // 진짜 openCv용 알고리즘
            Cv2.CvtColor(inCvImage, outCvImage, ColorConversionCodes.BGR2GRAY);
            Cv2.AdaptiveThreshold(outCvImage, outCvImage, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 25, 5);
            Cv2ToOutImage();
        }
        int getKernelSize()
        {
            KernelSize ks = new KernelSize();
            if (ks.ShowDialog() == DialogResult.Cancel)
            {
                return 1;
            }
            int value = (int)ks.numUp_value.Value;
            return value;
        }
        void blurrImage_CV()
        {
            outCvImage = new Mat();
            int kernel = Math.Abs(getKernelSize());
            Cv2.Blur(inCvImage, outCvImage, new OpenCvSharp.Size(kernel, kernel), new OpenCvSharp.Point(-1, -1), BorderTypes.Default);
            Cv2ToOutImage();
        }
        void boxFilterBlur_CV()
        {
            //Cv2.BoxFilter(inCvImage, box_filter, MatType.CV_8UC3, new Size(15, 15), new Point(-1, -1), true, BorderTypes.Default);
        }
    }
}
