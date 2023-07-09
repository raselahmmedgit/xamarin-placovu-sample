using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class PatientProcedureLocationViewModel
    {
        public long PatientProfileId { get; set; }

        public long ProcedureId { set; get; }

        public string ProcedureName { get; set; }

        public string LocationName { get; set; }

        public string StreetAddress { get; set; }

        public string CityName { get; set; }

        public string StateName { get; set; }

        public string ZipCode { get; set; }

        public string Address { get; set; }
    }
}
