using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class ResourceIndexPageViewModel
    {
        public string ProcedureName { get; set; }

        public Guid PatientProcedureDetailId { get; set; }

        public string PageTitle { get; set; }

        public string PageDescription { get; set; }

        public IEnumerable<PatientProcedureResourceViewModel> PatientProcedureResourceViewModels { get; set; }
    }
}
