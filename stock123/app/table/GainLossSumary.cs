using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using xlib.framework;
using xlib.ui;
using xlib.utils;

using stock123.app.data;

namespace stock123.app.table
{
    class GainLossSumary: xContainer
    {
        public GainLossSumary(int w):base(null)
        {
            createControl(w);
        }

        override
        public void invalidate(){
            removeAllControls();
            createControl(getW());
        }

        void createControl(int w)
        {
            xLabel l;
            int y = 0;
            xFillBackground bottom = xFillBackground.createFill(w, 1, C.COLOR_GRAY_LIGHT);
            bottom.setPosition(0, y);
            addControl(bottom);
            y += 1;

            int xPivot = w / 2 + 10;// - xApplication.point2Pixels(120);

            double[] values = { 0, 0, 0 };
            Context.getInstance().getGainLossValue(values);

            String sTmp = Utils.formatNumberDouble(values[0]);
            //	total
            l = xLabel.createSingleLabel("Tổng vốn đầu tư: ");
            l.setTextColor(C.COLOR_WHITE);
            l.setPosition(xPivot - l.getW() - 4, y);
           
            addControl(l);

            l = xLabel.createSingleLabel(sTmp);
            l.setTextColor(C.COLOR_WHITE);
            l.setPosition(xPivot, y);
            l.setSize(120, l.getH());
            addControl(l);

            // gia tri
            y += l.getH() + 4;
            l = xLabel.createSingleLabel("Tổng giá trị cổ phiếu: ");
            l.setTextColor(C.COLOR_WHITE);
            l.setPosition(xPivot - l.getW() - 4, y);
            addControl(l);

            sTmp = Utils.formatNumberDouble(values[1]);
            l = xLabel.createSingleLabel(sTmp);
            l.setSize(120, l.getH());
            l.setTextColor(C.COLOR_WHITE);
            l.setPosition(xPivot, y);
            addControl(l);

            //	loi nhuan
            y += l.getH() + 4;
            l = xLabel.createSingleLabel("Tổng lợi nhuận: ");
            l.setTextColor(C.COLOR_WHITE);
            l.setPosition(xPivot - l.getW() - 4, y);
            addControl(l);

            sTmp = Utils.formatNumberDouble(values[2]);

            //  percent
            if (values[0] > 0)
            {
                double percent = (values[1] - values[0]) / values[0];
                percent *= 100;

                String sTmp2 = String.Format("{0} ({1:F1}%)", sTmp, percent);
                l = xLabel.createSingleLabel(sTmp2);
            }
            else
            {
                l = xLabel.createSingleLabel(sTmp);
            }
            if (values[2] > 0)
                l.setTextColor(C.COLOR_GREEN);
            else if (values[2] < 0)
                l.setTextColor(C.COLOR_RED);
            else
                l.setTextColor(C.COLOR_WHITE);
            l.setPosition(xPivot, y);
            l.setSize(120, l.getH());
            addControl(l);

            y += l.getH() + 10;

            setSize(w, y);
        }
    }
}
