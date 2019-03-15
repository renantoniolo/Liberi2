using PCLAppConfig;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lux.Helpers
{
    public static class AppConfig
    {
        public static string urlBaseQA = ConfigurationManager.AppSettings["apiQA.url"];
        public static string urlBaseDEV = ConfigurationManager.AppSettings["apiDEV.url"];
        public static string urlBasePROD = ConfigurationManager.AppSettings["apiPROD.url"];
    }
}
