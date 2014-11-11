using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsUserActivity.Core.Extensions;

namespace TfsUserActivity.Core
{
    public class TfsActivity : IDisposable
    {
        private TfsTeamProjectCollection _teamProjectCollection;

        public TfsActivity(Uri tfsTeamProjectCollectionUri, NetworkCredential credential)
        {
            _teamProjectCollection = new TfsTeamProjectCollection(tfsTeamProjectCollectionUri, credential);
            _teamProjectCollection.EnsureAuthenticated();
        }

        public void Print(DateTime startDate, DateTime endDate)
        {
            var workItemStore = _teamProjectCollection.GetService<WorkItemStore>();
            
            WorkItemCollection workItemCollection = workItemStore.Query(
                string.Format("SELECT * FROM WorkItems WHERE [System.TeamProject] = 'FKS' AND ever ([Changed By] = @Me) AND [Changed Date] >= '{0}'", startDate.Date));

            var workItems = new List<WorkItem>();
            foreach (WorkItem item in workItemCollection)
            {
                var revisionCollection = item.Revisions;
                bool isInclude = false;

                foreach (Revision revision in revisionCollection)
                {

                    var workItemRevision = revision.Fields.ToWorkItemRevision();
                    if (workItemRevision.ChangeDate >= startDate.Date && workItemRevision.ChangeDate <= endDate.Date &&
                        workItemRevision.ChangeBy.Equals(workItemStore.UserDisplayName))
                    {
                        isInclude = true;
                    }
                }
                if (isInclude)
                {
                    Console.WriteLine("{0} {1} - {2}", item.Type.Name, item.Id, item.Title);
                    foreach (Revision revision in item.Revisions)
                    {
                        var workItemRevision = revision.Fields.ToWorkItemRevision();
                        if (workItemRevision.ChangeDate >= startDate.Date &&
                            workItemRevision.ChangeBy.Equals(workItemStore.UserDisplayName))
                        {
                            Console.WriteLine("{0}{1}{2}", workItemRevision.ChangeDate, Environment.NewLine, string.IsNullOrEmpty(workItemRevision.History) ? workItemRevision.State : workItemRevision.History);
                        }
                    }
                    Console.WriteLine();
                }
            }
        }

        public void Dispose()
        {
            if (_teamProjectCollection != null)
            {
                _teamProjectCollection.Dispose();
                _teamProjectCollection = null;
            }
        }
    }
}