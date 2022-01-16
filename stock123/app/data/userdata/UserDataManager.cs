using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xlib.framework;
using xlib.utils;
using stock123.app.data;

namespace stock123.app.data.userdata
{
    public class UserDataManager
    {
        public const int USER_DATA_SIGNATURE = 0x01020304;
        public const int DATA_SHARE_GROUPS = 1;
        public const int DATA_GAINLOSS = 2;
        public const int DATA_FILTERS_ANDROID = 3;
        public const int DATA_FILTERS_WINDOWS = 5;
        public const int DATA_FILTERS_WINDOWS_NEW = 6;
        public const int DATA_DRAWING = 4;
        public const int DATA_BLOCK_COUNT = 4;

        bool usingFilterWindowsNew = false;
        xVector _shareGroups;
        GainLossManager _gainLossManager;

        xVector _unknownBlocks;

        public GainLossManager gainLossManager(){
            if (_gainLossManager == null){
                _gainLossManager = new GainLossManager();
            }
            return _gainLossManager;
        }
        public xVector shareGroups(){
            if (_shareGroups == null){
                _shareGroups = new xVector();
            }
            return _shareGroups;
        }

        //  structure:ver |blockcnt| blocksize:id|data | blocksize:id|data
        public void loadUserData(xDataInput di){
            int signal = di.readInt();

            if (signal != USER_DATA_SIGNATURE){
                return; //  bad file
            }

            int blockCount = di.readInt();
            _unknownBlocks = new xVector();

            if (blockCount < 16){
                for (int i = 0; i < blockCount; i++){
                    UserDataBlock block = UserDataBlock.loadBlock(di);
                    if (block.id == DATA_SHARE_GROUPS){
                        loadShareGroup(block.dataInput);
                    }
                    else if (block.id == DATA_GAINLOSS){
                        loadGainload(block.dataInput);
                    }
                    else if (block.id == DATA_FILTERS_WINDOWS){
                        if (usingFilterWindowsNew)
                        {
                            continue;   //  old version, discard this block
                        }
                        bool ok = loadFilters(block.dataInput);
                        if (!ok)
                        {
                            _unknownBlocks.addElement(block);
                        }
                    }
                    else if (block.id == DATA_FILTERS_WINDOWS_NEW)
                    {
                        usingFilterWindowsNew = true;
                        loadFilters(block.dataInput);
                    }
                    else if (block.id == DATA_DRAWING){
                        loadDrawings(block.dataInput);
                    }
                    else{
                        _unknownBlocks.addElement(block);
                    }
                }
            }

        }

        public xDataOutput getUserDataAsStreamNew() {
            xDataOutput o = new xDataOutput(8 * 1024);

            o.writeInt(USER_DATA_SIGNATURE);

            int blockCnt = DATA_BLOCK_COUNT;   //  shares + filters + drawing
            if (_unknownBlocks != null) {
                blockCnt += _unknownBlocks.size();
            }
            o.writeInt(blockCnt);

            xDataOutput blockData = shareGroupsBlock();
            xDataOutput blockGainloss = gainlossBlock();
            xDataOutput blockFilters = filtersBlock();
            xDataOutput blockDrawings = drawingBlock();

            UserDataBlock.saveBlock(DATA_SHARE_GROUPS, blockData, o);
            UserDataBlock.saveBlock(DATA_GAINLOSS, blockGainloss, o);
            UserDataBlock.saveBlock(DATA_FILTERS_WINDOWS_NEW, blockFilters, o);
            UserDataBlock.saveBlock(DATA_DRAWING, blockDrawings, o);

            if (_unknownBlocks != null) {
                for (int i = 0; i < _unknownBlocks.size(); i++) {
                    UserDataBlock block = (UserDataBlock) _unknownBlocks.elementAt(i);
                    UserDataBlock.saveBlock(block.id, block.dataInput, o);
                }
            }

            return o;
        }

        //==========================================
        void loadShareGroup(xDataInput di)
        {
            int cnt = di.readInt();
            if (cnt > stShareGroup.MAX_SHARES_IN_GROUP){
                cnt = stShareGroup.MAX_SHARES_IN_GROUP;
            }
            xVector v = shareGroups();
            if (cnt > 0){
                v.removeAllElements();
            }
            for (int i = 0; i < cnt; i++)
            {
                stShareGroup g = new stShareGroup();
                g.load(di);

                if (g.getType() == stShareGroup.ID_GROUP_GAINLOSS || g.getName() == null || g.getName().Length == 0)
                {
                    continue;
                }
                v.addElement(g);
            }
        }
        xDataOutput shareGroupsBlock(){
            xDataOutput o = new xDataOutput(5 * 1024);

            xVector v = new xVector();
            for (int i = 0; i < _shareGroups.size(); i++)
            {
                stShareGroup p = (stShareGroup)_shareGroups.elementAt(i);

                if (p.getType() == stShareGroup.ID_GROUP_GAINLOSS || p.getName() == null || p.getName().Length == 0)
                {
                    continue;
                }

                v.addElement(p);
            }
            //

            int groupCnt = v.size() < 30? v.size():30;

            o.writeInt(groupCnt);

            for (int i = 0; i < groupCnt; i++)
            {
                stShareGroup p = (stShareGroup) v.elementAt(i);

                if (p.getType() == stShareGroup.ID_GROUP_GAINLOSS || p.getName() == null || p.getName().Length == 0)
                {
                    continue;
                }

                p.save(o);
            }
            return o;
        }

        //=================================
        xDataOutput gainlossBlock(){
            xDataOutput o = new xDataOutput(2*1024);

            o.writeInt(_gainLossManager.getTotal());
            for (int i = 0; i < _gainLossManager.getTotal(); i++)
            {
                stGainloss g = _gainLossManager.getGainLossAt(i);

                o.writeUTF(g.code);
                o.writeInt(g.date);
                o.writeInt((int)(g.price*1000));
                o.writeInt(g.volume);
            }

            return o;
        }
        void loadGainload(xDataInput di){
            int cnt = di.readInt();
            if (cnt > 50){
                cnt = 50;
            }
            GainLossManager m = gainLossManager();
            if (cnt > 0){
                m.clearAll();
            }
            for (int i = 0; i < cnt; i++){
                String code = di.readUTF();
                int date = di.readInt();
                float price = di.readInt()/1000.0f;
                int vol = di.readInt();

                if (date == 0){
                    date = Utils.getDateAsInt();
                    GlobalData.vars().setValueBool("should_flush_userdata", true);
                }

                if (code != null && code.Length >= 3&& code.Length < 15) {
                    m.addGainLoss(code, date, price, vol);
                }
            }
            m.sortList();
        }
        //==========================================

        xDataOutput filtersBlock(){
            xDataOutput o = new xDataOutput(1024);

            FilterManager.getInstance().saveFilterSets(o);

            return o;
        }
        bool loadFilters(xDataInput di){
            return FilterManager.getInstance().loadFilterSets(di);
        }
        //==========================================
        xDataOutput drawingBlock(){
            return new xDataOutput(1);
        }
        void loadDrawings(xDataInput di){

        }
        //===========================================
        public const int FILE_VERSION_FAVORITE = 5;
        public const int FILE_ONLINE_USERDATA_VER = 1;
        public const int FILE_GAINLOSS_SIGNAL    = 0x000a0b0c;
        public const int FILE_FILTER_SET_VERSION = 0x000a0b0e;
        public void loadUserDataOldVersion(xDataInput di){
            int ver = 0;
            if (di != null) {
                ver = di.readInt();
            }

            xVector mFavorGroups = shareGroups();
            GainLossManager mGainLossManager = gainLossManager();

            if (di != null && (ver == FILE_VERSION_FAVORITE
                    || ver == FILE_ONLINE_USERDATA_VER)) {
                mFavorGroups.removeAllElements();
                mGainLossManager.clearAll();

                int cnt = di.readInt();

                for (int i = 0; i < cnt; i++) {
                    stShareGroup g = new stShareGroup();

                    g.load(di);
                    //g.setType(stShareGroup::GROUP_TYPE_FAVORITE);

                     mFavorGroups.addElement(g);
                }

                /*
                //  sort
                Collections.sort(mFavorGroups, new Comparator<stShareGroup>() {
                    @Override
                    public int compare(stShareGroup o1, stShareGroup o2) {
                        return o1.getName().compareToIgnoreCase(o2.getName());
                    }
                });
    */

                //======this version does not support gainloss======
                if (di.available() > 0) {
                    ver = di.readInt();
                    if (ver == FILE_GAINLOSS_SIGNAL) {
                        cnt = di.readInt();
                        mGainLossManager.clearAll();
                        for (int i = 0; i < cnt; i++) {
                            String code = di.readUTF();
                            int date = di.readInt();
                            int price = di.readInt();
                            int vol = di.readInt();

                            if (date == 0){
                                GlobalData.vars().setValueBool("should_flush_userdata", true);
                            }

                            if (code != null && code.Length >= 3) {
                                mGainLossManager.addGainLoss(code, date, price, vol);
                            }
                        }
                        mGainLossManager.sortList();

                        if (mGainLossManager.getTotal() > 0) {
                            mGainLossManager.save();
                        }
                    }
                    ver = di.readInt();
                    if (ver == FILE_FILTER_SET_VERSION) {
                        FilterManager.getInstance().loadFilterSets(di);
                    }
                }
            }
        }

        public xDataOutput getUserDataAsStreamOldVersion()
        {
            xDataOutput o = new xDataOutput(5 * 1024);

            o.writeInt(FILE_ONLINE_USERDATA_VER);
            int i;

            xVector v = new xVector(10);
            xVector mFavorGroups = shareGroups();
            GainLossManager mGainLossManager = gainLossManager();

            int groupCnt = mFavorGroups.size() < 30?mFavorGroups.size():30;
            for (i = 0; i < groupCnt; i++)
            {
                stShareGroup p = (stShareGroup)mFavorGroups.elementAt(i);

                bool added = false;
                for (int j = 0; j < v.size(); j++){
                    stShareGroup o1 = (stShareGroup)v.elementAt(j);
                    if (o1.getName().CompareTo(p.getName()) == 0)
                    {
                        added = true;
                        break;
                    }
                }

                if (added == false){
                    v.addElement(p);
                }
            }

            o.writeInt(v.size());

            for (i = 0; i < v.size(); i++)
            {
                stShareGroup p = (stShareGroup)v.elementAt(i);

                p.save(o);
            }

            //  gainloss com.data
            o.writeInt(FILE_GAINLOSS_SIGNAL);
            o.writeInt(mGainLossManager.getTotal());
            for (i = 0; i < mGainLossManager.getTotal(); i++)
            {
                stGainloss g = mGainLossManager.getGainLossAt(i);

                o.writeUTF(g.code);
                o.writeInt(g.date);
                o.writeInt((int)g.price);
                o.writeInt(g.volume);
            }

            //  filter sets
            o.writeInt(FILE_FILTER_SET_VERSION);
            FilterManager.getInstance().saveFilterSets(o);

            //  down signal group
            //o.writeInt(FILE_DOWN_SIGNAL);
            //mAlarmGroup.save(o);

            return o;
        }

        public void onTick()
        {
            if (_flushRequestCount == 0){
                return;
            }
            long now = Utils.currentTimeMillis();

            if (_flushRequestCount > 5 || now - _timeFlushUserData > 10000)
            {
                forceFlushUserData();
            }
        }

        long _timeFlushUserData = 0;
        int _flushRequestCount = 0;
        public void flushUserData()
        {
            _flushRequestCount++;
        }

        public void forceFlushUserData()
        {
            if (_flushRequestCount == 0)
            {
                return;
            }

            app.net.NetProtocol net = Context.getInstance().createNetProtocol();
            xDataOutput o = getUserDataAsStreamNew();
            net.requestSaveUserData2(o);
            net.flushRequest();

            _flushRequestCount = 0;
            _timeFlushUserData = Utils.currentTimeMillis();
        }
        //================================================================
    }
}
