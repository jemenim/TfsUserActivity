using System;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsUserActivity.Core.Messaging
{
    public class WorkItemRevision
    {
        public WorkItem WorkItem { get; set; }

        public string ChangeBy { get; set; }

        public DateTime ChangeDate { get; set; }

        public string History { get; set; }

        public string State { get; set; }
    }
}