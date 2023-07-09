using OntrackHealthApp.ApiHelper.Response;
using OntrackHealthApp.ApiService.Model;
using OntrackHealthApp.ApiService.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.Response
{
    public class PatientProcedureDetailResponse : ApiResponse<PatientProcedureDetailModel>
    {
    }

    public class PatientProcedureDetailListResponse : ApiListResponse<List<PatientProcedureDetailModel>>
    {
    }
}
