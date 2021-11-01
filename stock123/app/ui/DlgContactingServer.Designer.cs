namespace stock123.app.ui
{
    partial class DlgContactingServer
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
            this.label1 = new System.Windows.Forms.Label();
            this.lb_Downloaded = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Đang kết nối server, vui lòng đợi...";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // lb_Downloaded
            // 
            this.lb_Downloaded.AutoSize = true;
            this.lb_Downloaded.Location = new System.Drawing.Point(112, 50);
            this.lb_Downloaded.Name = "lb_Downloaded";
            this.lb_Downloaded.Size = new System.Drawing.Size(30, 13);
            this.lb_Downloaded.TabIndex = 1;
            this.lb_Downloaded.Text = "0 KB";
            this.lb_Downloaded.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // DlgContactingServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 95);
            this.Controls.Add(this.lb_Downloaded);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgContactingServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "vnChart 7.47";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_Downloaded;
    }
}