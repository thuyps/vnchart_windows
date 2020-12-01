namespace stock123.app.ui
{
    partial class DlgSMA
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
            this.tb_Days1 = new System.Windows.Forms.TextBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.bt_Color1 = new System.Windows.Forms.Button();
            this.cb_Thickness1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.bt_OK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_SMA1 = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_SMA2 = new System.Windows.Forms.CheckBox();
            this.bt_Color2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_Thickness2 = new System.Windows.Forms.ComboBox();
            this.tb_Days2 = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cb_SMA3 = new System.Windows.Forms.CheckBox();
            this.bt_Color3 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cb_Thickness3 = new System.Windows.Forms.ComboBox();
            this.tb_Days3 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rb_EMA = new System.Windows.Forms.RadioButton();
            this.rb_SMA = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cb_SMA5 = new System.Windows.Forms.CheckBox();
            this.bt_Color5 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cb_Thickness5 = new System.Windows.Forms.ComboBox();
            this.tb_Days5 = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cb_SMA4 = new System.Windows.Forms.CheckBox();
            this.bt_Color4 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cb_Thickness4 = new System.Windows.Forms.ComboBox();
            this.tb_Days4 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Số ngày";
            // 
            // tb_Days1
            // 
            this.tb_Days1.Location = new System.Drawing.Point(77, 24);
            this.tb_Days1.Name = "tb_Days1";
            this.tb_Days1.Size = new System.Drawing.Size(103, 20);
            this.tb_Days1.TabIndex = 1;
            // 
            // bt_Color1
            // 
            this.bt_Color1.Location = new System.Drawing.Point(189, 19);
            this.bt_Color1.Name = "bt_Color1";
            this.bt_Color1.Size = new System.Drawing.Size(60, 57);
            this.bt_Color1.TabIndex = 3;
            this.bt_Color1.Text = "Màu";
            this.bt_Color1.UseVisualStyleBackColor = true;
            this.bt_Color1.Click += new System.EventHandler(this.bt_Color1_Click);
            // 
            // cb_Thickness1
            // 
            this.cb_Thickness1.FormattingEnabled = true;
            this.cb_Thickness1.Items.AddRange(new object[] {
            "1.0",
            "1.5",
            "2.0",
            "2.5",
            "3.0",
            "3.5",
            "4.0"});
            this.cb_Thickness1.Location = new System.Drawing.Point(77, 55);
            this.cb_Thickness1.Name = "cb_Thickness1";
            this.cb_Thickness1.Size = new System.Drawing.Size(103, 21);
            this.cb_Thickness1.TabIndex = 4;
            this.cb_Thickness1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Độ đậm";
            // 
            // bt_OK
            // 
            this.bt_OK.Location = new System.Drawing.Point(330, 343);
            this.bt_OK.Name = "bt_OK";
            this.bt_OK.Size = new System.Drawing.Size(75, 23);
            this.bt_OK.TabIndex = 6;
            this.bt_OK.Text = "Vẽ SMA";
            this.bt_OK.UseVisualStyleBackColor = true;
            this.bt_OK.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_SMA1);
            this.groupBox1.Controls.Add(this.bt_Color1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cb_Thickness1);
            this.groupBox1.Controls.Add(this.tb_Days1);
            this.groupBox1.Location = new System.Drawing.Point(3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 84);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // cb_SMA1
            // 
            this.cb_SMA1.AutoSize = true;
            this.cb_SMA1.Location = new System.Drawing.Point(20, 0);
            this.cb_SMA1.Name = "cb_SMA1";
            this.cb_SMA1.Size = new System.Drawing.Size(58, 17);
            this.cb_SMA1.TabIndex = 6;
            this.cb_SMA1.Text = "SMA 1";
            this.cb_SMA1.UseVisualStyleBackColor = true;
            this.cb_SMA1.CheckedChanged += new System.EventHandler(this.cb_SMA1_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cb_SMA2);
            this.groupBox2.Controls.Add(this.bt_Color2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cb_Thickness2);
            this.groupBox2.Controls.Add(this.tb_Days2);
            this.groupBox2.Location = new System.Drawing.Point(4, 98);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(263, 84);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            // 
            // cb_SMA2
            // 
            this.cb_SMA2.AutoSize = true;
            this.cb_SMA2.Location = new System.Drawing.Point(20, 0);
            this.cb_SMA2.Name = "cb_SMA2";
            this.cb_SMA2.Size = new System.Drawing.Size(58, 17);
            this.cb_SMA2.TabIndex = 6;
            this.cb_SMA2.Text = "SMA 2";
            this.cb_SMA2.UseVisualStyleBackColor = true;
            this.cb_SMA2.CheckedChanged += new System.EventHandler(this.cb_SMA2_CheckedChanged);
            // 
            // bt_Color2
            // 
            this.bt_Color2.Location = new System.Drawing.Point(188, 21);
            this.bt_Color2.Name = "bt_Color2";
            this.bt_Color2.Size = new System.Drawing.Size(60, 57);
            this.bt_Color2.TabIndex = 3;
            this.bt_Color2.Text = "Màu";
            this.bt_Color2.UseVisualStyleBackColor = true;
            this.bt_Color2.Click += new System.EventHandler(this.bt_Color2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Độ đậm";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Số ngày";
            // 
            // cb_Thickness2
            // 
            this.cb_Thickness2.FormattingEnabled = true;
            this.cb_Thickness2.Items.AddRange(new object[] {
            "1.0",
            "1.5",
            "2.0",
            "2.5",
            "3.0",
            "3.5",
            "4.0"});
            this.cb_Thickness2.Location = new System.Drawing.Point(77, 55);
            this.cb_Thickness2.Name = "cb_Thickness2";
            this.cb_Thickness2.Size = new System.Drawing.Size(102, 21);
            this.cb_Thickness2.TabIndex = 4;
            this.cb_Thickness2.TabStop = false;
            // 
            // tb_Days2
            // 
            this.tb_Days2.Location = new System.Drawing.Point(77, 24);
            this.tb_Days2.Name = "tb_Days2";
            this.tb_Days2.Size = new System.Drawing.Size(102, 20);
            this.tb_Days2.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cb_SMA3);
            this.groupBox3.Controls.Add(this.bt_Color3);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.cb_Thickness3);
            this.groupBox3.Controls.Add(this.tb_Days3);
            this.groupBox3.Location = new System.Drawing.Point(4, 194);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(263, 84);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            // 
            // cb_SMA3
            // 
            this.cb_SMA3.AutoSize = true;
            this.cb_SMA3.Location = new System.Drawing.Point(20, 0);
            this.cb_SMA3.Name = "cb_SMA3";
            this.cb_SMA3.Size = new System.Drawing.Size(58, 17);
            this.cb_SMA3.TabIndex = 6;
            this.cb_SMA3.Text = "SMA 3";
            this.cb_SMA3.UseVisualStyleBackColor = true;
            this.cb_SMA3.CheckedChanged += new System.EventHandler(this.cb_SMA3_CheckedChanged);
            // 
            // bt_Color3
            // 
            this.bt_Color3.Location = new System.Drawing.Point(185, 19);
            this.bt_Color3.Name = "bt_Color3";
            this.bt_Color3.Size = new System.Drawing.Size(63, 57);
            this.bt_Color3.TabIndex = 3;
            this.bt_Color3.Text = "Màu";
            this.bt_Color3.UseVisualStyleBackColor = true;
            this.bt_Color3.Click += new System.EventHandler(this.bt_Color3_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Độ đậm";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Số ngày";
            // 
            // cb_Thickness3
            // 
            this.cb_Thickness3.FormattingEnabled = true;
            this.cb_Thickness3.Items.AddRange(new object[] {
            "1.0",
            "1.5",
            "2.0",
            "2.5",
            "3.0",
            "3.5",
            "4.0"});
            this.cb_Thickness3.Location = new System.Drawing.Point(77, 55);
            this.cb_Thickness3.Name = "cb_Thickness3";
            this.cb_Thickness3.Size = new System.Drawing.Size(102, 21);
            this.cb_Thickness3.TabIndex = 4;
            this.cb_Thickness3.TabStop = false;
            // 
            // tb_Days3
            // 
            this.tb_Days3.Location = new System.Drawing.Point(77, 24);
            this.tb_Days3.Name = "tb_Days3";
            this.tb_Days3.Size = new System.Drawing.Size(102, 20);
            this.tb_Days3.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(458, 343);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Quay lại";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rb_EMA);
            this.groupBox4.Controls.Add(this.rb_SMA);
            this.groupBox4.Location = new System.Drawing.Point(298, 275);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(235, 47);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "SMA / EMA";
            // 
            // rb_EMA
            // 
            this.rb_EMA.AutoSize = true;
            this.rb_EMA.Location = new System.Drawing.Point(142, 19);
            this.rb_EMA.Name = "rb_EMA";
            this.rb_EMA.Size = new System.Drawing.Size(48, 17);
            this.rb_EMA.TabIndex = 1;
            this.rb_EMA.TabStop = true;
            this.rb_EMA.Text = "EMA";
            this.rb_EMA.UseVisualStyleBackColor = true;
            this.rb_EMA.CheckedChanged += new System.EventHandler(this.rb_EMA_CheckedChanged);
            // 
            // rb_SMA
            // 
            this.rb_SMA.AutoSize = true;
            this.rb_SMA.Location = new System.Drawing.Point(49, 19);
            this.rb_SMA.Name = "rb_SMA";
            this.rb_SMA.Size = new System.Drawing.Size(48, 17);
            this.rb_SMA.TabIndex = 0;
            this.rb_SMA.TabStop = true;
            this.rb_SMA.Text = "SMA";
            this.rb_SMA.UseVisualStyleBackColor = true;
            this.rb_SMA.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cb_SMA5);
            this.groupBox5.Controls.Add(this.bt_Color5);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.cb_Thickness5);
            this.groupBox5.Controls.Add(this.tb_Days5);
            this.groupBox5.Location = new System.Drawing.Point(285, 98);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(263, 84);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            // 
            // cb_SMA5
            // 
            this.cb_SMA5.AutoSize = true;
            this.cb_SMA5.Location = new System.Drawing.Point(20, 0);
            this.cb_SMA5.Name = "cb_SMA5";
            this.cb_SMA5.Size = new System.Drawing.Size(58, 17);
            this.cb_SMA5.TabIndex = 6;
            this.cb_SMA5.Text = "SMA 5";
            this.cb_SMA5.UseVisualStyleBackColor = true;
            this.cb_SMA5.CheckedChanged += new System.EventHandler(this.cb_SMA5_CheckedChanged);
            // 
            // bt_Color5
            // 
            this.bt_Color5.Location = new System.Drawing.Point(188, 21);
            this.bt_Color5.Name = "bt_Color5";
            this.bt_Color5.Size = new System.Drawing.Size(60, 57);
            this.bt_Color5.TabIndex = 3;
            this.bt_Color5.Text = "Màu";
            this.bt_Color5.UseVisualStyleBackColor = true;
            this.bt_Color5.Click += new System.EventHandler(this.bt_Color5_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 63);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Độ đậm";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 31);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Số ngày";
            // 
            // cb_Thickness5
            // 
            this.cb_Thickness5.FormattingEnabled = true;
            this.cb_Thickness5.Items.AddRange(new object[] {
            "1.0",
            "1.5",
            "2.0",
            "2.5",
            "3.0",
            "3.5",
            "4.0"});
            this.cb_Thickness5.Location = new System.Drawing.Point(77, 55);
            this.cb_Thickness5.Name = "cb_Thickness5";
            this.cb_Thickness5.Size = new System.Drawing.Size(102, 21);
            this.cb_Thickness5.TabIndex = 4;
            this.cb_Thickness5.TabStop = false;
            // 
            // tb_Days5
            // 
            this.tb_Days5.Location = new System.Drawing.Point(77, 24);
            this.tb_Days5.Name = "tb_Days5";
            this.tb_Days5.Size = new System.Drawing.Size(102, 20);
            this.tb_Days5.TabIndex = 1;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cb_SMA4);
            this.groupBox6.Controls.Add(this.bt_Color4);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Controls.Add(this.cb_Thickness4);
            this.groupBox6.Controls.Add(this.tb_Days4);
            this.groupBox6.Location = new System.Drawing.Point(284, 2);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(264, 84);
            this.groupBox6.TabIndex = 12;
            this.groupBox6.TabStop = false;
            // 
            // cb_SMA4
            // 
            this.cb_SMA4.AutoSize = true;
            this.cb_SMA4.Location = new System.Drawing.Point(20, 0);
            this.cb_SMA4.Name = "cb_SMA4";
            this.cb_SMA4.Size = new System.Drawing.Size(58, 17);
            this.cb_SMA4.TabIndex = 6;
            this.cb_SMA4.Text = "SMA 4";
            this.cb_SMA4.UseVisualStyleBackColor = true;
            this.cb_SMA4.CheckedChanged += new System.EventHandler(this.cb_SMA4_CheckedChanged);
            // 
            // bt_Color4
            // 
            this.bt_Color4.Location = new System.Drawing.Point(189, 19);
            this.bt_Color4.Name = "bt_Color4";
            this.bt_Color4.Size = new System.Drawing.Size(60, 57);
            this.bt_Color4.TabIndex = 3;
            this.bt_Color4.Text = "Màu";
            this.bt_Color4.UseVisualStyleBackColor = true;
            this.bt_Color4.Click += new System.EventHandler(this.bt_Color4_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(25, 63);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "Độ đậm";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(25, 31);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Số ngày";
            // 
            // cb_Thickness4
            // 
            this.cb_Thickness4.FormattingEnabled = true;
            this.cb_Thickness4.Items.AddRange(new object[] {
            "1.0",
            "1.5",
            "2.0",
            "2.5",
            "3.0",
            "3.5",
            "4.0"});
            this.cb_Thickness4.Location = new System.Drawing.Point(77, 55);
            this.cb_Thickness4.Name = "cb_Thickness4";
            this.cb_Thickness4.Size = new System.Drawing.Size(103, 21);
            this.cb_Thickness4.TabIndex = 4;
            this.cb_Thickness4.TabStop = false;
            // 
            // tb_Days4
            // 
            this.tb_Days4.Location = new System.Drawing.Point(77, 24);
            this.tb_Days4.Name = "tb_Days4";
            this.tb_Days4.Size = new System.Drawing.Size(103, 20);
            this.tb_Days4.TabIndex = 1;
            // 
            // DlgSMA
            // 
            this.AcceptButton = this.bt_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 375);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.bt_OK);
            this.Controls.Add(this.groupBox1);
            this.Name = "DlgSMA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DlgSMA";
            this.Load += new System.EventHandler(this.DlgSMA_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_Days1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button bt_Color1;
        private System.Windows.Forms.ComboBox cb_Thickness1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bt_OK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cb_SMA1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cb_SMA2;
        private System.Windows.Forms.Button bt_Color2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_Thickness2;
        private System.Windows.Forms.TextBox tb_Days2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cb_SMA3;
        private System.Windows.Forms.Button bt_Color3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cb_Thickness3;
        private System.Windows.Forms.TextBox tb_Days3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rb_EMA;
        private System.Windows.Forms.RadioButton rb_SMA;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox cb_SMA5;
        private System.Windows.Forms.Button bt_Color5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cb_Thickness5;
        private System.Windows.Forms.TextBox tb_Days5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox cb_SMA4;
        private System.Windows.Forms.Button bt_Color4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cb_Thickness4;
        private System.Windows.Forms.TextBox tb_Days4;
    }
}