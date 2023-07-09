using System;

namespace OntrackHealthApp.ApiService.Model
{
    public partial class AppIosReleaseHistory
    {
        public Guid ApplicationReleaseId { get; set; }

        public string ApplicationName { get; set; }

        public int? IosVersion { get; set; }

        public string IosVersionCode { get; set; }

        public DateTime? IosReleaseDate { get; set; }

        public string IosReleaseNote { get; set; }

        public DateTime? IosReleaseEffectiveDate { get; set; }

        public bool? IosShouldForceDownload { get; set; }
    }
}
