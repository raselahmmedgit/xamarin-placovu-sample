using OntrackHealthApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class PatientPortalResourceCategory
    {
        public Guid PatientResourceCategoryId { get; set; }
        public string PatientResourceCategoryName { get; set; }
        public int DisplayOrder { get; set; }
        public long ProcedureId { set; get; }
        public virtual ProcedureViewModel Procedure { set; get; }
        public virtual IList<PatientPortalResource> PatientPortalResources { set; get; }

    }
}
