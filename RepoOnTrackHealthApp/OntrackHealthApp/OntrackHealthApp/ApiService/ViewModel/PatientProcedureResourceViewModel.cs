using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class PatientProcedureResourceViewModel
    {
        public long ProcedureId { get; set; }

        public string ProcedureName { get; set; }

        public int DisplayOrder { set; get; }

        public IEnumerable<PatientResourceCategoryViewModel> PatientResourceCategoryViewModels { set; get; }
    }
}
