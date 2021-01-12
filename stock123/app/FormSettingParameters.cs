using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using xlib.framework;
using xlib.ui;
using xlib.utils;
using stock123.app.chart;
using stock123.app.data;

namespace stock123.app
{
    public class FormSettingParameters:Form, xIEventListener
    {
        private Label label1;
        int mCurrentIndicator = 0;
        private ComboBox cb_Indicator;
        xContainer mContainer;
        Context mContext;
        xIEventListener mListener;
        xFloat mValue1 = new xFloat();
        xFloat mValue2 = new xFloat();
        xFloat mValue3 = new xFloat();
        xFloat mValue4 = new xFloat();
        xFloat mValue5 = new xFloat();

        xTextField mTextField1;
        xTextField mTextField2;
        int IDX = 1;

        Share mShare;

        int[] mIndicators = {
                ChartBase.CHART_AROON,
                ChartBase.CHART_AROON_OSCILLATOR,
                ChartBase.CHART_ATR,
                ChartBase.CHART_BOLLINGER,
                ChartBase.CHART_CFM,
                ChartBase.CHART_ENVELOP,
                ChartBase.CHART_PSAR,
            ChartBase.CHART_VSTOP,
                ChartBase.CHART_MACD,
                ChartBase.CHART_RSI,
                ChartBase.CHART_MFI,
                ChartBase.CHART_STOCHASTIC_FAST,
                ChartBase.CHART_STOCHASTIC_SLOW,
                ChartBase.CHART_ICHIMOKU,
                ChartBase.CHART_ADX,
                ChartBase.CHART_ADL,
                ChartBase.CHART_CHAIKIN,
                ChartBase.CHART_ROC,
                ChartBase.CHART_CRS_RATIO,
                ChartBase.CHART_CRS_PERCENT,
                ChartBase.CHART_ZIGZAG,
                ChartBase.CHART_VOLUME,
                ChartBase.CHART_STOCHRSI,
                ChartBase.CHART_TRIX,
                ChartBase.CHART_OBV,
                ChartBase.CHART_NVI,
                ChartBase.CHART_PVI,

                ChartBase.CHART_WILLIAMR,

                ChartBase.CHART_PVT,
                ChartBase.CHART_CCI,
                ChartBase.CHART_PVO,
                ChartBase.CHART_MASSINDEX,
        };
        public FormSettingParameters(Share share, xIEventListener listener)
        {
            InitializeComponent();

            mListener = listener;
            mShare = share;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //==================================

            mContext = Context.getInstance();

            mContainer = new xContainer();

            mContainer.setSize(this.Size.Width-40, this.Size.Height - 40);
            mContainer.setPosition(20, 60);
            this.Controls.Add(mContainer.getControl());

            //=================================
            addOKButton(0);
        }

        public void setCurrentChart(int chart)
        {
            for (int i = 0; i < mIndicators.Length; i++)
            {
                if (chart == mIndicators[i])
                {
                    cb_Indicator.SelectedIndex = i;
                    cb_Indicator.Enabled = false;
                    break;
                }
            }
        }

        void addOKButton(int y)
        {
            xButton bt;
            bt = xButton.createStandardButton(C.ID_DLG_BUTTON_OK, this, "OK", 86);
            bt.setPosition(mContainer.getW() - bt.getW() - 10, y);
            this.AcceptButton = (Button)bt.getControl();
            mContainer.addControl(bt);
            int x = bt.getX();
            //  reset button
            bt = xButton.createStandardButton(C.ID_DLG_BUTTON_RESET, this, "Thông số ngầm định", 120);
            bt.setPosition(x - bt.getW() - 40, y);
            mContainer.addControl(bt);

            y = bt.getBottom();
            this.Size = new Size(this.Size.Width, y + mContainer.getY() + 50);  //  42 = borders + bottom space Y
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.cb_Indicator = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Chỉ báo";
            // 
            // cb_Indicator
            // 
            this.cb_Indicator.FormattingEnabled = true;
            this.cb_Indicator.Items.AddRange(new object[] {
                "Aroon",
                "Aroon oscillator",
                "Average true range (ATR)",
                "Bollinger Bands",
                "Chaikin Money Flow",
                "Envelops",
                "Parabollic (PSAR)",
                "Volatility Stop Indicator (VSTOP)",
                "Moving Average Convergence-Divergence (MACD)",
                "Relative Strength Index (RSI)",
                "Money Flow Index (MFI)",
                "Fast Stochastic",
                "Slow Stochastic",
                "Ichimoku",
                "Average Directional Index (ADX)",
                "Accumulation Distribution Line - (ADL)",
                "Chaikin Oscillator",
                "Rate of Change (ROC)",
                "Comparative Relative Strength ratio (RS)",
                "Comparative Relative Strength percent (cRS %)",
                "Zigzag",
                "Volume",
                "StochRSI",
                "Triple Exponential Average (TRIX)",
                "On Balance Volume",
                "Negative Volume Index (NVI)",
                "Positive Volume Index (PVI)",
                "William %R",
                "Price Volume Trend (PVT)",
                "Commodity Channel Index (CCI)",
                "Percentage Volume Oscillator (PVO)",
                "Mass Index (MASS)"
            });

            this.cb_Indicator.Location = new System.Drawing.Point(86, 12);
            this.cb_Indicator.Name = "cb_Indicator";
            this.cb_Indicator.Size = new System.Drawing.Size(351, 21);
            this.cb_Indicator.TabIndex = 2;
            this.cb_Indicator.SelectedIndexChanged += new System.EventHandler(this.cb_Indicator_SelectedIndexChanged);
            // 
            // FormSettingParameters
            // 
            this.ClientSize = new System.Drawing.Size(462, 328);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_Indicator);
            this.Name = "FormSettingParameters";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void setupForm()
        {
            switch (mCurrentIndicator)
            {
                case ChartBase.CHART_BOLLINGER:
                    setupBollinger();
                    break;
                case ChartBase.CHART_ENVELOP:
                    setupEnvelops();
                    break;
                case ChartBase.CHART_PSAR:
                    setupPSAR();
                    break;
                case ChartBase.CHART_VSTOP:
                    setupVSTOP();
                    break;
                case ChartBase.CHART_MACD:
                    setupMACD();
                    break;
                case ChartBase.CHART_RSI:
                    setupRSI();
                    break;
                case ChartBase.CHART_WILLIAMR:
                    setupWilliamR();
                    break;
                case ChartBase.CHART_STOCHRSI:
                    setupStochRSI();
                    break;
                case ChartBase.CHART_MFI:
                    setupMFI();
                    break;
                case ChartBase.CHART_STOCHASTIC_FAST:
                    setupStochasticFast();
                    break;
                case ChartBase.CHART_STOCHASTIC_SLOW:
                    setupStochasticSlow();
                    break;
                case ChartBase.CHART_ICHIMOKU:
                    setupIchimoku();
                    break;
                case ChartBase.CHART_ADX:
                    setupADX();
                    break;
                case ChartBase.CHART_MAE:
                    break;
                case ChartBase.CHART_ADL:
                    setupADL();
                    break;
                case ChartBase.CHART_CHAIKIN:
                    setupChaikin();
                    break;
                case ChartBase.CHART_ROC:
                    setupROC();
                    break;
                case ChartBase.CHART_ZIGZAG:
                    setupZigzag();
                    break;
                case ChartBase.CHART_VOLUME:
                    setupSMAVolume();
                    break;
                case ChartBase.CHART_TRIX:
                    setupTRIX();
                    break;
                case ChartBase.CHART_OBV:
                    setupOBV();
                    break;
                case ChartBase.CHART_NVI:
                    setupNVI();
                    break;
                case ChartBase.CHART_PVI:
                    setupPVI();
                    break;
                case ChartBase.CHART_PVT:
                    setupPVT();
                    break;
                case ChartBase.CHART_CCI:
                    setupCCI();
                    break;
                case ChartBase.CHART_AROON:
                case ChartBase.CHART_AROON_OSCILLATOR:
                    setupAroon();
                    break;
                //case ChartBase.CHART_ROC:
                    //setupROC();
                    //break;
                case ChartBase.CHART_CFM:
                    setupCMF();
                    break;
                case ChartBase.CHART_ATR:
                    setupATR();
                    break;
                case ChartBase.CHART_MASSINDEX:
                    setupMassIndex();
                    break;
                case ChartBase.CHART_CRS_RATIO:
                    setupRSRatio();
                    break;
                case ChartBase.CHART_CRS_PERCENT:
                    setupRSPercent();
                    break;
            }
        }

        private void cb_Indicator_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            int idx = cb.SelectedIndex;
            if (idx >= 0 && idx < mIndicators.Length)
            {
                int chart = mIndicators[idx];
                mCurrentIndicator = chart;
                setupForm();
            }
        }

        void setupMACD()
        {
            mContainer.removeAllControls();

            int y = 0;
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("(Giá trị hiện tại: Slow={0} Fast={1} Signal={2})", ((int)mContext.mOptMACDSlow), ((int)mContext.mOptMACDFast), ((int)mContext.mOptMACDSignal));
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;
            //  Slow period
            mValue1.Value = mContext.mOptMACDSlow;
            addSlider(y, "Slow period", 5, 50, 1, mValue1, 1);
            y += 44;
            //  Fast period
            mValue2.Value = mContext.mOptMACDFast;
            addSlider(y, "Fast period", 3, 20, 1, mValue2, 1);
            y += 44;
            //  Signal
            mValue3.Value = mContext.mOptMACDSignal;
            addSlider(y, "Signal period", 1, 20, 1, mValue3, 1);
            y += 44;
            //========================
            addOKButton(y);
        }

        void setupBollinger()
        {
            mContainer.removeAllControls();

            int y = 0;
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("(Giá trị hiện tại: Period={0} Deviation={1})", ((int)mContext.mOptBBPeriod), ((int)mContext.mOptBBD));
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;
            //  Slow period
            mValue1.Value = mContext.mOptBBPeriod;
            addSlider(y, "Period", 5, 50, 1, mValue1, 1);
            y += 44;
            //  Fast period
            mValue2.Value = mContext.mOptBBD;
            addSlider(y, "Deviation", 0.5f, 3.0f, 0.1f, mValue2, 10);
            y += 44;

            //========================
            addOKButton(y);
        }

        void setupEnvelops()
        {
            mContainer.removeAllControls();

            int y = 0;
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("(Giá trị hiện tại: Period={0})", ((int)mContext.mOptEnvelopPeriod));
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;
            //  period
            mValue1.Value = mContext.mOptEnvelopPeriod;
            addSlider(y, "Period", 3, 200, 1, mValue1, 1);
            y += 44;
            //  checkboxes
            xCheckbox cb = xCheckbox.createCheckbox("Đường 2.5% EMA", mContext.mOptEnvelopLine0, this, 200);
            cb.setPosition(50, y);
            mContainer.addControl(cb);

            y = cb.getBottom();
            cb = xCheckbox.createCheckbox("Đường 5% EMA", mContext.mOptEnvelopLine1, this, 200);
            cb.setPosition(50, y);
            mContainer.addControl(cb);

            y = cb.getBottom();
            cb = xCheckbox.createCheckbox("Đường 10% EMA", mContext.mOptEnvelopLine2, this, 200);
            cb.setPosition(50, y);
            mContainer.addControl(cb);

            y += 60;

            //========================
            addOKButton(y);
        }

        void setupPSAR()
        {
            mContainer.removeAllControls();

            int y = 0;
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("(Giá trị hiện tại: Acceleration Factor={0}; Acceleration Factor Max={1})", mContext.mOptPSAR_alpha, mContext.mOptPSAR_alpha_max);
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;
            //  Slow period
            mValue1.Value = mContext.mOptPSAR_alpha;
            addSlider(y, "AF", 0.01f, 0.1f, 0.01f, mValue1, 100);
            y += 44;
            //  Fast period
            mValue2.Value = mContext.mOptPSAR_alpha_max;
            addSlider(y, "AF Max", 0.1f, 0.5f, 0.02f, mValue2, 100);
            y += 44;

            //========================
            addOKButton(y);
        }

        void setupVSTOP()
        {
            mContainer.removeAllControls();

            int y = 0;
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("(Giá trị hiện tại: Lookback={0}; Mult Factor={1})", mContext.mOptVSTOP_ATR_Loopback, mContext.mOptVSTOP_MULT);
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;
            //  Slow period
            mValue1.Value = mContext.mOptVSTOP_ATR_Loopback;
            addSlider(y, "Lookback", 1, 50, 1, mValue1, 1);
            y += 44;
            //  Fast period
            mValue2.Value = mContext.mOptVSTOP_MULT;
            addSlider(y, "Mult Factor", 0.1f, 4.0f, 0.1f, mValue2, 100);
            y += 44;

            //========================
            addOKButton(y);
        }

        void setupRSI()
        {
            mContainer.removeAllControls();

            int y = 0;
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("(Giá trị hiện tại: Period={0})", mContext.mOptRSIPeriod[IDX]);
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;
            //  period
            mValue1.Value = mContext.mOptRSIPeriod[IDX];
            addSlider(y, "Period", 5, 50, 1, mValue1, 1);
            y += 44;
            //===============option EMA
            if (IDX == 0)
            {
                xCheckbox cb = xCheckbox.createCheckbox("EMA (bỏ chọn để vẽ SMA)", mContext.mOptRSI_EMA_ON, this, 250);
                cb.setPosition(50, y);

                mContainer.addControl(cb);
                y = cb.getBottom();
                mValue2.Value = mContext.mOptRSI_EMA;
                if (true)//cb.isCheck())
                {
                    addSlider(y, "EMA Period", 0, 50, 1, mValue2, 1);
                    y += 44;
                }
                else
                    y += 10;
            }
            //========================
            addOKButton(y);
        }

        void setupWilliamR()
        {
            mContainer.removeAllControls();

            int y = 0;
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("(Giá trị hiện tại: Period={0})", mContext.mOptWilliamRPeriod);
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;
            //  period
            mValue1.Value = mContext.mOptWilliamRPeriod;
            addSlider(y, "Period", 5, 50, 1, mValue1, 1);
            y += 44;
            //===============option EMA
            if (IDX == 0)
            {
                xCheckbox cb = xCheckbox.createCheckbox("EMA (bỏ chọn để vẽ SMA)", mContext.mOptWR_EMA_ON, this, 250);
                cb.setPosition(50, y);

                mContainer.addControl(cb);
                y = cb.getBottom();
                mValue2.Value = mContext.mOptWR_EMA;
                if (true)//cb.isCheck())
                {
                    addSlider(y, "EMA Period", 0, 50, 1, mValue2, 1);
                    y += 44;
                }
                else
                    y += 10;
            }
            //========================
            
            addOKButton(y);
        }

        void setupOBV()
        {
            mContainer.removeAllControls();

            int y = 0;
            //===============option EMA

            xCheckbox cb = xCheckbox.createCheckbox("EMA (bỏ chọn để vẽ SMA)", mContext.mOptOBV_EMA_ON, this, 250);
            cb.setPosition(50, y);

            mContainer.addControl(cb);
            y = cb.getBottom();
            mValue1.Value = mContext.mOptOBV_EMA;
            if (true)//cb.isCheck())
            {
                StringBuilder sb = Utils.getSB();
                sb.AppendFormat("(Giá trị hiện tại: EMA Period={0})", mContext.mOptOBV_EMA);
                xLabel l = new xLabel(sb.ToString());
                l.setPosition(0, y);
                l.setSize(mContainer.getW(), -1);
                l.setAlign(xGraphics.RIGHT);
                mContainer.addControl(l);
                y = l.getBottom() + 4;

                addSlider(y, "EMA Period", 0, 50, 1, mValue1, 1);
                y += 44;
            }
            else
                y += 10;
            //========================
            addOKButton(y);
        }

        void setupNVI()
        {
            mContainer.removeAllControls();

            int y = 0;
            //===============option EMA 1
            xCheckbox cb = xCheckbox.createCheckbox("EMA 1 (bỏ chọn để vẽ SMA)", mContext.mOptNVI_EMA_ON1, this, 250);
            cb.setPosition(50, y);

            mContainer.addControl(cb);
            y = cb.getBottom();
            mValue1.Value = mContext.mOptNVI_EMA[0];
            if (true)//cb.isCheck())
            {
                StringBuilder sb = Utils.getSB();
                sb.AppendFormat("(Giá trị hiện tại: EMA Period={0})", mContext.mOptNVI_EMA[0]);
                xLabel l = new xLabel(sb.ToString());
                l.setPosition(0, y);
                l.setSize(mContainer.getW(), -1);
                l.setAlign(xGraphics.RIGHT);
                mContainer.addControl(l);
                y = l.getBottom() + 4;

                addSlider(y, "EMA Period", 0, 50, 1, mValue1, 1);
                y += 44;
            }
            else
                y += 10;

            //===============option EMA 2
            cb = xCheckbox.createCheckbox("EMA 2 (bỏ chọn để vẽ SMA)", mContext.mOptNVI_EMA_ON2, this, 250);
            cb.setPosition(50, y);

            mContainer.addControl(cb);
            y = cb.getBottom();
            mValue2.Value = mContext.mOptNVI_EMA[1];
            if (true)//cb.isCheck())
            {
                StringBuilder sb = Utils.getSB();
                sb.AppendFormat("(Giá trị hiện tại: EMA Period={0})", mContext.mOptNVI_EMA[1]);
                xLabel l = new xLabel(sb.ToString());
                l.setPosition(0, y);
                l.setSize(mContainer.getW(), -1);
                l.setAlign(xGraphics.RIGHT);
                mContainer.addControl(l);
                y = l.getBottom() + 4;

                addSlider(y, "EMA Period", 0, 300, 3, mValue2, 1);
                y += 44;
            }
            else
                y += 10;
            //========================
            addOKButton(y);
        }

        void setupPVI()
        {
            mContainer.removeAllControls();

            int y = 0;
            //===============option EMA 1
            xCheckbox cb = xCheckbox.createCheckbox("EMA 1 (bỏ chọn để vẽ SMA)", mContext.mOptPVI_EMA_ON1, this, 250);
            cb.setPosition(50, y);

            mContainer.addControl(cb);
            y = cb.getBottom();
            mValue1.Value = mContext.mOptPVI_EMA[0];
            if (true)//scb.isCheck())
            {
                StringBuilder sb = Utils.getSB();
                sb.AppendFormat("(Giá trị hiện tại: EMA Period={0})", mContext.mOptPVI_EMA[0]);
                xLabel l = new xLabel(sb.ToString());
                l.setPosition(0, y);
                l.setSize(mContainer.getW(), -1);
                l.setAlign(xGraphics.RIGHT);
                mContainer.addControl(l);
                y = l.getBottom() + 4;

                addSlider(y, "EMA Period", 0, 50, 1, mValue1, 1);
                y += 44;
            }
            else
                y += 10;

            //===============option EMA 2
            cb = xCheckbox.createCheckbox("EMA 2 (bỏ chọn để vẽ SMA)", mContext.mOptPVI_EMA_ON2, this, 250);
            cb.setPosition(50, y);

            mContainer.addControl(cb);
            y = cb.getBottom();
            mValue2.Value = mContext.mOptPVI_EMA[1];
            if (true)//cb.isCheck())
            {
                StringBuilder sb = Utils.getSB();
                sb.AppendFormat("(Giá trị hiện tại: EMA Period={0})", mContext.mOptPVI_EMA[1]);
                xLabel l = new xLabel(sb.ToString());
                l.setPosition(0, y);
                l.setSize(mContainer.getW(), -1);
                l.setAlign(xGraphics.RIGHT);
                mContainer.addControl(l);
                y = l.getBottom() + 4;

                addSlider(y, "EMA Period", 0, 300, 3, mValue2, 1);
                y += 44;
            }
            else
                y += 10;
            //========================
            addOKButton(y);
        }

        void setupPVT()
        {
            mContainer.removeAllControls();

            int y = 0;
            //===============option EMA 1
            xCheckbox cb = xCheckbox.createCheckbox("EMA (bỏ chọn để vẽ SMA)", mContext.mOptPVT_EMA_ON, this, 100);
            cb.setPosition(50, y);

            mContainer.addControl(cb);
            y = cb.getBottom();
            mValue1.Value = mContext.mOptPVT;
            if (true)//scb.isCheck())
            {
                StringBuilder sb = Utils.getSB();
                sb.AppendFormat("(Giá trị hiện tại: EMA Period={0})", mContext.mOptPVT);
                xLabel l = new xLabel(sb.ToString());
                l.setPosition(0, y);
                l.setSize(mContainer.getW(), -1);
                l.setAlign(xGraphics.RIGHT);
                mContainer.addControl(l);
                y = l.getBottom() + 4;

                addSlider(y, "EMA Period", 0, 50, 1, mValue1, 1);
                y += 44;
            }
            else
                y += 10;

            //========================
            addOKButton(y);
        }

        void setupCCI()
        {
            mContainer.removeAllControls();

            int y = 0;
            //  constant
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("Constant ({0:F3})", mContext.mOptCCIConstant);
            mValue1.Value = mContext.mOptCCIConstant;    
            addSlider(y, sb.ToString(), 0.001f, 0.1f, 0.001f, mValue1, 1000);
            y += 44;
            //  period
            sb = Utils.getSB();
            sb.AppendFormat("Period ({0})", mContext.mOptCCIPeriod);

            mValue2.Value = mContext.mOptCCIPeriod;
            addSlider(y, sb.ToString(), 1, 150, 1, mValue2, 1);
            y += 44;

            //========================
            addOKButton(y);
        }

        void setupAroon()
        {
            mContainer.removeAllControls();

            int y = 0;
            //  constant
            StringBuilder sb = Utils.getSB();
            //  period
            sb = Utils.getSB();
            sb.AppendFormat("Aroon Period ({0})", mContext.mOptAroonPeriod);

            mValue1.Value = mContext.mOptAroonPeriod;
            addSlider(y, sb.ToString(), 1, 150, 1, mValue1, 1);
            y += 44;

            //========================
            addOKButton(y);
        }

        void setupCMF()
        {
            mContainer.removeAllControls();

            int y = 0;
            //  constant
            StringBuilder sb = Utils.getSB();
            //  period
            sb = Utils.getSB();
            sb.AppendFormat("CMF Period ({0})", mContext.mOptCFMPeriod);

            mValue1.Value = mContext.mOptCFMPeriod;
            addSlider(y, sb.ToString(), 1, 150, 1, mValue1, 1);
            y += 44;

            //========================
            addOKButton(y);
        }

        void setupATR()
        {
            mContainer.removeAllControls();

            int y = 0;
            //  constant
            StringBuilder sb = Utils.getSB();
            //  period
            sb = Utils.getSB();
            sb.AppendFormat("ATR Period ({0})", mContext.mOptATRLoopback);

            mValue1.Value = mContext.mOptATRLoopback;
            addSlider(y, sb.ToString(), 1, 150, 1, mValue1, 1);
            y += 44;

            //========================
            addOKButton(y);
        }

        void setupStochRSI()
        {
            mContainer.removeAllControls();

            int y = 0;
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("(Giá trị hiện tại: Period={0}, SMA={1})", mContext.mOptStochRSIPeriod, mContext.mOptStochRSISMAPeriod);
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;
            //  period
            mValue1.Value = mContext.mOptStochRSIPeriod;
            addSlider(y, "RSI Period", 5, 50, 1, mValue1, 1);
            y += 44;
            //  period
            mValue2.Value = mContext.mOptStochRSISMAPeriod;
            addSlider(y, "SMA Period", 5, 50, 1, mValue2, 1);
            y += 44;

            //========================
            addOKButton(y);
        }

        void setupADL()
        {
            mContainer.removeAllControls();

            int y = 0;
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("(Giá trị hiện tại: Period1={0}, Period2={1})", (int)mContext.mOptADL_SMA[0], (int)mContext.mOptADL_SMA[1]);
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;
            //  period
            mValue1.Value = mContext.mOptADL_SMA[0];
            addSlider(y, "EMA Period1", 0, 30, 1, mValue1, 1);
            y += 44;

            //  period
            mValue2.Value = mContext.mOptADL_SMA[1];
            addSlider(y, "EMA Period2", 0, 100, 1, mValue2, 1);
            y += 44;

            //========================
            addOKButton(y);
        }

        void setupChaikin()
        {
            mContainer.removeAllControls();

            int y = 0;
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("(Giá trị hiện tại: Period1={0}, Period2={1})", (int)mContext.mOptChaikin0[IDX], (int)mContext.mOptChaikin1[IDX]);
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;
            //  period
            mValue1.Value = mContext.mOptChaikin0[IDX];
            addSlider(y, "Period1", 1, 30, 1, mValue1, 1);
            y += 44;

            //  period
            mValue2.Value = mContext.mOptChaikin1[IDX];
            addSlider(y, "Period2", 1, 100, 1, mValue2, 1);
            y += 44;

            //===============option EMA
            if (IDX == 0)
            {
                xCheckbox cb = xCheckbox.createCheckbox("EMA (bỏ chọn để vẽ SMA)", mContext.mOptChaikinOscillatorEMA_ON, this, 250);
                cb.setPosition(50, y);

                mContainer.addControl(cb);
                y = cb.getBottom();
                mValue3.Value = mContext.mOptChaikinOscillatorEMA;
                if (true)//cb.isCheck())
                {
                    addSlider(y, "EMA Period", 0, 50, 1, mValue3, 1);
                    y += 44;
                }
                else
                    y += 10;
            }
            //========================
            addOKButton(y);
        }

        void setupROC()
        {
            mContainer.removeAllControls();

            int y = 0;
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("(Giá trị hiện tại: Period={0}", mContext.mOptROC);
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;
            //  period
            mValue1.Value = mContext.mOptROC[IDX];
            addSlider(y, "Period", 1, 250, 1, mValue1, 1);
            y += 44;
            //===============option EMA
            if (IDX == 0)
            {
                xCheckbox cb = xCheckbox.createCheckbox("EMA (bỏ chọn để vẽ SMA)", mContext.mOptROC_EMA_ON, this, 250);
                cb.setPosition(50, y);

                mContainer.addControl(cb);
                y = cb.getBottom();
                mValue2.Value = mContext.mOptROC_EMA;
                if (true)//cb.isCheck())
                {
                    addSlider(y, "EMA Period", 0, 50, 1, mValue2, 1);
                    y += 44;
                }
                else
                    y += 10;
            }

            //========================
            addOKButton(y);
        }

        void setupTRIX()
        {
            mContainer.removeAllControls();

            int y = 0;
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("(Giá trị hiện tại: TRIX's Period={0}, EMA = {1}", mContext.mOptTRIX[0], mContext.mOptTRIX[1]);
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;
            //  period
            mValue1.Value = mContext.mOptTRIX[0];
            addSlider(y, "TRIX", 1, 250, 1, mValue1, 1);
            y += 44;

            //  period
            mValue2.Value = mContext.mOptTRIX[1];
            addSlider(y, "EMA", 1, 50, 1, mValue2, 1);
            y += 44;

            //========================
            addOKButton(y);
        }

        void setupMFI()
        {
            mContainer.removeAllControls();

            int y = 0;
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("(Giá trị hiện tại: Period={0})", mContext.mOptMFIPeriod[IDX]);
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;
            //  period
            mValue1.Value = mContext.mOptMFIPeriod[IDX];
            addSlider(y, "Period", 5, 50, 1, mValue1, 1);
            y += 44;
            //===============option EMA
            if (IDX == 0)
            {
                xCheckbox cb = xCheckbox.createCheckbox("EMA (bỏ chọn để vẽ SMA)", mContext.mOptMFI_EMA_ON, this, 250);
                cb.setPosition(50, y);

                mContainer.addControl(cb);
                y = cb.getBottom();
                mValue2.Value = mContext.mOptMFI_EMA;
                if (true)//cb.isCheck())
                {
                    addSlider(y, "EMA Period", 0, 50, 1, mValue2, 1);
                    y += 44;
                }
                else
                    y += 10;
            }
            //========================
            addOKButton(y);
        }

        void setupStochasticFast()
        {
            mContainer.removeAllControls();

            int y = 0;
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("(Giá trị hiện tại: %K-Period={0}, %D-Period={1})", (int)mContext.mOptStochasticFastKPeriod, (int)mContext.mOptStochasticFastSMA);
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;
            //  period
            mValue1.Value = mContext.mOptStochasticFastKPeriod;
            addSlider(y, "%K-Period", 1, 30, 1, mValue1, 1);
            y += 44;

            //  period
            mValue2.Value = mContext.mOptStochasticFastSMA;
            addSlider(y, "%D-Period", 1, 30, 1, mValue2, 1);
            y += 44;

            //========================
            addOKButton(y);
        }

        void setupStochasticSlow()
        {
            mContainer.removeAllControls();

            int y = 0;
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("(Giá trị hiện tại: %K-Period={0}, %K-Smooth={1}, %D-Period={2})", 
                (int)mContext.mOptStochasticSlowKPeriod, 
                (int)mContext.mOptStochasticSlowKSmoothK,
                (int)mContext.mOptStochasticSlowSMA);
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;
            //  period
            mValue1.Value = mContext.mOptStochasticSlowKPeriod;
            addSlider(y, "%K-Period", 1, 50, 1, mValue1, 1);
            y += 44;

            //  smooth
            mValue2.Value = mContext.mOptStochasticSlowKSmoothK;
            addSlider(y, "%K-Smooth", 1, 50, 1, mValue2, 1);
            y += 44;

            //  sma
            mValue3.Value = mContext.mOptStochasticSlowSMA;
            addSlider(y, "%D-period", 1, 50, 1, mValue3, 1);
            y += 44;

            //========================
            addOKButton(y);
        }

        void setupADX()
        {
            mContainer.removeAllControls();

            int y = 0;
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("(Giá trị hiện tại: ADX Period={0})", (int)mContext.mOptADXPeriod);
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;

            //  period
            mValue1.Value = mContext.mOptADXPeriod;
            addSlider(y, "ADX Period", 1, 100, 1, mValue1, 1);
            y += 44;

            //==========================
            sb.Length = 0;
            sb.AppendFormat("(Giá trị hiện tại: DMI Period={0})", (int)mContext.mOptADXPeriodDMI);
            l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;

            //  period
            mValue2.Value = mContext.mOptADXPeriodDMI;
            addSlider(y, "DMI period", 1, 100, 1, mValue2, 1);
            y += 44;

            //========================
            addOKButton(y);
        }

        void setupIchimoku()
        {
            mContainer.removeAllControls();

            int y = 0;
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("(Giá trị hiện tại: Tenkan period={0}; Kijun period={1}; SpanB period={2})", (int)mContext.mOptIchimokuTime1, (int)mContext.mOptIchimokuTime2, (int)mContext.mOptIchimokuTime3);
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;
            //  period
            mValue1.Value = mContext.mOptIchimokuTime1;
            addSlider(y, "Tenkan-Period", 1, 50, 1, mValue1, 1);
            y += 44;

            //  period
            mValue2.Value = mContext.mOptIchimokuTime2;
            addSlider(y, "Kijun-Period", 1, 75, 1, mValue2, 1);
            y += 44;

            //  period
            mValue3.Value = mContext.mOptIchimokuTime3;
            addSlider(y, "SpanB-Period", 1, 150, 1, mValue3, 1);
            y += 44;

            //========================
            addOKButton(y);
        }

        xTextField addText(int y, string label, string defaultText)
        {
            int w = mContainer.getW();
            xLabel l = xLabel.createSingleLabel(label);
            l.setPosition(0, y);
            mContainer.addControl(l);
            l.setAlign(xGraphics.RIGHT);
            l.setSize(100, l.getH());

            //y += l.getH();
            //  text field
            xTextField tf = xTextField.createTextField(200);
            tf.setPosition(120, y);
            mContainer.addControl(tf);

            return tf;
        }

        void addSlider(int y, string label, float min, float max, float step, xFloat o, int zoomValue)
        {            //t = o;
            xFloat f = new xFloat();
            f.Value = 10;
            float d = f.Value;
            //c = 10;

            int w = mContainer.getW();
            xLabel l = xLabel.createSingleLabel(label);
            l.setPosition(0, y);
            mContainer.addControl(l);
            l.setAlign(xGraphics.RIGHT);
            l.setSize(78, l.getH());

            l = xLabel.createSingleLabel("0000" + step);
            l.setPosition(80, y);
            mContainer.addControl(l);

            xSlider slider = xSlider.createSlider(min, max, step, o, zoomValue, l);
            slider.setSize(w - l.getRight(), slider.getH());
            slider.setPosition(l.getRight(), y);
            slider.setListener(this);

            mContainer.addControl(slider);
        }

        public void onEvent(object sender, int evt, int aIntParameter, object aParameter)
        {
            if (evt == xBaseControl.EVT_SLIDER_CHANGE_VALUE)
            {
                updateOptionValues();

                if (mShare != null)
                {
                    mShare.clearCalculations();
                    mListener.onEvent(this, C.EVT_REPAINT_CHARTS, 0, 0);
                }
            }
            if (evt == xBaseControl.EVT_BUTTON_CLICKED)
            {
                if (aIntParameter == C.ID_DLG_BUTTON_OK)
                {
                    mContext.saveOptions();
                    this.Close();
                }
                if (aIntParameter == C.ID_DLG_BUTTON_RESET)
                {
                    resetOptions();
                }
            }
            if (evt == xBaseControl.EVT_CHECKBOX_VALUE_CHANGED)
            {
                setupForm();
                if (mShare != null)
                {
                    mShare.clearCalculations();
                    mListener.onEvent(this, C.EVT_REPAINT_CHARTS, 0, 0);
                }
            }
        }

        void updateOptionValues()
        {
            int idx = cb_Indicator.SelectedIndex;
            if (idx >= 0 && idx < mIndicators.Length)
            {
                int chart = mIndicators[idx];
                switch (chart)
                {
                    case ChartBase.CHART_BOLLINGER:
                        mContext.mOptBBPeriod = mValue1.Value;
                        mContext.mOptBBD = mValue2.Value;
                        break;
                    case ChartBase.CHART_ENVELOP:
                        mContext.mOptEnvelopPeriod = mValue1.Value;
                        break;

                    case ChartBase.CHART_PSAR:
                        mContext.mOptPSAR_alpha = mValue1.Value;
                        mContext.mOptPSAR_alpha_max = mValue2.Value;
                        break;
                    case ChartBase.CHART_VSTOP:
                        mContext.mOptVSTOP_ATR_Loopback = mValue1.Value;
                        mContext.mOptVSTOP_MULT = mValue2.Value;
                        break;
                    case ChartBase.CHART_MACD:
                        mContext.mOptMACDSlow = mValue1.Value;
                        mContext.mOptMACDFast = mValue2.Value;
                        mContext.mOptMACDSignal = mValue3.Value;
                        break;
                    case ChartBase.CHART_RSI:
                        mContext.mOptRSIPeriod[IDX] = mValue1.Value;
                        if (IDX == 0)
                            mContext.mOptRSI_EMA = mValue2.Value;
                        break;
                    case ChartBase.CHART_WILLIAMR:
                        mContext.mOptWilliamRPeriod = mValue1.Value;
                        if (IDX == 0)
                            mContext.mOptWR_EMA = mValue2.Value;
                        break;
                    case ChartBase.CHART_STOCHRSI:
                        mContext.mOptStochRSIPeriod = mValue1.Value;
                        mContext.mOptStochRSISMAPeriod = mValue2.Value;
                        break;
                    case ChartBase.CHART_MFI:
                        mContext.mOptMFIPeriod[IDX] = mValue1.Value;
                        if (IDX == 0)
                            mContext.mOptMFI_EMA = mValue2.Value;
                        break;
                    case ChartBase.CHART_STOCHASTIC_FAST:
                        mContext.mOptStochasticFastKPeriod = mValue1.Value;
                        mContext.mOptStochasticFastSMA = mValue2.Value;
                        break;
                    case ChartBase.CHART_STOCHASTIC_SLOW:
                        mContext.mOptStochasticSlowKPeriod = mValue1.Value;
                        mContext.mOptStochasticSlowKSmoothK = mValue2.Value;
                        mContext.mOptStochasticSlowSMA = mValue3.Value;
                        break;
                    case ChartBase.CHART_ICHIMOKU:
                        mContext.mOptIchimokuTime1 = mValue1.Value;
                        mContext.mOptIchimokuTime2 = mValue2.Value;
                        mContext.mOptIchimokuTime3 = mValue3.Value;
                        break;
                    case ChartBase.CHART_ADX:
                        mContext.mOptADXPeriod = mValue1.Value;
                        mContext.mOptADXPeriodDMI = mValue2.Value;
                        break;
                    case ChartBase.CHART_MAE:
                        break;
                    case ChartBase.CHART_ADL:
                        mContext.mOptADL_SMA[0] = mValue1.Value;
                        mContext.mOptADL_SMA[1] = mValue2.Value;
                        break;
                    case ChartBase.CHART_CHAIKIN:
                        mContext.mOptChaikin0[IDX] = mValue1.Value;
                        mContext.mOptChaikin1[IDX] = mValue2.Value;
                        if (IDX == 0)
                            mContext.mOptChaikinOscillatorEMA = mValue3.Value;
                        break;
                    case ChartBase.CHART_ROC:
                        mContext.mOptROC[IDX] = mValue1.Value;
                        if (IDX == 0)
                            mContext.mOptROC_EMA = mValue2.Value;
                        break;
                    case ChartBase.CHART_NVI:
                        mContext.mOptNVI_EMA[0] = mValue1.Value;   
                        mContext.mOptNVI_EMA[1] = mValue2.Value;
                        break;
                    case ChartBase.CHART_PVI:
                        mContext.mOptPVI_EMA[0] = mValue1.Value;
                        mContext.mOptPVI_EMA[1] = mValue2.Value;
                        break;
                    case ChartBase.CHART_PVT:
                        mContext.mOptPVT = mValue1.Value;
                        break;
                    case ChartBase.CHART_CCI:
                        mContext.mOptCCIConstant = mValue1.Value;
                        mContext.mOptCCIPeriod = mValue2.Value;
                        break;
                    case ChartBase.CHART_MASSINDEX:
                        mContext.mOptMassIndexDifferent = (int)mValue1.Value;
                        mContext.mOptMassIndexSum = (int)mValue2.Value;
                        break;
                    case ChartBase.CHART_ZIGZAG:
                        mContext.mOptZigzagPercent = mValue1.Value;
                        break;
                    case ChartBase.CHART_VOLUME:
                        mContext.mOptSMAVolume = mValue1.Value;
                        break;
                    case ChartBase.CHART_TRIX:
                        mContext.mOptTRIX[0] = mValue1.Value;
                        mContext.mOptTRIX[1] = mValue2.Value;
                        break;
                    case ChartBase.CHART_OBV:
                        mContext.mOptOBV_EMA = mValue1.Value;
                        break;
                    case ChartBase.CHART_AROON:
                    case ChartBase.CHART_AROON_OSCILLATOR:
                        mContext.mOptAroonPeriod = mValue1.Value;
                        break;
                    case ChartBase.CHART_CFM:
                        mContext.mOptCFMPeriod = mValue1.Value;
                        break;

                    case ChartBase.CHART_CRS_RATIO:
                        {
                            VTDictionary d = GlobalData.getCRSRatio();
                            d.setValueInt(GlobalData.kCRSBaseMa1, (int)mValue1.Value);
                            d.setValueInt(GlobalData.kCRSBaseMa2, (int)mValue2.Value);

                            String s = mTextField1.getText();
                            d.setValueString(GlobalData.kCRSBaseSymbol, s);
                            GlobalData.saveData();
                        }
                        break;

                    case ChartBase.CHART_CRS_PERCENT:
                        {
                            VTDictionary d = GlobalData.getCRSRatio();
                            d.setValueInt(GlobalData.kCRSBaseMa1, (int)mValue1.Value);
                            d.setValueInt(GlobalData.kCRSBaseMa2, (int)mValue2.Value);

                            d.setValueInt(GlobalData.kCRSPeriod, (int)mValue3.Value);

                            String s = mTextField1.getText();
                            d.setValueString(GlobalData.kCRSBaseSymbol, s);
                            GlobalData.saveData();
                        }
                        break;
                }
            }
        }

        void resetOptions()
        {
            int idx = cb_Indicator.SelectedIndex;
            if (idx >= 0 && idx < mIndicators.Length)
            {
                int chart = mIndicators[idx];
                switch (chart)
                {
                    case ChartBase.CHART_BOLLINGER:
                        mContext.mOptBBPeriod = mContext.mOptBBPeriodDefault;
                        mContext.mOptBBD = mContext.mOptBBDDefault;
                        break;
                    case ChartBase.CHART_ENVELOP:
                        mContext.mOptEnvelopPeriod = 14;
                        mContext.mOptEnvelopLine0[0] = true;
                        mContext.mOptEnvelopLine1[0] = true;
                        mContext.mOptEnvelopLine2[0] = true;
                        break;
                    case ChartBase.CHART_PSAR:
                        mContext.mOptPSAR_alpha = mContext.mOptPSAR_alphaDefault;
                        mContext.mOptPSAR_alpha_max = mContext.mOptPSAR_alpha_maxDefault;
                        break;
                    case ChartBase.CHART_VSTOP:
                        mContext.mOptVSTOP_ATR_Loopback = 20;
                        mContext.mOptVSTOP_MULT = 2.0f;
                        break;
                    case ChartBase.CHART_MACD:
                        mContext.mOptMACDSlow = mContext.mOptMACDSlowDefault;
                        mContext.mOptMACDFast = mContext.mOptMACDFastDefault;
                        mContext.mOptMACDSignal = mContext.mOptMACDSignalDefault;
                        break;
                    case ChartBase.CHART_RSI:
                        if (IDX == 0)
                            mContext.mOptRSIPeriod[IDX] = 14;
                        else
                            mContext.mOptRSIPeriod[IDX] = 7;
                        break;
                    case ChartBase.CHART_WILLIAMR:
                        mContext.mOptWilliamRPeriod = 14;
                        mContext.mOptWR_EMA = 5;
                        break;
                    case ChartBase.CHART_STOCHRSI:
                        mContext.mOptStochRSIPeriod = 14;
                        mContext.mOptStochRSISMAPeriod = 5;
                        break;
                    case ChartBase.CHART_MFI:
                        mContext.mOptMFIPeriod[IDX] = 14;// mContext.mOptMFIPeriodDefault;
                        break;
                    case ChartBase.CHART_STOCHASTIC_FAST:
                        mContext.mOptStochasticFastKPeriod = 14;
                        mContext.mOptStochasticFastSMA = 3;
                        break;
                    case ChartBase.CHART_STOCHASTIC_SLOW:
                        mContext.mOptStochasticSlowKPeriod = 14;
                        mContext.mOptStochasticSlowKSmoothK = 3;
                        mContext.mOptStochasticSlowSMA = 3;
                        break;
                    case ChartBase.CHART_ICHIMOKU:
                        mContext.mOptIchimokuTime1 = 9;
                        mContext.mOptIchimokuTime2 = 26;
                        mContext.mOptIchimokuTime3 = 52;
                        break;
                    case ChartBase.CHART_ADX:
                        mContext.mOptADXPeriod = 14;
                        mContext.mOptADXPeriodDMI = 14;
                        break;
                    case ChartBase.CHART_MAE:
                        break;
                    case ChartBase.CHART_ADL:
                        mContext.mOptADL_SMA[0] = 3;
                        mContext.mOptADL_SMA[1] = 10;   //  chaikin oscillator
                        break;
                    case ChartBase.CHART_CHAIKIN:
                        mContext.mOptChaikin0[IDX] = 3;
                        mContext.mOptChaikin1[IDX] = 10;
                        break;
                    case ChartBase.CHART_ROC:
                        mContext.mOptROC[IDX] = 12;
                        break;
                    case ChartBase.CHART_ZIGZAG:
                        mContext.mOptZigzagPercent = 6.0f;
                        break;
                    case ChartBase.CHART_VOLUME:
                        mContext.mOptSMAVolume = 9;
                        break;
                    case ChartBase.CHART_NVI:
                        mContext.mOptNVI_EMA[0] = 5;
                        mContext.mOptNVI_EMA[1] = 0;
                        break;
                    case ChartBase.CHART_PVI:
                        mContext.mOptPVI_EMA[0] = 5;
                        mContext.mOptPVI_EMA[1] = 0;
                        break;
                    case ChartBase.CHART_PVT:
                        mContext.mOptPVT = 3;
                        break;
                    case ChartBase.CHART_CCI:
                        mContext.mOptCCIPeriod = 20;
                        mContext.mOptCCIConstant = 0.015f;
                        break;
                    case ChartBase.CHART_MASSINDEX:
                        mContext.mOptMassIndexDifferent = 9;
                        mContext.mOptMassIndexSum = 25;
                        break;
                    case ChartBase.CHART_TRIX:
                        mContext.mOptTRIX[0] = 15;
                        mContext.mOptTRIX[1] = 9;
                        break;
                    case ChartBase.CHART_AROON:
                    case ChartBase.CHART_AROON_OSCILLATOR:
                        mContext.mOptAroonPeriod = 25;
                        break;
                    case ChartBase.CHART_CFM:
                        mContext.mOptCFMPeriod = 20;
                        break;
                    case ChartBase.CHART_ATR:
                        mContext.mOptATRLoopback = 14;
                        break;
                }

                if (mContext.getSelectedShare() != null)
                {
                    mContext.getSelectedShare().clearCalculations();
                    mListener.onEvent(this, C.EVT_REPAINT_CHARTS, 0, 0);
                }
                mContext.saveOptions();

                cb_Indicator_SelectedIndexChanged(cb_Indicator, null);
            }
        }
        void setupZigzag()
        {
            mContainer.removeAllControls();

            int y = 0;
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("(Giá trị hiện tại: Zigzag step={0:F1}%", mContext.mOptZigzagPercent);
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;
            //  period
            mValue1.Value = mContext.mOptZigzagPercent;
            addSlider(y, "Zigzag step(%)", 0.5f, 30.0f, 0.1f, mValue1, 10);
            y += 44;

            //========================
            addOKButton(y);
        }

        void setupSMAVolume()
        {
            mContainer.removeAllControls();

            int y = 0;
            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("(Giá trị hiện tại: SMA Volume={0:F0}", mContext.mOptSMAVolume);
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);

            y = l.getBottom() + 4;
            //  period
            mValue1.Value = mContext.mOptSMAVolume;
            addSlider(y, "Period", 0, 100, 1, mValue1, 1);
            y += 44;

            //========================
            addOKButton(y);
        }

        public void setOptionIDX(int idx)
        {
            IDX = idx;
        }

        void setupMassIndex()
        {
            mContainer.removeAllControls();

            int y = 0;

            //  difference
            mValue1.Value = mContext.mOptMassIndexDifferent;

            StringBuilder sb = Utils.getSB();
            sb.AppendFormat("Loopback for difference (Giá trị hiện tại: {0})", mContext.mOptMassIndexDifferent);
            xLabel l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);
            y = l.getBottom() + 4;

            addSlider(y, "Period", 1, 150, 1, mValue1, 1);
            y += 44;

            //  summary
            mValue2.Value = mContext.mOptMassIndexSum;

            sb = Utils.getSB();
            sb.AppendFormat("Loopback for summary (Giá trị hiện tại: {0})", mContext.mOptMassIndexSum);
            l = new xLabel(sb.ToString());
            l.setPosition(0, y);
            l.setSize(mContainer.getW(), -1);
            l.setAlign(xGraphics.RIGHT);
            mContainer.addControl(l);
            y = l.getBottom() + 4;

            addSlider(y, "Period", 1, 150, 1, mValue2, 1);
            y += 44;
            //========================
            addOKButton(y);
        }

        void setupRSRatio()
        {
            mContainer.removeAllControls();

            int y = 0;

            VTDictionary config = GlobalData.getCRSRatio();

            String symbol = config.getValueString(GlobalData.kCRSBaseSymbol);
            int ma1 = config.getValueInt(GlobalData.kCRSBaseMa1);
            int ma2 = config.getValueInt(GlobalData.kCRSBaseMa2);

            Share share = Context.getInstance().mShareManager.getShare(symbol);
            if (share == null)
            {
                symbol = "^VNINDEX";
            }

            //  difference
            mTextField1 = addText(y, "So sánh với mã", symbol);
            y += 44;

            //  MA1 & MA2
            mValue1.Value = ma1;
            addSlider(y, "MA 1", 0, 50, 1, mValue1, 1);
            y += 44;
            mValue2.Value = ma2;
            addSlider(y, "MA 2", 0, 100, 1, mValue2, 1);
            y += 44;

            //========================
            addOKButton(y);
        }

        void setupRSPercent()
        {
            mContainer.removeAllControls();

            int y = 0;

            VTDictionary config = GlobalData.getCRSPercent();

            String symbol = config.getValueString(GlobalData.kCRSBaseSymbol);
            int ma1 = config.getValueInt(GlobalData.kCRSBaseMa1);
            int ma2 = config.getValueInt(GlobalData.kCRSBaseMa2);

            Share share = Context.getInstance().mShareManager.getShare(symbol);
            if (share == null)
            {
                symbol = "^VNINDEX";
            }

            //  difference
            mTextField1 = addText(y, "So sánh với mã", symbol);
            y += 44;

            //  ref
            mValue3.Value = config.getValueInt(GlobalData.kCRSPeriod);
            if (mValue3.Value == 0)
            {
                mValue3.Value = 20;
            }
            addSlider(y, "Thời gian tham chiếu", 3, 100, 1, mValue3, 1);
            y += 44;

            //  MA1 & MA2
            mValue1.Value = ma1;
            addSlider(y, "MA 1", 0, 50, 1, mValue1, 1);
            y += 44;

            mValue2.Value = ma2;
            addSlider(y, "MA 2", 0, 100, 1, mValue2, 1);
            y += 44;

            //========================
            addOKButton(y);
        }
    }
}
