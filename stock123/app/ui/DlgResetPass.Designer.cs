namespace stock123.app.ui
{
    partial class DlgResetPass
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
            this.lbStatus = new System.Windows.Forms.Label();
            this.tb_Password = new System.Windows.Forms.TextBox();
            this.bt_OK = new System.Windows.Forms.Button();
            this.bt_Back = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(13, 16);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(99, 13);
            this.lbStatus.TabIndex = 0;
            this.lbStatus.Text = "Nhập mật khẩu mới";
            // 
            // tb_Password
            // 
            this.tb_Password.Location = new System.Drawing.Point(16, 36);
            this.tb_Password.Name = "tb_Password";
            this.tb_Password.Size = new System.Drawing.Size(222, 20);
            this.tb_Password.TabIndex = 1;
            // 
            // bt_OK
            // 
            this.bt_OK.Location = new System.Drawing.Point(37, 82);
            this.bt_OK.Name = "bt_OK";
            this.bt_OK.Size = new System.Drawing.Size(75, 23);
            this.bt_OK.TabIndex = 2;
            this.bt_OK.Text = "OK";
            this.bt_OK.UseVisualStyleBackColor = true;
            this.bt_OK.Click += new System.EventHandler(this.button1_Click);
            // 
            // bt_Back
            // 
            this.bt_Back.Location = new System.Drawing.Point(147, 82);
            this.bt_Back.Name = "bt_Back";
            this.bt_Back.Size = new System.Drawing.Size(75, 23);
            this.bt_Back.TabIndex = 3;
            this.bt_Back.Text = "Quay lại";
            this.bt_Back.UseVisualStyleBackColor = true;
            this.bt_Back.Click += new System.EventHandler(this.bt_Back_Click);
            // 
            // DlgResetPass
            // 
            this.AcceptButton = this.bt_Back;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(251, 117);
            this.ControlBox = false;
            this.Controls.Add(this.bt_Back);
            this.Controls.Add(this.bt_OK);
            this.Controls.Add(this.tb_Password);
            this.Controls.Add(this.lbStatus);
            this.Name = "DlgResetPass";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reset password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.TextBox tb_Password;
        private System.Windows.Forms.Button bt_OK;
        private System.Windows.Forms.Button bt_Back;
    }
}