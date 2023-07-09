using OntrackHealthApp.ApiService.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ViewModel
{
    public class ResourceDetailPageViewModel
    {
        public int PatientPortalResourceDetailId { get; set; }
        public string ProcedureName { get; set; }
        public ResourceDetailViewModel CurrentResourceDetailViewModel { get; set; }
        public List<ResourceDetailViewModel> ResourceDetailViewModels { get; set; }
    }
}
