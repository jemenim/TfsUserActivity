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

        public static string TfsUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["TfsUserName"];
            }
        }
        public static string TfsPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["TfsPassword"];
            }
        }

        public static string OneBoardUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["OneBoardUserName"];
            }
        }
        public static string OneBoardPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["OneBoardPassword"];
            }
        }
    }
}