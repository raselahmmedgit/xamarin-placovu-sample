using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.ViewModel
{
    public class PatientPortalResourceViewModel
    {
        public Guid PatientPortalResourceId { get; set; }
        
        public string PatientPortalResourceName { get; set; }

        public string ResourceLocation { get; set; }

        public Guid? PatientResourceCategoryId { get; set; }

        public bool IsDeleted { get; set; }

        public int DisplayOrder { get; set; }

        public long ProcedureId { set; get; }

        public string ProcedureName { set; get; }

        public long PracticeProfileId { set; get; }

        public string ResourceContent { get; set; }

    }
}
