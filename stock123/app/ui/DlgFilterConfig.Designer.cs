namespace stock123.app.ui
{
    partial class DlgFilterConfig
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
            this.cb_KLTB = new System.Windows.Forms.CheckBox();
            this.tb_GTTB = new System.Windows.Forms.TextBox();
            this.tb_HiPrice = new System.Windows.Forms.TextBox();
            this.cb_HiPrice = new System.Windows.Forms.CheckBox();
            this.tb_LoPrice = new System.Windows.Forms.TextBox();
            this.cbLoPrice = new System.Windows.Forms.CheckBox();
            this.bt_OK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cb_KLTB
            // 
            this.cb_KLTB.AutoSize = true;
            this.cb_KLTB.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cb_KLTB.Location = new System.Drawing.Point(9, 20);
            this.cb_KLTB.Name = "cb_KLTB";
            this.cb_KLTB.Size = new System.Drawing.Size(163, 17);
            this.cb_KLTB.TabIndex = 1;
            this.cb_KLTB.Text = "Giá trị giao dịch TB lớn hơn >";
            this.cb_KLTB.UseVisualStyleBackColor = true;
            // 
            // tb_GTTB
            // 
            this.tb_GTTB.Location = new System.Drawing.Point(179, 18);
            this.tb_GTTB.Name = "tb_GTTB";
            this.tb_GTTB.Size = new System.Drawing.Size(116, 20);
            this.tb_GTTB.TabIndex = 2;
            // 
            // tb_HiPrice
            // 
            this.tb_HiPrice.Location = new System.Drawing.Point(174, 84);
            this.tb_HiPrice.Name = "tb_HiPrice";
            this.tb_HiPrice.Size = new System.Drawing.Size(116, 20);
            this.tb_HiPrice.TabIndex = 4;
            // 
            // cb_HiPrice
            // 
            this.cb_HiPrice.AutoSize = true;
            this.cb_HiPrice.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cb_HiPrice.Location = new System.Drawing.Point(79, 84);
            this.cb_HiPrice.Name = "cb_HiPrice";
            this.cb_HiPrice.Size = new System.Drawing.Size(87, 17);
            this.cb_HiPrice.TabIndex = 3;
            this.cb_HiPrice.Text = "Giá cao nhất";
            this.cb_HiPrice.UseVisualStyleBackColor = true;
            // 
            // tb_LoPrice
            // 
            this.tb_LoPrice.Location = new System.Drawing.Point(174, 112);
            this.tb_LoPrice.Name = "tb_LoPrice";
            this.tb_LoPrice.Size = new System.Drawing.Size(116, 20);
            this.tb_LoPrice.TabIndex = 6;
            // 
            // cbLoPrice
            // 
            this.cbLoPrice.AutoSize = true;
            this.cbLoPrice.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbLoPrice.Location = new System.Drawing.Point(76, 112);
            this.cbLoPrice.Name = "cbLoPrice";
            this.cbLoPrice.Size = new System.Drawing.Size(90, 17);
            this.cbLoPrice.TabIndex = 5;
            this.cbLoPrice.Text = "Giá thấp nhất";
            this.cbLoPrice.UseVisualStyleBackColor = true;
            // 
            // bt_OK
            // 
            this.bt_OK.Location = new System.Drawing.Point(242, 159);
            this.bt_OK.Name = "bt_OK";
            this.bt_OK.Size = new System.Drawing.Size(75, 23);
            this.bt_OK.TabIndex = 7;
            this.bt_OK.Text = "OK";
            this.bt_OK.UseVisualStyleBackColor = true;
            this.bt_OK.Click += new System.EventHandler(this.bt_OK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cb_HiPrice);
            this.groupBox1.Controls.Add(this.cbLoPrice);
            this.groupBox1.Controls.Add(this.tb_LoPrice);
            this.groupBox1.Controls.Add(this.tb_HiPrice);
            this.groupBox1.Location = new System.Drawing.Point(5, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(328, 153);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(307, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Chỉ lọc cổ phiếu có giá nằm giữa Giá cao nhất và Giá thấp nhất";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(292, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "triệu";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(292, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "vnđ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(292, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "vnđ";
            // 
            // DlgFilterConfig
            // 
            this.AcceptButton = this.bt_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 191);
            this.ControlBox = false;
            this.Controls.Add(this.bt_OK);
            this.Controls.Add(this.tb_GTTB);
            this.Controls.Add(this.cb_KLTB);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgFilterConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Filter";
            this.Load += new System.EventHandler(this.DlgFilterConfig_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cb_KLTB;
        private System.Windows.Forms.TextBox tb_GTTB;
        private System.Windows.Forms.TextBox tb_HiPrice;
        private System.Windows.Forms.CheckBox cb_HiPrice;
        private System.Windows.Forms.TextBox tb_LoPrice;
        private System.Windows.Forms.CheckBox cbLoPrice;
        private System.Windows.Forms.Button bt_OK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;

    }
}