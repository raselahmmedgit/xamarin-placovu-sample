using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class PatientSurveyPracticeLocationViewModel
    {
        public Guid PracticeLocationId { get; set; }

        public long PracticeProfileId { get; set; }

        public int? PracticeLocationTypeId { get; set; }

        public string LocationName { get; set; }

        public string StreetAddress { get; set; }

        public string CityName { get; set; }

        public string ZipCode { get; set; }

        public string FullAddress
        {
            get
            {
                string tmp = LocationName;
                if (!string.IsNullOrEmpty(StreetAddress))
                    tmp += ", " + StreetAddress;
                if (!string.IsNullOrEmpty(CityName))
                    tmp += ", " + CityName;
                if (!string.IsNullOrEmpty(CityName))
                    tmp += ", " + CityName;
                return tmp;
            }
        }
    }
}
