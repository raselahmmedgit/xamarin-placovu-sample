using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.Model
{
    public class ApplicationSettingModel
    {
        public string ApplicationId { get; set; }

        public string HostAddress { get; set; }

        public string DefaultUtcTimeZone { get; set; }

        public string DefaultUtcTimeZoneOffset { get; set; }

        public bool? IsMobileDevice { get; set; }

        public string MobileNotificationIntervalMinute { get; set; }

        public string MobileNotificationElapsedMinute { get; set; }
    }
}
