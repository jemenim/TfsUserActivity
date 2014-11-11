using System;

namespace TfsUserActivity.Core
{
    public class WorkItemRevision
    {
        public string ChangeBy { get; set; }

        public DateTime ChangeDate { get; set; }

        public string History { get; set; }

        public string State { get; set; }
    }
}