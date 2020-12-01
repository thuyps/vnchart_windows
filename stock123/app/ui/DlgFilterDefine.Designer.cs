namespace stock123.app.ui
{
    partial class DlgFilterDefine
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
            this.cb_RSI_SMA = new System.Windows.Forms.CheckBox();
            this.cb_MACD_Signal = new System.Windows.Forms.CheckBox();
            this.cb_MFI_SMA = new System.Windows.Forms.CheckBox();
            this.cb_SlowStochastics_K_D = new System.Windows.Forms.CheckBox();
            this.cb_PSAR_Bullish = new System.Windows.Forms.CheckBox();
            this.cb_Tenkan_Kijun = new System.Windows.Forms.CheckBox();
            this.cb_PriceAboveKumo = new System.Windows.Forms.CheckBox();
            this.cb_VolumeUp = new System.Windows.Forms.CheckBox();
            this.cb_RSI_higher = new System.Windows.Forms.CheckBox();
            this.cb_MFI_higher = new System.Windows.Forms.CheckBox();
            this.cb_ROC_higher = new System.Windows.Forms.CheckBox();
            this.tb_Name = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_cut_DMIs = new System.Windows.Forms.CheckBox();
            this.cb_NVI_Bullish = new System.Windows.Forms.CheckBox();
            this.cb_MACDConvergency = new System.Windows.Forms.CheckBox();
            this.cb_ADL_SMA = new System.Windows.Forms.CheckBox();
            this.cb_ROC_SMA = new System.Windows.Forms.CheckBox();
            this.cb_SMA1_SMA2 = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cb_Accumulation = new System.Windows.Forms.CheckBox();
            this.bt_OK = new System.Windows.Forms.Button();
            this.bt_Back = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Advance = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lb_SMA2 = new System.Windows.Forms.Label();
            this.tb_SMA2 = new System.Windows.Forms.TextBox();
            this.tb_SMA1 = new System.Windows.Forms.TextBox();
            this.lb_SMA1 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cbSMACutPrice12 = new System.Windows.Forms.CheckBox();
            this.cbSMACutPrice100 = new System.Windows.Forms.CheckBox();
            this.cbSMACutPrice50 = new System.Windows.Forms.CheckBox();
            this.cbSMACutPrice26 = new System.Windows.Forms.CheckBox();
            this.cbSMACutPrice9 = new System.Windows.Forms.CheckBox();
            this.cbSMACutPrice5 = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cb_MFI_lower = new System.Windows.Forms.CheckBox();
            this.cb_RSI_lower = new System.Windows.Forms.CheckBox();
            this.cb_ROS_lower = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // cb_RSI_SMA
            // 
            this.cb_RSI_SMA.AutoSize = true;
            this.cb_RSI_SMA.Location = new System.Drawing.Point(25, 109);
            this.cb_RSI_SMA.Name = "cb_RSI_SMA";
            this.cb_RSI_SMA.Size = new System.Drawing.Size(143, 17);
            this.cb_RSI_SMA.TabIndex = 0;
            this.cb_RSI_SMA.Text = "RSI cắt trên đường SMA";
            this.cb_RSI_SMA.UseVisualStyleBackColor = true;
            this.cb_RSI_SMA.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // cb_MACD_Signal
            // 
            this.cb_MACD_Signal.AutoSize = true;
            this.cb_MACD_Signal.Location = new System.Drawing.Point(25, 21);
            this.cb_MACD_Signal.Name = "cb_MACD_Signal";
            this.cb_MACD_Signal.Size = new System.Drawing.Size(169, 17);
            this.cb_MACD_Signal.TabIndex = 1;
            this.cb_MACD_Signal.Text = "MACD cắt trên đường tín hiệu";
            this.cb_MACD_Signal.UseVisualStyleBackColor = true;
            // 
            // cb_MFI_SMA
            // 
            this.cb_MFI_SMA.AutoSize = true;
            this.cb_MFI_SMA.Location = new System.Drawing.Point(25, 131);
            this.cb_MFI_SMA.Name = "cb_MFI_SMA";
            this.cb_MFI_SMA.Size = new System.Drawing.Size(143, 17);
            this.cb_MFI_SMA.TabIndex = 2;
            this.cb_MFI_SMA.Text = "MFI cắt trên đường SMA";
            this.cb_MFI_SMA.UseVisualStyleBackColor = true;
            // 
            // cb_SlowStochastics_K_D
            // 
            this.cb_SlowStochastics_K_D.AutoSize = true;
            this.cb_SlowStochastics_K_D.Location = new System.Drawing.Point(25, 65);
            this.cb_SlowStochastics_K_D.Name = "cb_SlowStochastics_K_D";
            this.cb_SlowStochastics_K_D.Size = new System.Drawing.Size(181, 17);
            this.cb_SlowStochastics_K_D.TabIndex = 3;
            this.cb_SlowStochastics_K_D.Text = "Slow Stochastic: %K cắt trên %D";
            this.cb_SlowStochastics_K_D.UseVisualStyleBackColor = true;
            // 
            // cb_PSAR_Bullish
            // 
            this.cb_PSAR_Bullish.AutoSize = true;
            this.cb_PSAR_Bullish.Location = new System.Drawing.Point(24, 19);
            this.cb_PSAR_Bullish.Name = "cb_PSAR_Bullish";
            this.cb_PSAR_Bullish.Size = new System.Drawing.Size(130, 17);
            this.cb_PSAR_Bullish.TabIndex = 4;
            this.cb_PSAR_Bullish.Text = "PSAR đảo chiều tăng";
            this.cb_PSAR_Bullish.UseVisualStyleBackColor = true;
            // 
            // cb_Tenkan_Kijun
            // 
            this.cb_Tenkan_Kijun.AutoSize = true;
            this.cb_Tenkan_Kijun.Location = new System.Drawing.Point(25, 87);
            this.cb_Tenkan_Kijun.Name = "cb_Tenkan_Kijun";
            this.cb_Tenkan_Kijun.Size = new System.Drawing.Size(128, 17);
            this.cb_Tenkan_Kijun.TabIndex = 5;
            this.cb_Tenkan_Kijun.Text = "Tenkan cắt trên Kijun";
            this.cb_Tenkan_Kijun.UseVisualStyleBackColor = true;
            // 
            // cb_PriceAboveKumo
            // 
            this.cb_PriceAboveKumo.AutoSize = true;
            this.cb_PriceAboveKumo.Location = new System.Drawing.Point(24, 41);
            this.cb_PriceAboveKumo.Name = "cb_PriceAboveKumo";
            this.cb_PriceAboveKumo.Size = new System.Drawing.Size(118, 17);
            this.cb_PriceAboveKumo.TabIndex = 6;
            this.cb_PriceAboveKumo.Text = "Giá vượt mây Kumo";
            this.cb_PriceAboveKumo.UseVisualStyleBackColor = true;
            // 
            // cb_VolumeUp
            // 
            this.cb_VolumeUp.AutoSize = true;
            this.cb_VolumeUp.Location = new System.Drawing.Point(24, 63);
            this.cb_VolumeUp.Name = "cb_VolumeUp";
            this.cb_VolumeUp.Size = new System.Drawing.Size(106, 17);
            this.cb_VolumeUp.TabIndex = 7;
            this.cb_VolumeUp.Text = "Volume tăng dần";
            this.cb_VolumeUp.UseVisualStyleBackColor = true;
            // 
            // cb_RSI_higher
            // 
            this.cb_RSI_higher.AutoSize = true;
            this.cb_RSI_higher.Location = new System.Drawing.Point(12, 15);
            this.cb_RSI_higher.Name = "cb_RSI_higher";
            this.cb_RSI_higher.Size = new System.Drawing.Size(68, 17);
            this.cb_RSI_higher.TabIndex = 8;
            this.cb_RSI_higher.Text = "RSI > 30";
            this.cb_RSI_higher.UseVisualStyleBackColor = true;
            this.cb_RSI_higher.CheckedChanged += new System.EventHandler(this.cb_RSI_higher_CheckedChanged);
            // 
            // cb_MFI_higher
            // 
            this.cb_MFI_higher.AutoSize = true;
            this.cb_MFI_higher.Location = new System.Drawing.Point(12, 37);
            this.cb_MFI_higher.Name = "cb_MFI_higher";
            this.cb_MFI_higher.Size = new System.Drawing.Size(68, 17);
            this.cb_MFI_higher.TabIndex = 9;
            this.cb_MFI_higher.Text = "MFI > 30";
            this.cb_MFI_higher.UseVisualStyleBackColor = true;
            // 
            // cb_ROC_higher
            // 
            this.cb_ROC_higher.AutoSize = true;
            this.cb_ROC_higher.Location = new System.Drawing.Point(12, 60);
            this.cb_ROC_higher.Name = "cb_ROC_higher";
            this.cb_ROC_higher.Size = new System.Drawing.Size(75, 17);
            this.cb_ROC_higher.TabIndex = 12;
            this.cb_ROC_higher.Text = "ROC > 5%";
            this.cb_ROC_higher.UseVisualStyleBackColor = true;
            // 
            // tb_Name
            // 
            this.tb_Name.Location = new System.Drawing.Point(69, 12);
            this.tb_Name.Name = "tb_Name";
            this.tb_Name.Size = new System.Drawing.Size(391, 20);
            this.tb_Name.TabIndex = 13;
            this.tb_Name.Text = "Tín hiệu kỹ thuật";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_MFI_higher);
            this.groupBox1.Controls.Add(this.cb_RSI_higher);
            this.groupBox1.Controls.Add(this.cb_ROC_higher);
            this.groupBox1.Location = new System.Drawing.Point(260, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(95, 84);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cb_cut_DMIs);
            this.groupBox2.Controls.Add(this.cb_NVI_Bullish);
            this.groupBox2.Controls.Add(this.cb_MACDConvergency);
            this.groupBox2.Controls.Add(this.cb_ADL_SMA);
            this.groupBox2.Controls.Add(this.cb_ROC_SMA);
            this.groupBox2.Controls.Add(this.cb_MACD_Signal);
            this.groupBox2.Controls.Add(this.cb_SlowStochastics_K_D);
            this.groupBox2.Controls.Add(this.cb_RSI_SMA);
            this.groupBox2.Controls.Add(this.cb_MFI_SMA);
            this.groupBox2.Controls.Add(this.cb_Tenkan_Kijun);
            this.groupBox2.Location = new System.Drawing.Point(24, 51);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(216, 268);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Giao cắt Indicators";
            // 
            // cb_cut_DMIs
            // 
            this.cb_cut_DMIs.AutoSize = true;
            this.cb_cut_DMIs.ForeColor = System.Drawing.Color.Magenta;
            this.cb_cut_DMIs.Location = new System.Drawing.Point(25, 220);
            this.cb_cut_DMIs.Name = "cb_cut_DMIs";
            this.cb_cut_DMIs.Size = new System.Drawing.Size(124, 17);
            this.cb_cut_DMIs.TabIndex = 10;
            this.cb_cut_DMIs.Text = "ADX: DMI+ cắt DMI-";
            this.cb_cut_DMIs.UseVisualStyleBackColor = true;
            // 
            // cb_NVI_Bullish
            // 
            this.cb_NVI_Bullish.AutoSize = true;
            this.cb_NVI_Bullish.ForeColor = System.Drawing.Color.Magenta;
            this.cb_NVI_Bullish.Location = new System.Drawing.Point(25, 197);
            this.cb_NVI_Bullish.Name = "cb_NVI_Bullish";
            this.cb_NVI_Bullish.Size = new System.Drawing.Size(77, 17);
            this.cb_NVI_Bullish.TabIndex = 9;
            this.cb_NVI_Bullish.Text = "NVI Bullish";
            this.cb_NVI_Bullish.UseVisualStyleBackColor = true;
            this.cb_NVI_Bullish.CheckedChanged += new System.EventHandler(this.cb_NVI_Bullish_CheckedChanged);
            // 
            // cb_MACDConvergency
            // 
            this.cb_MACDConvergency.AutoSize = true;
            this.cb_MACDConvergency.Location = new System.Drawing.Point(25, 43);
            this.cb_MACDConvergency.Name = "cb_MACDConvergency";
            this.cb_MACDConvergency.Size = new System.Drawing.Size(126, 17);
            this.cb_MACDConvergency.TabIndex = 8;
            this.cb_MACDConvergency.Text = "MACD hội tụ về Zero";
            this.cb_MACDConvergency.UseVisualStyleBackColor = true;
            // 
            // cb_ADL_SMA
            // 
            this.cb_ADL_SMA.AutoSize = true;
            this.cb_ADL_SMA.Location = new System.Drawing.Point(25, 175);
            this.cb_ADL_SMA.Name = "cb_ADL_SMA";
            this.cb_ADL_SMA.Size = new System.Drawing.Size(146, 17);
            this.cb_ADL_SMA.TabIndex = 7;
            this.cb_ADL_SMA.Text = "ADL cắt trên đường SMA";
            this.cb_ADL_SMA.UseVisualStyleBackColor = true;
            // 
            // cb_ROC_SMA
            // 
            this.cb_ROC_SMA.AutoSize = true;
            this.cb_ROC_SMA.Location = new System.Drawing.Point(25, 152);
            this.cb_ROC_SMA.Name = "cb_ROC_SMA";
            this.cb_ROC_SMA.Size = new System.Drawing.Size(148, 17);
            this.cb_ROC_SMA.TabIndex = 6;
            this.cb_ROC_SMA.Text = "ROC cắt trên đường SMA";
            this.cb_ROC_SMA.UseVisualStyleBackColor = true;
            // 
            // cb_SMA1_SMA2
            // 
            this.cb_SMA1_SMA2.AutoSize = true;
            this.cb_SMA1_SMA2.ForeColor = System.Drawing.Color.Magenta;
            this.cb_SMA1_SMA2.Location = new System.Drawing.Point(10, 0);
            this.cb_SMA1_SMA2.Name = "cb_SMA1_SMA2";
            this.cb_SMA1_SMA2.Size = new System.Drawing.Size(143, 17);
            this.cb_SMA1_SMA2.TabIndex = 0;
            this.cb_SMA1_SMA2.Text = "SMA1 cắt lên trên SMA2";
            this.cb_SMA1_SMA2.UseVisualStyleBackColor = true;
            this.cb_SMA1_SMA2.CheckedChanged += new System.EventHandler(this.cb_SMA1_SMA2_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cb_Accumulation);
            this.groupBox3.Controls.Add(this.cb_PriceAboveKumo);
            this.groupBox3.Controls.Add(this.cb_PSAR_Bullish);
            this.groupBox3.Controls.Add(this.cb_VolumeUp);
            this.groupBox3.Location = new System.Drawing.Point(260, 141);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 117);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Chỉ báo khác";
            // 
            // cb_Accumulation
            // 
            this.cb_Accumulation.AutoSize = true;
            this.cb_Accumulation.Location = new System.Drawing.Point(24, 85);
            this.cb_Accumulation.Name = "cb_Accumulation";
            this.cb_Accumulation.Size = new System.Drawing.Size(106, 17);
            this.cb_Accumulation.TabIndex = 8;
            this.cb_Accumulation.Text = "Cổ phiếu tích lũy";
            this.cb_Accumulation.UseVisualStyleBackColor = true;
            // 
            // bt_OK
            // 
            this.bt_OK.Location = new System.Drawing.Point(260, 453);
            this.bt_OK.Name = "bt_OK";
            this.bt_OK.Size = new System.Drawing.Size(75, 27);
            this.bt_OK.TabIndex = 17;
            this.bt_OK.Text = "OK";
            this.bt_OK.UseVisualStyleBackColor = true;
            this.bt_OK.Click += new System.EventHandler(this.bt_OK_Click);
            // 
            // bt_Back
            // 
            this.bt_Back.Location = new System.Drawing.Point(384, 453);
            this.bt_Back.Name = "bt_Back";
            this.bt_Back.Size = new System.Drawing.Size(75, 27);
            this.bt_Back.TabIndex = 18;
            this.bt_Back.Text = "Quay lại";
            this.bt_Back.UseVisualStyleBackColor = true;
            this.bt_Back.Click += new System.EventHandler(this.bt_Back_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Đặt tên";
            // 
            // Advance
            // 
            this.Advance.Location = new System.Drawing.Point(24, 453);
            this.Advance.Name = "Advance";
            this.Advance.Size = new System.Drawing.Size(105, 27);
            this.Advance.TabIndex = 20;
            this.Advance.Text = "Điều kiện lọc";
            this.Advance.UseVisualStyleBackColor = true;
            this.Advance.Click += new System.EventHandler(this.Advance_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lb_SMA2);
            this.groupBox4.Controls.Add(this.tb_SMA2);
            this.groupBox4.Controls.Add(this.tb_SMA1);
            this.groupBox4.Controls.Add(this.lb_SMA1);
            this.groupBox4.Controls.Add(this.cb_SMA1_SMA2);
            this.groupBox4.Location = new System.Drawing.Point(24, 338);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(216, 85);
            this.groupBox4.TabIndex = 21;
            this.groupBox4.TabStop = false;
            // 
            // lb_SMA2
            // 
            this.lb_SMA2.AutoSize = true;
            this.lb_SMA2.Location = new System.Drawing.Point(24, 55);
            this.lb_SMA2.Name = "lb_SMA2";
            this.lb_SMA2.Size = new System.Drawing.Size(39, 13);
            this.lb_SMA2.TabIndex = 4;
            this.lb_SMA2.Text = "SMA2:";
            // 
            // tb_SMA2
            // 
            this.tb_SMA2.Location = new System.Drawing.Point(64, 52);
            this.tb_SMA2.Name = "tb_SMA2";
            this.tb_SMA2.Size = new System.Drawing.Size(104, 20);
            this.tb_SMA2.TabIndex = 3;
            // 
            // tb_SMA1
            // 
            this.tb_SMA1.Location = new System.Drawing.Point(64, 26);
            this.tb_SMA1.Name = "tb_SMA1";
            this.tb_SMA1.Size = new System.Drawing.Size(104, 20);
            this.tb_SMA1.TabIndex = 2;
            // 
            // lb_SMA1
            // 
            this.lb_SMA1.AutoSize = true;
            this.lb_SMA1.Location = new System.Drawing.Point(22, 29);
            this.lb_SMA1.Name = "lb_SMA1";
            this.lb_SMA1.Size = new System.Drawing.Size(39, 13);
            this.lb_SMA1.TabIndex = 1;
            this.lb_SMA1.Text = "SMA1:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cbSMACutPrice12);
            this.groupBox5.Controls.Add(this.cbSMACutPrice100);
            this.groupBox5.Controls.Add(this.cbSMACutPrice50);
            this.groupBox5.Controls.Add(this.cbSMACutPrice26);
            this.groupBox5.Controls.Add(this.cbSMACutPrice9);
            this.groupBox5.Controls.Add(this.cbSMACutPrice5);
            this.groupBox5.Location = new System.Drawing.Point(260, 264);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(200, 159);
            this.groupBox5.TabIndex = 22;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "SMA cắt đường giá";
            // 
            // cbSMACutPrice12
            // 
            this.cbSMACutPrice12.AutoSize = true;
            this.cbSMACutPrice12.Location = new System.Drawing.Point(24, 65);
            this.cbSMACutPrice12.Name = "cbSMACutPrice12";
            this.cbSMACutPrice12.Size = new System.Drawing.Size(136, 17);
            this.cbSMACutPrice12.TabIndex = 17;
            this.cbSMACutPrice12.Text = "SMA(12) cắt đường giá";
            this.cbSMACutPrice12.UseVisualStyleBackColor = true;
            // 
            // cbSMACutPrice100
            // 
            this.cbSMACutPrice100.AutoSize = true;
            this.cbSMACutPrice100.Location = new System.Drawing.Point(24, 134);
            this.cbSMACutPrice100.Name = "cbSMACutPrice100";
            this.cbSMACutPrice100.Size = new System.Drawing.Size(142, 17);
            this.cbSMACutPrice100.TabIndex = 16;
            this.cbSMACutPrice100.Text = "SMA(100) cắt đường giá";
            this.cbSMACutPrice100.UseVisualStyleBackColor = true;
            // 
            // cbSMACutPrice50
            // 
            this.cbSMACutPrice50.AutoSize = true;
            this.cbSMACutPrice50.Location = new System.Drawing.Point(24, 111);
            this.cbSMACutPrice50.Name = "cbSMACutPrice50";
            this.cbSMACutPrice50.Size = new System.Drawing.Size(136, 17);
            this.cbSMACutPrice50.TabIndex = 15;
            this.cbSMACutPrice50.Text = "SMA(50) cắt đường giá";
            this.cbSMACutPrice50.UseVisualStyleBackColor = true;
            // 
            // cbSMACutPrice26
            // 
            this.cbSMACutPrice26.AutoSize = true;
            this.cbSMACutPrice26.Location = new System.Drawing.Point(24, 88);
            this.cbSMACutPrice26.Name = "cbSMACutPrice26";
            this.cbSMACutPrice26.Size = new System.Drawing.Size(136, 17);
            this.cbSMACutPrice26.TabIndex = 14;
            this.cbSMACutPrice26.Text = "SMA(26) cắt đường giá";
            this.cbSMACutPrice26.UseVisualStyleBackColor = true;
            // 
            // cbSMACutPrice9
            // 
            this.cbSMACutPrice9.AutoSize = true;
            this.cbSMACutPrice9.Location = new System.Drawing.Point(24, 42);
            this.cbSMACutPrice9.Name = "cbSMACutPrice9";
            this.cbSMACutPrice9.Size = new System.Drawing.Size(130, 17);
            this.cbSMACutPrice9.TabIndex = 13;
            this.cbSMACutPrice9.Text = "SMA(9) cắt đường giá";
            this.cbSMACutPrice9.UseVisualStyleBackColor = true;
            // 
            // cbSMACutPrice5
            // 
            this.cbSMACutPrice5.AutoSize = true;
            this.cbSMACutPrice5.Location = new System.Drawing.Point(24, 19);
            this.cbSMACutPrice5.Name = "cbSMACutPrice5";
            this.cbSMACutPrice5.Size = new System.Drawing.Size(130, 17);
            this.cbSMACutPrice5.TabIndex = 12;
            this.cbSMACutPrice5.Text = "SMA(5) cắt đường giá";
            this.cbSMACutPrice5.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cb_MFI_lower);
            this.groupBox6.Controls.Add(this.cb_RSI_lower);
            this.groupBox6.Controls.Add(this.cb_ROS_lower);
            this.groupBox6.Location = new System.Drawing.Point(365, 51);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(95, 84);
            this.groupBox6.TabIndex = 23;
            this.groupBox6.TabStop = false;
            // 
            // cb_MFI_lower
            // 
            this.cb_MFI_lower.AutoSize = true;
            this.cb_MFI_lower.Location = new System.Drawing.Point(12, 37);
            this.cb_MFI_lower.Name = "cb_MFI_lower";
            this.cb_MFI_lower.Size = new System.Drawing.Size(68, 17);
            this.cb_MFI_lower.TabIndex = 9;
            this.cb_MFI_lower.Text = "MFI < 30";
            this.cb_MFI_lower.UseVisualStyleBackColor = true;
            // 
            // cb_RSI_lower
            // 
            this.cb_RSI_lower.AutoSize = true;
            this.cb_RSI_lower.Location = new System.Drawing.Point(12, 15);
            this.cb_RSI_lower.Name = "cb_RSI_lower";
            this.cb_RSI_lower.Size = new System.Drawing.Size(68, 17);
            this.cb_RSI_lower.TabIndex = 8;
            this.cb_RSI_lower.Text = "RSI < 30";
            this.cb_RSI_lower.UseVisualStyleBackColor = true;
            // 
            // cb_ROS_lower
            // 
            this.cb_ROS_lower.AutoSize = true;
            this.cb_ROS_lower.Location = new System.Drawing.Point(12, 60);
            this.cb_ROS_lower.Name = "cb_ROS_lower";
            this.cb_ROS_lower.Size = new System.Drawing.Size(78, 17);
            this.cb_ROS_lower.TabIndex = 12;
            this.cb_ROS_lower.Text = "ROC < -5%";
            this.cb_ROS_lower.UseVisualStyleBackColor = true;
            // 
            // DlgFilterDefine
            // 
            this.AcceptButton = this.bt_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 484);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.Advance);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bt_Back);
            this.Controls.Add(this.bt_OK);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tb_Name);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "DlgFilterDefine";
            this.Text = "Buy signal";
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
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cb_RSI_SMA;
        private System.Windows.Forms.CheckBox cb_MACD_Signal;
        private System.Windows.Forms.CheckBox cb_MFI_SMA;
        private System.Windows.Forms.CheckBox cb_SlowStochastics_K_D;
        private System.Windows.Forms.CheckBox cb_PSAR_Bullish;
        private System.Windows.Forms.CheckBox cb_Tenkan_Kijun;
        private System.Windows.Forms.CheckBox cb_PriceAboveKumo;
        private System.Windows.Forms.CheckBox cb_VolumeUp;
        private System.Windows.Forms.CheckBox cb_RSI_higher;
        private System.Windows.Forms.CheckBox cb_MFI_higher;
        private System.Windows.Forms.CheckBox cb_ROC_higher;
        private System.Windows.Forms.TextBox tb_Name;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button bt_OK;
        private System.Windows.Forms.Button bt_Back;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cb_ROC_SMA;
        private System.Windows.Forms.CheckBox cb_ADL_SMA;
        private System.Windows.Forms.Button Advance;
        private System.Windows.Forms.CheckBox cb_Accumulation;
        private System.Windows.Forms.CheckBox cb_MACDConvergency;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox cb_SMA1_SMA2;
        private System.Windows.Forms.Label lb_SMA2;
        private System.Windows.Forms.TextBox tb_SMA2;
        private System.Windows.Forms.TextBox tb_SMA1;
        private System.Windows.Forms.Label lb_SMA1;
        private System.Windows.Forms.CheckBox cb_NVI_Bullish;
        private System.Windows.Forms.CheckBox cb_cut_DMIs;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox cbSMACutPrice5;
        private System.Windows.Forms.CheckBox cbSMACutPrice100;
        private System.Windows.Forms.CheckBox cbSMACutPrice50;
        private System.Windows.Forms.CheckBox cbSMACutPrice26;
        private System.Windows.Forms.CheckBox cbSMACutPrice9;
        private System.Windows.Forms.CheckBox cbSMACutPrice12;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox cb_MFI_lower;
        private System.Windows.Forms.CheckBox cb_RSI_lower;
        private System.Windows.Forms.CheckBox cb_ROS_lower;
    }
}