using System;
using System.Configuration;

namespace TfsUserActivity.Console
{
    public static class Config
    {
        public static Uri TfsTeamProjectCollectionUri
        {
            get
            {
                return new Uri(ConfigurationManager.AppSettings["TfsTeamProjectCollectionUri"]);
            }
        }

        public static string UserName
        {
            get
            {
                return ConfigurationManager.AppSettings["UserName"];
            }
        }
        public static string Password
        {
            get
            {
                return ConfigurationManager.AppSettings["Password"];
            }
        }
    }
}