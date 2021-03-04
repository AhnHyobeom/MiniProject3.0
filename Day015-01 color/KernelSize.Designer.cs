
namespace Day015_01_color
{
    partial class KernelSize
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
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_ok = new System.Windows.Forms.Button();
            this.numUp_value = new System.Windows.Forms.NumericUpDown();
            this.lb_input = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numUp_value)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_cancel
            // 
            this.btn_cancel.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_cancel.Location = new System.Drawing.Point(272, 160);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 36);
            this.btn_cancel.TabIndex = 7;
            this.btn_cancel.Text = "cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_ok
            // 
            this.btn_ok.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_ok.Location = new System.Drawing.Point(110, 160);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 36);
            this.btn_ok.TabIndex = 6;
            this.btn_ok.Text = "OK";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // numUp_value
            // 
            this.numUp_value.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.numUp_value.Location = new System.Drawing.Point(272, 70);
            this.numUp_value.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numUp_value.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.numUp_value.Name = "numUp_value";
            this.numUp_value.Size = new System.Drawing.Size(120, 34);
            this.numUp_value.TabIndex = 5;
            // 
            // lb_input
            // 
            this.lb_input.AutoSize = true;
            this.lb_input.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_input.Location = new System.Drawing.Point(57, 74);
            this.lb_input.Name = "lb_input";
            this.lb_input.Size = new System.Drawing.Size(186, 24);
            this.lb_input.TabIndex = 4;
            this.lb_input.Text = "커널 크기 입력 :";
            // 
            // KernelSize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 275);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.numUp_value);
            this.Controls.Add(this.lb_input);
            this.Name = "KernelSize";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KernelSize";
            ((System.ComponentModel.ISupportInitialize)(this.numUp_value)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_ok;
        public System.Windows.Forms.NumericUpDown numUp_value;
        public System.Windows.Forms.Label lb_input;
    }
}