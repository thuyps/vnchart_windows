namespace stock123.app.ui
{
    partial class DlgLineColorPicker
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
            this.cb_Thickness = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_Color = new System.Windows.Forms.Button();
            this.bt_OK = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.SuspendLayout();
            // 
            // cb_Thickness
            // 
            this.cb_Thickness.FormattingEnabled = true;
            this.cb_Thickness.Items.AddRange(new object[] {
            "1.0",
            "1.5",
            "2.0",
            "2.5",
            "3.0",
            "3.5",
            "4.0"});
            this.cb_Thickness.Location = new System.Drawing.Point(131, 11);
            this.cb_Thickness.Name = "cb_Thickness";
            this.cb_Thickness.Size = new System.Drawing.Size(133, 21);
            this.cb_Thickness.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Độ đậm của đường";
            // 
            // bt_Color
            // 
            this.bt_Color.Location = new System.Drawing.Point(16, 47);
            this.bt_Color.Name = "bt_Color";
            this.bt_Color.Size = new System.Drawing.Size(248, 63);
            this.bt_Color.TabIndex = 2;
            this.bt_Color.Text = "Chọn màu";
            this.bt_Color.UseVisualStyleBackColor = true;
            this.bt_Color.Click += new System.EventHandler(this.bt_Color_Click);
            // 
            // bt_OK
            // 
            this.bt_OK.Location = new System.Drawing.Point(189, 135);
            this.bt_OK.Name = "bt_OK";
            this.bt_OK.Size = new System.Drawing.Size(75, 23);
            this.bt_OK.TabIndex = 3;
            this.bt_OK.Text = "OK";
            this.bt_OK.UseVisualStyleBackColor = true;
            this.bt_OK.Click += new System.EventHandler(this.bt_OK_Click);
            // 
            // DlgLineColorPicker
            // 
            this.AcceptButton = this.bt_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 170);
            this.ControlBox = false;
            this.Controls.Add(this.bt_OK);
            this.Controls.Add(this.bt_Color);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_Thickness);
            this.Name = "DlgLineColorPicker";
            this.Text = "LineColorPicker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_Thickness;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bt_Color;
        private System.Windows.Forms.Button bt_OK;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}