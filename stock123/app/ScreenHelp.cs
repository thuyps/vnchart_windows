using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using xlib.framework;
using xlib.ui;

namespace stock123.app
{
    class ScreenHelp: ScreenBase
    {
        public ScreenHelp(): base()
        {

        }

        public override void onActivate()
        {
            base.onActivate();

            xLabel l = xLabel.createSingleLabel("This is Help screen", null, getW());
            addControl(l);

            xButton bt = xButton.createStandardButton(1000, this, "back to Home screen", 250);
            bt.setPosition(150, 0);
//            bt.setSize(-1, bt.getH()*2);

            Font f = new Font(new FontFamily("Arial"), 13.0f);
            bt.setFont(f);
            bt.setTextColor(Color.Red.ToArgb());

            addControl(bt);

            //====================================try list view=======
            xContainer c = new xContainer();
            c.setSize(235, 200);
            c.setPosition(0, 40);
            addControl(c);
            /*
            String[] columns = {"CODE", "PRICE", "+/-"};
            float[] ws = { 50, 100, 80};
            //  create table
            xListView lv = xListView.createListView(this, columns, ws, null);
            for (int i = 0; i < 4; i++)
            {
                columns[0] = "CODE" + i;
                columns[1] = "" + (100 * (5-i));
                columns[2] = "" + (i * i);
                //  create row
                xListViewItem item = xListViewItem.createListViewItem(this, 3);//, columns);
                for (int j = 0; j < 3; j++)
                {
                    item.setTextForCell(j, columns[j]);
                }

                lv.addRow(item);
                item.setTextColor(1, 0xffff0000);
            }

            lv.setSize(230, 100);
            lv.setPosition(100, 100);

            c.addControl(lv);
             */
        }

        public override void onEvent(object sender, int evt, int aIntParameter, object aParameter)
        {
            if (evt == xBaseControl.EVT_BUTTON_CLICKED)
            {
                if (aIntParameter == 1000)
                {
                    goNextScreen(MainApplication.SCREEN_HOME);
                }
            }
        }
    }
}
