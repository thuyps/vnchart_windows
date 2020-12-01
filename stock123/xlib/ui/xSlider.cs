using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using xlib.framework;
using xlib.utils;

namespace xlib.ui
{
    public class xSlider: xBaseControl
    {
        xFloat mValue;
        int mValueZoom;
        xLabel mValueLabel;
        public xSlider(xFloat o, xIEventListener listener)
            : base(listener)
        {
            TrackBar tb = new TrackBar();
            setControl(tb);

            tb.Orientation = Orientation.Horizontal;
            tb.TickStyle = TickStyle.None;

            tb.ValueChanged += new EventHandler(tb_ValueChanged);

            mValueZoom = 1;

            mValue = o;
        }

        public void setValueLabel(xLabel l)
        {
            mValueLabel = l;

            if (mValueLabel != null)
            {
                //  update label
                if (mValueZoom == 1)
                {
                    mValueLabel.setText("" + ((int)mValue.Value));
                }
                else
                {
                    StringBuilder sb = utils.Utils.sb;
                    sb.Length = 0;
                    if (mValueZoom == 10)
                    {
                        sb.AppendFormat("{0:F1}", mValue.Value);
                    }
                    else if (mValueZoom == 100)
                    {
                        sb.AppendFormat("{0:F2}", mValue.Value);
                    }
                    else
                    {
                        sb.AppendFormat("{0:F3}", mValue.Value);
                    }
                    mValueLabel.setText(sb.ToString());
                }
            }
        }

        void tb_ValueChanged(object sender, EventArgs e)
        {
            TrackBar tb = (TrackBar)getControl();

            mValue.Value = (float)(tb.Value)/mValueZoom;

            setValueLabel(mValueLabel);

            if (mListener != null)
            {
                mListener.onEvent(this, xBaseControl.EVT_SLIDER_CHANGE_VALUE, 0, null);
            }
        }
        //  zoomValue: 1, 10, 100
        static public xSlider createSlider(float min, float max, float step, xFloat o, int zooomValue, xLabel valueLabel)
        {
            xSlider slider = new xSlider(o, null);
            slider.setSliderRange(min, max, o.Value, step, zooomValue);
            slider.setValueLabel(valueLabel);

            return slider;
        }

        public void setValue(float v)
        {
            TrackBar tb = (TrackBar)getControl();
            tb.Value = (int)v * mValueZoom;
        }

        public void setSliderRange(float min, float max, float defaultCursor, float step, int zooomValue)
        {
            if (defaultCursor < min || defaultCursor > max)
                defaultCursor = min + (max - min) / 2;
            TrackBar tb = (TrackBar)getControl();
            mValueZoom = zooomValue;
            tb.Minimum = (int)(mValueZoom * min);
            tb.Maximum = (int)(mValueZoom * max);
            tb.TickFrequency = (int)(mValueZoom * step);
            tb.Value = (int)(mValueZoom * defaultCursor);
        }
    }
}
