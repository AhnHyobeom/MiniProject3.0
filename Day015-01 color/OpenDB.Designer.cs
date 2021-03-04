
namespace Day015_01_color
{
    partial class OpenDB
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.btn_open = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.tb_selectFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(61, 39);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(496, 360);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // btn_open
            // 
            this.btn_open.Location = new System.Drawing.Point(147, 464);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(107, 36);
            this.btn_open.TabIndex = 1;
            this.btn_open.Text = "열기";
            this.btn_open.UseVisualStyleBackColor = true;
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(361, 464);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(102, 36);
            this.btn_close.TabIndex = 2;
            this.btn_close.Text = "닫기";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // tb_selectFileName
            // 
            this.tb_selectFileName.Font = new System.Drawing.Font("굴림", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_selectFileName.Location = new System.Drawing.Point(176, 423);
            this.tb_selectFileName.Name = "tb_selectFileName";
            this.tb_selectFileName.Size = new System.Drawing.Size(386, 28);
            this.tb_selectFileName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(19, 426);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 19);
            this.label1.TabIndex = 4;
            this.label1.Text = "선택한 파일명 :";
            // 
            // OpenDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 526);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_selectFileName);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_open);
            this.Controls.Add(this.listView1);
            this.Name = "OpenDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OpenDB";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OpenDB_FormClosed);
            this.Load += new System.EventHandler(this.OpenDB_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button btn_open;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.TextBox tb_selectFileName;
        private System.Windows.Forms.Label label1;
    }
}