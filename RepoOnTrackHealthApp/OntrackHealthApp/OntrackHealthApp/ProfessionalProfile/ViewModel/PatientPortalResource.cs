using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class PatientPortalResource
    {
        public Guid PatientPortalResourceId { get; set; }
        public string PatientPortalResourceName { get; set; }
        public string ResourceLocation { get; set; }
        public Guid? PatientResourceCategoryId { get; set; }
        public bool IsDeleted { get; set; }
        public int DisplayOrder { get; set; }
        public string ResourceContent { get; set; }
        public virtual PatientPortalResourceCategory PatientResourceCategory { set; get; }
    }
}
