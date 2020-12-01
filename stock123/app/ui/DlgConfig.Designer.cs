namespace stock123.app.ui
{
    partial class DlgConfig
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
            this.bt_ReloadSystem = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bt_ChangePass = new System.Windows.Forms.Button();
            this.bt_ExpandAccount = new System.Windows.Forms.Button();
            this.lb_RemainingDays = new System.Windows.Forms.Label();
            this.cb_Autologin = new System.Windows.Forms.CheckBox();
            this.bt_OK = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.cb_Gainloss = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.trackBarFont = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFont)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_ReloadSystem
            // 
            this.bt_ReloadSystem.Location = new System.Drawing.Point(13, 8);
            this.bt_ReloadSystem.Name = "bt_ReloadSystem";
            this.bt_ReloadSystem.Size = new System.Drawing.Size(267, 23);
            this.bt_ReloadSystem.TabIndex = 1;
            this.bt_ReloadSystem.Text = "Tải lại toàn bộ dữ liệu chứng khoán";
            this.bt_ReloadSystem.UseVisualStyleBackColor = true;
            this.bt_ReloadSystem.Click += new System.EventHandler(this.bt_ReloadSystem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bt_ChangePass);
            this.groupBox1.Controls.Add(this.bt_ExpandAccount);
            this.groupBox1.Controls.Add(this.lb_RemainingDays);
            this.groupBox1.Location = new System.Drawing.Point(11, 88);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(267, 110);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tài khoản";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // bt_ChangePass
            // 
            this.bt_ChangePass.Location = new System.Drawing.Point(10, 77);
            this.bt_ChangePass.Name = "bt_ChangePass";
            this.bt_ChangePass.Size = new System.Drawing.Size(147, 23);
            this.bt_ChangePass.TabIndex = 2;
            this.bt_ChangePass.Text = "Đổi password";
            this.bt_ChangePass.UseVisualStyleBackColor = true;
            this.bt_ChangePass.Click += new System.EventHandler(this.button3_Click);
            // 
            // bt_ExpandAccount
            // 
            this.bt_ExpandAccount.Location = new System.Drawing.Point(10, 48);
            this.bt_ExpandAccount.Name = "bt_ExpandAccount";
            this.bt_ExpandAccount.Size = new System.Drawing.Size(147, 23);
            this.bt_ExpandAccount.TabIndex = 1;
            this.bt_ExpandAccount.Text = "Gia hạn tài khoản";
            this.bt_ExpandAccount.UseVisualStyleBackColor = true;
            this.bt_ExpandAccount.Click += new System.EventHandler(this.bt_ExpandAccount_Click);
            // 
            // lb_RemainingDays
            // 
            this.lb_RemainingDays.AutoSize = true;
            this.lb_RemainingDays.Location = new System.Drawing.Point(7, 20);
            this.lb_RemainingDays.Name = "lb_RemainingDays";
            this.lb_RemainingDays.Size = new System.Drawing.Size(132, 13);
            this.lb_RemainingDays.TabIndex = 0;
            this.lb_RemainingDays.Text = "Tài khoản bạn còn 0 ngày";
            // 
            // cb_Autologin
            // 
            this.cb_Autologin.AutoSize = true;
            this.cb_Autologin.Location = new System.Drawing.Point(11, 230);
            this.cb_Autologin.Name = "cb_Autologin";
            this.cb_Autologin.Size = new System.Drawing.Size(92, 17);
            this.cb_Autologin.TabIndex = 3;
            this.cb_Autologin.Text = "Tự động login";
            this.cb_Autologin.UseVisualStyleBackColor = true;
            this.cb_Autologin.CheckedChanged += new System.EventHandler(this.cb_Autologin_CheckedChanged);
            // 
            // bt_OK
            // 
            this.bt_OK.Location = new System.Drawing.Point(208, 285);
            this.bt_OK.Name = "bt_OK";
            this.bt_OK.Size = new System.Drawing.Size(75, 23);
            this.bt_OK.TabIndex = 4;
            this.bt_OK.Text = "Đóng";
            this.bt_OK.UseVisualStyleBackColor = true;
            this.bt_OK.Click += new System.EventHandler(this.bt_OK_Click);
            // 
            // cb_Gainloss
            // 
            this.cb_Gainloss.AutoSize = true;
            this.cb_Gainloss.Location = new System.Drawing.Point(11, 205);
            this.cb_Gainloss.Name = "cb_Gainloss";
            this.cb_Gainloss.Size = new System.Drawing.Size(261, 17);
            this.cb_Gainloss.TabIndex = 6;
            this.cb_Gainloss.Text = "Không dùng nhóm \"Lãi - Lỗ\" (restart app required)";
            this.cb_Gainloss.UseVisualStyleBackColor = true;
            this.cb_Gainloss.CheckedChanged += new System.EventHandler(this.cb_Gainloss_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(11, 253);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(224, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Hiện thị đồ thị lịch sử ở màn hình bảng giá";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // trackBarFont
            // 
            this.trackBarFont.Location = new System.Drawing.Point(108, 37);
            this.trackBarFont.Maximum = 2;
            this.trackBarFont.Minimum = -2;
            this.trackBarFont.Name = "trackBarFont";
            this.trackBarFont.Size = new System.Drawing.Size(170, 45);
            this.trackBarFont.TabIndex = 8;
            this.trackBarFont.ValueChanged += new System.EventHandler(this.onFontChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Font bảng giá";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "(y/c chạy lại app)";
            // 
            // DlgConfig
            // 
            this.AcceptButton = this.bt_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 316);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBarFont);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.cb_Gainloss);
            this.Controls.Add(this.bt_OK);
            this.Controls.Add(this.cb_Autologin);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bt_ReloadSystem);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setting";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFont)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_ReloadSystem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bt_ChangePass;
        private System.Windows.Forms.Button bt_ExpandAccount;
        private System.Windows.Forms.Label lb_RemainingDays;
        private System.Windows.Forms.CheckBox cb_Autologin;
        private System.Windows.Forms.Button bt_OK;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.CheckBox cb_Gainloss;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TrackBar trackBarFont;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}