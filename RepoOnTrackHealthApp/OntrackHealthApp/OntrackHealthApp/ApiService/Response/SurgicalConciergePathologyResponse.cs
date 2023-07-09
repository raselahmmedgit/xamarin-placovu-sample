using OntrackHealthApp.ApiHelper.Response;
using OntrackHealthApp.ApiService.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.Response
{
    public class SurgicalConciergePathologyResponse : ApiResponse<string>
    {
    }
    public class SurgicalConciergePatientProfileResponse : ApiListResponse<List<PatientProfileWithProfessionalProcedureView>>
    {
    }
}
