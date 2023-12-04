using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xlib.framework;
using xlib.utils;
using xlib.ui;
using stock123.app;
using stock123.app.data;
using System.Drawing;

namespace stock123.app.chart
{
    class TA
    {
        static float[][] buffers = {null, null, null, null, null, null, null, null, null, null};
        static bool[][] booleans = {null, null, null, null};
        
        static public void initTA(){
            booleans[0] = new bool[Share.MAX_CANDLE_CHART_COUNT];
            booleans[1] = new bool[Share.MAX_CANDLE_CHART_COUNT];

            for (int i = 0; i < 10; i++){
                buffers[i] = new float[Share.MAX_CANDLE_CHART_COUNT];
            }

            for (int i = 0; i < 4; i++){
                booleans[i] = new bool[Share.MAX_CANDLE_CHART_COUNT];
            }
        }
        static public void aveSum14(float[] t, int cnt, int period)
        {
            int k = cnt < period ? cnt : period;
            int i = 0;
            for (i = 0; i < k; i++)
            {
                for (int j = k - i - 1; j > 0; j--)
                {
                    t[k - i - 1] += t[j];
                }
            }
            //=============TR14=============
            for (i = period; i < cnt; i++)
            {
                t[i] = t[i - 1] - t[i - 1] / period + t[i];
            }
        }

        //=========================
        /*
        S = EMA_Integer[(X � EMA_Integer(X))2] = EMA_Integer(X2) � [EMA_Integer(X)]2
        d = sqrt(S);
        //=============input: period, d============
        MA = SMA_Integer(price, period);
        DELTA_i = (PRICE_i - MA_i)2
        UPPPER_i = MA_i+d*sqrt(SMA_Integer(DELTA));
        LOWWER_i = MA_i-d*sqrt(SMA_Integer(DELTA));
         */

        public static void calcBollinger(float[] prices, int cnt, int period, float d, float[] lower, float[] upper)
        {
            //	int start = 0;

            if (cnt < period)
            {
                for (int i = 0; i < cnt; i++)
                {
                    upper[i] = 0;
                    lower[i] = 0;
                }
                return;
            }

            float[] sma = TA.SMA(prices, 0, cnt, period, null);

            if (sma == null)
            {
                return;
            }

    //        xUtils.trace("Calc Bollinger Bands: cnt=" + cnt + " period: " + period + " d=" + d);

            float[] deltaSma2 = new float[cnt];
            for (int i = 0; i < period; i++)
            {
                deltaSma2[i] = 0;
            }

            float tmp = 0;

            //int t = (2012 << 16) | (6<<8) | 11;
            for (int i = period; i < cnt; i++)
            {
                tmp = 0;
                for (int j = 0; j < period; j++)
                {
                    tmp += (prices[i - j] - sma[i]) * (prices[i - j] - sma[i]);		//	square
                }
                deltaSma2[i] = (float)Math.Sqrt(tmp / period);

                //if (mCDate[i] == t)
                //{
                //xUtils.trace("tmp=" + tmp + " deltaSma2[i]:" + deltaSma2[i] + " price: " + prices[i] + " sma[i]:" + sma[i]);
                //}
            }

            for (int i = 0; i < cnt; i++)
            {
                upper[i] = sma[i] + (d * (deltaSma2[i]));
                lower[i] = sma[i] - (d * (deltaSma2[i]));

                //if (mCDate[i] == t)
                //{
                //xUtils.trace("UPPER:" + upper[i]);
                //xUtils.trace("LOWER:" + lower[i]);
                //}
            }

            sma = null;
            deltaSma2 = null;
        }

        static public void calcRSI(Share share, int period, float[] rsi)
        {
            float[] closes = share.readCloses();
            calcRSI(closes, share.getCandleCnt(), period, rsi);
        }

        static public void calcRSI(float[] prices, int cnt, int period, float[] rsi)
        {
            if (cnt < 4)
                return;

            //if (mIsCalcRSI)
            //return;
            //mIsCalcRSI = true;

            float[] av_up = { 0, 0 };
            float[] av_do = { 0, 0 };
            float[] rs = new float[cnt];
            float currgain;
            float currlost;

            int j = 0;
            av_up[0] = 0;
            av_do[0] = 0;

            for (int i = 0; i < cnt; i++)
            {
                //		if (i == 53)
                //			int k = 0;
                if (i <= 0)
                {
                    rs[j++] = 1.0f;
                    continue;
                }
                currgain = prices[i] - prices[i - 1];
                currlost = prices[i] - prices[i - 1];

                if (currgain < 0)
                {
                    currgain = 0;
                    currlost = -currlost;
                }
                if (currgain > 0)
                {
                    currlost = 0;
                }

                av_up[1] = (av_up[0] * (period - 1) + currgain) / period;
                av_do[1] = (av_do[0] * (period - 1) + currlost) / period;

                if (av_do[1] == 0)
                    av_do[1] = av_up[1];

                if (av_do[1] == 0)
                    rs[j] = 1.0f;
                else
                    rs[j] = av_up[1] / av_do[1];
                av_up[0] = av_up[1];
                av_do[0] = av_do[1];
                j++;
            }

            for (int i = 0; i < cnt; i++)
            {
                rsi[i] = 100 - (100 / (1 + rs[i]));
            }
        }

        static public int[] SMA(int[] val, int val_off, int cnt, int period, int[] o)
        {
            if (cnt < 3)
            {
                return null;
            }

            int[] ema = o;
            if (ema == null)
            {
                ema = new int[cnt];
            }

            long pre_total = 0;
            long period_cnt = 0;
            int first = 0;

            for (int i = 0; i < cnt; i++)
            {
                long total = pre_total + val[val_off + i];
                if (period_cnt < period)
                {
                    period_cnt++;
                }

                ema[i] = (int)(total / period_cnt);

                if (period_cnt == period)
                {
                    first = val[val_off + i - period + 1];
                }
                pre_total = total - first;
            }

            return ema;
        }

        public static float[] SMA(float[] val, int cnt, int period, float[] o)
        {
            float[] ema = o;
            if (ema == null)
                ema = new float[cnt];

            float pre_total = 0;
            float period_cnt = 0;
            float first = 0;

            for (int i = 0; i < cnt; i++)
            {
                if (i == cnt - 4)
                {
                    int k = 0;
                }

                float total = pre_total + val[i];
                if (period_cnt < period)
                    period_cnt++;

                ema[i] = total / period_cnt;

                if (period_cnt == period)
                    first = val[i - period + 1];
                pre_total = total - first;
            }

            return ema;
        }

        static float[] SMA_VW(float[] val, int[] vol, int cnt, int period, float[] o)
        {
            float[] ema = o;
            if (ema == null) {
                ema = new float[cnt];
            }

            for (int i = 0; i < cnt; i++)
            {
                if (i < period){
                    ema[i] = val[i];
                }
                else{
                    double totalVol = 0;
                    double totalVal = 0;
                    for (int j = i - period+1; j <= i; j++){
                        totalVol += vol[j];
                        totalVal += val[j]*vol[j];
                    }
                    ema[i] = (float)(totalVal/totalVol);
                }
            }

            return ema;
        }

        static public float[] SMA(int[] val, int val_off, int cnt, int period, float[] o)
        {
            if (cnt < 3)
            {
                return null;
            }

            float[] ema = o;
            if (ema == null)
            {
                ema = new float[cnt];
            }

            double pre_total = 0;
            int period_cnt = 0;
            float first = 0;

            for (int i = 0; i < cnt; i++)
            {
                double total = pre_total + val[val_off + i];
                if (period_cnt < period)
                {
                    period_cnt++;
                }

                ema[i] = (float)(total / period_cnt);

                if (period_cnt == period)
                {
                    first = val[val_off + i - period + 1];
                }
                pre_total = total - first;
            }

            return ema;
        }

        static public float[] SMA(float[] val, int val_off, int cnt, int period, float[] o)
        {
            if (cnt < 3)
            {
                return null;
            }

            float[] ema = o;
            if (ema == null)
            {
                ema = new float[cnt];
            }

            float pre_total = 0;
            int period_cnt = 0;
            float first = 0;

            for (int i = 0; i < cnt; i++)
            {
                float total = pre_total + val[val_off + i];
                if (period_cnt < period)
                {
                    period_cnt++;
                }

                ema[i] = total / period_cnt;

                if (period_cnt == period)
                {
                    first = val[val_off + i - period + 1];
                }
                pre_total = total - first;
            }

            return ema;
        }

        public static float calculateSD(float[] numArray, int offset, int period)
        {
            float sum = 0.0f;
            float standardDeviation = 0.0f;

            if (offset < period){
                return 0.0f;
            }

            int begin = offset - period + 1;

            //  calc mean
            for (int i = begin; i <= offset; i++){
                sum += numArray[i];
            }

            double mean = sum/period;

            //  calc stdDv
            for (int i = begin; i <= offset; i++){
                standardDeviation += (float)Math.Pow(numArray[i] - mean, 2);
            }

            return (float)Math.Sqrt(standardDeviation/period);
        }

        static public float[] StdDv(float[] val, int offset, int cnt, int period, float[] o)
        {
            if (o == null){
                o = buffers[0];
            }

            for (int i = offset; i < offset+period; i++){
                o[i] = 0;
            }

            for (int i = period; i < cnt; i++){
                int off = offset + i;
                o[i] = calculateSD(val, off, period);
            }
            return  o;
        }

        static public float[] EMA(float[] val, int cnt, int period, float[] o)
        {
            if (cnt < 3)
                return null;

            float[] ema = null;

            if (o != null)
                ema = o;
            else
                ema = new float[cnt];

            if (period == 0)
            {
                for (int i = 1; i < cnt; i++)
                {
                    ema[i] = val[i];
                }
            }else{
                ema[0] = val[0];
                float exp = (float)2 / (period + 1);
                for (int i = 1; i < cnt; i++)
                {
                    //ema[i] = (float)val[i] * exp + ema[i - 1] * (1 - exp);
                    ema[i] = (val[i] - ema[i - 1]) * exp + ema[i - 1];
                }
            }
            return ema;
        }

        static public float[] EMA(int[] val, int cnt, int period, float[] o)
        {
            if (cnt < 3)
                return null;

            float[] ema = null;

            if (o != null)
                ema = o;
            else
                ema = new float[cnt];

            ema[0] = val[0];
            float exp = (float)2 / (period + 1);
            for (int i = 1; i < cnt; i++)
            {
                //ema[i] = (float)val[i] * exp + ema[i - 1] * (1 - exp);
                ema[i] = (val[i] - ema[i - 1]) * exp + ema[i - 1];
            }

            return ema;
        }

        static public float[] EMA(int[] val, int val_off, int cnt, int period)
        {
            if (cnt < 3)
            {
                return null;
            }

            float[] ema = new float[cnt];

            ema[0] = val[val_off];
            float exp = (float)2 / (period + 1);
            for (int i = 1; i < cnt; i++)
            {
                ema[i] = (float)val[val_off + i] * exp + ema[i - 1] * (1 - exp);
            }

            return ema;
        }

        static public float[] EMA(int[] val, int val_off, int cnt, int period, float[] o)
        {
            if (cnt < 3)
            {
                return null;
            }

            float[] ema = o;
            if (ema == null)
            {
                ema = new float[cnt];
            }

            ema[0] = val[val_off];
            float exp = (float)2 / (period + 1);
            for (int i = 1; i < cnt; i++)
            {
                ema[i] = (float)val[val_off + i] * exp + ema[i - 1] * (1 - exp);
            }

            return ema;
        }

        static public float[] EMA(float[] val, int val_off, int cnt, int period, float[] o)
        {
            if (cnt < 3)
            {
                return null;
            }

            float[] ema = o;
            if (ema == null)
            {
                ema = new float[cnt];
            }

            ema[0] = val[val_off];
            float exp = (float)2 / (period + 1);
            for (int i = 1; i < cnt; i++)
            {
                ema[i] = val[val_off + i] * exp + ema[i - 1] * (1 - exp);
            }

            return ema;
        }

        //  line1 cut above line2 or going to cut above
        static public bool hasIntersesionAbove(float[] line1, float[] line2, int dayBacks, int cnt)
        {
            if (cnt < 3) {
                return false;
            }


            int e = cnt - 1;

            //  07072020 - loai bo dieu kien nay
            /*
            if (isLineGoingdown(line1, e)){
                return false;
            }

             */

            if (line1[e] >= line2[e]){
                if (dayBacks > 0)
                {
                    int b = cnt - dayBacks - 1;
                    if (b < 0) {
                        b = 0;
                    }
                    for (int i = e-1; i >= b; i--){
                        if (line1[i] < line2[i]){
                            return true;
                        }
                    }
                    return false;
                }
                else{
                    return true;
                }
            }
    /*
            //  about cutting: line1 is below line2
            if (cnt > 10)
            {
                float d0 = line2[e] - line1[e];
                float d1 = line2[e-1] - line1[e-1];
                float d2 = line2[e-2] - line1[e-2];
                if (d0 < d1 && d1 < d2){
                    if (d0*1.2f < d1)
                    {
                        float percentToClose = (d0*100)/line1[e];
                        return percentToClose <= 4;
                    }
                }
            }

     */

            return false;
        }

        //  line1 cut above line2 or going to cut above
        static public bool hasIntersesionBelow(float[] line1, float[] line2, int dayBacks, int cnt)
        {
            if (cnt < 3)
                return false;

            int e = cnt - 1;

            //  case 1: had cut below
            if (line1[e] <= line2[e]){
                if (dayBacks > 0)
                {
                    int b = cnt - dayBacks - 1;
                    if (b < 0){
                        b = 0;
                    }
                    for (int i = e-1; i >= b; i--){
                        if (line1[i] > line2[i]){
                            return true;
                        }
                    }
                    return false;
                }
                else{
                    return true;
                }
            }
            return false;
        }

        static public bool isUpTrend(float[] trend, int cnt)
        {
            if (cnt < 10)
                return false;

            float[] pTMP = buffers[0];
            SMA(trend, cnt - 10, cnt, 4, pTMP);
            for (int i = 9; i > 6; i--)
            {
                if (pTMP[i] < pTMP[i - 1])
                    return false;
            }

            return true;
        }

        static public bool isUpTrend(float[] trend, int cnt, int daysback)
        {
            if (cnt < 10){
                return false;
            }
            float[] pTMP = buffers[0];
            SMA(trend, cnt, 3, daysback, pTMP);

            int last = cnt - 1;
            if (trend[last] > pTMP[last]){
                if (daysback > 0) {
                    for (int i = 0; i < daysback; i++) {
                        if (trend[last - i] <= pTMP[last-i]) {
                            return true;
                        }
                    }
                }
                else{
                    return true;
                }
            }

            return false;
        }

        static public bool isDowntrend(float[] prices, int cnt)
        {
            if (cnt < 7)
                return false;

            return (prices[cnt - 1] < prices[cnt - 4] && prices[cnt - 2] < prices[cnt - 5]);
        }

        static public bool isDowntrend(int[] prices, int offset, int cnt)
        {
            if (offset < 7)
                return false;

            float[] pTMP = buffers[0];
            SMA(prices, offset - 10, 10, 3, pTMP);
            for (int i = 6; i < 10; i++)
            {
                if (pTMP[i] > pTMP[i - 1])
                    return false;
            }

            return true;
        }

        static public bool isLower(float[] trend1, float[] trend2, int cnt)
        {
            if (cnt < 3)
                return false;

            return trend1[cnt - 1] < trend2[cnt - 1];
        }

        static public bool isHigher(float[] trend1, float[] trend2, int cnt)
        {
            if (cnt < 3)
                return false;

            return trend1[cnt - 1] > trend2[cnt - 1];
        }

        static public bool hasIntersesion(float[] line1, float[] line2, int cnt, int dayBacks)
        {
            if (cnt < 3)
                return false;

            int b = cnt - dayBacks - 1;
            int e = cnt;

            if (b < 0)
                b = 0;

            float delta = line1[b] - line2[b];
            float signal = 1;

            for (int i = b + 1; i < e; i++)
            {
                signal = (line1[i] - line2[i]) * delta;
                delta = (line1[i] - line2[i]);
                if (signal < 0)
                {
                    //  cut
                    return true;
                }
            }
            //===========going to cut?==============
            delta = line1[b] - line2[b];
            float delta2 = line1[e-1] - line2[e-1];
            return Math.Abs(delta2) < Math.Abs(delta) / 2;
        }

        public static bool isLineGoingUp(float[] line, int end)
        {
            float d0 = line[end] - line[end-1];
            float d1 = line[end] - line[end-2];
            float d2 = line[end] - line[end-3];

            if (d0 > 0) return true;

            if (d0 == 0){
                return d1 > 0;
            }

            if (d0 == 0 && d1 == 0){
                return d2 > 0;
            }

            return false;
        }

        public static bool isLineGoingdown(float[] line, int end)
        {
            float d0 = line[end] - line[end-1];
            float d1 = line[end] - line[end-2];
            float d2 = line[end] - line[end-3];

            if (d0 < 0) return true;

            if (d0 == 0){
                return d1 < 0;
            }

            if (d0 == 0 && d1 == 0){
                return d2 < 0;
            }

            return false;
        }

        public static bool isLine1CutAboveLine2(float[] line1, float[] line2, int last, int lookback)
        {
            if (line1[last] > line2[last] && last-lookback > 0){
                for (int i = last - 1; i > last - lookback; i--){
                    if (line1[i] < line2[i]){
                        return true;
                    }
                }
            }

            return false;
        }

        static public bool isLine1CutBelowLine2(float[] line1, float[] line2, int last, int lookback)
        {
            if (line1[last] < line2[last] && last-lookback > 0){
                for (int i = last - 1; i > last - lookback; i--){
                    if (line1[i] > line2[i]){
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool isLine1CutAboveZero(float[] line1, int last, int lookback)
        {
            if (line1[last] > 0 && last-lookback > 0){
                for (int i = last - 1; i > last - lookback; i--){
                    if (line1[i] < 0){
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool isLine1CutBelowZero(float[] line1, int last, int lookback)
        {
            if (line1[last] < 0 && last-lookback > 0){
                for (int i = last - 1; i > last - lookback; i--){
                    if (line1[i] > 0){
                        return true;
                    }
                }
            }

            return false;
        }

        public static float[] WMA(float[] val, int cnt, int period, float[] outBuffer)
        {
            if (cnt < 3) {
                return outBuffer;
            }

            float[] wma = null;

            if (outBuffer != null)
                wma = outBuffer;
            else
                wma = new float[cnt];

            wma[0] = val[0];

            int PERIOD = period*(period+1)/2;

            for (int i = 1; i < cnt; i++)
            {
                if (i < period){
                    float t = 0;
                    for (int j = 1; j <= i; j++){
                        t += val[j]*j;
                    }
                    wma[i] = t/(i*(i+1)/2);
                }
                else{
                    float t = 0;
                    for (int j = 1; j <= period; j++){
                        int idx = i - period + j;
                        t += val[idx]*j;
                    }
                    wma[i] = t/PERIOD;
                }
            }

            return wma;
        }

        public static float[] HMA(float[] src, int cnt, int period, float[] o, float[] buff1, float[] buff2)
        {
            //  Hull
            //  wma(2 * wma(src, len / 2) - wma(src, len), round(sqrt(len)))
            TA.WMA(src, cnt, period/2, buff1);
            TA.WMA(src, cnt, period, buff2);

            float sqrtPeriod = (float)Math.Round(Math.Sqrt(period));

            for (int i = 0; i < cnt; i++){
                buff1[i] = 2*buff1[i];
            }
            for (int i = 0; i < cnt; i++){
                buff1[i] = buff1[i] - buff2[i];
            }

            TA.WMA(buff1, cnt, (int)sqrtPeriod, o);
            return o;
        }

        public static float getPercentBase(float baseValue, float value)
        {
            if (baseValue == 0){
                return 0;
            }

            return 100*(value-baseValue)/baseValue;
        }

        //  true strength index
        public static void TSI(float[] prices, int len, int shortLength, int longLength, float[] tsi)
        {
            //  price changed
            float[] pc = buffers[0];
            pc[0] = 0;
            for (int i = 1; i < len; i++){
                pc[i] = prices[i] - prices[i-1];
            }
            //  first smooth: long
            float[] pcs = buffers[1];
            TA.EMA(pc, len, longLength, pcs);

            float[] pPcds = buffers[2];   //  doubleSmoothLong
            //  second smooth: short
            TA.EMA(pcs, len, shortLength, pPcds);
            //---------------------
            float[] apc = buffers[3];  //  absolute price changed
            apc[0] = 0;
            for (int i = 1; i < len; i++){
                apc[i] = Math.Abs(prices[i] - prices[i-1]);
            }
            //  first smooth: long
            float[] apcs = buffers[4];
            TA.EMA(apc, len, longLength, apcs);
            float[] pApcdps = buffers[5];

            //  second smooth: short
            TA.EMA(apcs, len, shortLength, pApcdps);
            //---------------------
            for (int i = 0; i < len; i++){
                if (pApcdps[i]> 0){
                    tsi[i] = 100*(pPcds[i]/pApcdps[i]);
                }
                else if (i > 0){
                    tsi[i] = tsi[i-1];
                }
                else{
                    tsi[i] = 0;    //  i==0
                }
            }
        }

        public static void SMI(float[] prices, int len,
                     int shortLengthTSI, int longLengthTSI,
                     int smoothingLengthSignal,
                     float[] tsi, float[] signal, float[] histogram
        )
        {
            TSI(prices, len, shortLengthTSI, longLengthTSI, tsi);

            //  signal
            TA.EMA(tsi, len, smoothingLengthSignal, signal);

            //  histogram
            for (int i = 0; i < len; i++){
                histogram[i] = tsi[i] - signal[i];
            }
        }

        static public float[] TRUERANGE(float[] close, float[] hi, float[] lo, int cnt,
                                        float[] TR)
        {
            if (cnt <= 0) {
                return null;
            }

            float t0, t1, t2;

            TR[0] = hi[0] - lo[0];

            for (int i = 1; i < cnt; i++){
                t0 = Math.Abs(hi[i] - lo[i]);
                t1 = Math.Abs(hi[i] - close[i-1]);
                t2 = Math.Abs(lo[i] - close[i]);

                t0 = Math.Max(t0, t1);
                t0 = Math.Max(t0, t2);

                TR[i] = t0;
            }

            return TR;
        }

        static public float[] TRUERANGE_Average(float[] close, float[] hi, float[] lo, int cnt,
                                         int period, float[] pATR)
        {
            if (cnt <= period || period <= 0) {
                return null;
            }

            float[] truerange = buffers[0];
            TRUERANGE(close, hi, lo, cnt, truerange);

            TA.SMA(truerange, cnt, period, pATR);

            return pATR;
        }

        static public float calcLinregY(float x1, float y1, float x2, float y2, float x)
        {
            if (x2 == x1){
                return y1;  //  imposible
            }

            //  general function
            //  (𝑦−𝑦1)/(𝑦2−𝑦1) = (𝑥−𝑥1)/(𝑥2−𝑥1)
            //  (y-y1) = (x-x1)*(y2-y1)/(x2-x1)
            //  y = y1 + (x-x1)*(y2-y1)/(x2-x1)
            float y = y1 + (x - x1)*(y2-y1)/(x2-x1);

            return y;
        }

        static public float calcLinregX(float x1, float y1, float x2, float y2, float y)
        {
            if (y2 == y1){
                return x1;  //  imposible
            }

            //  general function
            //  (𝑦−𝑦1)/(𝑦2−𝑦1) = (𝑥−𝑥1)/(𝑥2−𝑥1)
            //  x-x1 = (𝑦−𝑦1)*(x2-x1)/(y2-y1)
            //  x = x1 + (𝑦−𝑦1)*(x2-x1)/(y2-y1)

            float x = x1 + (y-y1)*(x2-x1)/(y2-y1);

            return x;
        }

        static public float calcDistanceBetweenPointToLine(float x0, float y0,
                                                           float x1, float y1,
                                                           float x2, float y2
        )
        {
            //  ax + by + c = 0
            //  (y1−y2)𝑥−(x1−x2)𝑦+x1y2−x2y1=0
            //  a = y1 - y2
            //  b = x2 - x1
            //  c = x1y2 - x2y1

            //  Distance = |ax0 + by0 + c|/sqrt(a*a + b*b)
            if ((x2 - x1) != 0)
            {
                //  x0 is out of range
                PointF m1 = new PointF();
                PointF m2 = new PointF();
                if (x1 < x2){
                    m1.X = x1; m1.Y = y1;
                    m2.X = x2; m2.Y = y2;
                }
                else{
                    m1.X = x2; m1.Y = y2;
                    m2.X = x1; m2.Y = y1;
                }
                if (x0 < m1.X){
                    double delta = Math.Sqrt((x0-m1.X)*(x0-m1.X) + (y0-m1.Y)*(y0-m1.Y));
                    return (float)delta;
                }
                if (x0 > m2.X){
                    double delta = Math.Sqrt((x0-m2.X)*(x0-m2.X) + (y0-m2.Y)*(y0-m2.Y));
                    return (float)delta;
                }
                //=======================

                double a = y1 - y2;
                double b = x2 - x1;
                double c = x1*y2 - x2*y1;

                double aabb = Math.Sqrt(a*a + b*b);
                if (aabb > 0) {
                    double t = Math.Abs(a * x0 + b * y0 + c) / Math.Sqrt(a * a + b * b);
                    return (float) t;
                }
            }
            else{
                //  vertical line
                return Math.Abs(x0 - x1);
            }

            return 0;
        }

        static public void Highest(float[] prices, int count, int period, float[] highest)
        {
            for (int i = 0; i < count; i++){
                float value = prices[i];
                for (int j = 0; j < period; j++)
                {
                    int idx = i - j;
                    if (idx < 0){
                        idx = 0;
                    }

                    if (prices[idx] > value){
                        value = prices[idx];
                    }
                }
                highest[i] = value;
            }
        }

        static public void Lowest(float[] prices, int count, int period, float[] lowest)
        {
            for (int i = 0; i < count; i++){
                float value = prices[i];
                for (int j = 0; j < period; j++)
                {
                    int idx  = i - j;
                    if (idx < 0){
                        idx = 0;
                    }

                    if (prices[idx] < value){
                        value = prices[idx];
                    }
                }
                lowest[i] = value;
            }
        }

        static public float HighestInRange(float[] prices, int begin, int end, int cnt)
        {
            if (begin < 0){
                begin = 0;
            }
            if (end > cnt-1){
                end = cnt-1;
            }

            float highest = prices[begin];
            for (int i = begin; i <= end; i++){
                float value = prices[i];
                if (value > highest){
                    highest = value;
                }
            }

            return highest;
        }

        static public float LowestInRange(float[] prices, int begin, int end, int cnt)
        {
            if (begin < 0){
                begin = 0;
            }
            if (end > cnt-1){
                end = cnt-1;
            }
            float lowest = prices[begin];
            for (int i = begin; i <= end; i++){
                float value = prices[i];
                if (value < lowest){
                    lowest = value;
                }
            }
            return lowest;
        }

        //  change == mom
        static public void change(float[] prices, int cnt, int period, float[] change)
        {
            for (int i = 0; i < cnt; i++){
                if (i >= period){
                    float v = prices[i] - prices[i - period];
                    change[i] = v;
                }
                else{
                    change[i] = prices[i] - prices[0];
                }
            }
        }

        static public void mom(float[] prices, int cnt, int period, float[] mo)
        {
            for (int i = 0; i < cnt; i++){
                if (i >= period){
                    float v = prices[i] - prices[i - period];
                    mo[i] = v;
                }
                else{
                    mo[i] = prices[i] - prices[0];
                }
            }
        }

        static public float valueWhen(bool[] condition,
                                      float[] price,
                                      int index,
                                      int occurrence,
                                      int[] offset,
                                      int adjustIndex
                                      )
        {
            int occurred = 0;
            int j = index;
            int found = -1;
            while (occurred <= occurrence && j >= 0) {
                if (condition[j]){
                    occurred++;
                    found = j;

                    if (found + adjustIndex > 0){
                        found = found + adjustIndex;
                    }
                }
                j--;
            }

            if (found >= 0){
                if (offset != null){
                    offset[0] = found;
                }
                return price[found];
            }
            else{
                return price[0];
            }
        }

        /*
        static public int valueWhen(boolean[] condition, int[] price, int index, int occurrence)
        {
            int occurred = 0;
            int j = index;
            int found = -1;
            while (occurred <= occurrence && j >= 0) {
                if (condition[j]){
                    occurred++;
                    found = j;
                }
                j--;
            }

            if (found >= 0){
                return price[found];
            }
            else{
                return price[0];
            }
        }

         */

        static public int valueIndexWhen(bool[] condition, int index, int occurrence)
        {
            int occurred = 0;
            int j = index;
            int found = -1;
            while (occurred <= occurrence && j >= 0) {
                if (condition[j]){
                    occurred++;
                    found = j;
                }
                j--;
            }

            if (found >= 0){
                return found;
            }
            else{
                return 0;
            }
        }

        static public void crossUnder(float[] price, int cnt, float baseValue, bool[] cross)
        {
            cross[0] = false;
            for (int i = 1; i < cnt; i++)
            {
                if (price[i-1] > baseValue && price[i] < baseValue){
                    cross[i] = true;
                }
                else{
                    cross[i] = false;
                }
            }
        }

        static public void crossOver(float[] price, int cnt, float baseValue, bool[] cross)
        {
            cross[0] = false;
            for (int i = 1; i < cnt; i++)
            {
                if (price[i-1] < baseValue && price[i] > baseValue){
                    cross[i] = true;
                }
                else{
                    cross[i] = false;
                }
            }
        }

        static public bool crossOverInWithinDays(float[] price, float[] signal, int index, int days)
        {
            int begin = index - days;
            if (begin < 0){
                begin = 0;
            }
            if (price[index] > signal[index]){
                for (int i = index - 1; i >= begin; i--){
                    if (price[index] < signal[index]){
                        return true;
                    }
                }
            }
            return false;
        }

        static public float[] D_SMA(float[] val, int cnt, int period, float[] outBuffer)
        {
            float[] sma = buffers[0];

            if (outBuffer == null){
                outBuffer = buffers[1];
            }

            SMA(val, cnt, period, sma);

            SMA(sma, cnt, period, outBuffer);

            return outBuffer;
        }
        static public float[] D_EMA(float[] val, int cnt, int period, float[] outBuffer)
        {
            float[] sma = buffers[0];

            if (outBuffer == null){
                outBuffer = buffers[1];
            }

            EMA(val, cnt, period, sma);

            EMA(sma, cnt, period, outBuffer);

            return outBuffer;
        }

        static public void MACD(float[] prices, int cnt,
                                    int fast, int slow, int signal,
                                    float[] pMacd, float[] pSignal, float[] pHistogram)
        {
            float[] ema12 = TA.EMA(prices, cnt, fast, buffers[0]);
            float[] ema26 = TA.EMA(prices, cnt, slow, buffers[1]);

            if (pMacd == null){
                pMacd = buffers[2];
            }
            if (pSignal == null){
                pSignal = buffers[3];
            }
            if (pHistogram == null){
                pHistogram = buffers[4];
            }

            float[] macd = pMacd;
            for (int i = 0; i < cnt; i++)
            {
                macd[i] = ema12[i] - ema26[i];

                if (ema26[i] > 0){
                    macd[i] = 100*(ema12[i] - ema26[i])/ema26[i];
                }
            }

            float[] sig = TA.EMA(macd, cnt, signal, pSignal);

            //=======histogram===============
            for (int i = 0; i < cnt; i++)
            {
                pHistogram[i] = macd[i] - sig[i];
            }
        }

        static public float[] Diff(float[] bufferA, float[] bufferB, int len, float[] bufferOut)
        {
            if (bufferOut == null){
                bufferOut = buffers[7];
            }

            for (int i = 0; i < len; i++){
                bufferOut[i] = bufferA[i] - bufferB[i];
            }

            return bufferOut;
        }

        static public float[] Diff(float[] bufferA, float baseValue, int len, float[] bufferOut)
        {
            if (bufferOut == null){
                bufferOut = buffers[7];
            }

            for (int i = 0; i < len; i++){
                bufferOut[i] = bufferA[i] - baseValue;
            }

            return bufferOut;
        }

        static public void pivotLow(float[] v, int len, int leftBars, int rightBars, float[] pivots)
        {
            // Calculate the pivot range as the sum of left bars and right bars
            for (int i = 0; i < len; i++){
                pivots[i] = 0;
            }

            int pivotRange = leftBars + rightBars;

            int last = 0;
            for (int i = pivotRange; i < len; i++){
                int index = i - leftBars;
                if (index == 2781){
    //                NSLog(@"");
                }
                float possiblePivotHigh = v[index];
                bool isValid = true;

                for (int j = i - pivotRange; j <= i; j++)
                {
                    if (j <= index){
                        if (v[j] < possiblePivotHigh){
                            isValid = false;
                            break;
                        }
                    }
                    else{
                        if (v[j] <= possiblePivotHigh){
                            isValid = false;
                            break;
                        }
                    }
                }

                if (isValid){
                    pivots[index] = possiblePivotHigh;
                    last = index;
                    i += rightBars;

                }
            }
        }

        static public void pivotHigh(float[] v, int len, int leftBars, int rightBars, float[] pivots)
        {
            // Calculate the pivot range as the sum of left bars and right bars
            for (int i = 0; i < len; i++){
                pivots[i] = 0;
            }

            int pivotRange = leftBars + rightBars;

            for (int i = pivotRange; i < len; i++){
                int index = i - leftBars;
                float possiblePivotHigh = v[index];
                bool isValid = true;

                if (index == 2752){
                    //NSLog(@"");
                }

                for (int j = i - pivotRange; j <= i; j++)
                {
                    if (v[j] > possiblePivotHigh){
                        isValid = false;
                        break;
                    }
                }

                if (isValid){
                    pivots[index] = possiblePivotHigh;
                    i += rightBars;
                }
            }
        }

        static public void detectConvergenceDivergence(
                Share share,    //  just for testing
                float[] momentum,
                float momentumeBaseValue,
                int cnt,
                int period,
                int momentum_sma,
                bool[] bear,
                bool[] bull,
                int[] line)
        {
            if (cnt <= 10){
                return;
            }

            float[] momentumSmoothed = buffers[4];

            if (momentum_sma > 1){
                TA.SMA(momentum, cnt, momentum_sma, momentumSmoothed);
            }
            else{
                momentumSmoothed = momentum;
            }

            float[] up = buffers[0];
            float[] dn = buffers[1];
            float[] tmp = buffers[2];

            share.readHighests(tmp);
            TA.Highest(tmp, cnt, period, up);

            share.readLowest(tmp);
            TA.Lowest(tmp, cnt, period, dn);

            //TA.Highest(price, cnt, period, up);
            //TA.Lowest(price, cnt, period, dn);

            //=========================
            //float[] macd = mBuffers[4];
            //TA.MACD(price, cnt, 10, 22, 9, macd, null, osc);
            //TA.EMA(macd, cnt, 3, osc);
            //TA.Diff(osc, 0, cnt, osc);
            //=========================


            //phosc = crossunder(change(osc),0)
            float[] changeOsc = buffers[2];
            TA.change(momentumSmoothed, cnt, 1, changeOsc);
            bool[] phosc = booleans[0];
            TA.crossUnder(changeOsc, cnt, 0, phosc);

            //plosc = crossover(change(osc),0)
            bool[] plosc = booleans[1];
            TA.crossOver(changeOsc, cnt, 0, plosc);

            //bear = osc > 0 and phosc and valuewhen(phosc,osc,0) < valuewhen(phosc,osc,1)
            //and valuewhen(phosc,up,0) > valuewhen(phosc,up,1) ? 1 : 0
            int[] offset1 = {0};
            int[] offset2 = {0};
            int[] offset3 = {0};

            int dateInt = Utils.CREATE_DATE(2021, 7, 5);
            //int timeInt = (10<<8)|59;
            //int date = Share.dateToPacked30m(2023, 9, 19, 9, 29);
            for (int i = 1; i < cnt; i++) {
                line[i] = 0;
            }

            bear[0] = false;
            for (int i = 1; i < cnt; i++){
                //if (Share.compare30mPackedDate(share.getDate(i), date) == 0){
                //if (dateInt == share.getDate(i)){
                    ////xUtils.trace("");
                //}
                if (i == 239){
                    //xUtils.trace("");
                }
                bear[i] = false;
                if (momentumSmoothed[i] > momentumeBaseValue && phosc[i]){
                    float valueWhen0_osc = TA.valueWhen(phosc, momentumSmoothed, i, 0, null, -1);
                    float valueWhen1_osc = TA.valueWhen(phosc, momentumSmoothed, i, 1, offset1, -1);
                    float valueWhen2_osc = TA.valueWhen(phosc, momentumSmoothed, i, 2, offset2, -1);
                    float valueWhen3_osc = TA.valueWhen(phosc, momentumSmoothed, i, 3, offset3, -1);

                    float valueWhen0_up = TA.valueWhen(phosc, up, i, 0, null, -1);
                    float valueWhen1_up = TA.valueWhen(phosc, up, i, 1, null, -1);
                    float valueWhen2_up = TA.valueWhen(phosc, up, i, 2, null, -1);
                    float valueWhen3_up = TA.valueWhen(phosc, up, i, 3, null, -1);
                    if (valueWhen0_osc < valueWhen1_osc && valueWhen0_up >= valueWhen1_up
                            && i - offset1[0] > 4){
                        bear[i] = true;
                        line[i] = offset1[0];
                    }
                    else if (valueWhen0_osc > valueWhen1_osc
                            && valueWhen0_osc < valueWhen2_osc
                            && i - offset2[0] > 10
                    )
                    {
                        if (valueWhen0_up >= valueWhen2_up) {
                            bear[i] = true;
                            line[i] = offset2[0];
                        }
                        else{
                            float lo = TA.LowestInRange(dn, i-30, i, cnt);
                            valueWhen0_up = valueWhen0_up + 2*(valueWhen2_up - lo)/100.0f;
                            if (valueWhen0_up >= valueWhen2_up && valueWhen0_osc < 1.2*valueWhen2_osc) {
                                bear[i] = true;
                                line[i] = offset2[0];
                            }
                        }
                    }
                    else if (valueWhen0_osc > valueWhen1_osc
                            && valueWhen0_osc > valueWhen2_osc
                            && valueWhen0_osc <= valueWhen3_osc
                            && valueWhen0_up >= valueWhen3_up
                            && i - offset3[0] < 50
                    ){
                        bear[i] = true;
                        line[i] = offset3[0];
                    }

                    //  a little process
                    if (line[i] > 0 && line[i] - 5 > 0){
                        int off1 = line[i];
                        int off2 = i;

                        int newOff1 = off1;
                        int newOff2 = off2;

                        for (int j = 1; j < 4; j++){
                            if (momentum[off1 - j] > momentum[newOff1]){
                                newOff1 = off1 - j;
                            }
                            if (momentum[off2 - j] > momentum[newOff2]){
                                newOff2 = off2 - j;
                            }
                        }
                        line[i] = (newOff1 << 16) | newOff2;
                    }
                }
            }

            //bull = osc < 0 and plosc and valuewhen(plosc,osc,0) > valuewhen(plosc,osc,1)
            //and valuewhen(plosc,dn,0) < valuewhen(plosc,dn,1) ? 1 : 0

            bull[0] = false;
            //dateInt = xUtils.CREATE_DATE(2022, 11, 16);

            for (int i = 1; i < cnt; i++){
                //if (i == 2335){
                    //xUtils.trace("");
                //}
                //if (Share.compare30mPackedDate(share.getDate(i), date) == 0){
                    //xUtils.trace("");
                //}
                if (dateInt == share.getDate(i)){
                    dateInt = 0;
                }

                bull[i] = false;
                if (momentumSmoothed[i] < momentumeBaseValue && plosc[i]){
                    float valueWhen0_osc = TA.valueWhen(plosc, momentumSmoothed, i, 0, null, -1);
                    float valueWhen1_osc = TA.valueWhen(plosc, momentumSmoothed, i, 1, offset1, -1);
                    float valueWhen2_osc = TA.valueWhen(plosc, momentumSmoothed, i, 2, offset2, -1);
                    float valueWhen3_osc = TA.valueWhen(plosc, momentumSmoothed, i, 3, offset3, -1);

                    float valueWhen0_dn = TA.valueWhen(plosc, dn, i, 0, null, -1);
                    float valueWhen1_dn = TA.valueWhen(plosc, dn, i, 1, null, -1);
                    float valueWhen2_dn = TA.valueWhen(plosc, dn, i, 2, null, -1);
                    float valueWhen3_dn = TA.valueWhen(plosc, dn, i, 3, null, -1);
                    if (valueWhen0_osc > valueWhen1_osc && valueWhen0_dn <= valueWhen1_dn
                            && i - offset1[0] > 4)
                    {
                        bull[i] = true;
                        line[i] = offset1[0];
                    }
                    else if (valueWhen0_osc < valueWhen1_osc
                            && valueWhen0_osc > valueWhen2_osc
                            && i - offset2[0] > 10
                    )
                    {
                        if (valueWhen0_dn <= valueWhen2_dn) {
                            bull[i] = true;
                            line[i] = offset2[0];
                        }
                        else{
                            float hi = TA.HighestInRange(dn, i-30, i, cnt);
                            valueWhen0_dn = valueWhen0_dn + 2*(valueWhen2_dn - hi)/100.0f;
                            if (valueWhen0_dn <= valueWhen2_dn && valueWhen0_osc > 1.2*valueWhen2_osc) {
                                bull[i] = true;
                                line[i] = offset2[0];
                            }
                        }
                    }
                    else if (valueWhen0_osc < valueWhen1_osc
                            && valueWhen0_osc < valueWhen2_osc
                            && valueWhen0_osc >= valueWhen3_osc
                            && valueWhen0_dn <= valueWhen3_dn
                            && i - offset3[0] < 50
                    )
                    {
                        bull[i] = true;
                        line[i] = offset3[0];
                    }

                    //  a little process
                    if (line[i] > 0 && line[i] - 4 > 0){
                        int off1 = line[i];
                        int off2 = i;

                        int newOff1 = off1;
                        int newOff2 = off2;

                        for (int j = 1; j < 4; j++){
                            if (momentum[off1 - j] < momentum[newOff1]){
                                newOff1 = off1 - j;
                            }
                            if (momentum[off2 - j] < momentum[newOff2]){
                                newOff2 = off2 - j;
                            }
                        }
                        line[i] = (newOff1 << 16) | newOff2;
                    }
                }
            }

            bull[0] = false;    //  just nothing
        }
    }
}
