namespace stock123.app.ui
{
    partial class DlgEditSlogan
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
            this.bt_OK = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbSloganText = new System.Windows.Forms.TextBox();
            this.btTextColor = new System.Windows.Forms.Button();
            this.btBackColor = new System.Windows.Forms.Button();
            this.btTextFont = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bt_OK
            // 
            this.bt_OK.Location = new System.Drawing.Point(199, 106);
            this.bt_OK.Name = "bt_OK";
            this.bt_OK.Size = new System.Drawing.Size(75, 23);
            this.bt_OK.TabIndex = 4;
            this.bt_OK.Text = "Đóng";
            this.bt_OK.UseVisualStyleBackColor = true;
            this.bt_OK.Click += new System.EventHandler(this.bt_OK_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbSloganText);
            this.groupBox2.Controls.Add(this.btTextColor);
            this.groupBox2.Controls.Add(this.btBackColor);
            this.groupBox2.Controls.Add(this.btTextFont);
            this.groupBox2.Location = new System.Drawing.Point(11, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(269, 84);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Slogan";
            // 
            // tbSloganText
            // 
            this.tbSloganText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSloganText.Location = new System.Drawing.Point(10, 16);
            this.tbSloganText.Name = "tbSloganText";
            this.tbSloganText.Size = new System.Drawing.Size(253, 20);
            this.tbSloganText.TabIndex = 4;
            this.tbSloganText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btTextColor
            // 
            this.btTextColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btTextColor.Location = new System.Drawing.Point(88, 44);
            this.btTextColor.Name = "btTextColor";
            this.btTextColor.Size = new System.Drawing.Size(69, 35);
            this.btTextColor.TabIndex = 3;
            this.btTextColor.Text = "Text Color";
            this.btTextColor.UseVisualStyleBackColor = true;
            this.btTextColor.Click += new System.EventHandler(this.btTextColor_Click);
            // 
            // btBackColor
            // 
            this.btBackColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btBackColor.Location = new System.Drawing.Point(163, 44);
            this.btBackColor.Name = "btBackColor";
            this.btBackColor.Size = new System.Drawing.Size(100, 35);
            this.btBackColor.TabIndex = 2;
            this.btBackColor.Text = "Background";
            this.btBackColor.UseVisualStyleBackColor = true;
            this.btBackColor.Click += new System.EventHandler(this.button2_Click);
            // 
            // btTextFont
            // 
            this.btTextFont.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btTextFont.Location = new System.Drawing.Point(10, 44);
            this.btTextFont.Name = "btTextFont";
            this.btTextFont.Size = new System.Drawing.Size(69, 35);
            this.btTextFont.TabIndex = 1;
            this.btTextFont.Text = "Font";
            this.btTextFont.UseVisualStyleBackColor = true;
            this.btTextFont.Click += new System.EventHandler(this.btTextFont_Click);
            // 
            // DlgEditSlogan
            // 
            this.AcceptButton = this.bt_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 139);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.bt_OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgEditSlogan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setting";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bt_OK;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btBackColor;
        private System.Windows.Forms.Button btTextFont;
        private System.Windows.Forms.Button btTextColor;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.TextBox tbSloganText;
    }
}