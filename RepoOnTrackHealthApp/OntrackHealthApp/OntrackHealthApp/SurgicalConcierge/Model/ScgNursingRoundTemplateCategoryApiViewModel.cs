using System.Collections.Generic;

namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class ScgNursingRoundTemplateCategoryApiViewModel
    {
        public ScgNursingRoundTemplateCategoryApiViewModel()
        {
            ScgNursingRoundTemplateCategoryDetailApiViewModels = new List<ScgNursingRoundTemplateCategoryDetailApiViewModel>();
        }
        public long TemplateCategoryId { get; set; }
        public string TemplateCategoryName { get; set; }
        public bool IsDeleted { get; set; }
        public long? DisplayOrder { get; set; }
        public virtual List<ScgNursingRoundTemplateCategoryDetailApiViewModel> ScgNursingRoundTemplateCategoryDetailApiViewModels { set; get; }
    }
}