using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class PatientResourceCategoryViewModel
    {
        public Guid PatientResourceCategoryId { get; set; }

        public string PatientResourceCategoryName { get; set; }

        public int DisplayOrder { get; set; }

        public long ProcedureId { set; get; }

        public IList<PatientPortalResourceViewModel> PatientPortalResourceViewModels { set; get; }
    }
}
