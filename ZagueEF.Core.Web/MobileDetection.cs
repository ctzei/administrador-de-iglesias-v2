using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ZagueEF.Core.Web
{
    public static class MobileDetection
    {
        private static string[] mobiles = new[]
                {
                    "iphone", "ipod", "ipad", 
                    "webos", "hpwos", "touchpad", "android", "rim tablet",
                    "opera mini", "opera mobile", "dolphin", "fennec",
                    "blackberry", "mib/", "symbian", 
                    "midp", "j2me", "palmos", "palmsource", 
                    "240x320", "opwv", "chtml",
                    "pda", "windows ce", "mmp/", 
                    "wireless", "nokia", "hand", "mobi",
                    "phone", "cdm", "samsung", "htc", 
                    "mot-", "mitsu", "sagem", "sony", 
                    "alcatel", "lg", "eric", "philips", "mmm", 
                    "panasonic", "sharp", "wap", "sch",
                    "rover", "pocket", "benq", "java", 
                    "vox", "amoi", "bird", "compal", "kg", "voda",
                    "sany", "kdd", "dbt", "sendo", 
                    "sgh", "gradi", "jb", "dddi", "moto"
                };

        public static bool IsMobileBrowser()
        {
            //GETS THE CURRENT USER CONTEXT
            HttpContext context = HttpContext.Current;

            //FIRST TRY BUILT IN ASP.NET CHECK
            if (context.Request.Browser.IsMobileDevice)
            {
                return true;
            }

            //THEN TRY CHECKING FOR THE HTTP_X_WAP_PROFILE HEADER
            if (context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
            {
                return true;
            }

            //THEN TRY CHECKING THAT HTTP_ACCEPT EXISTS AND CONTAINS WAP
            if (context.Request.ServerVariables["HTTP_ACCEPT"] != null && context.Request.ServerVariables["HTTP_ACCEPT"].ToLowerInvariant().Contains("wap"))
            {
                return true;
            }

            //AND FINALLY CHECK THE HTTP_USER_AGENT 
            if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                string userAgent = context.Request.ServerVariables["HTTP_USER_AGENT"].ToLowerInvariant();
                for (int i = 0; i < mobiles.Length; i++)
                {
                    if (userAgent.Contains(mobiles[i]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
