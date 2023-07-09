using System;

namespace OntrackHealthApp.ViewModel
{
    public class PatientProgressReportResourceHistoryView
    {
        public Guid AutoId { get; set; }

        public Guid ResourceId { get; set; }

        public Guid PatientProgressReportHistoryId { get; set; }

        public string ResourceName { get; set; }

        public string ResourceContent { get; set; }

        public int DisplayOrder { get; set; }
    }
}
