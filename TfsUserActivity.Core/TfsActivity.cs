using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TfsUserActivity.Core.Extensions;
using TfsUserActivity.Core.Messaging;

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

        public WorkItemRevision[] GetRevisions(DateTime startDate, DateTime endDate)
        {
            IList<WorkItemRevision> result = new List<WorkItemRevision>();
            var workItemStore = _teamProjectCollection.GetService<WorkItemStore>();
            
            WorkItemCollection workItemCollection = workItemStore.Query(
                string.Format("SELECT * FROM WorkItems WHERE [System.TeamProject] = 'FKS' AND ever ([Changed By] = @Me) AND [Changed Date] >= '{0}'", startDate.Date));

            foreach (WorkItem item in workItemCollection)
            {
                var revisionCollection = item.Revisions;
                bool isInclude = false;

                foreach (Revision revision in revisionCollection)
                {

                    var workItemRevision = revision.ToWorkItemRevision();
                    if (workItemRevision.ChangeDate >= startDate.Date && workItemRevision.ChangeDate <= endDate.Date &&
                        workItemRevision.ChangeBy.Equals(workItemStore.UserDisplayName))
                    {
                        isInclude = true;
                    }
                }
                if (isInclude)
                {
                    foreach (Revision revision in item.Revisions)
                    {
                        var workItemRevision = revision.ToWorkItemRevision();
                        if (workItemRevision.ChangeDate >= startDate.Date &&
                            workItemRevision.ChangeBy.Equals(workItemStore.UserDisplayName))
                        {
                            result.Add(workItemRevision);
                        }
                    }
                }
            }
            return result.ToArray();
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