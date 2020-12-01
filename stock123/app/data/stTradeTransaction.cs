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
    public class stTradeTransaction
    {
        public int date;		//	0xYYYYMMDD
        public int time;		//	0x00hhmmss
        public int price;
        public int volume;
        public int trade_volume;
    }
}