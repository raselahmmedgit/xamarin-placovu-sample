using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ProfessionalProfile.ViewModel
{
    public class ScgNursingRoundTemplateCategoryApiViewModel
    {
        public long TemplateCategoryId { get; set; }
        public string TemplateCategoryName { get; set; }
        public bool IsDeleted { get; set; }
        public long? DisplayOrder { get; set; }

        public virtual IList<ScgNursingRoundTemplateCategoryDetailApiViewModel> ScgNursingRoundTemplateCategoryDetailApiViewModels { set; get; }
    }

    public class ScgNursingRoundTemplateCategoryDetailApiViewModel
    {
        public long TemplateCategoryDetailId { get; set; }
        public string TemplateCategoryDetailText { get; set; }

        public long? TemplateCategoryId { get; set; }
        public bool IsFloorPhoneField { get; set; }
        public bool IsCustomMessageField { get; set; }
        public bool IsDeleted { get; set; }
        public long? DisplayOrder { get; set; }
    }
}
