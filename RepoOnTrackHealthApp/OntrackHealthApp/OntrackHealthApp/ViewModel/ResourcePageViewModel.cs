using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class ResourcePageViewModel
    {
        public ResourcePageViewModel() {
            ResourceViewModels = new List<ResourceViewModel>();
        }
        public List<ResourceViewModel> ResourceViewModels { get; set; }
    }
}
