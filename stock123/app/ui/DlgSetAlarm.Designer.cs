namespace stock123.app.ui
{
    partial class DlgSetAlarm
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
            this.lb_Title = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.Back = new System.Windows.Forms.Button();
            this.tb_Lower = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lb_LowerPercent = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_Lower = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lb_UpperPercent = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_Upper = new System.Windows.Forms.CheckBox();
            this.tb_Upper = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel_Chart = new System.Windows.Forms.Panel();
            this.tb_comment = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lb_Title
            // 
            this.lb_Title.AutoSize = true;
            this.lb_Title.Location = new System.Drawing.Point(10, 83);
            this.lb_Title.Name = "lb_Title";
            this.lb_Title.Size = new System.Drawing.Size(47, 13);
            this.lb_Title.TabIndex = 0;
            this.lb_Title.Text = "Ghi chú:";
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(569, 2);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(91, 23);
            this.OK.TabIndex = 6;
            this.OK.Text = "Lưu";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Back
            // 
            this.Back.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Back.Location = new System.Drawing.Point(569, 29);
            this.Back.Name = "Back";
            this.Back.Size = new System.Drawing.Size(91, 23);
            this.Back.TabIndex = 7;
            this.Back.Text = "Quay lại";
            this.Back.UseVisualStyleBackColor = true;
            this.Back.Click += new System.EventHandler(this.Back_Click);
            // 
            // tb_Lower
            // 
            this.tb_Lower.Location = new System.Drawing.Point(25, 21);
            this.tb_Lower.Name = "tb_Lower";
            this.tb_Lower.Size = new System.Drawing.Size(131, 20);
            this.tb_Lower.TabIndex = 4;
            this.tb_Lower.TextChanged += new System.EventHandler(this.tb_Lower_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lb_LowerPercent);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cb_Lower);
            this.groupBox1.Controls.Add(this.tb_Lower);
            this.groupBox1.Location = new System.Drawing.Point(288, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(261, 58);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // lb_LowerPercent
            // 
            this.lb_LowerPercent.AutoSize = true;
            this.lb_LowerPercent.Location = new System.Drawing.Point(211, 28);
            this.lb_LowerPercent.Name = "lb_LowerPercent";
            this.lb_LowerPercent.Size = new System.Drawing.Size(35, 13);
            this.lb_LowerPercent.TabIndex = 9;
            this.lb_LowerPercent.Text = "label1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(162, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "(vnđ)";
            // 
            // cb_Lower
            // 
            this.cb_Lower.AutoSize = true;
            this.cb_Lower.Location = new System.Drawing.Point(6, 0);
            this.cb_Lower.Name = "cb_Lower";
            this.cb_Lower.Size = new System.Drawing.Size(187, 17);
            this.cb_Lower.TabIndex = 3;
            this.cb_Lower.Text = "Cảnh báo nếu giá xuống thấp hơn";
            this.cb_Lower.UseVisualStyleBackColor = true;
            this.cb_Lower.CheckedChanged += new System.EventHandler(this.cb_Lower_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lb_UpperPercent);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cb_Upper);
            this.groupBox2.Controls.Add(this.tb_Upper);
            this.groupBox2.Location = new System.Drawing.Point(11, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(261, 58);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            // 
            // lb_UpperPercent
            // 
            this.lb_UpperPercent.AutoSize = true;
            this.lb_UpperPercent.Location = new System.Drawing.Point(208, 28);
            this.lb_UpperPercent.Name = "lb_UpperPercent";
            this.lb_UpperPercent.Size = new System.Drawing.Size(35, 13);
            this.lb_UpperPercent.TabIndex = 9;
            this.lb_UpperPercent.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(161, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "(vnđ)";
            // 
            // cb_Upper
            // 
            this.cb_Upper.AutoSize = true;
            this.cb_Upper.Location = new System.Drawing.Point(6, 0);
            this.cb_Upper.Name = "cb_Upper";
            this.cb_Upper.Size = new System.Drawing.Size(176, 17);
            this.cb_Upper.TabIndex = 1;
            this.cb_Upper.Text = "Cảnh báo nếu giá tăng cao hơn";
            this.cb_Upper.UseVisualStyleBackColor = true;
            this.cb_Upper.CheckedChanged += new System.EventHandler(this.cb_Upper_CheckedChanged);
            // 
            // tb_Upper
            // 
            this.tb_Upper.Location = new System.Drawing.Point(24, 23);
            this.tb_Upper.Name = "tb_Upper";
            this.tb_Upper.Size = new System.Drawing.Size(131, 20);
            this.tb_Upper.TabIndex = 2;
            this.tb_Upper.TextChanged += new System.EventHandler(this.tb_Upper_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(569, 72);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Xóa cảnh báo";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel_Chart
            // 
            this.panel_Chart.Location = new System.Drawing.Point(12, 109);
            this.panel_Chart.Name = "panel_Chart";
            this.panel_Chart.Size = new System.Drawing.Size(648, 440);
            this.panel_Chart.TabIndex = 11;
            // 
            // tb_comment
            // 
            this.tb_comment.Location = new System.Drawing.Point(63, 80);
            this.tb_comment.MaxLength = 100;
            this.tb_comment.Name = "tb_comment";
            this.tb_comment.Size = new System.Drawing.Size(486, 20);
            this.tb_comment.TabIndex = 5;
            // 
            // DlgSetAlarm
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Back;
            this.ClientSize = new System.Drawing.Size(672, 570);
            this.Controls.Add(this.tb_comment);
            this.Controls.Add(this.panel_Chart);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Back);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.lb_Title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgSetAlarm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alarm setting";
            this.Load += new System.EventHandler(this.DlgSetAlarm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_Title;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Back;
        private System.Windows.Forms.TextBox tb_Lower;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cb_Lower;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cb_Upper;
        private System.Windows.Forms.TextBox tb_Upper;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel_Chart;
        private System.Windows.Forms.TextBox tb_comment;
        private System.Windows.Forms.Label lb_LowerPercent;
        private System.Windows.Forms.Label lb_UpperPercent;
    }
}