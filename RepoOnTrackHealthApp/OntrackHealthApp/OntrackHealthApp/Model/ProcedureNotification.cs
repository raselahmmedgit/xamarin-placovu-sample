using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.Model
{
    public class ProcedureNotification
    {
        public long NotificationId { get; set; }

        public string NotificationTitle { get; set; }

        public int NotificationTypeId { get; set; }

        public string NotificationHeader { get; set; }
    }
}
