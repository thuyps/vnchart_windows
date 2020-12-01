using System;
using System.Collections.Generic;
using System.Text;
using xlib.framework;

namespace stock123.app.data
{
    public class AlarmManager
    {
        public const int FILE_ALARM_SIGNAL = 0x010a0b0c;
        public xVector mAlarms = new xVector();
        Context mContext;

        public AlarmManager()
        {
            mContext = Context.getInstance();
        }

        public void loadAlarms()
        {
            xDataInput di = xFileManager.readFile(Context.FILE_ALARM, false);

            if (di == null)
                return;

            int ver = 0;
            if (di != null)
                ver = di.readInt();
            if (ver >= Context.FILE_VERSION_33)
            {
                int cnt = di.readInt();
                for (int i = 0; i < cnt; i++)
                {
                    stAlarm a = new stAlarm();
                    a.code = di.readUTF();
                    a.date = di.readInt();
                    a.lowerPrice = di.readInt();
                    a.upperPrice = di.readInt();

                    a.comment = di.readUTF();

                    mAlarms.addElement(a);
                }
            }
        }

        public void saveAlarms()
        {
            xDataOutput o = new xDataOutput(2048);

            o.writeInt(Context.FILE_VERSION_LATEST);

            int cnt = mAlarms.size();
            o.writeInt(cnt);
            for (int i = 0; i < cnt; i++)
            {
                stAlarm a = (stAlarm)mAlarms.elementAt(i);
                o.writeUTF(a.code);
                o.writeInt(a.date);
                o.writeInt(a.lowerPrice);
                o.writeInt(a.upperPrice);

                o.writeUTF(a.comment);
            }

            xFileManager.saveFile(o, Context.FILE_ALARM);
        }

        public stAlarm getAlarm(String code)
        {
            int cnt = mAlarms.size();
            for (int i = 0; i < cnt; i++)
            {
                stAlarm a = (stAlarm)mAlarms.elementAt(i);
                if (a.code.CompareTo(code) == 0)
                {
                    return a;
                }
            }

            return null;
        }

        public void removeAlarm(stAlarm a)
        {
            mAlarms.removeElement(a);
        }

        public int getAlarmCount()
        {
            return mAlarms.size();
        }
        public stAlarm getAlarmAt(int idx)
        {
            if (idx >= 0 && idx < mAlarms.size())
            {
                return (stAlarm)mAlarms.elementAt(idx);
            }
            return null;
        }

        public void addAlarm(stAlarm a)
        {
            mAlarms.addElement(a);
        }

        public bool hasAlarm()
        {
            int cnt = getAlarmCount();
            for (int i = 0; i < cnt; i++)
            {
                stAlarm a = getAlarmAt(i);
                if (a.hasAlarm() != 0)
                    return true;
            }

            return false;
        }

        public bool hasTriggerredAlarm()
        {
            int cnt = getAlarmCount();
            for (int i = 0; i < cnt; i++)
            {
                stAlarm a = getAlarmAt(i);
                if (a.hasTriggerredAlarm())
                    return true;
            }

            return false;
        }

        public bool isAlarmInstalled(String code)
        {
            int cnt = getAlarmCount();
            for (int i = 0; i < cnt; i++)
            {
                stAlarm a = getAlarmAt(i);
                if (a.code.CompareTo(code) == 0)
                {
                    if (a.lowerPrice > 0 || a.upperPrice > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void clearAll()
        {
            mAlarms.removeAllElements();
        }
    }
}
