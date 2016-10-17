using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAME_REPLACE
{
    public class Configs
    {
        public static string CorsOrigins = string.Empty;
        public static string CorsMethods = string.Empty;
        public static string CorsHeaders = string.Empty;

        static Configs()
        {
            CorsOrigins = ConfigurationManager.AppSettings["webapi.cors.origins"];
            CorsMethods = ConfigurationManager.AppSettings["webapi.cors.methods"];
            CorsHeaders = ConfigurationManager.AppSettings["webapi.cors.headers"];
        }
    }
}
