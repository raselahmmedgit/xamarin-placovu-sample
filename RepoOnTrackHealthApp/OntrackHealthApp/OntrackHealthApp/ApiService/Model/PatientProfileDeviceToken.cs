using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.Model
{
    public class PatientProfileDeviceToken
    {
        public long PatientProfileDeviceTokenId { get; set; }
        public long PatientProfileId { get; set; }
        public string DeviceToken { get; set; }
        public bool IsExpired { set; get; }
        public DateTime CreatedOn { set; get; }
    }
}
