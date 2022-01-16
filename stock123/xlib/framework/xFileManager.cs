using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace xlib.framework
{
    public class xFileManager
    {
        static public string RootDir;   //  E:\data\
        static public string UserDir;   //  E:\data\
        static public void saveFile(byte[] data, int offset, int len, String filename)
        {
            try
            {
                FileStream fs = File.OpenWrite(UserDir + filename);// new FileStream(UserDir + filename, FileMode.Create);
                if (fs != null)
                {
                    BinaryWriter bw = new BinaryWriter(fs);
                    if (bw != null)
                    {
                        bw.Write(data, offset, len);

                        bw.Close();
                    }
                    fs.Close();
                }
            }
            catch (Exception e)
            {
            }
        }
        static public void saveFile(xDataOutput o, String filename)
        {
            if (o != null)
            {
                saveFile(o.getBytes(), 0, o.size(), filename);
            }
        }

        static public xDataInput readFile(String filename, bool resource)
        {
            if (filename == null || filename.Contains("*"))
            {
                return null;
            }
            xDataInput di = null;
            try
            {
                //System.Windows.Forms.MessageBox.Show("readfile1");
                string root = resource ? RootDir : UserDir;
                //System.Windows.Forms.MessageBox.Show("readfile2: root=" + root + " file=" + filename);
                FileStream fs = File.OpenRead(root + filename);// new FileStream(root + filename, FileMode.Open);
                //System.Windows.Forms.MessageBox.Show("readfile3");
                if (fs != null)
                {
                    //System.Windows.Forms.MessageBox.Show("readfile4");
                    BinaryReader br = new BinaryReader(fs);
                    //System.Windows.Forms.MessageBox.Show("readfile5");
                    if (br != null)
                    {
                        //System.Windows.Forms.MessageBox.Show("readfile6");
                        long size = fs.Length;
                        if (size > 0)
                        {
                            byte[] p = new byte[size];
                            br.Read(p, 0, (int)size);

                            di = new xDataInput(p, 0, (int)size, false);
                        }
                        //System.Windows.Forms.MessageBox.Show("readfile7");
                        br.Close();
                        //System.Windows.Forms.MessageBox.Show("readfile8");
                    }

                    fs.Close();
                    //System.Windows.Forms.MessageBox.Show("readfile8");
                }
            }
            catch (IOException e)
            {
                //System.Windows.Forms.MessageBox.Show("readfile: " + e.ToString());
            }
            catch (System.Exception e1)
            {
            }
            finally
            {

            }
            return di;
        }
        public static void removeFile(String file)
        {
            try
            {
                System.IO.File.Delete(UserDir + file);
            }
            catch (IOException e)
            {
            }
        }

        static public bool isFileExist(String file)
        {
            try
            {
                return System.IO.File.Exists(UserDir + file);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
