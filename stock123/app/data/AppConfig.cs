using System;
using System.Collections.Generic;
using System.Text;
using xlib.framework;
using xlib.utils;
using Newtonsoft.Json;

namespace stock123.app.data
{
    class AppConfig
    {
        public int allShareUpdateDate;

        public static AppConfig appConfig = new AppConfig();

        public static void loadAppConfig(){
            xDataInput di = xFileManager.readFile("appconfig2", false);
            if (di != null)
            {
                try
                {
                    String s = di.readUTF();
                    appConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<AppConfig>(s);
                }
                catch (Exception e)
                {
                    Utils.trace(e.Message);
                }
            }
        }

        public static void saveAppConfig()
        {
            if (appConfig != null)
            {
                String s = JsonConvert.SerializeObject(appConfig);
                xDataOutput o = new xDataOutput(s.Length * 2);
                o.writeUTF(s);

                xFileManager.saveFile(o, "appconfig2");
            }
        }

        public static void deleteConfig()
        {
            xFileManager.removeFile("appconfig2");
            appConfig = new AppConfig();
        }
    }
}
