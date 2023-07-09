using OntrackHealthApp.ApiHelper.Response;
using OntrackHealthApp.ApiService.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.Response
{
    public class PatientSurveyPostResponse : ApiResponse<string>
    {
    }
    public class PatientSurveyGetResponse : ApiResponse<PatientSurveyQuestionSetViewModel>
    {
    }
}
