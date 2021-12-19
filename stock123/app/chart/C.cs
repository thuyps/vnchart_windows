/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

/**
 *
 * @author ThuyPham
 */
namespace stock123.app
{
    public class C
    {
        public const int CLIENT_VERSION = 727;

        public const int DATE_BEGIN = (2001 << 16) | (1 << 8) | 1;
        public const string URL_EXPAND_ACCOUNT = "http://soft123.com.vn/web/expand_date?device=PC";
        public const string URL_WEB = "http://soft123.com.vn";
        //public const string URL_SERVER = "http://soft123.com.vn:8888/SmaSrv/SSTK";
        //public const string URL_SERVER = "http://soft123.com.vn:8080/SmaSrv/SSTK";
        //public string URL_SERVER = "http://soft123.com.vn:8080/SmaSrv/SSTK";

        public const string IMG_MAIN_ICONS = "mainicons.png";
        public const string IMG_TOOLSTRIPS = "toolstrip.png";
        public const string IMG_MARKET_ICONS = "markets.png";   //  16/21
        public const string IMG_BLANK_ROW_ICON = "global_row.png";   //  1/21
        public const string IMG_MARKET_ICONS_MINI = "mini_markets.png";   //  16/19
        public const string IMG_MINI = "mini.png";  //  20/20
        public const string IMG_SMALL_ARROWS = "small_arrows.png";
        public const string IMG_SUB_BUTTONS = "sub_buttons.png";
        public const string IMG_DRAWER_BUTTONS = "drawer_buttons.png";
        public const string IMG_ALARM_BELL = "alarm.png";

        public const uint TITLE_BACKGROUND_COLOR = 0xff004000;
        public const uint BOTTOM_BACKGROUND_COLOR = 0xff605450;
        public const uint TITLE_BORDER_COLOR = 0xfffafafa;
        public const uint CHAT_LABEL_BG_COLOR = 0xff166891;

        public const uint GREY_LINE_COLOR = 0xff252525;
        public const uint GREY_LINE_COLOR2 = 0xff252525;
        public const uint GREY_LINE_COLOR3 = 0xff606060;
        public const uint BLUE_LINE_COLOR = 0xff0000ff;
        public const uint YELLOW_LINE_COLOR = 0xffaaaa5f;
        public const uint ORANGE_LINE_COLOR = 0xffff7000;
        public const uint CHART_COLOR_GREEN = 0xff00e000;

        public const uint RED_LINE_COLOR = 0xffff0000;
        public const uint GREEN_LINE_COLOR = 0xff00f000;
        public const uint WHITE_LINE_COLOR = 0xffffffff;
        public const uint COLOR_TABLE_TITLE_BG = 0xff004600;
        public const uint COMMON_BACKGROUND_COLOR = 0xff001000;
        public const uint COMMON_BACKGROUND_COLOR2 = 0xff2F3634;//0xff152515;

        //public const uint GREY_LINE_COLOR = 0xff4a4a4a;
        //public const uint GREY_LINE_COLOR2 = 0xff3a3a3a;
        //public const uint YELLOW_LINE_COLOR = 0xffaaaa5f;

        public const uint COLOR_GREEN = 0xff00ff00;
        public const uint COLOR_BLUE = 0xff0000ff;
        public const uint COLOR_BLUE_LIGHT = 0xff4040ff;
        public const uint COLOR_YELLOW = 0xffffff00;
        public const uint COLOR_RED = 0xffff0000;
        public const uint COLOR_BLACK = 0xff000000;
        public const uint COLOR_WHITE = 0xffffffff;
        public const uint COLOR_GREEN_DARK = 0xff008000;
        public const uint COLOR_MAGENTA = 0xffff00ff;
        public const uint COLOR_CYAN = 0xff00ffff;
        public const uint COLOR_GRAY = 0xff808080;
        public const uint COLOR_RED_ORANGE = 0xffff4000;
        public const uint COLOR_ORANGE = 0xffff8000;
        public const uint COLOR_GRAY_DARK = 0xff505050;
        public const uint COLOR_GRAY_LIGHT = 0xffa0a0a0;
        public const uint COLOR_FIBO_DOT_LINE = 0xffff8000;
        public const uint COLOR_FIBO_DOT_LINE2 = 0xff804000;

        public const uint COLOR_FADE_YELLOW0 = 0x90909040;
        public const uint COLOR_FADE_YELLOW = 0x40909040;
        public const uint COLOR_SECOND_CHART = 0xff2280ff;

        //public const uint YELLOW_LINE_COLOR = 0xffaaaa5f;                  
        public const uint COLOR_BG_GRADIENT_UPPER = 0xff101a10;
        public const uint COLOR_BG_GRADIENT_LOWER = 0xff000000;

        public const uint COLOR_BUTTON_SEL = 0xff2050a0;

        //================================
        //  BaseControl's types < 100
        //  app's control types > 100
        public const uint CONTROL_TYPE_INDICATTOR_ITEM = 100;

        public const int ID_DIALOG_NETWORK_CONTACTING = 1000;
        //=====================================
        public const int ID_LINE = 0;
        public const int ID_CANDLE = 1;
        public const int ID_BB = 2;
        public const int ID_PSAR = 3;
        public const int ID_ICHIMOKU = 4;

        public const int ID_CUSTOM1 = 5;
        public const int ID_CUSTOM2 = 6;
        public const int ID_CUSTOM3 = 7;

        public const int ID_FIBONACCIE = 8;

        public const int ID_VOLUME = 9;
        public const int ID_MACD = 10;
        public const int ID_ADX = 11;
        public const int ID_RSI = 12;
        public const int ID_MFI = 13;
        public const int ID_STOCHASTIC_FAST = 14;
        public const int ID_STOCHASTIC_SLOW = 15;
        public const int ID_STOCHRSI = 16;
        public const int ID_WILLIAM_R = 17;
        public const int ID_NUMS = 18;

        //==============================================
        public const int EVT_REPAINT_CHARTS     = 90;
        public const int EVT_SHOW_TUTORIAL = 91;
        public const int EVT_SUB_CHART_CONTAINER_CHANGED = 92;
        public const int EVT_WINDOW_INITIALIZED = 93;
        public const int EVT_FOCUS_AT_CURSOR = 94;
        public const int EVT_REFRESH_SHARE_DATA = 95;
        //==============================================

        public const int ID_BUTTON_INDICES_DETAIL = 1000;
        public const int ID_SHARE_GROUP_BASE = 1100;
        public const int ID_SHARE_GROUP_BASE_END = 1150;

        public const int ID_HIDE_CONTEXT_MENU = 1151;
        public const int ID_BUTTON_SWITCH_SUBCHART = 1152;
        public const int ID_BUTTON_SETTING_CHART = 1153;
        public const int ID_BUTTON_DRAW_GRID = 1154;
        public const int ID_BUTTON_SECOND_CHART = 1155;

        public const int ID_BUTTON_EDIT_SLOGAN = 1160;

        public const int ID_BUTTON_CONTEXT_HELP = 1180;
        public const int ID_CHART_RANGE = 1181;
        public const int ID_CHART_RANGE_END = 1199;
        public const int ID_WEEKLY_CHART = 1200;

        public const int ID_ADD_SHARE = 1201;
        public const int ID_REMOVE_SHARE = 1202;
        public const int ID_ADD_GROUP = 1203;
        public const int ID_REMOVE_GROUP = 1204;

        public const int ID_MARKET_INDEX = 1205;

        public const int ID_SET_ALARM = 1206;
        public const int ID_EXPORT_TO_EXCEL = 1207;

        public const int ID_ADD_SHARE_GAINLOSS = 1210;
        public const int ID_REMOVE_SHARE_GAINLOSS = 1211;
        public const int ID_BUY_MORE = 1212;
        public const int ID_SELL_MORE = 1213;

        public const int ID_REFRESH_DATA = 1219;

        public const int ID_GOTO_SEARCH_SCREEN = 1220;
        public const int ID_GOTO_HOME_SCREEN = 1221;
        //public const int ID_GOTO_MINI_SCREEN = 1222;
        public const int ID_GOTO_HELP = 1223;
        public const int ID_SEARCH_ON = 1224;
        public const int ID_GOTO_SETTING = 1225;
        public const int ID_LOGIN = 1226;
        public const int ID_LOGOUT = 1227;
        public const int ID_SETUP_INDICATOR_PARAMETER = 1228;

        public const int ID_HISTORY_CHART = 1229;
        public const int ID_SPLIT_VIEW = 1230;

        public const int ID_TOOGLE_DRAWING_TOOL = 1231;

        public const int ID_ALARM_MANAGER = 1232;
        public const int ID_ALARM_MODIFY = 1233;
        public const int ID_ALARM_REMOVE = 1234;

        public const int ID_PREVIEW_HISTORY_CHART = 1235;
        public const int ID_SWITCH_VIEW = 1236;
        public const int ID_SELECT_SHARE_CANDLE = 1237;
        public const int ID_CHANGES_STATISTICS_VIEW = 1238;
        public const int ID_SELECT_SHARE_CANDLE_RT = 1239;

        public const int ID_SHOW_FILTER_PARAMETER_FORM = 1241;

        //==============sort================
        public const int ID_SORT_BASE = 1242;
        public const int SORT_MOST_INCREASE = 0;
        public const int SORT_MOST_DECREASE = 1;
        public const int SORT_EPS = 2;
        public const int SORT_PE = 3;
        public const int SORT_TODAY_BIGGEST_VOLUME = 4;
        public const int SORT_ON_MARKET_VOLUME = 5;
        public const int SORT_LOWEST_PRICE = 6;
        public const int SORT_VONHOATT = 7;
        public const int SORT_ABC = 8;
        public const int SORT_BETA = 9;
        public const int SORT_ROA = 10;
        public const int SORT_ROE = 11;
        public const int SORT_VOL_DOTBIEN = 12;

        public const int SORT_HEIKEN = 13;
        public const int SORT_TICHLUY = 14;
        //=================technical filter
        public const int _SORT_MACD_CUT_SIGNAL = (1<<0);
        public const int _SORT_SLOW_STOCHASTIC_K_CUT_D  = (1 << 1);
        public const int _SORT_TENKAN_CUT_KIJUN  = (1 << 2);
        public const int _SORT_RSI_CUT_SMA  = (1 << 3);
        public const int _SORT_MFI_CUT_SMA  = (1 << 4);
        public const int _SORT_ROC_CUT_SMA  = (1 << 5);
        public const int _SORT_ADL_CUT_SMA  = (1 << 6);
        public const int _SORT_RSI_HIGHER = (1 << 7);
        public const int _SORT_MFI_HIGHER = (1 << 8);
        public const int _SORT_ROC_HIGHER = (1 << 9);
        public const int _SORT_PSAR_REVERT = (1 << 10);
        public const int _SORT_PRICE_ABOVE_KUMO = (1 << 11);
        public const int _SORT_VOLUME_IS_UP = (1 << 12);
        public const int _SORT_ACCUMULATION = (1 << 13);
        public const int _SORT_MACD_CONVERGENCY = (1 << 14);
        public const int _SORT_SMA1_CUT_SMA2 = (1 << 15);
        public const int _SORT_BULLISH_NVI = (1 << 16);
        public const int _SORT_ADX_CUT_DMIs = (1 << 17);
        public const int _SORT_SMA_PRICE_5 = (1 << 18);
        public const int _SORT_SMA_PRICE_9 = (1 << 19);
        public const int _SORT_SMA_PRICE_12 = (1 << 20);
        public const int _SORT_SMA_PRICE_26 = (1 << 21);
        public const int _SORT_SMA_PRICE_50 = (1 << 22);
        public const int _SORT_SMA_PRICE_100 = (1 << 23);

        public const int ID_SORT_END = 1270;
        //=============end of sort=================
        public const int ID_SORT_TECHNICAL = 1271;
        public const int ID_SORT_TECHNICAL_EDIT = 1272;

        public const int ID_SORT_TECHNICAL2 = 1274;
        public const int ID_SORT_TECHNICAL_EDIT2 = 1275;

        public const int ID_SORT_SMA_CUT_PRICE = 1276;
        public const int ID_SORT_SMA_CUT_PRICE_EDIT = 1277;

        public const int ID_DLG_ADD_SHARE = 1300;
        public const int ID_DLG_ADD_SHARE_GAINLOSS = 1301;
        public const int ID_DLG_BUTTON_BACK = 1340;
        public const int ID_DLG_BUTTON_OK = 1341;
        public const int ID_DLG_BUTTON_RESET = 1342;

        public const int ID_DROPDOWN_FAVOR_GROUP = 1400;
        public const int ID_DROPDOWN_COMMON_GROUP = 1401;
        public const int ID_GROUP_SPECIAL = 1402;
        public const int ID_GAIN_LOSS = 1399;

        public const int ID_ADD_MASTER_CHART = 1403;
        public const int ID_ADD_SUB_CHART = 1405;
        public const int ID_REMOVE_SUB_CHART = 1406;

        public const int ID_BUTTON_INDICES = 1407;

        public const int ID_TS_INFO = 1419;
        public const int ID_TS_CHARTLINE = 1420;
        public const int ID_TS_CHARTCANDLE = 1421;
        public const int ID_TS_CHARTOHLC = 1422;
        public const int ID_TS_CHARTHLC = 1423;
        public const int ID_TS_CHARTCANDLE_HEIKEN = 1424;

        public const int ID_TS_BOLLINGER = 1425;
        public const int ID_TS_PSAR = 1426;
        public const int ID_TS_ZIGZAG = 1427;
        public const int ID_TS_ICHIMOKU = 1428;
        public const int ID_TS_ENVELOP = 1429;
        public const int ID_TS_SMA1 = 1430;
        public const int ID_TS_SMA2 = 1431;
        public const int ID_TS_SMA3 = 1432;

        public const int ID_TS_VOLUMEBYPRICE = 1433;
        public const int ID_TS_COMPARE_YEAR1 = 1434;
        public const int ID_TS_COMPARE_YEAR2 = 1435;
        public const int ID_TS_COMPARE_2_SHARES = 1436;
        public const int ID_TS_PIVPOT_POINT = 1437;

        public const int ID_TS_DRAW_TREND_ARROW = 1439;
        public const int ID_TS_DRAW_TREND = 1440;
        public const int ID_TS_DRAW_RETRACE = 1441;
        public const int ID_TS_DRAW_PROJECT = 1442;
        public const int ID_TS_DRAW_TIME = 1443;
        public const int ID_TS_DRAW_ARC = 1444;
        public const int ID_TS_DRAW_FAN = 1445;
        public const int ID_TS_DRAW_RECTANGLE = 1446;
        public const int ID_TS_DRAW_ECLIPSE = 1447;
        public const int ID_TS_DRAW_TRIANGLE = 1448;
        public const int ID_TS_DRAW_ANDREWS_PITCHFORK = 1449;
        public const int ID_TS_DRAW_ABC = 1450;
        public const int ID_TS_DRAW_CLEAR_ALL = 1451;

        public const int ID_TS_ADD_SUBCHART = 1454;

        public const int ID_BUTTON_QUOTE = 1455;

        public const int ID_EDIT_BOLLINGER = 1460;
        public const int ID_EDIT_ENVELOP = 1461;
        public const int ID_EDIT_ICHIMOKU = 1462;
        public const int ID_EDIT_PSAR = 1463;
        public const int ID_EDIT_ZIGZAG = 1464;

        public const int ID_CAPTURE_IMAGE = 1470;
        public const int ID_RELOAD_DATA_OF_SYMBOL = 1471;
        public const int ID_ADD_SYMBOL_TO_GROUP = 1472;

        public const int ID_TS_VSTOP = 1480;

        public const int ID_BUTTON_CHON_NHOM_CP = 1481;
        public const int ID_BUTTON_CHON_NHOM_CP_NGANH = 1482;
        //-------------------------------------------
        public const int ID_SORT_TA_BASE = 1500;
        public const int SORT_TA_MACD_CUT_SIGNAL = 0;
        public const int SORT_STOCHASTIC = 1;
        public const int SORT_RSI_OVERSOLD = 2;
        public const int SORT_RSI_OVERBOUGHT = 3;
        public const int SORT_MFI_OVERSOLD = 4;
        public const int SORT_MFI_OVERBOUGHT = 5;
        public const int SORT_PSAR_REVERSE_UP = 15;
        public const int SORT_TENKAN_CUT_KIJUN = 16;
        public const int SORT_PRICE_ENTERS_KUMO = 17;
        public const int SORT_PRICE_UP_ABOVE_KUMO = 18;
        public const int SORT_VOLUME_UP = 19;

        public const int ID_SORT_TA_END = 1549;
        //-------------------------------------------
        public const int ID_SORT_CANDLE_BASE = 1550;
        public const int SORT_BULLISH_ENGULFING = 0;
        public const int SORT_BULLISH_PEARCING = 1;
        public const int SORT_MORNING_STAR = 2;
        public const int SORT_HAMMER = 3;
        public const int SORT_BULLISH_HARAMI = 4;

        public const int ID_SORT_CANDLE_END = 1600;
        //-------------------------------------------

        public const int ID_SUBCHART_CONTAINER_0 = 2200;
        public const int ID_SUBCHART_CONTAINER_1 = 2201;
        public const int ID_SUBCHART_CONTAINER_2 = 2202;
        public const int ID_SUBCHART_CONTAINER_3 = 2203;
        public const int ID_SUBCHART_CONTAINER_4 = 2204;

        public const int ID_PRICEBOARD_TABLE = 2300;
        public const int ID_GAINLOSS_TABLE = 2301;

        public const int ID_MINI_REALTIMECHART = 2400;
        public const int ID_MINI_RESTORE = 2410;
        public const int ID_MINI_EXIT = 2411;
        //===========================================
        public const int ID_BUTTON_ADD_FAVOR_SHARE = 3000;

        //==================================================
        public const int ID_SYMBOL_CLICK_START = 3100;
        public const int ID_SYMBOL_CLICK_END = 3500;

        public static String S_CONTACTING_SERVER = "Contacting server ...";
    }
}