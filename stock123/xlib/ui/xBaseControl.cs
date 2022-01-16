using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using xlib.framework;

namespace xlib.ui
{
    public class xBaseControl
    {
        public delegate void OnMenuItemClick(int id, string title);
        public OnMenuItemClick onMenuItemClick;

        public const int EVT_BUTTON_CLICKED = 100;
        public const int EVT_ON_MOUSE_DOWN = 101;
        public const int EVT_ON_MOUSE_MOVE = 102;
        public const int EVT_ON_MOUSE_UP = 103;
        public const int EVT_ON_CONTEXT_MENU = 104;
        public const int EVT_ON_SPLITTER_SIZE_CHANGED = 105;

        public const int EVT_SLIDER_CHANGE_VALUE = 106;
        public const int EVT_CHECKBOX_VALUE_CHANGED = 107;

        public const int EVT_ON_MOUSE_DOUBLE_CLICK = 108;

        public const int EVT_ON_ROW_SELECTED = 120;
        public const int EVT_ON_ROW_DOUBLE_CLICK = 121;

        public const int EVT_CLOSE_DIALOG = 126;

        public const int EVT_NET_CONNECTING = 500;
        public const int EVT_NET_DATA_AVAILABLE = 501;
        public const int EVT_NET_DONE = 502;  //  with all data
        public const int EVT_NET_CONNECTED = 503;
        public const int EVT_NET_ERROR = 504;
        //-----------------------------------------------------
        protected Control mControl;
        protected xIEventListener mListener;
        protected bool mIsShow = true;
        protected int mID;
        protected int mDataInt;
        protected object mData;
        //-------------------------------------------
        protected bool mDoubleBuffer;
        protected bool mIsCustomRenderControl;

        //=================================
        public xBaseControl(xIEventListener listener)
        {
            mControl = null;
            mListener = listener;
        }

        public void setControl(Control c)
        {
            mControl = c;
            mControl.Tag = this;

            System.Threading.ThreadPool.QueueUserWorkItem(delegate
            {
                mControl.LostFocus += new EventHandler(mControl_LostFocus);
            }, null);

        }

        public Control getControl()
        {
            return mControl;
        }
        //==================mouse events=====================
        virtual public void onMouseDown(int x, int y)
        {
        }
        virtual public void onMouseMove(int x, int y)
        {
        }

        virtual public void onMouseUp(int x, int y)
        {
        }

        virtual public void onMouseDoubleClick()
        {
        }

        virtual public void onMouseDoubleClick(int x, int y)
        {
        }

        virtual public void preRender()
        {
        }

        virtual public void render(xGraphics g)
        {
        }

        //==================shwo & hide=======================
        virtual public void show(bool isShow)
        {
            if (isShow)
            {
                mControl.Show();
            }
            else
            {
                mControl.Hide();
            }
            mIsShow = isShow;
        }

        public bool isShow()
        {
            return mIsShow;
        }

        //==================position & size========================
        public void setPosition(int x, int y)
        {
            mControl.Location = new Point(x, y);
        }

        public void centerPosition(int w, int h)
        {
            int ox = mControl.Location.X;
            int oy = mControl.Location.Y;
            if (w != -1)
            {
                ox = (w-mControl.Size.Width)/2;
            }
            if (h != -1)
            {
                oy = (h - mControl.Size.Height) / 2;
            }
            mControl.Location = new Point(ox, oy);
        }

        public virtual void setSize(int w, int h)
        {
            int nw = mControl.Size.Width;
            int nh = mControl.Size.Height;

            if (nw == w && nh == h)
            {
                return;
            }

            if (mIsCustomRenderControl)
            {
                xCanvas c = (xCanvas)getControl();
                c.recalcDoubleBuffer();
            }

            if (w != -1)
            {
                nw = w;
            }
            if (h != -1)
            {
                nh = h;
            }
            mControl.Size = new Size(nw, nh);
        }
        public virtual void setSize(xBaseControl c)
        {
            setSize(c.getW(), c.getH());
        }
        public int getX()
        {
            return mControl.Location.X;
        }
        public int getY()
        {
            return mControl.Location.Y;
        }
        public int getW()
        {
            return mControl.Size.Width;
        }

        public int getH()
        {
            return mControl.Size.Height;
        }

        public int getRight()
        {
            return getX() + getW();
        }
        public int getBottom()
        {
            return getY() + getH();
        }
        //============================================================

        public void setOpaque(float v)
        {
            
        }
        //==================identical & data======================
        public void setID(int _id)
        {
            mID = _id;
        }

        public int getID()
        {
            return mID;
        }

        virtual public void invalidate()
        {
            if (mControl != null)
                mControl.Invalidate();
        }

        public void setDataInt(int v)
        {
            mDataInt = v;
        }

        public int getDataInt()
        {
            return mDataInt;
        }

        public void setData(object data)
        {
            mData = data;
        }

        public object getData()
        {
            return mData;
        }

        public object getUserData()
        {
            return getData();
        }
        //===============================================
        public void setText(string text)
        {
            getControl().Text = text;
        }

        public void setFont(Font f)
        {
            getControl().Font = f;
        }

        public void setTextColor(int argb)
        {
            getControl().ForeColor = Color.FromArgb(argb);
        }
        public void setTextColor(uint argb)
        {
            getControl().ForeColor = Color.FromArgb((int)argb);
        }
        public void setTextColor(Color color)
        {
            getControl().ForeColor = color;
        }

        public void setBackgroundColor(int color)
        {
            getControl().BackColor = Color.FromArgb(color);
        }

        public void setBackgroundColor(uint color)
        {
            getControl().BackColor = Color.FromArgb((int)color);
        }

        public void makeCustomRenderAndTransparent()
        {
            mIsCustomRenderControl = true;
            xCanvas c = new xCanvas(false, true);
            c.setxBaseControl(this);

            setControl(c);
        }

        public void makeCustomRender(bool doubleBuffer)
        {
            mIsCustomRenderControl = true;
            xCanvas c = new xCanvas(doubleBuffer);
            c.setxBaseControl(this);

            setControl(c);

            mDoubleBuffer = doubleBuffer;
        }

        virtual public void onTick()
        {
        }

        public void setMenuContext(ContextMenu menu)
        {
            getControl().ContextMenu = menu;
        }

        public void setMenuContext(int[] ids, String[] texts, int cnt)
        {
            getControl().ContextMenu = createMenuContext(ids, texts, cnt);
        }

        public ContextMenu createMenuContext(int[] ids, String[] texts, int cnt)
        {
            ContextMenu menu = new ContextMenu();

            for (int i = 0; i < cnt; i++)
            {
                if (ids[i] == -1)
                {
                    menu.MenuItems.Add(new MenuItem("-"));
                }
                else
                {
                    MenuItem item = new MenuItem(texts[i]);
                    if (ids[i] > 0)
                    {
                        item.Tag = ids[i];
                        System.Threading.ThreadPool.QueueUserWorkItem(delegate
                        {
                            item.Click += new EventHandler(onMenuClick);
                        }, null);

                    }

                    menu.MenuItems.Add(item);
                }
            }

            return menu;
        }

        public ContextMenu createMenuContext(int[] ids, String[] texts, int cnt, EventHandler onClick)
        {
            ContextMenu menu = new ContextMenu();
            for (int i = 0; i < cnt; i++)
            {
                if (ids[i] == -1)
                {
                    menu.MenuItems.Add(new MenuItem("-"));
                }
                else
                {
                    MenuItem item = new MenuItem(texts[i]);
                    item.Tag = ids[i];
                    item.Click += onClick;

                    menu.MenuItems.Add(item);
                }
            }

            return menu;
        }

        public void onMenuClick(object sender, EventArgs e)
        {
            try
            {
                MenuItem item = (MenuItem)sender;
                if (!onMenuEvent(item))
                {
                    if (mListener != null)
                    {
                        mListener.onEvent(this, xBaseControl.EVT_ON_CONTEXT_MENU, (int)item.Tag, null);
                    }
                }
            }
            catch(Exception ex)
            {
            }
        }

        virtual public bool onMenuEvent(MenuItem item)
        {
            if (onMenuItemClick != null)
            {
                onMenuItemClick((int)item.Tag, item.Text);
                return true;
            }
            return false;
        }

        public void setListener(xIEventListener listener)
        {
            mListener = listener;
        }

        virtual public void onKeyDown(int key)
        {
        }

        virtual public void onKeyPress(int key)
        {
        }

        public bool isKeyPressing(System.Windows.Forms.Keys key)
        {
            return ((Control.ModifierKeys & key) != 0);
        }

        public bool isFocus()
        {
            return getControl().Focused;
        }

        public void setFocus()
        {
            getControl().Focus();
        }

        void mControl_LostFocus(object sender, EventArgs e)
        {
            onLostFocus();
        }

        virtual public void onLostFocus()
        {
        }

        virtual public void dispose()
        {
        }
    }
}

