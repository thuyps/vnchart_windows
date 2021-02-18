/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
using System;
using System.Collections.Generic;
using System.Text;
using xlib.framework;
/**
 *
 * @author ThuyPham
 */
namespace stock123.app.data
{
    public class stCompanyInfo
    {
        public byte floor;
        public String code;    //  quote
        public int shareID;
        public String company_name;
        public int EPS;
        public int Beta;		//	*1000
        public int PE;
        public int book_value;
        public int ROA;
        public int ROE;
        public int volume;					//	KL niem yet, don vi x1000
        public int vontt;
        public int volumeOwnedByForeigner;	//	KL hold by foreigner
        public int max52weeks;
        public int min52weeks;

        public int Q1;          //  (Year<<16)|Q1   VD: Q1 2010 == (2010<<16)|1;
        public int loi_nhuan1;
        public int doanh_thu1;
        public int Q2;          //  (Year<<16)|Q1   VD: Q1 2010 == (2010<<16)|1;
        public int loi_nhuan2;
        public int doanh_thu2;
        public int Q3;          //  (Year<<16)|Q1   VD: Q1 2010 == (2010<<16)|1;
        public int loi_nhuan3;
        public int doanh_thu3;
        public int Q4;          //  (Year<<16)|Q1   VD: Q1 2010 == (2010<<16)|1;
        public int loi_nhuan4;
        public int doanh_thu4;
    }
}