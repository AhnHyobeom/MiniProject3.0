
namespace Day015_01_color
{
    partial class subform
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
            this.lb_input = new System.Windows.Forms.Label();
            this.numUp_value = new System.Windows.Forms.NumericUpDown();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.tb_GetString = new System.Windows.Forms.TextBox();
            this.lb_ext = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numUp_value)).BeginInit();
            this.SuspendLayout();
            // 
            // lb_input
            // 
            this.lb_input.AutoSize = true;
            this.lb_input.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_input.Location = new System.Drawing.Point(142, 109);
            this.lb_input.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_input.Name = "lb_input";
            this.lb_input.Size = new System.Drawing.Size(95, 28);
            this.lb_input.TabIndex = 0;
            this.lb_input.Text = "입력 : ";
            this.lb_input.Click += new System.EventHandler(this.label1_Click);
            // 
            // numUp_value
            // 
            this.numUp_value.DecimalPlaces = 2;
            this.numUp_value.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.numUp_value.Location = new System.Drawing.Point(318, 107);
            this.numUp_value.Margin = new System.Windows.Forms.Padding(4);
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
            this.numUp_value.Size = new System.Drawing.Size(150, 39);
            this.numUp_value.TabIndex = 1;
            // 
            // btn_ok
            // 
            this.btn_ok.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_ok.Location = new System.Drawing.Point(170, 215);
            this.btn_ok.Margin = new System.Windows.Forms.Padding(4);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(94, 43);
            this.btn_ok.TabIndex = 2;
            this.btn_ok.Text = "OK";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_cancel.Location = new System.Drawing.Point(335, 215);
            this.btn_cancel.Margin = new System.Windows.Forms.Padding(4);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(94, 43);
            this.btn_cancel.TabIndex = 3;
            this.btn_cancel.Text = "cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.button2_Click);
            // 
            // tb_GetString
            // 
            this.tb_GetString.Location = new System.Drawing.Point(290, 160);
            this.tb_GetString.Name = "tb_GetString";
            this.tb_GetString.Size = new System.Drawing.Size(226, 28);
            this.tb_GetString.TabIndex = 4;
            // 
            // lb_ext
            // 
            this.lb_ext.AutoSize = true;
            this.lb_ext.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_ext.Location = new System.Drawing.Point(142, 160);
            this.lb_ext.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_ext.Name = "lb_ext";
            this.lb_ext.Size = new System.Drawing.Size(123, 28);
            this.lb_ext.TabIndex = 5;
            this.lb_ext.Text = "확장명 : ";
            // 
            // subform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 329);
            this.Controls.Add(this.lb_ext);
            this.Controls.Add(this.tb_GetString);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.numUp_value);
            this.Controls.Add(this.lb_input);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "subform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "subform";
            ((System.ComponentModel.ISupportInitialize)(this.numUp_value)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.NumericUpDown numUp_value;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_cancel;
        public System.Windows.Forms.TextBox tb_GetString;
        public System.Windows.Forms.Label lb_input;
        public System.Windows.Forms.Label lb_ext;
    }
}