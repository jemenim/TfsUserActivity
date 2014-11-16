using System;

namespace TfsUserActivity.Core.Messaging
{
    public class OneBoardSheet
    {
        public string Project { get; set; }
        public string Task { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public string Url { get; set; }
        public string Comment { get; set; }
    }
}
