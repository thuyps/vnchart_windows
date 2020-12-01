using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using xlib.framework;
using xlib.ui;

namespace xlib.ui
{
    public class xPicture: xBaseControl
    {
        public const int PICTURE_TYPE_NORMAL = 0;
        public const int PICTURE_TYPE_ZOOM = 1;
        public const int PICTURE_TYPE_HORIZONTAL = 2;
        public const int PICTURE_TYPE_VERTICAL = 3;

        ImageList mImageList;
        protected int mImgIndex;
        Image mImage;
        int mType;

        public xPicture(Image img)
            : base(null)
        {
            mImage = img;
            mType = PICTURE_TYPE_NORMAL;

            makeCustomRender(false);

            setSize(img.Width, img.Height);
        }

        public xPicture(Image img, int type)
            : base(null)
        {
            makeCustomRender(false);

            mImage = img;
            mType = type;
            setSize(img.Width, img.Height);
        }

        public xPicture(ImageList imglist, int imgIdx): base(null)
        {
            mType = PICTURE_TYPE_NORMAL;
            mImageList = imglist;
            mImgIndex = imgIdx;
            makeCustomRender(false);

            setSize(imglist.ImageSize.Width, imglist.ImageSize.Height);
        }

        public void setType(int pictype)
        {
            mType = pictype;
        }

        public override void render(xGraphics g)
        {
            if (mType == PICTURE_TYPE_NORMAL)
            {
                if (mImage != null)
                {
                    g.drawImage(mImage, 0, 0);
                }
                else if (mImageList != null)
                {
                    g.drawImage(mImageList.Images[mImgIndex], 0, 0);
                }
            }
            else
            {
                g.drawImage(mImage, 0, 0);
            }
        }
    }
}
