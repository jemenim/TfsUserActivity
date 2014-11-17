using System;
using System.Linq;
using System.Net;
using TfsUserActivity.Core;
using TfsUserActivity.Core.Messaging;

namespace TfsUserActivity.Console
{
    internal class Program
    {
        private const string WorkItemUrlFormat = "https://ve-lab.visualstudio.com/DefaultCollection/FKS/_workitems/edit/{0}";

        private static void Main(string[] args)
        {
            WorkItemRevision[] revisions;
            using (var tfsActivity = new TfsActivity(
                Config.TfsTeamProjectCollectionUri, new NetworkCredential(Config.TfsUserName, Config.TfsPassword)))
            {
                revisions = tfsActivity.GetRevisions(DateTime.Parse("10.11.2014"), DateTime.Parse("16.11.2014"));
            }

            var sheets = revisions.GroupBy(x => new {x.ChangeDate.Date, x.WorkItem})
                .Select(x=>new OneBoardSheet
                {
                    Project = "РТС. Госзакупки - ФКС",
                    Task = "Общие задачи по всем процедурам",
                    Date = x.Key.Date,
                    Duration = TimeSpan.Parse("01:00"),
                    Url = string.Format(WorkItemUrlFormat, x.Key.WorkItem.Id),
                    Comment =x.Key.WorkItem.Title + Environment.NewLine + string.Join(Environment.NewLine, x.OrderBy(y=>y.ChangeDate).Select(y=>string.IsNullOrWhiteSpace(y.History) ? y.State : y.History))
                })
                .ToArray();

            using (var boardExporter = new OneBoardExporter())
            {
                boardExporter.ExportSheets(Config.OneBoardUserName, Config.OneBoardPassword, sheets);
            }

            System.Console.ReadKey();
        }
    }
}