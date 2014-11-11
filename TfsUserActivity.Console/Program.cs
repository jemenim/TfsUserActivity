using System;
using System.Net;
using TfsUserActivity.Core;

namespace TfsUserActivity.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var tfsActivity = new TfsActivity(
                Config.TfsTeamProjectCollectionUri, new NetworkCredential(Config.UserName, Config.Password)))
            {
                tfsActivity.Print(DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(1));
            }
        }
    }
}