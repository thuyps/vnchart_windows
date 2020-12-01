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
public class stPriceboardStateIndex {
    public int id;
    public string code;
    public int marketID;
    public float current_point;
    public float changed_point;
    public int changed_percent;	//	*100
    public double total_volume;
    public int inc_cnt;
    public int ce_cnt;
    public int dec_cnt;
    public int floor_cnt;
    public int ref_num;
    public int totalGTGD;		//	*10000
    public int market_status;
    public float reference;
    public string update_time;
    public bool status_changed;
    public string mDate;
    public double du_mua;
    public double du_ban;
    public bool supported;
    public stPriceboardStateIndex(){
        
    }

    public void copyFrom(stPriceboardStateIndex ps)
    {
        this.id = ps.id;
        this.code = ps.code;
        this.marketID = ps.marketID;
        this.current_point = ps.current_point;
        this.changed_point = ps.changed_point;
        this.changed_percent = ps.changed_percent;
        this.status_changed = ps.status_changed;

        this.total_volume = ps.total_volume;
        this.inc_cnt = ps.inc_cnt;
        this.ce_cnt = ps.ce_cnt;
        this.dec_cnt = ps.dec_cnt;
        this.floor_cnt = ps.floor_cnt;
        this.ref_num = ps.ref_num;
        this.totalGTGD = ps.totalGTGD;
        this.reference = ps.reference;
        this.update_time = ps.update_time;
        this.market_status = ps.market_status;
        this.mDate = ps.mDate;
        this.du_ban = ps.du_ban;
        this.du_mua = ps.du_mua;
    }
}
}