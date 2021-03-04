
namespace Day015_01_color
{
    partial class SaveDB
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
            this.tb_fname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_ing = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tb_fname
            // 
            this.tb_fname.Location = new System.Drawing.Point(160, 69);
            this.tb_fname.Name = "tb_fname";
            this.tb_fname.Size = new System.Drawing.Size(424, 25);
            this.tb_fname.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 12F);
            this.label1.Location = new System.Drawing.Point(21, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "파일명 입력 : ";
            // 
            // lb_ing
            // 
            this.lb_ing.AutoSize = true;
            this.lb_ing.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_ing.Location = new System.Drawing.Point(278, 142);
            this.lb_ing.Name = "lb_ing";
            this.lb_ing.Size = new System.Drawing.Size(104, 17);
            this.lb_ing.TabIndex = 2;
            this.lb_ing.Text = "저장 진행률";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(74, 171);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(510, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(160, 247);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(90, 44);
            this.btn_save.TabIndex = 4;
            this.btn_save.Text = "저장";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(372, 250);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(88, 44);
            this.btn_cancel.TabIndex = 5;
            this.btn_cancel.Text = "닫기";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(221, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(239, 19);
            this.label2.TabIndex = 6;
            this.label2.Text = "파일명 입력 예시 : dog.png";
            // 
            // SaveDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 335);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lb_ing);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_fname);
            this.Name = "SaveDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SaveDB";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SaveDB_FormClosed);
            this.Load += new System.EventHandler(this.SaveDB_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_fname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_ing;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Label label2;
    }
}