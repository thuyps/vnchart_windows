namespace stock123.app.ui
{
    partial class DlgRegister
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
            this.tb_Email = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bt_Register = new System.Windows.Forms.Button();
            this.bt_Back = new System.Windows.Forms.Button();
            this.lbStatus = new System.Windows.Forms.Label();
            this.tb_Password = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tb_Email
            // 
            this.tb_Email.Location = new System.Drawing.Point(81, 30);
            this.tb_Email.Name = "tb_Email";
            this.tb_Email.Size = new System.Drawing.Size(199, 20);
            this.tb_Email.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Email";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password";
            // 
            // bt_Register
            // 
            this.bt_Register.Location = new System.Drawing.Point(103, 103);
            this.bt_Register.Name = "bt_Register";
            this.bt_Register.Size = new System.Drawing.Size(75, 23);
            this.bt_Register.TabIndex = 4;
            this.bt_Register.Text = "Đăng ký";
            this.bt_Register.UseVisualStyleBackColor = true;
            this.bt_Register.Click += new System.EventHandler(this.button1_Click);
            // 
            // bt_Back
            // 
            this.bt_Back.Location = new System.Drawing.Point(205, 103);
            this.bt_Back.Name = "bt_Back";
            this.bt_Back.Size = new System.Drawing.Size(75, 23);
            this.bt_Back.TabIndex = 5;
            this.bt_Back.Text = "Quay lại";
            this.bt_Back.UseVisualStyleBackColor = true;
            this.bt_Back.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(13, 2);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(94, 13);
            this.lbStatus.TabIndex = 7;
            this.lbStatus.Text = "Đăng ký tài khoản";
            // 
            // tb_Password
            // 
            this.tb_Password.Location = new System.Drawing.Point(81, 61);
            this.tb_Password.Name = "tb_Password";
            this.tb_Password.PasswordChar = '*';
            this.tb_Password.Size = new System.Drawing.Size(199, 20);
            this.tb_Password.TabIndex = 3;
            // 
            // DlgLogin
            // 
            this.AcceptButton = this.bt_Register;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 132);
            this.ControlBox = false;
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.bt_Back);
            this.Controls.Add(this.bt_Register);
            this.Controls.Add(this.tb_Password);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_Email);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Register";
            this.Load += new System.EventHandler(this.DlgLogin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_Email;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bt_Register;
        private System.Windows.Forms.Button bt_Back;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.TextBox tb_Password;
    }
}