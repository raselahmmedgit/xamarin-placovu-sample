using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.Model
{
    public class PatientNotification
    {
        public long NotificationId { get; set; }

        public long NotificationDetailId { get; set; }

        public string NotificationTitle { get; set; }        
    }
}
