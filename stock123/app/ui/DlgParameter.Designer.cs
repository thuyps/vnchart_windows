namespace stock123.app.ui
{
    partial class DlgParameter
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
            this.cb_Indicator = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_Param1 = new System.Windows.Forms.TrackBar();
            this.lb_Param1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.bt_Color1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_Donet = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.tb_Param1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            this.SuspendLayout();
            // 
            // cb_Indicator
            // 
            this.cb_Indicator.FormattingEnabled = true;
            this.cb_Indicator.Items.AddRange(new object[] {
            "Bollinger Bands",
            "Parabollic (PSAR)",
            "Moving Average Convergence-Divergence (MACD)",
            "Relative Strength Index (RSI)",
            "Money Flow Index (MFI)",
            "Fast Stochastic",
            "Slow Stochastic",
            "Ichimoku",
            "Average Directional Index (ADX)",
            "Moving Average Envelopes (MAE)"});
            this.cb_Indicator.Location = new System.Drawing.Point(86, 13);
            this.cb_Indicator.Name = "cb_Indicator";
            this.cb_Indicator.Size = new System.Drawing.Size(272, 21);
            this.cb_Indicator.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Chỉ báo";
            // 
            // tb_Param1
            // 
            this.tb_Param1.Location = new System.Drawing.Point(25, 68);
            this.tb_Param1.Name = "tb_Param1";
            this.tb_Param1.Size = new System.Drawing.Size(487, 45);
            this.tb_Param1.TabIndex = 2;
            this.tb_Param1.TickStyle = System.Windows.Forms.TickStyle.Both;
            // 
            // lb_Param1
            // 
            this.lb_Param1.AutoSize = true;
            this.lb_Param1.Location = new System.Drawing.Point(25, 52);
            this.lb_Param1.Name = "lb_Param1";
            this.lb_Param1.Size = new System.Drawing.Size(35, 13);
            this.lb_Param1.TabIndex = 3;
            this.lb_Param1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "label1";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(25, 143);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(487, 45);
            this.trackBar1.TabIndex = 4;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.Both;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 197);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "label1";
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(25, 213);
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(487, 45);
            this.trackBar2.TabIndex = 6;
            this.trackBar2.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // bt_Color1
            // 
            this.bt_Color1.Location = new System.Drawing.Point(32, 315);
            this.bt_Color1.Name = "bt_Color1";
            this.bt_Color1.Size = new System.Drawing.Size(83, 56);
            this.bt_Color1.TabIndex = 8;
            this.bt_Color1.Text = "button1";
            this.bt_Color1.UseVisualStyleBackColor = true;
            this.bt_Color1.Click += new System.EventHandler(this.bt_Color1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 272);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Nét đậm";
            // 
            // cb_Donet
            // 
            this.cb_Donet.FormattingEnabled = true;
            this.cb_Donet.Items.AddRange(new object[] {
            "1.0",
            "1.5",
            "2.0",
            "2.5",
            "3.0"});
            this.cb_Donet.Location = new System.Drawing.Point(32, 288);
            this.cb_Donet.Name = "cb_Donet";
            this.cb_Donet.Size = new System.Drawing.Size(83, 21);
            this.cb_Donet.TabIndex = 10;
            this.cb_Donet.SelectedIndexChanged += new System.EventHandler(this.cb_Donet_SelectedIndexChanged);
            // 
            // DlgParameter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 393);
            this.Controls.Add(this.cb_Donet);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.bt_Color1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.trackBar2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.lb_Param1);
            this.Controls.Add(this.tb_Param1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_Indicator);
            this.Name = "DlgParameter";
            this.Text = "DlgParameter";
            ((System.ComponentModel.ISupportInitialize)(this.tb_Param1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_Indicator;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar tb_Param1;
        private System.Windows.Forms.Label lb_Param1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.Button bt_Color1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_Donet;
    }
}