using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xlib.framework;

namespace stock123.app.data.userdata
{
    public class UserDataBlock
    {
        public const int DATA_MAX = 8000;
        public int blockSize;
        public int id;

        public xDataInput dataInput;

        public static UserDataBlock loadBlock(xDataInput di){
            int blockSize = di.readInt();
            int id = di.readInt();
            int dataSize = 0;
            byte[] data = null;
            if (blockSize > 4){
                dataSize = blockSize - 4;
                if (dataSize > DATA_MAX){
                    dataSize = DATA_MAX;
                }
                data = new byte[dataSize];
                di.read(data, 0, dataSize);
            }

            return new UserDataBlock(id, data, 0, dataSize);
        }
        public UserDataBlock(int id, byte[] data, int offset, int size){
            this.id = id;
            blockSize = size + 4;
            if (size > 0 && data != null) {
                dataInput = new xDataInput(data, offset, size, true);
            }
            else{
                dataInput = new xDataInput(1);
            }
        }

        static public void saveBlock(int id, xDataOutput data, xDataOutput o)
        {
            if (data == null)
            {
                return;
            }
            int blockSize = 4 + data.size();

            if (blockSize + o.size() < DATA_MAX) {
                o.writeInt(blockSize);
                o.writeInt(id);
                o.write(data.getBytes(), 0, data.size());
            }
        }
        static public void saveBlock(int id, xDataInput data, xDataOutput o)
        {
            if (data == null){
                return;
            }
            int blockSize = 4 + data.size();

            if (blockSize + o.size() < DATA_MAX) {
                o.writeInt(blockSize);
                o.writeInt(id);
                o.write(data.getBytes(), 0, data.size());
            }
        }
    }
}
