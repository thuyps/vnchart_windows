using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using xlib.utils;
using xlib.ui;
using stock123.app;
using stock123.app.data;
using xlib.framework;

namespace stock123.app.chart
{
    public class ChartMCDX: ChartBase
    {
        float[] rsiBanker = new float[Share.MAX_CANDLE_CHART_COUNT]; 
        float[] rsiBankerSMA = new float[Share.MAX_CANDLE_CHART_COUNT];
        float[] rsiHotmoney = new float[Share.MAX_CANDLE_CHART_COUNT];

        short[] bankerXY;
        short[] bankerXYSMA;
        short[] hotmoneyXY;

        short[] mPricelines = new short[10];
        //==================
        short[] mChartEMA;
        int checkSum = 0;
        //===================================
        public ChartMCDX(Font f)
            : base(f)
        {
            mShouldDrawCursor = true;
            mChartType = CHART_MCDX;
            //CHART_BORDER_SPACING_Y = 1;
        }

        const int FIELD_PERIOD = 0;
        const int FIELD_BASE = 1;
        const int FIELD_SENSITIVITY = 2;
        float getConfigValue(bool isBanker, int field)
        {
            float defValue = 0;
            try{
                String s;
                if (isBanker){
                    s = GlobalData.getData().getValueString(GlobalData.kBankerPeriod_Base_Sens, "50/50/1.5");

                    float[] dv = {50, 50, 1.5f};
                    defValue = dv[field];
                }
                else{
                    s = GlobalData.getData().getValueString(GlobalData.kHotmoneyPeriod_Base_Sens, "40/30/0.7");

                    float[] dv = {40, 30, 0.7f};
                    defValue = dv[field];
                }

                String[] ss = s.Split('/');
                String sv = ss[field];
                float v = Utils.stringToFloat(sv);

                return v;
            }
            catch (Exception e){

            }

            return defValue;
        }

        public static void rsi_function(Share share, float sensitivity, int period, int baseValue, float[] rsi)
        {
            //Share share = getShare();
            float[] rsiPeriod = Share.pStaticTMP;
            share.calcRSICustom(share.mCClose, period, rsiPeriod);

            int cnt = share.getCandleCnt();
            for (int i = 0; i < cnt; i++)
            {
                float v = sensitivity*(rsiPeriod[i] - baseValue);

                if (v > 20){
                    v = 20;
                }
                else if (v < 0){
                    v = 0;
                }

                rsi[i] = v;
            }
        }

        public override void render(xGraphics g)
        {
            Share share = getShare(3);

            if (share == null)
            {
                return;
            }
            if (detectShareCursorChanged())
            {
                bankerXY = allocMem(bankerXY, mChartLineLength * 2);
                bankerXYSMA = allocMem(bankerXYSMA, mChartLineLength * 2);
                hotmoneyXY = allocMem(hotmoneyXY, mChartLineLength * 2);

                // banker
                int baseBanker = (int)getConfigValue(true, FIELD_BASE);
                int periodBanker = (int)getConfigValue(true, FIELD_PERIOD);
                float sensitivityBanker = getConfigValue(true, FIELD_SENSITIVITY);

                //  hot money
                int baseHM = (int)getConfigValue(false, FIELD_BASE);
                int periodHM = (int)getConfigValue(false, FIELD_PERIOD);
                float sensitivityHM = getConfigValue(false, FIELD_SENSITIVITY);

                int newChecksum = 0;
                //-----------------------
                newChecksum = share.getID() + baseBanker * 100 + periodBanker * 99 + (int)(sensitivityBanker * 1000)
                                + baseHM * 88 + periodHM * 77 + (int)(sensitivityHM * 2000);
                int checkSumData = share.getCandleCnt();
                for (int i = 0; i < 10; i++)
                {
                    checkSumData += (int)(share.getClose(share.getCandleCnt() - 1 - i) * 10);
                }
                newChecksum = newChecksum + checkSumData;
                //-----------------------
                if (checkSum != newChecksum)
                {
                    checkSum = newChecksum;

                    rsi_function(share, sensitivityBanker, periodBanker, baseBanker, rsiBanker);
                    Share.SMA(rsiBanker, 0, share.getCandleCnt(), 5, rsiBankerSMA);

                    rsi_function(share, sensitivityHM, periodHM, baseHM, rsiHotmoney);
                }

                //  banker
                pricesToYs(rsiBanker, share.mBeginIdx, bankerXY, mChartLineLength, 0, 20);

                //  sma of banker
                pricesToYs(rsiBankerSMA, share.mBeginIdx, bankerXYSMA, mChartLineLength, 0, 20);

                //  hot money
                pricesToYs(rsiHotmoney, share.mBeginIdx, hotmoneyXY, mChartLineLength, 0, 20);
            }

            String[] ss = { "5", "10", "15"};
            float[] tmp = { 5, 10, 15};
            pricesToYs(tmp, 0, mPricelines, 3, 0, 20);

            for (int i = 0; i < 3; i++)
            {
                g.setColor(C.COLOR_FADE_YELLOW);
                g.drawLine(0, mPricelines[2 * i + 1], getW() - 34, mPricelines[2 * i + 1]);
                
                g.setColor(C.COLOR_FADE_YELLOW0);
                g.drawString(mFont, ss[i], getW() - 8, mPricelines[2 * i + 1], xGraphics.VCENTER | xGraphics.RIGHT);
            }

            int mY = 0;
            int mX = 0;

            //  bars
            mVolumeBarW = (((float)getDrawingW() / mChartLineLength) * 2.0f / 3);
            for (int i = 0; i < mChartLineLength; i++)
            {
                //  retail
                g.setColor(themeDark()?0xff005e07:C.COLOR_GREEN_DARK);

                float x = bankerXY[i*2] - mVolumeBarW/2;
                float y = mY + getMarginY();

                g.fillRectF(x, y, mVolumeBarW, getDrawingH());

                //  hot money
                g.setColor(themeDark()?0xffd8c200:C.COLOR_YELLOW);
                //y = mY + getMarginY() + getDrawingH() - rsiHotMoneyXY[2*i+1];
                y = hotmoneyXY[2*i+1];
                g.fillRectF(x, y, mVolumeBarW, mY + getMarginY() + getDrawingH() - y);

                //  banker
                g.setColor(themeDark()?0xffff0000:C.COLOR_RED);
                //y = mY + getMarginY() + getDrawingH() - rsiBankerXY[2*i+1];
                y = bankerXY[2*i+1];
                g.fillRectF(x, y, mVolumeBarW, mY + getMarginY() + getDrawingH() - y);
            }

            //  sma of banker
            g.setColor(C.COLOR_BLUE_LIGHT);// colorSMAOfIndicator());
            g.drawLines(bankerXYSMA, mChartLineLength, 1.0f);

            mMouseTitle = null;//"" + (int)yToPrice(mLastY, 0, 100);

            renderCursor(g);
        }


        override public xVector getTitles()
        {
            xVector v = new xVector();

            String s1 = "Banker:" + GlobalData.getData().getValueString(GlobalData.kBankerPeriod_Base_Sens, "50/50/1.5");
            String s2 = "HotMoney:" + GlobalData.getData().getValueString(GlobalData.kHotmoneyPeriod_Base_Sens, "40/30/0.7");

            v.addElement(new stTitle("(Period/Base/Sens)", themeDark() ? C.COLOR_WHITE : C.COLOR_BLACK));
            v.addElement(new stTitle(s1, themeDark()?C.COLOR_RED:C.COLOR_RED));
            v.addElement(new stTitle(s2, themeDark()?C.COLOR_YELLOW:C.COLOR_YELLOW));

            return v;
            
        }
    }
}
