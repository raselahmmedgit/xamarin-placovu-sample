using System;

namespace OntrackHealthApp.ApiService.Model
{
    public partial class AppAndroidReleaseHistory
    {
        public Guid ApplicationReleaseId { get; set; }

        public string ApplicationName { get; set; }

        public int? AndroidVersion { get; set; }

        public string AndroidVersionCode { get; set; }

        public DateTime? AndroidReleaseDate { get; set; }

        public string AndroidReleaseNote { get; set; }

        public DateTime? AndroidReleaseEffectiveDate { get; set; }

        public string GooglePlayStoreAppUrl { get; set; }

    }
}
