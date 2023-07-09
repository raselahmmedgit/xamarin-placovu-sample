using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ViewModel
{
    public class OtherProcedurePageViewModel : ViewModelBase
    {
        public string PatientProcedureDetailId { get; set; }

        public long ProcedureId { get; set; }

        public string ProcedureName { get; set; }

        public string PatientProcedureDetailIdAndProcedureName { get; set; }
    }
}
