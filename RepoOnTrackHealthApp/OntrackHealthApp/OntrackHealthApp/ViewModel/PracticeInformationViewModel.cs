using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ViewModel
{
    public class PracticeInformationViewModel
    {
        public Guid PracticeInformationId { get; set; }
        public long PracticeProfileId { get; set; }
        public Guid PracticeLocationId { get; set; }
        public string Header { get; set; }
        public string DiningInfo { get; set; }
        public string ParkingInfo { get; set; }
        public string InternetInfo { get; set; }
        public string AtmInfo { get; set; }
        public string VisitorGuide { get; set; }
        public string PharmacyInfo { get; set; }

        public string MapInfo { get; set; }
        public string HelpInfo { get; set; }
        public string DietInfo { get; set; }

    }
}
