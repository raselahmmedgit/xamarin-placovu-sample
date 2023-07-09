using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.Model
{
    public class ProcedureNotificationDetail
    {
        public long NotificationDetailId { get; set; }

        public long? NotificationId { get; set; }

        public string NotificationDetailHeader { get; set; }

        public string NotificationDetail { get; set; }
    }
}
