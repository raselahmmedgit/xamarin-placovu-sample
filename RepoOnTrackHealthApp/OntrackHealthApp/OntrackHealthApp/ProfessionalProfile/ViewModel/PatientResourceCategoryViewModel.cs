using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class PatientResourceCategoryViewModel
    {
        public Guid PatientResourceCategoryId { get; set; }
        public string PatientResourceCategoryName { get; set; }
        public int DisplayOrder { get; set; }
        public long ProcedureId { set; get; }
        //public Procedure Procedure { set; get; }
        public IList<PatientPortalResourceViewModel> PatientPortalResourceViewModels { set; get; }
    }
}
