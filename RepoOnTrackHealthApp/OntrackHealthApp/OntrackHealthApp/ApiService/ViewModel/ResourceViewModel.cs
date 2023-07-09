using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class ResourceViewModel
    {
        public ResourceViewModel() {
            ResourceDetailViewModels = new List<ResourceDetailViewModel>();
        }
        public int ResourceViewModelId { get; set; }
        public Guid PatientResourceCategoryId { get; set; }
        public string PatientResourceCategoryName { get; set; }
        public int PatientResourceCategoryDisplayOrder { get; set; }
        public IEnumerable<ResourceDetailViewModel> ResourceDetailViewModels { get; set; }
    }
}
