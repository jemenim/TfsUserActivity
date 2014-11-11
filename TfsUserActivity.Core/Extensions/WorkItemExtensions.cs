using System;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsUserActivity.Core.Extensions
{
    public static class WorkItemExtensions
    {
        public static WorkItemRevision ToWorkItemRevision(this FieldCollection @fieldCollection)
        {
            var workItemRevision = new WorkItemRevision();
            foreach (Field field in fieldCollection)
            {
                if (field.Name.Equals("Changed By"))
                {
                    workItemRevision.ChangeBy = field.Value as string;
                }

                if (field.Name.Equals("Changed Date"))
                {
                    workItemRevision.ChangeDate = (DateTime)field.Value;
                }

                if (field.Name.Equals("History"))
                {
                    workItemRevision.History = (string)field.Value;
                }

                if (field.Name.Equals("State"))
                {
                    workItemRevision.State = (string)field.Value;
                }
            }

            return workItemRevision;
        }
    }
}