namespace stock123.app.ui
{
    partial class DlgBuyMore
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
            this.lb_Quote = new System.Windows.Forms.Label();
            this.bt_Buy = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_Volume = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_Price = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mã cổ phiếu";
            // 
            // lb_Quote
            // 
            this.lb_Quote.AutoSize = true;
            this.lb_Quote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Quote.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lb_Quote.Location = new System.Drawing.Point(99, 13);
            this.lb_Quote.Name = "lb_Quote";
            this.lb_Quote.Size = new System.Drawing.Size(31, 13);
            this.lb_Quote.TabIndex = 1;
            this.lb_Quote.Text = "ACB";
            // 
            // bt_Buy
            // 
            this.bt_Buy.Location = new System.Drawing.Point(45, 121);
            this.bt_Buy.Name = "bt_Buy";
            this.bt_Buy.Size = new System.Drawing.Size(75, 23);
            this.bt_Buy.TabIndex = 3;
            this.bt_Buy.Text = "Mua";
            this.bt_Buy.UseVisualStyleBackColor = true;
            this.bt_Buy.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(179, 121);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Quay lại";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(236, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "(cổ phiếu)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(236, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "(vnđ)";
            // 
            // tb_Volume
            // 
            this.tb_Volume.Location = new System.Drawing.Point(102, 75);
            this.tb_Volume.Name = "tb_Volume";
            this.tb_Volume.Size = new System.Drawing.Size(128, 20);
            this.tb_Volume.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Khối lượng mua";
            // 
            // tb_Price
            // 
            this.tb_Price.Location = new System.Drawing.Point(102, 39);
            this.tb_Price.Name = "tb_Price";
            this.tb_Price.Size = new System.Drawing.Size(128, 20);
            this.tb_Price.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(50, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Giá mua";
            // 
            // DlgBuyMore
            // 
            this.AcceptButton = this.bt_Buy;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(303, 154);
            this.ControlBox = false;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_Volume);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_Price);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.bt_Buy);
            this.Controls.Add(this.lb_Quote);
            this.Controls.Add(this.label1);
            this.Name = "DlgBuyMore";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Buy stock";
            this.Load += new System.EventHandler(this.DlgBuyMore_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_Quote;
        private System.Windows.Forms.Button bt_Buy;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_Volume;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_Price;
        private System.Windows.Forms.Label label6;
    }
}